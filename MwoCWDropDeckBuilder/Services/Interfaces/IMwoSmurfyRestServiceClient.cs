using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MwoCWDropDeckBuilder.Services.Interfaces
{
    public interface IMwoSmurfyRestServiceClient
    {
        Task<JObject> GetLoadoutByMechIdAndLoadoutId(string mechId, string loadoutId);

        Task<JObject> GetMechs();

        Task<JObject> GetWeapons();

        Task<JObject> GetMechBay(string smurfyApiKey);
    }
}