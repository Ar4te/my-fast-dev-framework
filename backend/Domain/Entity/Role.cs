using Common.CustomAttribute;
using Domain.Entity.Base;
using SqlSugar;

namespace Domain.Entity;

[SugarTable("Role")]
[DatabaseConfigId("RBAC")]
public class Role : BaseEntity
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
}