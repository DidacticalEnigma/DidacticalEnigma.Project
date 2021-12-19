using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DidacticalEnigma.MiharuScanHelperProject.Json;

public class ChapterJson
{
    [JsonProperty("Pages")]
    public IList<PageJson> Pages { get; set; }
    
    [UsedImplicitly]
    [JsonExtensionData]
    private readonly Dictionary<string, JToken> extensionData = new Dictionary<string, JToken>();
}