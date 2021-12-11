using System.Linq;
using Newtonsoft.Json;

namespace MagicTranslatorProject.Json
{
    [JsonConverter(typeof(NoteJsonConverter))]
    public class NoteJson
    {
        public string SideText { get; set; }
        
        public string Text { get; set; }

        public NoteJson(string sideText, string text)
        {
            SideText = sideText;
            Text = text;
        }
    }
}