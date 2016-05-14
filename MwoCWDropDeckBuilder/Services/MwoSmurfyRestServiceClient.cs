using System;
using System.Net;
using System.Threading.Tasks;
using MwoCWDropDeckBuilder.Services.Interfaces;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace MwoCWDropDeckBuilder.Services
{
    public class MwoSmurfyRestServiceClient : IMwoSmurfyRestServiceClient
    {
        private readonly RestClient _client;

        public MwoSmurfyRestServiceClient()
        {
            _client = new RestClient("https://mwo.smurfy-net.de");
            _client.AddHandler("application/json", new DynamicJsonDeserializer());
        }


        public async Task<JObject> GetLoadoutByMechIdAndLoadoutId(string mechId, string loadoutId)
        {
            dynamic returnValue = null;
            RestRequest request = new RestRequest("api/data/mechs/{mechId}/loadouts/{loadoutId}.json", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "APIKEY b3dc78f06b6b5f5b66b39b601d3b8734f1e06c5c");
            request.AddUrlSegment("mechId", mechId);
            request.AddUrlSegment("loadoutId", loadoutId);
            var response = await _client.ExecuteTaskAsync<dynamic>(request);
            if (response.StatusCode == HttpStatusCode.OK)
                returnValue = response.Data;
            return returnValue;
        }

        public async Task<JObject> GetWeapons()
        {
            dynamic returnValue = null;
            RestRequest request = new RestRequest("api/data/weapons.json", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "APIKEY b3dc78f06b6b5f5b66b39b601d3b8734f1e06c5c");
            var response = await _client.ExecuteTaskAsync<dynamic>(request);
            if (response.StatusCode == HttpStatusCode.OK)
                returnValue = response.Data;
            return returnValue;
        }

        public async Task<JObject> GetMechs()
        {
            dynamic returnValue = null;
            RestRequest request = new RestRequest("api/data/mechs.json", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "APIKEY b3dc78f06b6b5f5b66b39b601d3b8734f1e06c5c");
            var response = await _client.ExecuteTaskAsync<dynamic>(request);
            if (response.StatusCode == HttpStatusCode.OK)
                returnValue = response.Data;
            return returnValue;
        }


        public async Task<JObject> GetMechBay(string smurfyApiKey)
        {
            dynamic returnValue = null;
            RestRequest request = new RestRequest("api/data/user/mechbay.json", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", string.Format("APIKEY {0}", smurfyApiKey));
            var response = await _client.ExecuteTaskAsync<dynamic>(request);
            if (response.StatusCode == HttpStatusCode.OK)
                returnValue = response.Data;
            return returnValue;
        }
    }
}