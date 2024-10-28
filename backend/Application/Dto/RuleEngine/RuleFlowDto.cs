namespace Application.Dto.RuleEngine;

public class RuleFlowDto
{
    public string FlowName { get; set; }
    public string Description { get; set; }
    public List<RuleFlowNodeDto> NodeDtos { get; set; } = new List<RuleFlowNodeDto>();
}