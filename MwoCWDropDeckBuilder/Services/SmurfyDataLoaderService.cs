using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Documents;
using MwoCWDropDeckBuilder.Model;
using MwoCWDropDeckBuilder.Services.Interfaces;
using Newtonsoft.Json.Linq;
using RestSharp.Extensions.MonoHttp;

namespace MwoCWDropDeckBuilder.Services
{
    public class SmurfyDataLoaderService : ISmurfyDataLoaderService
    {
        private readonly IMwoSmurfyRestServiceClient _restClient;
        private BlockingCollection<SmurfyBuild> _buildsCache;
        private BlockingCollection<SmurfyMech> _mechsCache;
        private BlockingCollection<SmurfyWeapon> _weaponsCache;

        public SmurfyDataLoaderService(IMwoSmurfyRestServiceClient restClient)
        {
            _restClient = restClient;
            _buildsCache = new BlockingCollection<SmurfyBuild>();
            _mechsCache = new BlockingCollection<SmurfyMech>();
            _weaponsCache = new BlockingCollection<SmurfyWeapon>();
        }

        public async Task<bool> LoadMechsAsync()
        {
            var returnValue = true;
            try
            {
                var mechsDynamic = await _restClient.GetMechs();
                Parallel.ForEach(mechsDynamic.PropertyValues(), token =>
                {
                    var mechDyamic = token.ToObject<dynamic>();
                    _mechsCache.Add(new SmurfyMech(mechDyamic));
                });

                //var quirkNames =
                //    _mechsCache.SelectMany(x => x.Quirks.Select(y => y.Name).Distinct().ToList())
                //        .Distinct()
                //        .OrderBy(x => x)
                //        .ToList();
                //var sb = new StringBuilder();
                //quirkNames.ForEach(x => sb.AppendLine(x));
                //var quirkNamesConcat = sb.ToString();
            }
            catch (Exception e)
            {
                returnValue = false;
            }
            return returnValue;
        }

        public async Task<bool> LoadWeaponsAsync()
        {
            var returnValue = true;
            try
            {
                var weaponsDynamic = await _restClient.GetWeapons();
                Parallel.ForEach(weaponsDynamic.PropertyValues(), token =>
                {
                    var weaponDynamic = token.ToObject<dynamic>();
                    _weaponsCache.Add(new SmurfyWeapon(weaponDynamic));
                });
            }
            catch (Exception e)
            {
                returnValue = false;
            }

            //var wNames = _weaponsCache.Select(x => x.Name).OrderBy(x => x).ToList();
            //var sb = new StringBuilder();
            //wNames.ForEach(x => sb.AppendLine(x));
            //var wNameConcat = sb.ToString();

            return returnValue;
        }

        public async Task<bool> LoadBuildsFromFileSourceAsync(string path)
        {
            var returnValue = true;
            try
            {
                var buildUrls = GetBuildUrlsFromTextFile(path);

                var buildUrlTransformBlock = new TransformBlock<string, dynamic>(buildUrl =>
                {
                    var urlParts = buildUrl.Split(new char[] { '#' });
                    var queryString = HttpUtility.ParseQueryString(urlParts.Last());
                    var mechId = queryString["i"];
                    var loadoutId = queryString["l"];

                    dynamic buildUrlReturnValue = new ExpandoObject();
                    buildUrlReturnValue.mechId = mechId;
                    buildUrlReturnValue.loadoutId = loadoutId;
                    buildUrlReturnValue.buildUrl = buildUrl;
                    return buildUrlReturnValue;

                },new ExecutionDataflowBlockOptions()
                {
                    MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
                });
                var restCallBlock = new TransformBlock<dynamic, dynamic>(async buildParams =>
                {
                    dynamic restReturnValue = new ExpandoObject();
                    restReturnValue.buildObject = await _restClient.GetLoadoutByMechIdAndLoadoutId(buildParams.mechId, buildParams.loadoutId);
                    restReturnValue.buildUrl = buildParams.buildUrl;
                    return restReturnValue;
                    ;
                },new ExecutionDataflowBlockOptions()
                {
                    MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
                });
                var storageBlock = new ActionBlock<dynamic>(build =>
                {
                    var buildObject = SmurfyBuild.GetObject(build.buildObject);
                    buildObject.Url = build.buildUrl;
                    buildObject.Mech = _mechsCache.Where(x => x.Id == buildObject.MechId).DefaultIfEmpty(null).Single();
                    buildObject.WeaponSummary = GetWeaponSummary(build.buildObject);
                    buildObject.Weapons = GetWeapons(build.buildObject);
                    buildObject.PerformCalculations();
                    _buildsCache.Add(buildObject);
                }, new ExecutionDataflowBlockOptions()
                {
                    MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
                });

                buildUrlTransformBlock.LinkTo(restCallBlock, new DataflowLinkOptions
                {
                    PropagateCompletion = true
                });
                restCallBlock.LinkTo(storageBlock, new DataflowLinkOptions
                {
                    PropagateCompletion = true
                });

                foreach (var buildUrl in buildUrls)
                    buildUrlTransformBlock.Post(buildUrl);
                buildUrlTransformBlock.Complete();
                await storageBlock.Completion;

            }
            catch (Exception e)
            {
                returnValue = false;
            }
            return returnValue;
        }

