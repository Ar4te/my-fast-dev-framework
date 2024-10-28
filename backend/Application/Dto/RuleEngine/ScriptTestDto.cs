using Domain.Enum.RuleEngine;

namespace Application.Dto.RuleEngine;

public class ScriptTestDto
{
    public ScriptLanguageType ScriptLanguageType { get; set; }
    public string ScriptCode { get; set; }
    public Dictionary<string, object>? Inputs { get; set; }
}