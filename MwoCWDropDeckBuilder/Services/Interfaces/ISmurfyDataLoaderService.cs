using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MwoCWDropDeckBuilder.Services.Interfaces
{
    public interface ISmurfyDataLoaderService
    {
        Task<bool> LoadBuildsFromFileSourceAsync(string path);

        Task<bool> LoadBuildsFromSmurfyMechBayAsync(string smurfyApiKey);

        List<SmurfyBuild> GetBuilds();

        Task<bool> LoadMechsAsync();

        Task<bool> LoadWeaponsAsync();

        List<string> GetChassisNames();
    }
}