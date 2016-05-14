using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MwoCWDropDeckBuilder.Services.Interfaces
{
    public enum MetaMechsMetaTier
    {
        [Description("Tier 1")]
        Tier1,
        [Description("Tier 2")]
        Tier2,
        [Description("Tier 3")]
        Tier3,
        [Description("Tier 4")]
        Tier4,
        [Description("Tier 5")]
        Tier5
    } 

    public interface IMetaMechsDataLoaderService
    {
        Task<List<string>> GetMetaMechsMetaTierList(MetaMechsMetaTier metaTier);
    }
}