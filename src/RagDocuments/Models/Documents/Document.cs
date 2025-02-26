namespace RagDocuments.Models.Documents;

public class Document
{ 
    public Guid Id { get; private set; }
     
    public string Name { get; private set; }
     
    public string Chapter { get; private set; }

    public int PageNumber { get; private set; }
    public int Index { get; private set; } 
    public string Content { get; private set; }
     
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
