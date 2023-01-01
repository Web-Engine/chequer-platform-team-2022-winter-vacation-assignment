namespace CsvLite.Models.Domains;

public class TupleDomain : IDomain
{
    public IEnumerable<IDomain> Domains { get; }

    public TupleDomain(IEnumerable<IDomain> domains)
    {
        Domains = domains;
    }
}