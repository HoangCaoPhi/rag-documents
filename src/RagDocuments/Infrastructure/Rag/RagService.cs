using RagDocuments.Abstractions.Rag;
using RagDocuments.Models.Documents;

namespace RagDocuments.Infrastructure.Rag;

public class RagService(
    IChatService chatService,
    IDocumentVectorRepository documentVectorRepository) : IRagService
{
    public async Task<string> Search(string query)
    {

        var searchResult = await documentVectorRepository.SearchDocument(query);
        var searchResults = searchResult
            .Results.ToBlockingEnumerable()
            .Select(x => x)
            .ToList();

        var matchingRecords = searchResults.Select(x => x.Record).ToArray();
        var contexts = new List<string>();
        foreach (var record in matchingRecords)
        {
            var text =
                $"{record.Content}{Environment.NewLine}(source: {record.Name} - {record.Chapter})";

            contexts.Add(text.Trim());
        }

        var response = await chatService.AskRaggedQuestion(query, [.. contexts]);
        return response;
    }
}