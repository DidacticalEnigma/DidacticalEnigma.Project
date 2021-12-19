using DidacticalEnigma.MiharuScanHelperProject.Json;
using DidacticalEnigma.Project;
using Newtonsoft.Json;
using Optional;
using Utility.Utils;

namespace DidacticalEnigma.MiharuScanHelperProject.Context;

public class TextEntryTranslation : Translation
{
    private readonly TextEntryJson json;
    public override Option<Guid> Guid => json.Uuid.Some();
    public override string OriginalText => json.Source;
    public override string TranslatedText => json.Target;
    public override IEnumerable<GlossNote> Glosses => Enumerable.Empty<GlossNote>();
    public override IEnumerable<TranslatorNote> Notes => json.Notes.Select(n => new TranslatorNote("", n.Content));
    public override IEnumerable<TranslatedText> AlternativeTranslations => Enumerable.Empty<TranslatedText>();

    internal TextEntryTranslation(TextEntryJson json)
    {
        this.json = json;
    }

    public override Translation With(
        string? originalText = null,
        string? translatedText = null,
        IEnumerable<GlossNote>? glosses = null,
        IEnumerable<TranslatorNote>? notes = null,
        IEnumerable<TranslatedText>? alternativeTranslations = null)
    {
        var clone = GetJson();
        clone.Source = originalText ?? clone.Source;
        clone.Target = translatedText ?? clone.Target;
        if(notes != null)
        {
            clone.Notes = clone.Notes
                .Select(n => n.Uuid)
                .Concat(EnumerableExt.Repeat("infinity").Select(_ => System.Guid.NewGuid()))
                .Zip(
                    notes,
                    (id, note) => new NoteJson()
                    {
                        Uuid = id,
                        Content = note.Text
                    })
                .ToList();
        }

        return new TextEntryTranslation(clone);
    }

    public TextEntryJson GetJson()
    {
        return JsonConvert.DeserializeObject<TextEntryJson>(JsonConvert.SerializeObject(json))!;
    }
}