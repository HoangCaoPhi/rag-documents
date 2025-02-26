using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Text;
using RagDocuments.Abstractions.Converter;
using RagDocuments.Abstractions.VectorStores;
using RagDocuments.Infrastructure.Options;
using RagDocuments.Models.Documents;

namespace RagDocuments.Infrastructure.VectorStores;

#pragma warning disable SKEXP0050 // For TextChunker
#pragma warning disable SKEXP0001 // For ITextEmbeddingGenerationService

public class VectorStoreImporter(
    IDocumentConverter documentConverter,
    ITextEmbeddingGenerationService textEmbeddingGenerationService,
    IDocumentVectorRepository documentVectorRepository,
    IOptions<DocumentOptions> documentOptions
    ) : IVectorStoreImporter
{
    private readonly DocumentOptions _documentOptions = documentOptions.Value;

    public async Task ImportFileAndCreateEmbeddings(string[] fileNames)
    {      
        var file = fileNames.ElementAt(0);

        var documentInfo = await documentConverter.Convert(file);

        var pages = documentInfo.Pages;

        List<Models.Documents.Document> documents = [];

        int bookIndex = 0;
        foreach (var page in pages)
        {
            var paragraphs = TextChunker.SplitPlainTextParagraphs(
                TextChunker.SplitPlainTextLines(page.TextContent, _documentOptions.Chunk.MaxTokensPerLine),
                _documentOptions.Chunk.MaxTokensPerLine,
                _documentOptions.Chunk.OverlapTokens
            );

            paragraphs = paragraphs.Select(x => x.Replace("-\n", " ")).ToList();

            var contentEmbeddings = await textEmbeddingGenerationService.GenerateEmbeddingsAsync(
                paragraphs
            );

            foreach (var (index, paragraph) in paragraphs.Select((x, index) => (index, x)))
            {
                var embedding = contentEmbeddings[index];

                var document = Models.Documents.Document.Create(
                    Guid.CreateVersion7(),
                    documentInfo.Title,
                    documentInfo.ChapterPath.ByPageNumber(page.PageNumber),
                    page.PageNumber,
                    bookIndex,
                    paragraph,
                    embedding
                );

                documents.Add(document);
                bookIndex++;
            }
        }


        await documentVectorRepository.UpsertItems([.. documents]);

    }
}
