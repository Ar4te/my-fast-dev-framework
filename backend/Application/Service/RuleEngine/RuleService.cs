using Application.Dto.RuleEngine;
using Application.Extension;
using Application.IService.RuleEngine;
using Domain.Entity.RuleEngine;
using Domain.IRepository;

namespace Application.Service.RuleEngine;

public class RuleService : BaseService<Rule>, IRuleService
{
    public RuleService(IBaseRepository<Rule> ruleRepo)
    {
        BaseRepo = ruleRepo;
    }

    public async Task<bool> AddRule(Rule rule)
    {
        var res = await BaseRepo.CreateAsync(rule);
        return res == 1;
    }

    public async Task<object?> ExecuteRuleAsync(int ruleId, Dictionary<string, object>? inputs = null)
    {
        var rule = await BaseRepo.Client.Queryable<Rule>().In(ruleId).SingleAsync();
        if (rule is null) return $"Rule {ruleId} is not found";
        if (rule.ScriptNeedParameters && (inputs is null || inputs.Count == 0)) return $"Rule has parameters, but not in inputs";
        var res = RuleExtension.ExecuteRule(rule, inputs);
        var output = new RuleExecuteResultDto(ruleId, rule.RuleName, rule.Script, res);
        return output;
    }

    public object? TestScriptCode(ScriptTestDto scriptTestDto)
    {
        var res = RuleExtension.ExecuteScript(scriptTestDto.ScriptCode, scriptTestDto.ScriptLanguageType, scriptTestDto.Inputs);
        return res;
    }
}