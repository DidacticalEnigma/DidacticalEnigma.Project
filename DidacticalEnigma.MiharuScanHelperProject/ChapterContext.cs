using DidacticalEnigma.MiharuScanHelperProject.Json;
using DidacticalEnigma.Project;

namespace DidacticalEnigma.MiharuScanHelperProject;

public class ChapterContext : ITranslationContext<PageContext>
{
    private readonly MiharuScan scan;
    private readonly Action<TextEntryTranslation> saveTranslation;
    private readonly List<PageContext> children;

    public ChapterContext(MiharuScan scan, Action<TextEntryTranslation> saveTranslation)
    {
        this.scan = scan;
        this.saveTranslation = saveTranslation;
        this.children = scan.Data.Pages
            .Select(page => new PageContext(this, page, saveTranslation))
            .ToList();
    }

    IEnumerable<ITranslationContext> ITranslationContext.Children => Children;

    public IEnumerable<PageContext> Children => children.AsReadOnly();

    public string ReadableIdentifier => "";

    public string ShortDescription => "";
}