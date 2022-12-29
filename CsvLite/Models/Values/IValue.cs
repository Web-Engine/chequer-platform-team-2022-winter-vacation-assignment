namespace CsvLite.Models.Values;

public interface IValue
{
    PrimitiveValue AsPrimitive();

    TupleValue AsTuple();
}
