namespace RagDocuments.Abstractions;

public interface IVectorStoreImporter
{
    Task ImportFileAndCreateEmbeddings(string[] fileNames);
}
