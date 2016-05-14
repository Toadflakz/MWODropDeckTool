using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MwoCWDropDeckBuilder.Model
{
    [DebuggerDisplay("{Id} - {Name} - {Type}")]
    public class SmurfyWeapon
    {
        public SmurfyWeapon(JObject weaponDynamic)
        {
            var type = weaponDynamic["type"].ToString();
            Id = weaponDynamic["id"].ToString();
            Name = CleanName(weaponDynamic["translated_name"].ToString());
            Type = (type == "BEAM") ? "ENERGY" : type;
            LaserType = (Name.Contains("PULSE LASER")) ? "PULSE LASER" : "LASER";

            Damage = weaponDynamic["calc_stats"]["dmg"].ToObject<decimal>();
            Cooldown = weaponDynamic["cooldown"].ToObject<decimal>();
            Duration = weaponDynamic["duration"].ToObject<decimal>();
            Heat = weaponDynamic["heat"].ToObject<decimal>();

            MinRange = weaponDynamic["min_range"].ToObject<decimal>();
            LongRange = weaponDynamic["long_range"].ToObject<decimal>();

            RawData = weaponDynamic.ToString();
        }

        private string CleanName(string name)
        {
            return name.Replace("LRG ", "LARGE ")
                .Replace("MED ", "MEDIUM ")
                .Replace("SML ", "SMALL ")
                .Replace("LRG ", "LARGE ")
                .Replace("ULTRA AC", "UAC");
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string LaserType { get; set; }

        public decimal Cooldown { get; set; }
        public decimal Duration { get; set; }
        public decimal Heat { get; set; }
        public decimal Damage { get; set; }

        public decimal MinRange { get; set; }
        public decimal LongRange { get; set; }

        public string RawData { get; private set; }


    }
}
