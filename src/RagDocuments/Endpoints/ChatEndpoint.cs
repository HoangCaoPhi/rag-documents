using Microsoft.AspNetCore.Mvc;
using RagDocuments.Abstractions.Rag;
using RagDocuments.Abstractions.VectorStores;
using RagDocuments.Models.Documents;

namespace RagDocuments.Endpoints;

public record SearchRequest(string Query);

public static class ChatEndpoint
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/import-document", async (IVectorStoreImporter importer) =>
        { 
            await importer.ImportFileAndCreateEmbeddings(["SampleFile.pdf"]);
        });

        app.MapPost("/search", async (
            [FromBody] SearchRequest request,
            IChatService chatService,
            IDocumentVectorRepository documentVectorRepository) =>
        {
            var searchResult = await documentVectorRepository.SearchDocument(request.Query);
            var searchResults = searchResult.Results.ToBlockingEnumerable().Select(x => x).ToList();

            var textChunks = searchResults.Select(x => x.Record.Content).ToArray();
            var books = searchResults.Select(x => x.Record.Name).Distinct().ToArray();
 
            var response = await chatService.AskRaggedQuestion(request.Query, [.. textChunks]);

            return response;
        });
    }
}
