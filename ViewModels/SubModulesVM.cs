using HollyJson.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.IO;

namespace HollyJson.ViewModels
{
    public class SubModulesVM
    {
        CommandHandler _unlocktags;
        CommandHandler _setdurationtime;
        public DateTime Now { get; set; }
        public ObservableCollection<string> TagBank { get; set; }
        public ObservableCollection<TagPool> TagPool { get; set; }
        public ObservableCollection<TechInfo> TechCollLst { get; set; }
        public Dictionary<string, string> GameVarLst { get; set; }
        public SubModulesVM()
        {
            TechCollLst = [];
            GameVarLst = [];
        }

        public CommandHandler UnlockTagsCmd
        {
            get
            {
                return _unlocktags ??= new CommandHandler(obj =>
                {
                    if (TagBank.Count > 0)
                    {
                        foreach (string tag in TagBank)
                        {
                            TagPool.Add(new TagPool(tag, Now.AddDays(-1)));
                        }
                        TagBank.Clear();
                    }
                }, (obj) => true);
            }
        }
        public CommandHandler SetDurationTimeTech
        {
            get
            {
                return _setdurationtime ??= new CommandHandler(obj =>
                {
                    foreach (var item in TechCollLst)
                    {
                        item.duration = (int)obj;
                    }
                    return;
                }, (obj) => true);
            }
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
                TechInfo nm = JsonConvert.DeserializeObject<TechInfo>(q.Value.ToString());
                TechCollLst.Add(nm!);
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
                GameVarLst.Add(q.SelectToken("Key")?.Value<string>()!, q.SelectToken("Value")?.Value<string>()!);
            }
        }


    }

}
