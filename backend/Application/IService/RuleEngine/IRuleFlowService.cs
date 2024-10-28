using System.Diagnostics.CodeAnalysis;
using Application.Dto.RuleEngine;
using Domain.Entity.RuleEngine;

namespace Application.IService.RuleEngine;

public interface IRuleFlowService : IBaseService<RuleFlow>
{
    Task<bool> AddNodeToFlow(int flowId, List<RuleFlowNodeDto> ruleFlowNodeDtos);
    Task<bool> AddNodeToFlow([NotNull] int flowId, [NotNull] int preNodeId, [NotNull] RuleFlowNodeDto ruleFlowNodeDto);
    Task<object> ExecuteRuleFlow(int flowId, Dictionary<int, Dictionary<string, object>> inputs);
    Task<RuleFlow> GetRuleFlow(int flowId);
    Task<bool> SaveRuleFlow(RuleFlowDto ruleFlowDto);
}
