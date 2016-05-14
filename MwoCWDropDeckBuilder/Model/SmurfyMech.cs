using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MwoCWDropDeckBuilder;

namespace MwoCWDropDeckBuilder.Model
{
    public class SmurfyMech
    {
        public SmurfyMech(JObject mechDynamic)
        {
            
            Id = mechDynamic["id"].ToString();
            Chassis = mechDynamic["chassis_translated"].ToString();
            Name =
                String.Format("{0} {1}", mechDynamic["chassis_translated"].ToString(),
                    mechDynamic["translated_name"].ToString());
            Type = mechDynamic["mech_type"].ToString().ToUppercaseFirst();
            Faction = mechDynamic["faction"].ToString();
            Tonnage = mechDynamic["details"]["tons"].ToObject<int>();
            var quirks = (mechDynamic["details"]["quirks"] != null) ? mechDynamic["details"]["quirks"].ToArray().Select(x => new SmurfyQuirk(x)).ToList() : new List<SmurfyQuirk>();
            Quirks = quirks;
            RawData = mechDynamic.ToString();
        }

        public string Id { get; set; }
        public string Chassis { get; set; }
        public string Name { get; set; }
        public string Faction { get; set; }
        public string Type { get; set; }
        public int Tonnage { get; set; }
        public List<SmurfyQuirk> Quirks { get; set; }

        public string RawData { get; private set; }

        
    }
}
