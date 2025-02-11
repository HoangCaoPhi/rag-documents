namespace RagDocuments.Models;

public record Document(
    string Title,
    List<Page> Pages,
    ChapterCollection ChapterPath,
    string Authors,
    string Filename
);