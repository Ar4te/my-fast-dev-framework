using Common.CustomAttribute;
using Domain.Entity.Base;
using Domain.Enum.RuleEngine;
using SqlSugar;
namespace Domain.Entity.RuleEngine;

[SugarTable("Rule")]
[DatabaseConfigId("RuleEngine")]
public class Rule : BaseEntity
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }

    [SugarColumn(IsNullable = false)]
    public string RuleName { get; set; }

    [SugarColumn(IsNullable = false)]
    public string Script { get; set; }

    [SugarColumn(IsNullable = false)]
    public ScriptLanguageType ScriptLanguageType { get; set; } = ScriptLanguageType.CSharp;
    public string Description { get; set; }

    public bool ScriptNeedParameters { get; set; } = false;
}