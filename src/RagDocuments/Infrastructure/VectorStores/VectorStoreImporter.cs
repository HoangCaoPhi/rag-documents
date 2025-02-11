using RagDocuments.Abstractions;

namespace RagDocuments.Infrastructure.VectorStores;

public class VectorStoreImporter(
    IDocumentConverter documentConverter
    ) : IVectorStoreImporter
{
    public async Task ImportFileAndCreateEmbeddings(string[] fileNames)
    {      
        var file = fileNames.ElementAt(0);

        await documentConverter.Convert(file);


    }
}
