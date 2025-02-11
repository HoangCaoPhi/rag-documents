using RagDocuments.Abstractions;
using RagDocuments.Infrastructure.Converter;
using RagDocuments.Infrastructure.Options;
using RagDocuments.Infrastructure.VectorStores;

namespace RagDocuments;

public static class DependencyInjection
{
    public static void ConfigureSettings(this IServiceCollection services)
    {
        services.AddOptions<OllamaOptions>()
                .BindConfiguration(OllamaOptions.Key);

        services.AddOptions<QdrantOptions>()
                .BindConfiguration(QdrantOptions.Key);
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IDocumentConverter, PdfConverter>();
        services.AddScoped<IVectorStoreImporter, VectorStoreImporter>();
    }
}
