namespace RagDocuments.Abstractions.Converter;

public interface IDocumentConverter
{
    Task<Document> Convert(string fileName);
}
