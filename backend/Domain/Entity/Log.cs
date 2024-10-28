using Common.CustomAttribute;
using Domain.Entity.Base;
using SqlSugar;

namespace Domain.Entity;

[SugarTable("Log")]
[DatabaseConfigId("LOG")]
public class Log : BaseEntity
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public string DatetimeStr { get; set; }

    [SugarColumn(IsNullable = true)]
    public string Message { get; set; }
}