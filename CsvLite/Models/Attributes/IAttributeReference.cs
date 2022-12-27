namespace CsvLite.Models.Attributes;

public interface IAttributeReference
{
    bool IsReferencing(IAttribute attribute);
}