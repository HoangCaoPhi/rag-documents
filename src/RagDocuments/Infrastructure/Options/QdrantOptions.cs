namespace RagDocuments.Infrastructure.Options;

public class QdrantOptions
{
    public const string Key = "Qdrant";
    public required string Url { get; set; }

    public required string BaseUrl { get; set; }
}
