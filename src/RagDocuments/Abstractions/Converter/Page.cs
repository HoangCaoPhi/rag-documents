namespace RagDocuments.Abstractions.Converter;

public class Page
{
    public string TextContent { get; private set; }
    public int PageNumber { get; private set; }

    public static Page Create(string textContent, int pageNumber)
    {
        return new Page
        {
            TextContent = textContent,
            PageNumber = pageNumber
        };
    }
}
