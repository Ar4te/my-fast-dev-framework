using desktop.Exntesions;
using desktop.Models.Base;
using SqlSugar;

namespace desktop.Models;

[SugarTable("Contact")]
[DatabaseConfigId("Test")]
public class Contact : BaseEntity
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}