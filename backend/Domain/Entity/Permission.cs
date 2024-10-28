using Common.CustomAttribute;
using Domain.Entity.Base;
using SqlSugar;

namespace Domain.Entity;

[SugarTable("Permission")]
[DatabaseConfigId("RBAC")]
public class Permission : BaseEntity
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    public string PermissionName { get; set; }
    public string Description { get; set; }
}