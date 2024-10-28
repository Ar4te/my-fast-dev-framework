using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Application.Dto.RuleEngine;
using Application.Extension;
using Application.IService.RuleEngine;
using Domain.Entity.RuleEngine;
using Domain.Enum.RuleEngine;
using Domain.IRepository;
using Infrastructure.UnitOfWork;

namespace Application.Service.RuleEngine;

public class RuleFlowService : BaseService<RuleFlow>, IRuleFlowService
{
    private readonly IRuleFlowNodeService _rfnServ;
    private readonly IUnitOfWorkManager _uow;

    public RuleFlowService(IBaseRepository<RuleFlow> rfRepo, IRuleFlowNodeService rfnServ, IUnitOfWorkManager uow)
    {
        BaseRepo = rfRepo;
        _rfnServ = rfnServ;
        _uow = uow;
    }

    public async Task<RuleFlow> GetRuleFlow(int flowId)
    {
        return await BaseRepo.Client.Queryable<RuleFlow>()
                .In(flowId)
                .Includes(rf => rf.Nodes.OrderBy(rfn => rfn.Index).ToList(), rfn => rfn.Rule)
                .Includes(rf => rf.Nodes, rfn => rfn.NextNode)
                .SingleAsync();
    }

    public async Task<bool> SaveRuleFlow(RuleFlowDto ruleFlowDto)
    {
        var ruleFlow = new RuleFlow
        {
            FlowName = ruleFlowDto.FlowName,
            Description = ruleFlowDto.Description
        };
        try
        {
            _uow.BeginTran();
            var createdRuleFlowId = await BaseRepo.CreateAndReturnPkAsync<int>(ruleFlow);
            if (createdRuleFlowId is default(int)) return false;

            if (ruleFlowDto.NodeDtos.Count == 0) return true;
            var ruleFlowNodes = new List<RuleFlowNode>();
            ruleFlowDto.NodeDtos?.ForEach(nodeDto =>
            {
                ruleFlowNodes.Add(new RuleFlowNode
                {
                    FlowId = createdRuleFlowId,
                    RuleId = nodeDto.RuleId,
                    NodeType = (NodeType)nodeDto.NodeType,
                    NextNodeId = nodeDto.NextNodeId,
                    UseResultAsNextParameter = nodeDto.NextNodeId is not null && nodeDto.UseResultAsNextParameter,
                    Index = nodeDto.NodeIndex,
                });
            });
            var createdRuleFlowNodeIds = await _rfnServ.CreateListAndReturnPksAsync<int>(ruleFlowNodes);
            var res = createdRuleFlowNodeIds?.Count == ruleFlowNodes.Count;
            if (res)
            {
                _uow.CommitTran();
            }
            else
            {
                _uow.RollBackTran();
            }
            return res;
        }
        catch
        {
            _uow.RollBackTran();
            return false;
        }
    }

    public async Task<bool> AddNodeToFlow(int flowId, List<RuleFlowNodeDto> ruleFlowNodeDtos)
    {
        try
        {
            if (ruleFlowNodeDtos is null || ruleFlowNodeDtos.Count == 0) return false;

            var nodes = new List<RuleFlowNode>();
            ruleFlowNodeDtos.ForEach(nodeDto =>
            {
                nodes.Add(new RuleFlowNode
                {
                    FlowId = flowId,
                    RuleId = nodeDto.RuleId,
                    NodeType = (NodeType)nodeDto.NodeType,
                    NextNodeId = nodeDto.NextNodeId,
                    UseResultAsNextParameter = nodeDto.NextNodeId is not null && nodeDto.UseResultAsNextParameter,
                    Index = nodeDto.NodeIndex,
                });
            });
            nodes = [.. nodes.OrderByDescending(rfn => rfn.Index)];
            _uow.BeginTran();
            var res = await _rfnServ.CreateAsync(nodes);
            if (res == nodes.Count)
            {
                _uow.CommitTran();
            }
            else
            {
                _uow.RollBackTran();
            }

            return res == nodes.Count;
        }
        catch
        {
            _uow.RollBackTran();
            return false;
        }
    }

