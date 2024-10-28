using Common.CustomAttribute;
using Domain.Entity.Base;
using SqlSugar;

namespace Domain.Entity;

[SugarTable("UserRole")]
[DatabaseConfigId("RBAC")]
public class UserRole : BaseEntity
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
}