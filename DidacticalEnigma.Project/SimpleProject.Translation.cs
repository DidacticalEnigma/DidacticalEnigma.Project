using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Optional;

namespace DidacticalEnigma.Project
{
    public partial class SimpleProject
    {
        public class Translation : Project.Translation
        {
            public override Option<Guid> Guid { get; }
            public override string OriginalText { get; }
            public override string TranslatedText { get; }
            public override IEnumerable<GlossNote> Glosses { get; }
            public override IEnumerable<TranslatorNote> Notes { get; }
            public override IEnumerable<TranslatedText> AlternativeTranslations { get; }

            public override Project.Translation With(
                string originalText = null,
                string translatedText = null,
                IEnumerable<GlossNote> glosses = null,
                IEnumerable<TranslatorNote> notes = null, IEnumerable<TranslatedText> alternativeTranslations = null)
            {
                return new Translation(
                    this.Guid,
                    originalText ?? this.OriginalText,
                    translatedText ?? this.TranslatedText,
                    glosses ?? this.Glosses,
                    notes ?? this.Notes,
                    alternativeTranslations ?? this.AlternativeTranslations);
            }

            [UsedImplicitly]
            public Translation(
                Option<Guid> guid,
                string originalText,
                string translatedText,
                IEnumerable<GlossNote> glosses,
                IEnumerable<TranslatorNote> notes,
                IEnumerable<TranslatedText> alternativeTranslations)
            {
                Guid = guid;
                OriginalText = originalText ?? throw new ArgumentNullException(nameof(originalText));
                TranslatedText = translatedText ?? throw new ArgumentNullException(nameof(translatedText));
                Glosses = glosses ?? throw new ArgumentNullException(nameof(glosses));
                Notes = notes ?? throw new ArgumentNullException(nameof(notes));
                AlternativeTranslations = alternativeTranslations ?? throw new ArgumentNullException(nameof(alternativeTranslations));
            }

            public Translation(
                Guid guid)
            {
                Guid = guid.Some();
                OriginalText = "";
                TranslatedText = "";
                Glosses = Enumerable.Empty<GlossNote>();
                Notes = Enumerable.Empty<TranslatorNote>();
                AlternativeTranslations = Enumerable.Empty<TranslatedText>();
            }
        }
    }
}