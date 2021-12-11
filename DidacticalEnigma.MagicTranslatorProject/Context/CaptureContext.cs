using System;
using System.Collections.Generic;
using System.Linq;
using DidacticalEnigma.Project;
using MagicTranslatorProject.Json;

namespace MagicTranslatorProject.Context
{
    public class CaptureContext : IEditableTranslation
    {
        private readonly Func<CaptureJson, ModificationResult> saveAction;
        private readonly CaptureId captureId;
        private readonly PageContext pageContext;
        private readonly CaptureJson json;

        internal CaptureContext(
            PageContext pageContext,
            CaptureJson json,
            Func<CaptureJson, ModificationResult> saveAction,
            Translation translation,
            CaptureId captureId)
        {
            this.pageContext = pageContext;
            this.json = json;
            this.saveAction = saveAction;
            this.captureId = captureId;
            this.Translation = translation;
        }

        public CharacterType Character => json.Character;

        public DidacticalEnigma.Project.Translation Translation { get; private set; }

        public ModificationResult Modify(DidacticalEnigma.Project.Translation translation)
        {
            var input = ((Translation) translation);
            var r = saveAction(input.With(json).Capture);
            if(r.IsSuccessful)
                Translation = translation;
            return r;
        }

        public IEnumerable<ITranslationContext> Children => Enumerable.Empty<ITranslationContext>();

        public string ShortDescription => $"{pageContext.ShortDescription}. {json.Character}";

        public string ReadableIdentifier => captureId.ToString();
    }
}