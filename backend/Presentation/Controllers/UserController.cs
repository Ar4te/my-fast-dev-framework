using Domain.Entity.RuleEngine;
using Application.IService;
using Domain.IRepository;
using Microsoft.AspNetCore.Mvc;
using Application.IService.RuleEngine;
using Application.Dto.RuleEngine;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IBaseRepository<Rule> _ruleServ;
    private readonly IRuleFlowService _ruleFlowServ;

    public UserController(IUserService userService, IBaseRepository<Rule> ruleServ, IRuleFlowService ruleFlowServ)
    {
        _userService = userService;
        _ruleServ = ruleServ;
        _ruleFlowServ = ruleFlowServ;
    }

    [HttpGet(Name = "CreateUser")]
    public async Task<IActionResult> CreateUser()
    {
        return Ok(await _userService.CreateUser("SuperAdmin", "Aa123456!", "super-admin@example.com"));
    }

    [HttpGet(Name = "GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _userService.GetListAsync());
    }

    [HttpPost(Name = "CreateRule")]
    public async Task<IActionResult> CreateRule([FromBody] Rule ruleDto)
    {
        return Ok(await _ruleServ.CreateAsync(ruleDto));
    }

    [HttpPost(Name = "CreateRuleFlow")]
    public async Task<IActionResult> CreateRuleFlowNode([FromBody] RuleFlowDto ruleFlowDto)
    {
        return Ok(await _ruleFlowServ.SaveRuleFlow(ruleFlowDto));
    }

    [HttpGet(Name = "ExecuteFlow")]
    public async Task<IActionResult> ExecuteFlow(int flowId = 2)
    {
        return Ok(await _ruleFlowServ.ExecuteRuleFlow(flowId, new Dictionary<int, Dictionary<string, object>>()));
    }

    [HttpPost(Name = "AddNodeToFlow")]
    public async Task<IActionResult> AddNodeToFlow([FromBody] List<RuleFlowNodeDto> ruleFlowNodeDtos, int flowId = 2)
    {
        return Ok(await _ruleFlowServ.AddNodeToFlow(flowId, ruleFlowNodeDtos));
    }

    [HttpPost("AddNodeToFlow")]
    public async Task<IActionResult> AddNodeToFlow2([FromBody] RuleFlowNodeDto ruleFlowNodeDto, int flowId = 2, int preNodeId = 1)
    {
        return Ok(await _ruleFlowServ.AddNodeToFlow(flowId, preNodeId, ruleFlowNodeDto));
    }

    [HttpGet("GetFlow")]
    public async Task<IActionResult> GetFlow(int flowId = 2)
    {
        return Ok(await _ruleFlowServ.GetRuleFlow(flowId));
    }
}
