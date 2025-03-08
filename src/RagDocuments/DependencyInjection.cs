using Microsoft.SemanticKernel;
using Qdrant.Client;
using RagDocuments.Abstractions.Converter;
using RagDocuments.Abstractions.Rag;
using RagDocuments.Abstractions.VectorStores;
using RagDocuments.Infrastructure.Converter;
using RagDocuments.Infrastructure.Options;
using RagDocuments.Infrastructure.Rag;
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
        services.AddScoped<IDocumentVectorRepository, DocumentVectorRepository>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IRagService, RagService>();
    }

    public static void ConfigureQdrant(this IServiceCollection services,
        IConfiguration configuration)
    {
        QdrantOptions qdrantOptions = configuration.Get<QdrantOptions>()!;
        services.AddSingleton(sp => new QdrantClient("qdrant"));
        services.AddQdrantVectorStore(); 
    }

    public static void ConfigureKernel(this IServiceCollection services)
    {
        #pragma warning disable SKEXP0070
        services.AddOllamaTextEmbeddingGeneration(
            modelId: "mxbai-embed-large",
            endpoint: new Uri("http://ollama:11434")
        );

        services.AddOllamaChatCompletion(
            modelId: "qwen2:0.5b",
            endpoint: new Uri("http://ollama:11434")
        );

        services.AddTransient((serviceProvider) => {
            return new Kernel(serviceProvider);
        });
    }
}
