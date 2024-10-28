using Common.CustomAttribute;
using Domain.Entity.Base;
using SqlSugar;

namespace Domain.Entity;

[SugarTable("User")]
[DatabaseConfigId("RBAC")]
public class User : BaseEntity
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}