namespace RagDocuments.Abstractions.VectorStores;

public interface IVectorStoreImporter
{
    Task ImportFileAndCreateEmbeddings(string[] fileNames);
}
