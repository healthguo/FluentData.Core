namespace FluentData.Core
{
    /// <summary>
    /// Indicates that a property should be ignored during auto-mapping operations.
    /// Apply this attribute to entity properties that should not be mapped to database columns.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnorePropertyAttribute : Attribute
    {
    }
}
