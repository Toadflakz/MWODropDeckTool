using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MwoCWDropDeckBuilder.Model;
using Newtonsoft.Json.Linq;

namespace MwoCWDropDeckBuilder
{
     [DebuggerDisplay("{MechName} - {Url}")]
    public class SmurfyBuild
    {
        public bool IsSelected { get; set; }
        public string Url { get; set; }
        public string MechId { get; set; }
        public SmurfyMech Mech { get; set; }

        public string MechName { get { return (Mech != null) ? Mech.Name : string.Empty; } }
        public int MechTonnage { get { return (Mech != null) ? Mech.Tonnage : 0; } }

        public string WeaponSummary { get; set; }
        public decimal TopSpeed { get; set; }
        public bool IsClan { get; set; }
        public bool HasXL { get; set; }
        public bool IsECM { get; set; }
        public bool IsDHS { get; set; }
        public int Heatsinks { get; set; }
        public int InternalHeatsinks { get; set; }
        
        public List<SmurfyArmament> Weapons { get; set; }
        public decimal MaxDps { get; set; }
        public decimal SusDps { get; set; }
        public decimal Firepower { get; set; }
        public decimal HeatEfficiency { get; set; }
        public decimal EffectiveRange { get; set; }

        public string RawData { get; private set; }

        internal static SmurfyBuild GetObject(JObject build)
        {
            var externalHeatSinks =
                build["stats"]["equipment"].ToArray()
                    .Where(x => x["name"].ToString().Contains("HEAT SINK"))
                    .Select(x => x["count"].ToObject<int>()).DefaultIfEmpty(0).Single();
            var totalHeatSinks = build["stats"]["heatsinks"].ToObject<int>();
            return new SmurfyBuild
            {
                MechId = build["mech_id"].ToString(),
                TopSpeed = build["stats"]["top_speed_tweak"].ToObject<decimal>(),
                IsClan = build["stats"]["engine_name"].ToString().Contains("CLAN"),
                HasXL = build["stats"]["engine_type"].ToString() == "XL",
                IsDHS = build["upgrades"].ToArray().Any(x => x["name"].ToString().Contains("DOUBLE HEAT SINK")),
                Heatsinks = totalHeatSinks,
                InternalHeatsinks = totalHeatSinks - externalHeatSinks,
                IsECM = build["stats"]["equipment"].ToArray().Any(x => x["name"].ToString().Contains("ECM")),
                RawData = build.ToString()
            };
        }

        public void PerformCalculations(MwoLevel level = null)
        {
            if (level == null)
                level = new MwoLevel();

            var firepower = Weapons.Select(x => x.Count*x.Weapon.Damage).Sum();
            Firepower = firepower;

            var effectiveRange = Weapons.Min(x => x.Weapon.LongRange * GetRangeQuirk(x.Weapon, Mech.Quirks));
            EffectiveRange = effectiveRange;

            var externalHeatsinks = Heatsinks - InternalHeatsinks;
            var internalHeatsinkRate = (IsDHS) ? 0.2m : 0.10m;
            var externalHeatsinkRate = (IsDHS) ? 0.14m : 0.10m;
            var heatDissipation = (internalHeatsinkRate * InternalHeatsinks) + (externalHeatsinkRate * externalHeatsinks) - level.HeatDissipationPenalty;
            var heatGeneration =
                Weapons.Select(
                    x =>
                        x.Count*
                        ((x.Weapon.Heat * GetHeatGenerationQuirk(x.Weapon, Mech.Quirks))/
                         ((x.Weapon.Cooldown * GetCooldownQuirk(x.Weapon, Mech.Quirks)) +
                          (x.Weapon.Duration * GetDurationQuirk(x.Weapon, Mech.Quirks))))).Sum();
            var heatEfficiency = heatDissipation/heatGeneration;
            HeatEfficiency = heatEfficiency;

            var maxDps =
                Weapons.Select(
                    x =>
                        x.Count*
                        (x.Weapon.Damage /
                         ((x.Weapon.Cooldown*GetCooldownQuirk(x.Weapon, Mech.Quirks)) +
                          (x.Weapon.Duration*GetDurationQuirk(x.Weapon, Mech.Quirks)))))
                          .Sum();
            MaxDps = maxDps;
            SusDps = heatEfficiency*maxDps;


        }

        private decimal GetHeatGenerationQuirk(SmurfyWeapon smurfyWeapon, List<SmurfyQuirk> quirks)
        {
            var quirkName = "HEAT GEN";
            var targetQuirks = quirks.Where(x => ((x.Name.Contains(String.Format("{0} {1}", smurfyWeapon.Name, quirkName))) ||
                                                  (x.Name.Contains(String.Format("{0} {1}", smurfyWeapon.Type, quirkName))) ||
                                                  (x.Name.Contains(String.Format("{0} {1}", smurfyWeapon.LaserType, quirkName)))))
                                     .Select(x => x.Value)
                                     .ToList();
            var quirkFactor = 1 + targetQuirks.Sum();
            return quirkFactor;
        }

        private decimal GetRangeQuirk(SmurfyWeapon smurfyWeapon, List<SmurfyQuirk> quirks)
        {
            var quirkName = "RANGE";
            var targetQuirks = quirks.Where(x => ((x.Name.Contains(String.Format("{0} {1}", smurfyWeapon.Name, quirkName))) ||
                                                  (x.Name.Contains(String.Format("{0} {1}", smurfyWeapon.Type, quirkName))) ||
                                                  (x.Name.Contains(String.Format("{0} {1}", smurfyWeapon.LaserType, quirkName)))))
                                     .Select(x => x.Value)
                                     .ToList();
            var quirkFactor = 1 + targetQuirks.Sum();
            return quirkFactor;
        }

        private decimal GetCooldownQuirk(SmurfyWeapon smurfyWeapon, List<SmurfyQuirk> quirks)
        {
            var quirkName = "COOLDOWN";
            var targetQuirks = quirks.Where(x => ((x.Name.Contains(String.Format("{0} {1}", smurfyWeapon.Name, quirkName))) ||
                                                  (x.Name.Contains(String.Format("{0} {1}", smurfyWeapon.Type, quirkName))) ||
                                                  (x.Name.Contains(String.Format("{0} {1}", smurfyWeapon.LaserType, quirkName)))))
                                     .Select(x => x.Value)
                                     .ToList();
            var quirkFactor = 1 - targetQuirks.Sum();
            return quirkFactor;
        }

        private decimal GetDurationQuirk(SmurfyWeapon smurfyWeapon, List<SmurfyQuirk> quirks)
        {
            var quirkName = "DURATION";
            var targetQuirks = quirks.Where(x => ((x.Name.Contains(String.Format("{0} {1}", smurfyWeapon.Name, quirkName))) ||
                                                  (x.Name.Contains(String.Format("{0} {1}", smurfyWeapon.Type, quirkName))) ||
                                                  (x.Name.Contains(String.Format("{0} {1}", smurfyWeapon.LaserType, quirkName)))))
                                     .Select(x => x.Value)
                                     .ToList();
            var quirkFactor = 1 - targetQuirks.Sum();
            return quirkFactor;
        }



        //DPS = damage / ((cooldown + duration) * (cooldown quirks factor)
        //cooldown quirks factor = cooldown for specific weapon * cooldown for weapon class
        //Range = range * range quirk

        //Heat efficiency = heat dissipated / heat generated (sum per weapon heat * heat quirk)

        //standard heatsink = 1 hps
        //double heatsink = 1.4

        //Sustained dps = heat efficiency * max dps


        
    }
}
