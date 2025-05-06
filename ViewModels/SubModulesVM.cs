using HollyJson.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.IO;

namespace HollyJson.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class SubModulesVM
    {
        CommandHandler _unlocktags;
        CommandHandler _setdurationtime;
        public string PathToConfDir { get; set; }
        //FROM CONF FILES
        public ObservableCollection<TechInfo> Techs{ get; set; }
        public ObservableCollection<Titans> Titans { get; set; }
        public ObservableCollection<Building> Buildings { get; set; }
        public Dictionary<string, string> GameVarLst { get; set; }
        public ObservableCollection<CharXp> CharXps { get; set; }
        //FROM state JSON
        public DateTime Now { get; set; }
        public ObservableCollection<string> TagBank { get; set; }
        public ObservableCollection<TagPool> TagPools { get; set; }
        public Dictionary<string, DateTime> NextSpawnDays { get; set; }
        public ObservableCollection<TagInCodex> currentTagsInCodex { get; set; }
        public bool? isCodexOpened { get; set; }

        public SubModulesVM()
        {
            TagBank = [];
            TagPools = [];
            Techs = [];
            GameVarLst = [];
            Titans = [];
            Buildings = [];
            NextSpawnDays = [];
            currentTagsInCodex = [];
            CharXps = [];
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
                            TagPools.Add(new TagPool(tag, Now.AddDays(-1)));
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
                    foreach (var item in Techs)
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
                Techs.Add(nm!);
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
