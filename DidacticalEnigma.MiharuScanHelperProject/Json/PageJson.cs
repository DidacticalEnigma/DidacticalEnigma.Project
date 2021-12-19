using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DidacticalEnigma.MiharuScanHelperProject.Json;

public class PageJson
{
    [JsonProperty("Path")]
    public string Path { get; set; }
    
    [JsonProperty("TextEntries")]
    public IList<TextEntryJson> TextEntries { get; set; }

    [UsedImplicitly]
    [JsonExtensionData]
    private readonly Dictionary<string, JToken> extensionData = new Dictionary<string, JToken>();
}