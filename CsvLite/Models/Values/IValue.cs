using CsvLite.Models.Values.Primitives;

namespace CsvLite.Models.Values;

public interface IValue
{
    BooleanValue AsBoolean();

    IntegerValue AsInteger();

    StringValue AsString();
    
    PrimitiveValue AsPrimitive();

    TupleValue AsTuple();
}