using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MagicTranslatorProject.Json
{
    public class NoteJsonConverter : JsonConverter<NoteJson>
    {
        public override void WriteJson(JsonWriter writer, NoteJson value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, new object[]
            {
                value.SideText,
                value.Text
            });
        }

        public override NoteJson ReadJson(
            JsonReader reader,
            Type objectType,
            NoteJson existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var arr = serializer.Deserialize<JArray>(reader);
            var newValue = new NoteJson(arr[0].Value<string>(), arr[1].Value<string>());
            return newValue;
        }
    }
}