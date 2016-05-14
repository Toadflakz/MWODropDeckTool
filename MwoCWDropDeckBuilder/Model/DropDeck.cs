using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MwoCWDropDeckBuilder.Model
{
    public class DropDeck : IEquatable<DropDeck>
    {
        private readonly string _key;

        public DropDeck(IList<SmurfyBuild> builds)
        {
            Mechs = new ReadOnlyCollection<SmurfyBuild>(builds);

            StringBuilder sb = new StringBuilder();
            Mechs.OrderBy(x => x.MechId).ToList().ForEach(x => sb.AppendFormat("{0}:", x.MechId));
            _key = sb.ToString();

            UniqueId = Guid.NewGuid().ToString();
        }

        public string UniqueId { get; set; }

        public ReadOnlyCollection<SmurfyBuild> Mechs { get; private set; }

        public string MechSummary
        {
            get { return GetMechSummary(); }
        }

        public int Tonnage
        {
            get { return Mechs.Sum(x => x.MechTonnage); }
        }

        public decimal AverageSusDps
        {
            get { return Math.Round(Mechs.Average(x => x.SusDps),0); }
        }

        public decimal DeltaSusDps
        {
            get { return Math.Round(Mechs.Max(x => x.SusDps) - Mechs.Min(x => x.SusDps),0); }
        }

        public decimal AverageMaxDps
        {
            get { return Math.Round(Mechs.Average(x => x.MaxDps),0); }
        }

        public decimal DeltaMaxDps
        {
            get { return Math.Round(Mechs.Max(x => x.MaxDps) - Mechs.Min(x => x.MaxDps),0);}
        }

        public decimal TotalFirepower
        {
            get { return Mechs.Sum(x => x.Firepower); }
        }

        public decimal AverageFirepower
        {
            get { return Math.Round(Mechs.Average(x => x.Firepower),0); }
        }

        public decimal DeltaFirepower
        {
            get { return Mechs.Max(x => x.Firepower) - Mechs.Min(x => x.Firepower);}
        }

        public decimal AverageHeatEfficiency
        {
            get { return Math.Round(Mechs.Average(x => x.HeatEfficiency),0); }
        }

        public decimal DeltaHeatEfficiency
        {
            get { return Math.Round(Mechs.Max(x => x.HeatEfficiency) - Mechs.Min(x => x.HeatEfficiency),0);}
        }

        public decimal AverageRangeExclLights
        {
            get { return Math.Round(Mechs.Where(x => x.Mech.Type != "Light").Average(x => x.EffectiveRange),0); }
        }

        public decimal DeltaRangeExclLights
        {
            get { return Math.Round(Mechs.Where(x => x.Mech.Type != "Light").Max(x => x.EffectiveRange) - Mechs.Where(x => x.Mech.Type != "Light").Min(x => x.EffectiveRange),0); }
        }

        public decimal AverageSpeed
        {
            get { return Math.Round(Mechs.Average(x => x.TopSpeed),0); }
        }

        public decimal DeltaSpeedExclLights
        {
            get { return Math.Round(Mechs.Where(x => x.Mech.Type != "Light").Max(x => x.TopSpeed) - Mechs.Where(x => x.Mech.Type != "Light").Min(x => x.TopSpeed),0); }
        }

        public int ECMChassis
        {
            get { return Mechs.Count(x => x.IsECM); }
        }

        private string GetMechSummary()
        {
            var returnValue = string.Empty;
            if (Mechs != null && Mechs.Count > 0)
            {
                var sb = new StringBuilder();
                int index = 0; //References are the same, duplicate references will mean duplicate data - this is the only sure way to count the indexes AFAIK!!!
                foreach (var smurfyBuild in Mechs.OrderByDescending(x => x.MechTonnage))
                {
                    if (index > 0)
                        sb.Append(", ");
                    sb.AppendFormat("{0}", smurfyBuild.MechName);
                    index++;
                }
                returnValue = sb.ToString();
            }
            return returnValue;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DropDeck))
                return false;

            return this.Equals((DropDeck) obj);
        }

        public bool Equals(DropDeck other)
        {
            return this.ToString() == other.ToString();
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return _key;
        }
    }
} 