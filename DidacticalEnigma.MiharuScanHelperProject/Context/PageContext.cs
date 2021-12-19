using DidacticalEnigma.MiharuScanHelperProject.Json;
using DidacticalEnigma.Project;

namespace DidacticalEnigma.MiharuScanHelperProject.Context;

public class PageContext : ITranslationContext<TextEntryContext>
{
    private readonly ChapterContext parent;
    private readonly PageJson page;
    private readonly Action<TextEntryTranslation> saveTranslation;
    private readonly List<TextEntryContext> children;

    public PageContext(ChapterContext parent, PageJson page, Action<TextEntryTranslation> saveTranslation)
    {
        this.parent = parent;
        this.page = page;
        this.saveTranslation = saveTranslation;
        this.children = page.TextEntries
            .Select(textEntry => new TextEntryContext(this, textEntry, saveTranslation))
            .ToList();
    }

    IEnumerable<ITranslationContext> ITranslationContext.Children => Children;

    public IEnumerable<TextEntryContext> Children => children;

    public string ReadableIdentifier => $"/{page.Path}";

    public string ShortDescription => $"Page {page.Path}";
}