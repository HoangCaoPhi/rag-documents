using Microsoft.Extensions.VectorData;

namespace RagDocuments.Models.Documents;

public interface IDocumentVectorRepository
{
    Task UpsertItems(Document[] items);
    Task<IVectorStoreRecordCollection<ulong, Document>> GetCollection();
    Task<VectorSearchResults<Document>> SearchDocument(string query);
}
