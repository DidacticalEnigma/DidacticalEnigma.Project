using Newtonsoft.Json;

namespace MagicTranslatorProject.Json
{
    public class CharacterJson
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}