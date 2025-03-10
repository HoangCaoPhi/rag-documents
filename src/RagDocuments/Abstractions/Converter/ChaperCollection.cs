﻿namespace RagDocuments.Abstractions.Converter;

public sealed class ChapterCollection(IEnumerable<Chapter> chapters)
{
    private readonly List<Chapter> _chapters = chapters.ToList();
    private readonly Dictionary<int, string> _chapterPathCache = [];

    /// <summary>
    /// Will output full path for a chapter, which could mean that a chapter with child chapters
    /// will result in something like: "chapter 1 > child 0 > child 1".
    /// 
    /// Will take the nearest chapter to a page number. If a page number is 10 and that page is between chapter 1 and child 1, then
    /// only "chapter 1" is returned. 
    /// </summary>
    /// <param name="pageNumber">A page number that should range between the books first and last page</param>
    /// <returns>"chapter 1 > child 0 > child 1"</returns>
    public string ByPageNumber(int pageNumber)
    {
        if (_chapterPathCache.TryGetValue(pageNumber, out var cachedPath))
        {
            return cachedPath;
        }

        var path = new List<string>();

        Chapter? lastValidChapter = null;

        foreach (var chapter in _chapters.OrderBy(c => c.PageNumber))
        {
            // no reason to go beyond chapters with higher page numbers than we are looking for
            if (chapter.PageNumber > pageNumber)
                break;

            if (lastValidChapter == null || chapter.Level > lastValidChapter.Level)
            {
                path.Add(chapter.Title);
                lastValidChapter = chapter;
            }
            else if (chapter.Level == lastValidChapter.Level)
            {
                // replace the previous chapter at the same level
                path[^1] = chapter.Title;
                lastValidChapter = chapter;
            }
        }

        var resultPath = string.Join(" > ", path);

        _chapterPathCache[pageNumber] = resultPath;

        return resultPath;
    }
}
