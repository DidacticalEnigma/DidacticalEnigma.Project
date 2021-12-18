using System;
using System.Collections.Generic;
using System.Linq;
using DidacticalEnigma.Project;
using MagicTranslatorProject.Json;

namespace MagicTranslatorProject.Context
{
    public class CaptureContext : IReadOnlyTranslation
    {
        private readonly Func<CaptureJson, ModificationResult> saveAction;
        private readonly CaptureId captureId;
        private readonly PageContext pageContext;
        private readonly CaptureJson json;

        internal CaptureContext(
            PageContext pageContext,
            CaptureJson json,
            Translation translation,
            CaptureId captureId)
        {
            this.pageContext = pageContext;
            this.json = json;
            this.captureId = captureId;
            this.Translation = translation;
        }

        public CharacterType Character => json.Character;

        public DidacticalEnigma.Project.Translation Translation { get; private set; }

        public IEnumerable<ITranslationContext> Children => Enumerable.Empty<ITranslationContext>();

        public string ShortDescription => $"{pageContext.ShortDescription}. {json.Character}";

        public string ReadableIdentifier => captureId.ToString();
    }
}