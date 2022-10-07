namespace DocumentDataAPI.Models;

public class SourceModel
{
    public SourceModel()
    {
    }

    public SourceModel(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }
    public string? Name { get; init; }
}