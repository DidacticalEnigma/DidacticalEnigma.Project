using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MagicTranslatorProject.Json;

public class Point
{
    [JsonProperty("x")]
    public double X { get; set; }
    
    [JsonProperty("y")]
    public double Y { get; set; }

    [UsedImplicitly]
    [JsonExtensionData]
    private readonly Dictionary<string, JToken> extensionData = new Dictionary<string, JToken>();
}