using System.Collections.Generic;
using Newtonsoft.Json;

namespace MagicTranslatorProject.Json
{
    public class CharactersJson
    {
        [JsonProperty("characterId")]
        public int CharacterId { get; set; }

        [JsonProperty("characters")]
        public IEnumerable<CharacterJson> Characters { get; set; }
    }
}