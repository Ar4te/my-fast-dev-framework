using Common.CustomAttribute;
using Domain.Entity.Base;
using SqlSugar;

namespace Domain.Entity.RuleEngine;

[SugarTable("RuleFlow")]
[DatabaseConfigId("RuleEngine")]
public class RuleFlow : BaseEntity
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }

    [SugarColumn(IsNullable = false)]
    public string FlowName { get; set; }
    public string Description { get; set; }

    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(RuleFlowNode.FlowId), nameof(Id))]
    public List<RuleFlowNode> Nodes { get; set; }
}
