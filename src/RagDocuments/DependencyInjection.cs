using Microsoft.SemanticKernel;
using Qdrant.Client;
using RagDocuments.Abstractions.Converter;
using RagDocuments.Abstractions.VectorStores;
using RagDocuments.Infrastructure.Converter;
using RagDocuments.Infrastructure.Options;
using RagDocuments.Infrastructure.VectorStores;
using RagDocuments.Models.Documents;

namespace RagDocuments;

public static class DependencyInjection
{
    public static void ConfigureSettings(this IServiceCollection services)
    {
        services.AddOptions<OllamaOptions>()
                .BindConfiguration(OllamaOptions.Key);

        services.AddOptions<QdrantOptions>()
                .BindConfiguration(QdrantOptions.Key);

        services.AddOptions<DocumentOptions>()
                .BindConfiguration(DocumentOptions.Key);
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IDocumentConverter, PdfConverter>();
        services.AddScoped<IVectorStoreImporter, VectorStoreImporter>();
        services.AddScoped<IDocumentVector, DocumentVector>();
    }

    public static void ConfigureQdrant(this IServiceCollection services,
        IConfiguration configuration)
    {
        QdrantOptions qdrantOptions = configuration.Get<QdrantOptions>()!;
        services.AddSingleton(sp => new QdrantClient(qdrantOptions.BaseUrl));
        services.AddQdrantVectorStore();
    }
}
