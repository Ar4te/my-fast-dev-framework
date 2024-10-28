using Common.CustomAttribute;
using Domain.Entity.Base;
using SqlSugar;

namespace Domain.Entity;

[SugarTable("RolePermission")]
[DatabaseConfigId("RBAC")]
public class RolePermission : BaseEntity
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
}