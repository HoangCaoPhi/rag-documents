using Microsoft.Extensions.VectorData;

namespace RagDocuments.Models.Documents;

public class Document
{
    [VectorStoreRecordKey]
    public Guid Id { get; private set; }

    [VectorStoreRecordData(IsFilterable = true)]
    public string Name { get; private set; }

    [VectorStoreRecordData(IsFilterable = true)]
    public string Chapter { get; private set; }

    public int PageNumber { get; private set; }
    public int Index { get; private set; }

    [VectorStoreRecordData(IsFullTextSearchable = true)]
    public string Content { get; private set; }

    [VectorStoreRecordVector(Dimensions: 768, DistanceFunction.CosineDistance, IndexKind.Hnsw)]
    public ReadOnlyMemory<float>? ContentEmbedding { get; private set; }

    public static Document Create(
        Guid id,
        string name,
        string chapter,
        int pageNumber,
        int index,
        string content,
        ReadOnlyMemory<float>? contentEmbedding)
    {
        return new Document()
        {
            Id = id,
            Name = name,
            Chapter = chapter,
            PageNumber = pageNumber,
            Index = index,
            Content = content,
            ContentEmbedding = contentEmbedding
        };
    }
}
