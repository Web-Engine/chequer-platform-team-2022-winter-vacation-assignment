using System.Collections;

namespace CsvLite.Models.Values;

public sealed class AggregateValue : IEnumerable<IValue>, IValue
{
    public IEnumerable<IValue> Values { get; }

    private readonly bool _canBePrimitive;

    public AggregateValue(IEnumerable<IValue> values, bool canBePrimitive)
    {
        Values = values;
        _canBePrimitive = canBePrimitive;
    }

    public PrimitiveValue AsPrimitive()
    {
        if (!_canBePrimitive)
            throw new InvalidOperationException($"Cannot convert {GetType()} to PrimitiveValue");

        return Values.First().AsPrimitive();
    }

    public TupleValue AsTuple()
    {
        return AsPrimitive().AsTuple();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<IValue> GetEnumerator()
    {
        return Values.GetEnumerator();
    }
}