namespace RagDocuments.Abstractions.Rag;

public interface IChatService
{
    Task<string> AskRaggedQuestion(string question, string[] contexts);
}