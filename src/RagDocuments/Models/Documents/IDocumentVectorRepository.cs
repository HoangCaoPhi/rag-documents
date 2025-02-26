using Microsoft.Extensions.VectorData;

namespace RagDocuments.Models.Documents;

public interface IDocumentVectorRepository
{
    Task UpsertItems(Document[] items);
    Task<IVectorStoreRecordCollection<ulong, Document>> GetCollection();
}
