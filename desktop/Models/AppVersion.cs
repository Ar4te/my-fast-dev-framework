using desktop.Exntesions;
using desktop.Models.Base;
using SqlSugar;

namespace desktop.Models;

[SugarTable("AppVersion")]
[DatabaseConfigId("Test")]
public class AppVersion : BaseEntity
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }

    public string Version { get; set; }
    public DateTime ReleaseDate { get; set; }
    public DateTime UpdateTime { get; set; }
}

public class ServerVersionDto
{
    public string Version { get; set; }
    public DateTime ReleaseDate { get; set; }
}