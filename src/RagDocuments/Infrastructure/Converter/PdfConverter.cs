using RagDocuments.Abstractions;
using RagDocuments.Models;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig.Outline;

namespace RagDocuments.Infrastructure.Converter;

public class PdfConverter : IDocumentConverter
{
    public async Task<Document> Convert(string fileName)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", fileName);
        using var pdfDocument = PdfDocument.Open(filePath);

        var pages = await GetContentAsync(pdfDocument);
        var chapters = await GetChapters(pdfDocument);

        var chapterCollection = new ChapterCollection(chapters);

        var title = pdfDocument.Information.Title ?? string.Empty;
        var authors = pdfDocument.Information.Author ?? string.Empty;

        return new Document(
            title,
            pages,
            chapterCollection,
            authors,
            Path.GetFileName(fileName));
    }


    private static Task<List<Page>> GetContentAsync(PdfDocument pdfDocument)
    {
        var pages = new List<Page>();
         
        foreach (var page in pdfDocument.GetPages().Where(x => x is not null))
        {
            var pageContent = ContentOrderTextExtractor.GetText(page) ?? string.Empty;
            pages.Add(Page.Create(pageContent, page.Number));
        }

        return Task.FromResult(pages);
    }

    private static Task<List<Chapter>> GetChapters(PdfDocument pdfDocument)
    {
        var result = new List<Chapter>();

        if (pdfDocument.TryGetBookmarks(out var bookmarks))
        {
            var bookmarkNodes = bookmarks.GetNodes();
            foreach (BookmarkNode node in bookmarkNodes)
            {
                if (node is DocumentBookmarkNode docmark)
                {                    
                    result.Add(Chapter.Create(
                        docmark.Title,
                        docmark.Level,
                        docmark.PageNumber
                    ));
                }
            }
        }

        return Task.FromResult(result);
    }
}
