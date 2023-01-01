using CsvLite.Models.Domains;

namespace CsvLite;

public class DomainDetector
{
    private bool _canBoolean = true;
    private bool _canInteger = true;

    public IPrimitiveDomain Domain
    {
        get
        {
            if (_canBoolean) return new BooleanDomain();
            if (_canInteger) return new IntegerDomain();

            return new StringDomain();
        }
    }

    public void Detect(string? data)
    {
        if (string.IsNullOrEmpty(data))
            return;
        
        if (_canBoolean)
        {
            switch (data.ToUpperInvariant())
            {
                case "TRUE":
                case "FALSE":
                    return;
                    
                default:
                    _canBoolean = false;
                    break;
            }
        }

        if (_canInteger)
        {
            _canInteger = int.TryParse(data, out _);
        }
    }
}