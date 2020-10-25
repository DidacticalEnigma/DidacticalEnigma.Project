using System.Collections.Generic;
using Newtonsoft.Json;

namespace MagicTranslatorProject.Json
{
    internal class PageJson
    {
        [JsonProperty("captureId")]
        public int CaptureId { get; set; }

        [JsonProperty("captures")]
        public IList<CaptureJson> Captures { get; set; }
    }
}