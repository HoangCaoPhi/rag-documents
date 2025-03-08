using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Embeddings;
using RagDocuments.Models.Documents;

namespace RagDocuments.Infrastructure.VectorStores;

#pragma warning disable SKEXP0001 // For ITextEmbeddingGenerationService
public class DocumentVectorRepository(
    IVectorStore vectorStore,    
    ITextEmbeddingGenerationService textEmbeddingGenerationService) : IDocumentVectorRepository 
{
    private static VectorStoreRecordDefinition a = SetupVectorStoreRecordDefinition();
    private readonly IVectorStoreRecordCollection<ulong, Document> _collections = vectorStore.GetCollection<ulong, Document>("documents", a);

    public async Task<IVectorStoreRecordCollection<ulong, Document>> GetCollection()
    {
        await _collections.CreateCollectionIfNotExistsAsync();
        return _collections;
    }

    private static VectorStoreRecordDefinition SetupVectorStoreRecordDefinition()
    {
        var vectorStoreRecordDefinition = new VectorStoreRecordDefinition()
        {
            Properties =
            [
                new VectorStoreRecordKeyProperty("Id", typeof(Guid)),
                new VectorStoreRecordDataProperty("Name", typeof(string)) { IsFilterable = true }, 
                new VectorStoreRecordDataProperty("Chapter", typeof(string)),
                new VectorStoreRecordDataProperty("PageNumber", typeof(int)),
                new VectorStoreRecordDataProperty("Index", typeof(int)),
                new VectorStoreRecordDataProperty("Content", typeof(string)),
                new VectorStoreRecordVectorProperty(
                    "ContentEmbedding",
                    typeof(ReadOnlyMemory<float>)
                )
                {
                    Dimensions = 1024
                },
            ],
        };

        return vectorStoreRecordDefinition;
    }

    public async Task UpsertItems(Document[] items)
    {
        var collection = await GetCollection();

        var keys = new List<ulong>();
        await foreach (var key in collection.UpsertBatchAsync(items))
        {
            keys.Add(key);
        }
    }

    public async Task<VectorSearchResults<Document>> SearchDocument(string query)
    {
        var searchVector = await textEmbeddingGenerationService.GenerateEmbeddingAsync(query);

        var collection = await GetCollection();

        var searchResult = await collection.VectorizedSearchAsync(
            searchVector,
            new VectorSearchOptions { Top = 10, IncludeVectors = false }
        );

        return searchResult;
    }
}
