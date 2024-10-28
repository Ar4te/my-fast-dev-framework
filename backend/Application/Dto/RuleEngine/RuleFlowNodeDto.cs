namespace Application.Dto.RuleEngine;

public class RuleFlowNodeDto
{
    /// <summary>
    /// 引用的规则ID
    /// </summary>
    public int RuleId { get; set; }
    public int NodeType { get; set; } = 0;
    public int? NextNodeId { get; set; } = null;
    public int NodeIndex { get; set; }
    //public string ResultParametersDicJson { get; set; }
    public bool UseResultAsNextParameter { get; set; } = false;
}