        public async Task<bool> LoadBuildsFromSmurfyMechBayAsync(string smurfyApiKey)
        {
            var returnValue = true;
            try
            {

                var mechBay = await _restClient.GetMechBay(smurfyApiKey);
                var builds = mechBay.ToObject<Dictionary<string, JObject>>().Values.Select(x => x["loadout"].ToObject<JObject>()).ToList();
                foreach (var build in builds.AsParallel())
                {
                    var buildObject = SmurfyBuild.GetObject(build);
                    buildObject.Url = String.Format("https://mwo.smurfy-net.de/mechlab#i={0}&l={1}", build["mech_id"].ToString(), build["id"].ToString());
                    buildObject.Mech = _mechsCache.Where(x => x.Id == buildObject.MechId).DefaultIfEmpty(null).Single();
                    buildObject.WeaponSummary = GetWeaponSummary(build);
                    buildObject.Weapons = GetWeapons(build);
                    buildObject.PerformCalculations();
                    _buildsCache.Add(buildObject);
                }

            }
            catch (Exception e)
            {
                returnValue = false;
            }
            return returnValue;
        }


        private string GetWeaponSummary(JObject buildObject)
        {
            var returnValue = new StringBuilder();

            var arms = buildObject["stats"]["armaments"].ToList();
            foreach (var arm in arms)
            {
                if (arms.IndexOf(arm) > 0)
                    returnValue.Append(", ");
                returnValue.AppendFormat("{0}x {1}", arm["count"], arm["name"]);
            }

            return returnValue.ToString();
        }

        private List<SmurfyArmament> GetWeapons(JObject buildObject)
        {
            List<SmurfyArmament> armaments = new List<SmurfyArmament>();
            var arms = buildObject["stats"]["armaments"].ToList();
            foreach (var arm in arms)
            {
                if (arm["name"].ToString() != "AMS")
                {
                    var armament = new SmurfyArmament()
                    {
                        Count = int.Parse(arm["count"].ToString()),
                        Weapon = _weaponsCache.Where(x => x.Id == arm["id"].ToString()).DefaultIfEmpty(null).Single()
                    };
                    armaments.Add(armament);
                }
                
            }
            return armaments;
        }

        public List<SmurfyBuild> GetBuilds()
        {
            return _buildsCache.ToList();
        }

        private List<string> GetBuildUrlsFromTextFile(string path)
        {
            var returnValue = new List<string>();
            using (var fileStream = new FileStream(path, FileMode.Open))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                while (reader.EndOfStream == false)
                    returnValue.Add(reader.ReadLine());
            }
            return returnValue;
        }

        public List<string> GetChassisNames()
        {
            return _mechsCache.Select(x => x.Chassis).Distinct().OrderBy(x => x).ToList();
        }

    }
}