using Application.Dto.RuleEngine;
using Application.IService.RuleEngine;
using Domain.Enum.RuleEngine;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class RuleController : ControllerBase
{
    private readonly IRuleService _ruleServ;

    public RuleController(IRuleService ruleServ)
    {
        _ruleServ = ruleServ;
    }

    [HttpPost("ExecuteRule")]
    public async Task<IActionResult> ExecuteRule(int ruleId, [FromBody] Dictionary<string, object>? inputs = null)
    {
        return Ok(await _ruleServ.ExecuteRuleAsync(ruleId, inputs));
    }

    [HttpPost]
    public IActionResult TestScriptCode([FromBody] ScriptTestDto scriptTestDto)
    {
        var result = _ruleServ.TestScriptCode(scriptTestDto);
        return Ok(result);
    }
}