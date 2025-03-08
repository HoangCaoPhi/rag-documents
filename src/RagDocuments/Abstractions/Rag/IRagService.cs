namespace RagDocuments.Abstractions.Rag;

public interface IRagService
{
    Task<string> Search(string query);
}