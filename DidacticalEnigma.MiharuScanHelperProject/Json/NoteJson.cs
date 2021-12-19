using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DidacticalEnigma.MiharuScanHelperProject.Json;

public class NoteJson
{
    [JsonProperty("Uuid")]
    public Guid Uuid { get; set; }
    
    [JsonProperty("Content")]
    public string Content { get; set; }
    
    [UsedImplicitly]
    [JsonExtensionData]
    private readonly Dictionary<string, JToken> extensionData = new Dictionary<string, JToken>();
}