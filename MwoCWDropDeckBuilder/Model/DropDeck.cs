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
            get { return Mechs.Average(x => x.SusDps); }
        }

        public decimal AverageMaxDps
        {
            get { return Mechs.Average(x => x.MaxDps); }
        }

        public decimal AverageFirepower
        {
            get { return Mechs.Average(x => x.Firepower); }
        }

        public decimal AverageHeatEfficiency
        {
            get { return Mechs.Average(x => x.HeatEfficiency); }
        }

        public decimal AverageRange
        {
            get { return Mechs.Average(x => x.EffectiveRange); }
        }

        public decimal AverageSpeed
        {
            get { return Mechs.Average(x => x.TopSpeed); }
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