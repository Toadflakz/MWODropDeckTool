using System.Collections.Generic;
using System.Threading.Tasks;

namespace MwoCWDropDeckBuilder.Services.Interfaces
{
    public interface ISmurfyDataLoaderService
    {
        Task<bool> LoadBuildsFromFileSourceAsync(string path);

        Task<bool> LoadBuildsFromSmurfyMechBayAsync(string smurfyApiKey);

        Task<bool> LoadBuildsFromMetaMechsMetaTierListAsync(MetaMechsMetaTier metaMechsMetaTier);

        List<SmurfyBuild> GetBuilds();

        Task<bool> LoadMechsAsync();

        Task<bool> LoadWeaponsAsync();

        List<string> GetChassisNames();
    }
}