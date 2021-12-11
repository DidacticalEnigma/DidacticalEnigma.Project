using System.Collections.Generic;
using JetBrains.Annotations;
using MagicTranslatorProject.Context;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MagicTranslatorProject.Json
{
    internal class CaptureJson
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("translation")]
        public string Translation { get; set; }

        [JsonProperty("notes")]
        public IEnumerable<NoteJson> Notes { get; set; }
        
        [JsonProperty("glossNotes")]
        public IEnumerable<NoteJson> GlossNotes { get; set; }

        [JsonProperty("character")]
        public CharacterType Character { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [UsedImplicitly]
        [JsonExtensionData]
        private readonly Dictionary<string, JToken> extensionData = new Dictionary<string, JToken>();
    }
}