using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DidacticalEnigma.MiharuScanHelperProject.Json;

public class TextEntryJson
{
    [JsonProperty("Uuid")]
    public Guid Uuid { get; set; }
    
    [JsonProperty("ParsedText")]
    public string Source { get; set; }
    
    [JsonProperty("TranslatedText")]
    public string Target { get; set; }
    
    [JsonProperty("UniqueNotes")]
    public IList<NoteJson> Notes { get; set; }
    
    [UsedImplicitly]
    [JsonExtensionData]
    private readonly Dictionary<string, JToken> extensionData = new Dictionary<string, JToken>();
    
}