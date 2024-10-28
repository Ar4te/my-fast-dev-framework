namespace Application.Dto.RuleEngine;

public class RuleExecuteResultDto
{
    public RuleExecuteResultDto(int ruleId, string ruleName, string ruleScript, object? scriptOutput)
    {
        RuleId = ruleId;
        RuleName = ruleName;
        RuleScript = ruleScript;
        ScriptOutput = scriptOutput;
    }

    public int RuleId { get; set; }
    public string RuleName { get; set; }
    public string RuleScript { get; set; }
    public object? ScriptOutput { get; set; }
    public string ScriptOutputTypeName => ScriptOutput?.GetType().Name ?? typeof(object).Name;
}

public class RuleFlowExecuteResultDto
{
    public RuleFlowExecuteResultDto(int flowId, string flowName)
    {
        FlowId = flowId;
        FlowName = flowName;
        RuleExecuteResult = new List<RuleFlowNodeExecuteResultDto>();
    }

    public int FlowId { get; set; }
    public string FlowName { get; set; }
    public List<RuleFlowNodeExecuteResultDto> RuleExecuteResult { get; set; }
}

public class RuleFlowNodeExecuteResultDto : RuleExecuteResultDto
{
    public RuleFlowNodeExecuteResultDto(int flowNodeId, int flowNodeIndex, int ruleId, string ruleName, string ruleScript, object? scriptOutput) :
        base(ruleId, ruleName, ruleScript, scriptOutput)
    {
        FlowNodeId = flowNodeId;
        FlowNodeIndex = flowNodeIndex;
    }

    public int FlowNodeId { get; set; }
    public int FlowNodeIndex { get; set; }
}