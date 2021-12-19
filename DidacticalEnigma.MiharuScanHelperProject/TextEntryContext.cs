using DidacticalEnigma.MiharuScanHelperProject.Json;
using DidacticalEnigma.Project;

namespace DidacticalEnigma.MiharuScanHelperProject;

public class TextEntryContext : IEditableTranslation
{
    private readonly PageContext parent;
    private readonly Action<TextEntryTranslation> saveTranslation;
    private TextEntryTranslation translation;

    public TextEntryContext(PageContext parent, TextEntryJson textEntry, Action<TextEntryTranslation> saveTranslation)
    {
        this.parent = parent;
        this.saveTranslation = saveTranslation;
        this.translation = new TextEntryTranslation(textEntry);
    }

    public IEnumerable<ITranslationContext> Children => Enumerable.Empty<ITranslationContext>();

    public string ReadableIdentifier => $"{parent.ReadableIdentifier}/{this.translation.Guid}";

    public string ShortDescription => $"{parent.ShortDescription}, Entry {this.translation.Guid}";

    public Translation Translation => this.translation;
    
    public ModificationResult Modify(Translation translation)
    {
        var t = (TextEntryTranslation)translation;
        this.saveTranslation(t);
        this.translation = t;
        return ModificationResult.WithSuccess(t);
    }
}