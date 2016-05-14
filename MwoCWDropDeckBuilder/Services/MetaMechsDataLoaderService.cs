using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CsvHelper;
using MwoCWDropDeckBuilder.Services.Interfaces;

namespace MwoCWDropDeckBuilder.Services
{
    public class MetaMechsDataLoaderService : IMetaMechsDataLoaderService
    {
        public async Task<List<string>> GetMetaMechsMetaTierList(MetaMechsMetaTier metaTier)
        {
            List<string> buildUrls = new List<string>();

            var validTierValues = GetTierValues(metaTier);

            using (var client = new WebClient())
            using (var streamReader = new StreamReader(new MemoryStream(client.DownloadData(new Uri("http://metamechs.com/wp-content/plugins/metamechs/data/metamechs-base.csv")))))
            using (var csvReader = new CsvReader(streamReader))
            {
                csvReader.Configuration.HasHeaderRecord = true;
                while (csvReader.Read())
                {
                    if (validTierValues.Contains(csvReader["cat2"].ToUpperInvariant()))
                    {
                        buildUrls.Add(csvReader["Smurfy"]);
                    }
                }
            }
            return await Task.FromResult(buildUrls);
        }

        private List<string> GetTierValues(MetaMechsMetaTier metaTier)
        {
            var returnValue = new List<string>();
            switch (metaTier)
            {
                case MetaMechsMetaTier.Tier1:
                    returnValue.Add(MetaMechsMetaTier.Tier1.GetDescription().ToUpperInvariant());
                    break;
                case MetaMechsMetaTier.Tier2:
                    returnValue.Add(MetaMechsMetaTier.Tier1.GetDescription().ToUpperInvariant());
                    returnValue.Add(MetaMechsMetaTier.Tier2.GetDescription().ToUpperInvariant());
                    break;
                case MetaMechsMetaTier.Tier3:
                    returnValue.Add(MetaMechsMetaTier.Tier1.GetDescription().ToUpperInvariant());
                    returnValue.Add(MetaMechsMetaTier.Tier2.GetDescription().ToUpperInvariant());
                    returnValue.Add(MetaMechsMetaTier.Tier3.GetDescription().ToUpperInvariant());
                    break;
                case MetaMechsMetaTier.Tier4:
                    returnValue.Add(MetaMechsMetaTier.Tier1.GetDescription().ToUpperInvariant());
                    returnValue.Add(MetaMechsMetaTier.Tier2.GetDescription().ToUpperInvariant());
                    returnValue.Add(MetaMechsMetaTier.Tier3.GetDescription().ToUpperInvariant());
                    returnValue.Add(MetaMechsMetaTier.Tier4.GetDescription().ToUpperInvariant());
                    break;
                case MetaMechsMetaTier.Tier5:
                    returnValue.Add(MetaMechsMetaTier.Tier1.GetDescription().ToUpperInvariant());
                    returnValue.Add(MetaMechsMetaTier.Tier2.GetDescription().ToUpperInvariant());
                    returnValue.Add(MetaMechsMetaTier.Tier3.GetDescription().ToUpperInvariant());
                    returnValue.Add(MetaMechsMetaTier.Tier4.GetDescription().ToUpperInvariant());
                    returnValue.Add(MetaMechsMetaTier.Tier5.GetDescription().ToUpperInvariant());
                    break;
                
            }
            return returnValue;
        }
    }
}