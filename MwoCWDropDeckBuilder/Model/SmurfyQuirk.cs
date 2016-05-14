using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace MwoCWDropDeckBuilder.Model
{
    [DebuggerDisplay("{Name} - {Value}")]
    public class SmurfyQuirk
    {
        public string Name { get; set; }
        public decimal Value { get; set; }

        public SmurfyQuirk(JToken quirk)
        {
            Name = CleanName(quirk["translated_name"].ToString());
            Value = quirk["value"].ToObject<decimal>();
        }

        private string CleanName(string name)
        {
            return name.Replace("STD ", "");

        }
            

    }
}