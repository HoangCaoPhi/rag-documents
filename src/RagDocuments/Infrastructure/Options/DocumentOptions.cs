namespace RagDocuments.Infrastructure.Options;

public class DocumentOptions
{
    public const string Key = "Document";
    public required ChunkOptions Chunk { get; set; }
}

public class ChunkOptions
{
    public int MaxTokensPerParagraph { get; set; } = 512;
    public int MaxTokensPerLine { get; set; } = 128;
    public int OverlapTokens { get; set; } = 50;
}