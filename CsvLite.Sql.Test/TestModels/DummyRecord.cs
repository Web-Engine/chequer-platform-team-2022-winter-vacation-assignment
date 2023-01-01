using System.Collections.Generic;
using System.Linq;
using CsvLite.Models.Records;
using CsvLite.Models.Values;

namespace CsvLite.Sql.Test.TestModels;

public class DummyRecord : List<IValue>, IRecord
{
    public DummyRecord(params string[] values)
        : base(
            values.Select(PrimitiveValue.Parse)
        )
    {
    }
}