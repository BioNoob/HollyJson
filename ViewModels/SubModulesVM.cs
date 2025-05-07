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
        CommandHandler _switchcodex;
        CommandHandler _unlockbuildparams;
        CommandHandler _setxpmultiplayer;
        CommandHandler _setresduration;
        CommandHandler _settechduration;

        private bool isX1Chosen;
        private bool isX2Chosen;
        private bool isX5Chosen;
        private bool isX10Chosen;

        public bool IsX1Chosen { get => isX1Chosen; set => isX1Chosen = value; }
        public bool IsX2Chosen { get => isX2Chosen; set => isX2Chosen = value; }
        public bool IsX5Chosen { get => isX5Chosen; set => isX5Chosen = value; }
        public bool IsX10Chosen { get => isX10Chosen; set => isX10Chosen = value; }
        public string PathToConfDir { get; set; }
        //FROM CONF FILES
        public ObservableCollection<TechInfo> Techs { get; set; }
        public ObservableCollection<Titans> Titans { get; set; }
        public ObservableCollection<Building> Buildings { get; set; }
        public Dictionary<string, string> GameVarLst { get; set; }
        public ObservableCollection<CharXp> CharXps { get; set; }

        public string ResearchSpd_curr
        {
            get
            {
                if (Techs.Count > 0)
                {
                    var q = Techs.Select(t => t.duration);
                    if (q.Any(t => t != 0))
                    {
                        return "Default";
                    }
                    else
                        return "Modified";
                }
                else
                    return string.Empty;
            }
        }
        public int ResearchPreset { get; set; }
        public string TechhSpd_curr
        {
            get
            {
                if (GameVarLst.Count > 0)
                {
                    int a = int.Parse(GameVarLst["tech_creation_days_per_point_below_average"]);
                    int b = int.Parse(GameVarLst["tech_creation_days_per_point_average"]);
                    int c = int.Parse(GameVarLst["tech_creation_days_per_point_above_average"]);
                    if (a + b + c == 600)
                        return "Default";
                    else
                        return "Modified";
                }
                else
                    return string.Empty;
            }
        }
        public int TechPreset { get; set; }
        //FROM state JSON
        public DateTime Now { get; set; }
        public ObservableCollection<string> TagBank { get; set; }
        public ObservableCollection<TagPool> TagPools { get; set; }
        public Dictionary<string, DateTime> NextSpawnDays { get; set; }
        public ObservableCollection<TagInCodex> currentTagsInCodex { get; set; }
        public int ValOfActivePolicy { get; set; }
        public bool HaveActivePolicy { get; set; }
        public string NameOfActivePolicy { get; set; }
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
                }, (obj) => TagBank.Count > 0);
            }
        }
        public CommandHandler SwitchCodexCmd
        {
            get
            {
                return _switchcodex ??= new CommandHandler(obj =>
                {
                    isCodexOpened = !isCodexOpened;
                }, (obj) => PathToConfDir.Length > 0);
            }
        }
        public CommandHandler SetTechDurationCmd
        {
            get
            {
                return _settechduration ??= new CommandHandler(obj =>
                {
                    GameVarLst["tech_creation_days_per_point_below_average"] = 
                    GameVarLst["tech_creation_days_per_point_average"] = 
                    GameVarLst["tech_creation_days_per_point_above_average"] = TechPreset.ToString();
                }, (obj) => true);
            }
        }
        public CommandHandler SetResDurationCmd
        {
            get
            {
                return _setresduration ??= new CommandHandler(obj =>
                {
                    foreach (var item in Techs)
                        item.duration = ResearchPreset;
                }, (obj) => Techs.Count > 0);
            }
        }
        public CommandHandler UnlockBuildsParamsCmd
        {
            get
            {
                return _unlockbuildparams ??= new CommandHandler(obj =>
                {
                    if (obj is not null)
                        foreach (var item in Buildings)
                        {
                            switch (obj as string)
                            {
                                case "W":
                                    item.baseWater = item.baseWater > 0 ? item.baseWater = 1 : item.baseWater;
                                    break;
                                case "E":
                                    item.baseElectricity = item.baseElectricity > 0 ? item.baseElectricity = 1 : item.baseElectricity;
                                    break;
                                case "D":
                                    item.baseDuration = item.baseDuration > 0 ? item.baseDuration = 1 : item.baseDuration;
                                    break;
                                case "C":
                                    item.baseCost = item.baseCost > 0 ? item.baseCost = 1 : item.baseCost;
                                    break;
                                case "S":
                                    item.staff = item.staff > 0 ? item.staff = 1 : item.staff;
                                    break;
                            }
                        }
                }, (obj) => true);
            }
        }
        public CommandHandler SetXpMultCmd
        {
            get
            {
                return _setxpmultiplayer ??= new CommandHandler(obj =>
                {
                    if (obj is not null)
                        foreach (var item in CharXps)
                        {
                            switch (obj as string)
                            {
                                case "1":
                                    item.SetXpMod(1); //ТУТ дефолт ставить..
                                    break;
                                case "2":
                                    item.SetXpMod(2.0d);
                                    break;
                                case "5":
                                    item.SetXpMod(5.0d);
                                    break;
                                case "10":
                                    item.SetXpMod(10.0d);
                                    break;
                            }
                        }
                }, (obj) => true);
            }
        }

        public async Task ReadPerksData(string path)
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
