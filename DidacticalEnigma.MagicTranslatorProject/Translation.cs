﻿using System;
using System.Collections.Generic;
using System.Linq;
using DidacticalEnigma.Project;
using MagicTranslatorProject.Json;
using Newtonsoft.Json;
using Optional;

namespace MagicTranslatorProject
{
    internal class Translation : DidacticalEnigma.Project.Translation
    {
        public readonly CaptureJson Capture;

        public long Id => Capture.Id;

        public string Character => Capture.Character.ToString();

        public override Option<Guid> Guid { get; }

        public override string OriginalText => Capture.Text;

        public override string TranslatedText => Capture.Translation;

        public override IEnumerable<GlossNote> Glosses => Capture.Notes.Select(c => new GlossNote(c[0], c[1]));

        public override IEnumerable<TranslatorNote> Notes => Enumerable.Empty<TranslatorNote>();

        public override IEnumerable<TranslatedText> AlternativeTranslations => Enumerable.Empty<TranslatedText>();

        internal Translation(
            CaptureJson capture,
            Option<Guid> guid = default)
        {
            Guid = guid;
            this.Capture = capture ?? throw new ArgumentNullException(nameof(capture));
        }

        public override DidacticalEnigma.Project.Translation With(
            string originalText = null,
            string translatedText = null,
            IEnumerable<GlossNote> glosses = null,
            IEnumerable<TranslatorNote> notes = null,
            IEnumerable<TranslatedText> alternativeTranslations = null)
        {
            return With(
                JsonConvert.DeserializeObject<CaptureJson>(ToJson()),
                originalText,
                translatedText,
                glosses,
                notes,
                alternativeTranslations);
        }

        internal Translation With(
            CaptureJson json,
            string originalText = null,
            string translatedText = null,
            IEnumerable<GlossNote> glosses = null,
            IEnumerable<TranslatorNote> notes = null,
            IEnumerable<TranslatedText> alternativeTranslations = null)
        {
            var other = new Translation(json, this.Guid);
            other.Capture.Text = originalText ?? this.OriginalText;
            other.Capture.Translation = translatedText ?? this.TranslatedText;
            other.Capture.Notes = (glosses ?? this.Glosses).Select(g => new[] { g.Foreign, g.Text }).ToList();
            return other;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this.Capture);
        }
    }
}