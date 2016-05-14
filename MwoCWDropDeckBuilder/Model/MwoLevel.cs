using System.Collections.Generic;
using System.ComponentModel;

namespace MwoCWDropDeckBuilder.Model
{
    public enum LevelType
    {
        [Description("Community Warfare")]
        CommunityWarfare,
        [Description("Quick Play")]
        QuickPlay
    };

    public class MwoLevel
    {
        public string Name { get; set; }
        public LevelType Type { get; set; }
        public decimal HeatDissipationPenalty { get; set; }

        public static IEnumerable<MwoLevel> GetMaps()
        {
            return new List<MwoLevel>()
            {
                new MwoLevel() {Name = "No selection", Type = LevelType.CommunityWarfare, HeatDissipationPenalty = 0m},
                new MwoLevel() {Name = "Sulfurous Rift", Type = LevelType.CommunityWarfare, HeatDissipationPenalty = 0.3m},
                new MwoLevel() {Name = "Grim Portico", Type = LevelType.CommunityWarfare, HeatDissipationPenalty = -0.5m},
                new MwoLevel() {Name = "Boreal Vault", Type = LevelType.CommunityWarfare, HeatDissipationPenalty = -0.5m},
                new MwoLevel() {Name = "Hellebore Springs", Type = LevelType.CommunityWarfare, HeatDissipationPenalty = 0m},
                new MwoLevel() {Name = "Emerald Taiga", Type = LevelType.CommunityWarfare, HeatDissipationPenalty = 0m},
                new MwoLevel() {Name = "Vitric Forge", Type = LevelType.CommunityWarfare, HeatDissipationPenalty = 0.3m},

                new MwoLevel() {Name = "No selection", Type = LevelType.QuickPlay, HeatDissipationPenalty = 0m},
                new MwoLevel() {Name = "Alpine Peaks", Type = LevelType.QuickPlay, HeatDissipationPenalty = -0.5m},
                new MwoLevel() {Name = "Canyon Network", Type = LevelType.QuickPlay, HeatDissipationPenalty = 0m},
                new MwoLevel() {Name = "Caustic Vallye", Type = LevelType.QuickPlay, HeatDissipationPenalty = 0.3m},
                new MwoLevel() {Name = "Forest Colony", Type = LevelType.QuickPlay, HeatDissipationPenalty = 0m},
                new MwoLevel() {Name = "Frozen City", Type = LevelType.QuickPlay, HeatDissipationPenalty = -0.5m},
                new MwoLevel() {Name = "Crimson Straits", Type = LevelType.QuickPlay, HeatDissipationPenalty = 0m},
                new MwoLevel() {Name = "Terra Therma", Type = LevelType.QuickPlay, HeatDissipationPenalty = 0.5m},
                new MwoLevel() {Name = "The Mining Collective", Type = LevelType.QuickPlay, HeatDissipationPenalty = 0m},
                new MwoLevel() {Name = "HPG Manifold", Type = LevelType.QuickPlay, HeatDissipationPenalty = -0.3m},
                new MwoLevel() {Name = "River City", Type = LevelType.QuickPlay, HeatDissipationPenalty = 0m},
                new MwoLevel() {Name = "Tourmaline Desert", Type = LevelType.QuickPlay, HeatDissipationPenalty = 0.3m},
                new MwoLevel() {Name = "Viridian Bog", Type = LevelType.QuickPlay, HeatDissipationPenalty = 0m},
                new MwoLevel() {Name = "Polar Highlands", Type = LevelType.QuickPlay, HeatDissipationPenalty = -0.5m},
            };
        }

    }
}