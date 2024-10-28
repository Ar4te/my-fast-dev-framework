using System.Text.Json;
using Common.CustomAttribute;
using Domain.Entity.Base;
using Domain.Enum.RuleEngine;
using SqlSugar;
namespace Domain.Entity.RuleEngine;

[SugarTable("RuleFlowNode")]
[DatabaseConfigId("RuleEngine")]
public class RuleFlowNode : BaseEntity
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }

    [SugarColumn(IsNullable = false)]
    public int FlowId { get; set; }

    [SugarColumn(IsNullable = false)]
    public int RuleId { get; set; }

    [SugarColumn(IsNullable = false)]
    public NodeType NodeType { get; set; }

    [SugarColumn(IsNullable = true)]
    public int? NextNodeId { get; set; }

    [SugarColumn(IsNullable = false)]
    public int Index { get; set; }

    /// <summary>
    /// 此节点执行结果作为下一节点的执行参数的字典Json
    /// </summary>
    // public string ResultParametersDicJson { get; set; }

    /// <summary>
    /// 此节点执行结果作为下一节点的执行参数
    /// 此节点的执行结果必须为字典或字典Json字符串
    /// </summary>
    public bool UseResultAsNextParameter { get; set; } = false;

    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToOne, nameof(RuleId), nameof(Rule.Id))]
    public Rule Rule { get; set; }

    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToOne, nameof(NextNodeId), nameof(Id))]
    public RuleFlowNode? NextNode { get; set; }
}
