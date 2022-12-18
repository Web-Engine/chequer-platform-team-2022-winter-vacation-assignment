using CsvLite.Models.Schemas;
using CsvLite.Sql.Projections;
using CsvLite.Sql.Results;

namespace CsvLite.Sql.Actions;

public class SelectAction : IRelationAction
{
    private readonly IList<IProjection> _projections;
    
    public SelectAction(IList<IProjection> projections)
    {
        _projections = projections;
    }
    
    public IRelationResult Execute(ISchema schema)
    {
    }
}
