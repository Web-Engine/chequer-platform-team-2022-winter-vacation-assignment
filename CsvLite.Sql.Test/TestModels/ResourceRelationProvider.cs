using System;
using System.IO;
using CsvLite.IO.Csv;
using CsvLite.Models.Identifiers;
using CsvLite.Models.Relations;

namespace CsvLite.Sql.Test.TestModels;

public sealed class ResourceRelationProvider : IPhysicalRelationProvider, IDisposable
{
    private const string ResourceDirectory = "Resources/Relations";
    private readonly CsvRelationProvider _provider = new();

    private readonly string _tempDirectory;

    public ResourceRelationProvider()
    {
        _tempDirectory = Path.Join(Path.GetTempPath(), "CsvLite.Sql.Test", Path.GetRandomFileName());

        if (Directory.Exists(_tempDirectory))
            Directory.Delete(_tempDirectory, true);
    
        Directory.CreateDirectory(_tempDirectory);

        foreach (var file in Directory.GetFiles(ResourceDirectory))
        {
            var dest = Path.Join(_tempDirectory, Path.GetFileName(file));
            File.Copy(file, dest);
        }
    }

    public string GetRelationFilePath(string relationName)
    {
        var filePath = Path.Join(_tempDirectory, relationName);

        return filePath;
    }

    public IPhysicalRelation GetRelation(Identifier identifier)
    {
        var filePath = GetRelationFilePath(identifier.Value);

        return _provider.GetRelation(new Identifier(filePath));
    }

    public void Dispose()
    {
        Directory.Delete(_tempDirectory, true);
    }
}