    public async Task<bool> AddNodeToFlow([NotNull] int flowId, [NotNull] int preNodeId, [NotNull] RuleFlowNodeDto ruleFlowNodeDto)
    {
        var flow = await BaseRepo.Client.Queryable<RuleFlow>()
            .In(flowId)
            .Includes(rf => rf.Nodes.OrderBy(rfn => rfn.Index).ToList(), rfn => rfn.NextNode)
            .SingleAsync();
        if (flow is null) return false;

        var preNode = flow.Nodes.Find(node => node.Id == preNodeId);

        if (preNode == null) return false;

        var newNodeIndex = ruleFlowNodeDto.NodeIndex > preNode.Index ? ruleFlowNodeDto.NodeIndex : preNode.Index + 1;

        var node = new RuleFlowNode
        {
            FlowId = flowId,
            RuleId = ruleFlowNodeDto.RuleId,
            NodeType = (NodeType)ruleFlowNodeDto.NodeType,
            NextNodeId = preNode.NextNodeId,
            UseResultAsNextParameter = preNode.NextNodeId is not null && ruleFlowNodeDto.UseResultAsNextParameter,
            Index = newNodeIndex,
        };
        try
        {
            _uow.BeginTran();
            var createdNewNodeId = await _rfnServ.CreateAndReturnPkAsync<int>(node);
            if (createdNewNodeId <= 0)
            {
                _uow.RollBackTran();
                return false;
            }

            preNode.NextNodeId = createdNewNodeId;

            var _index = newNodeIndex + 1;
            var lastNodes = flow.Nodes.SkipWhile(_node => _node.Index < newNodeIndex).Select(t =>
            {
                if (t.Index < _index)
                {
                    t.Index = _index;
                    _index += 1;
                }
                return t;
            }).ToList();

            var updatePreRes = await _rfnServ.UpdateAsync(preNode);
            var updateLastNodesRes = lastNodes.Count <= 0 || (await _rfnServ.UpdateAsync(lastNodes) == lastNodes.Count);
            if (updatePreRes == 1 && updateLastNodesRes)
            {
                _uow.CommitTran();
                return true;
            }
            _uow.RollBackTran();
        }
        catch
        {
            _uow.RollBackTran();
        }
        return false;
    }

    public async Task<object> ExecuteRuleFlow(int flowId, Dictionary<int, Dictionary<string, object>> inputs)
    {
        var ruleFlow = await BaseRepo.Client.Queryable<RuleFlow>()
            .In(flowId)
            .Includes(rf => rf.Nodes.OrderBy(node => node.Index).ToList(), rfn => rfn.Rule)
            .Includes(rf => rf.Nodes.OrderBy(node => node.Index).ToList(), rfn => rfn.NextNode)
            .SingleAsync();
        if (ruleFlow == null || ruleFlow.Nodes.TrueForAll(t => t is null))
        {
            return "Rule flow not found";
        }

        var currentNode = ruleFlow.Nodes.FirstOrDefault();
        var ruleOutputs = new RuleFlowExecuteResultDto(flowId, ruleFlow.FlowName);
        while (currentNode is not null)
        {
            if (currentNode.NodeType == NodeType.Rule)
            {
                var _inputs = inputs.Count > currentNode.Index ? inputs[currentNode.Index] : null;
                var ruleOutput = currentNode.Rule.ExecuteRule(_inputs);

                ruleOutputs.RuleExecuteResult.Add(new RuleFlowNodeExecuteResultDto(currentNode.Id, currentNode.Index, currentNode.Rule.Id, currentNode.Rule.RuleName, currentNode.Rule.Script, ruleOutput));
                if (currentNode.NextNodeId is null)
                {
                    return ruleOutputs;
                }

                if (currentNode.NextNode is null)
                {
                    return ruleOutputs;
                }

                if (currentNode.UseResultAsNextParameter && currentNode.NextNode.Rule.ScriptNeedParameters)
                {
                    var rParameters = JsonSerializer.Deserialize<Dictionary<string, object>>(JsonSerializer.Serialize(ruleOutput));

                    if (rParameters?.Count != 0)
                    {
                        foreach (var p in rParameters!)
                        {
                            if (!inputs[currentNode.NextNode.Index].TryAdd(p.Key, p.Value))
                            {
                                var _ValList = (List<object>)inputs[currentNode.NextNode.Index][p.Key];
                                _ValList.Add(p.Value);
                                inputs[currentNode.NextNode.Index][p.Key] = _ValList;
                            }
                        }
                    }
                }
                currentNode = ruleFlow.Nodes.First(node => node.Index > currentNode.Index);
            }
            else if (currentNode.NodeType == NodeType.Condition)
            {
                var _inputs = inputs.Count > currentNode.Index ? inputs[currentNode.Index] : null;
                var ruleOutput = (bool)(currentNode.Rule.ExecuteRule(_inputs) ?? false);
                ruleOutputs.RuleExecuteResult.Add(new RuleFlowNodeExecuteResultDto(currentNode.Id, currentNode.Index, currentNode.Rule.Id, currentNode.Rule.RuleName, currentNode.Rule.Script, ruleOutput));
                if (ruleOutput && currentNode.NextNode is not null)
                {
                    currentNode = ruleFlow.Nodes.First(node => node.Index > currentNode.Index);
                }
                else
                {
                    currentNode = null;
                }
            }
            else
            {
                // NodeType.Parallel
            }
        }
        return ruleOutputs;
    }
}