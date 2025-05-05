using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HollyJson.Models;
using System.IO;
using System.Collections.ObjectModel;

namespace HollyJson.ViewModels
{
    public class SubModulesVM
    {
        public ObservableCollection<BuildingInfo> BuildCollLst { get; set; }
        public Dictionary<string,string> GameVarLst { get; set; }
        public SubModulesVM() 
        {
            BuildCollLst = [];
            GameVarLst = [];
        }
        public async Task ReadBuildingsData(string path)
        {
            JObject jobj = null;
            string jsonstr = await File.ReadAllTextAsync(path);
            using (var str_reader = new StringReader(jsonstr))
            {
                using (var reader = new JsonTextReader(str_reader))
                {
                    reader.FloatParseHandling = FloatParseHandling.Decimal;
                    jobj = null;
                    jobj = JObject.Load(reader);
                    str_reader.Close();
                    reader.Close();
                }
            }
            jsonstr = null;
            foreach (var item in jobj.Children())
            {
                var q = item.ToObject<JProperty>();
                BuildingInfo nm = JsonConvert.DeserializeObject<BuildingInfo>(q.Value.ToString());
                BuildCollLst.Add(nm!);
            }
        }
        public async Task ReadGameVarData(string path)
        {
            JObject jobj = null;
            string jsonstr = await File.ReadAllTextAsync(path);
            using (var str_reader = new StringReader(jsonstr))
            {
                using (var reader = new JsonTextReader(str_reader))
                {
                    reader.FloatParseHandling = FloatParseHandling.Decimal;
                    jobj = null;
                    jobj = JObject.Load(reader);
                    str_reader.Close();
                    reader.Close();
                }
            }
            jsonstr = null;
            foreach (var item in jobj.Children())
            {
                var q = item.ToObject<JProperty>().Value;
                //Dictionary<string,string> nm = JsonConvert.DeserializeObject<Dictionary<string, string>>(q.Value.ToString());
                GameVarLst.Add(q.SelectToken("Key")?.Value<string>()!, q.SelectToken("Value")?.Value<string>()!);
            }
        }


    }
    
}
