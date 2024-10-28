namespace desktop.Exntesions;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class DatabaseConfigIdAttribute : Attribute
{
    public string ConfigId { get; set; }
    public DatabaseConfigIdAttribute(string configId)
    {
        ConfigId = configId;
    }
}