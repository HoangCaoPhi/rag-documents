using RagDocuments.Models;

namespace RagDocuments.Abstractions;

public interface IDocumentConverter
{
    Task<Document> Convert(string fileName);
}
