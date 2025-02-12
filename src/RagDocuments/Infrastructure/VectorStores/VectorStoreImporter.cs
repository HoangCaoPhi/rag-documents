using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Text;
using RagDocuments.Abstractions.Converter;
using RagDocuments.Abstractions.VectorStores;
using RagDocuments.Infrastructure.Options;

namespace RagDocuments.Infrastructure.VectorStores;

#pragma warning disable SKEXP0050 // For TextChunker
#pragma warning disable SKEXP0001 // For ITextEmbeddingGenerationService

public class VectorStoreImporter(
    IDocumentConverter documentConverter,
    ITextEmbeddingGenerationService textEmbeddingGenerationService,
    IOptions<DocumentOptions> documentOptions
    ) : IVectorStoreImporter
{
    private readonly DocumentOptions _documentOptions = documentOptions.Value;

    public async Task ImportFileAndCreateEmbeddings(string[] fileNames)
    {      
        var file = fileNames.ElementAt(0);

        var documentInfo = await documentConverter.Convert(file);

        var pages = documentInfo.Pages;

        List<Document> documents = [];

        int index = 0;
        foreach (var page in pages)
        {
            var paragraphs = TextChunker.SplitPlainTextParagraphs(
                TextChunker.SplitPlainTextLines(page.TextContent, _documentOptions.Chunk.MaxTokensPerLine),
                _documentOptions.Chunk.MaxTokensPerLine,
                _documentOptions.Chunk.OverlapTokens
            );

            paragraphs = paragraphs.Select(x => x.Replace("-\n", " ")).ToList();

            var contentEmbeddings = textEmbeddingGenerationService.GenerateEmbeddingsAsync(
                paragraphs
            );


        }
    }
}
