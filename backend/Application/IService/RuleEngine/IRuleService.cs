using Application.Dto.RuleEngine;
using Domain.Entity.RuleEngine;

namespace Application.IService.RuleEngine;

public interface IRuleService : IBaseService<Rule>
{
    Task<object?> ExecuteRuleAsync(int ruleId, Dictionary<string, object>? inputs = null);
    object TestScriptCode(ScriptTestDto scriptTestDto);
}
