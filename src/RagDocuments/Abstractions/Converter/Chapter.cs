namespace RagDocuments.Abstractions.Converter;

public class Chapter
{
    public string Title { get; set; }
    public int Level { get; set; }
    public int PageNumber { get; set; }

    public static Chapter Create(string title, int level, int pageNumber)
    {
        return new Chapter
        {
            Title = title,
            Level = level,
            PageNumber = pageNumber
        };
    }
}
