using HollyJson.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Windows;

namespace HollyJson.ViewModels
{

    [AddINotifyPropertyChangedInterface]
    public class MainModel
    {
        private string opennedfileplace = string.Empty;
        CommandHandler _savefile;
        CommandHandler _openfile;
        CommandHandler _addtrait;
        CommandHandler _removetrait;
        CommandHandler _addskill;
        CommandHandler _removeskill;

        CommandHandler _setmoodandatt;
        CommandHandler _setcontrdays;
        CommandHandler _setskilltolimit;
        CommandHandler _setskiiltocap;

        CommandHandler _showspawndate;
        CommandHandler _showtechs;
        CommandHandler _unlocktechs;

        private string search_txt;
        JObject? jobj = null;
        private Character selectedChar;
        private string filter_Prof;
        private string filter_studio;
        private ObservableCollection<Character> filtered_Obj;
        private bool showOnlyTalent = false;
        private bool showOnlyDead = false;
        private bool showWithDead = true;

        public static Dictionary<string, string> LocaleNames { get; set; } = [];
        public static Dictionary<string, string> LocaleTranslator { get; set; } = [];
        public static string MyStudio { get; set; }
        public stateJson Info { get; set; }
        public ObservableCollection<Character> Filtered_Obj
        {
            get => filtered_Obj;
            set
            {
                filtered_Obj = value;
                if (SelectedChar is null)
                {
                    if (value?.Count > 0)
                        SelectedChar = Filtered_Obj[0];
                }
            }
        }
        public Character SelectedChar
        {
            get => selectedChar;
            set
            {
                selectedChar = value;
            }
        }
        public string StatusBarText { get; set; } = "Hello";
        public bool ShowSpawn { get; set; } = false;
        public bool ShowTechs { get; set; } = false;
        public bool Save_Loaded { get; set; } = false;
        public bool Save_done { get; set; } = false;
        public bool ShowOnlyTalent
        {
            get => showOnlyTalent;
            set { showOnlyTalent = value; SetSearched(); }
        }
        public bool ShowOnlyDead
        {
            get => showOnlyDead;
            set
            {
                showOnlyDead = value;
                if (value && !ShowWithDead)
                    ShowWithDead = true;
                SetSearched();
            }
        }
        public bool ShowWithDead
        {
            get => showWithDead;
            set
            {
                showWithDead = value;
                if (!value && ShowOnlyDead)
                    ShowOnlyDead = false;
                SetSearched();
            }
        }
        public List<string> StudioListForChar => StudioList is null ? new List<string>() : StudioList.Where(t => t != "All").ToList();
        public List<string> StudioList { get; set; }
        public List<string> ProfList { get; set; }
        public string Filter_Prof
        {
            get => filter_Prof;
            set
            {
                filter_Prof = value;
                SetSearched();
            }
        }
        public string Filter_studio
        {
            get => filter_studio;
            set
            {
                filter_studio = value;
                SetSearched();
            }
        }
        public string Filter_txt
        {
            get => search_txt;
            set
            {
                search_txt = value;
                SetSearched();
            }
        }
        public MainModel()
        {
            Filter_txt = "";
            Filter_studio = "";
            Filter_Prof = "";
            StatusBarText = "Prepared to unzip";
            UnzipResources();
            StatusBarText = "Done";
        }
        public void SetSearched()
        {
            if (Info is null) return;
            if (Info.characters is null) return;
            IEnumerable<Character> q = Info.characters;
            if (Filter_studio != "All")
            {
                q = q.Where(t => t.studioId == Filter_studio);
            }
            if (Filter_Prof != "All")
            {
                q = q.Where(t => t.professions.ProfToDecode == Filter_Prof);
            }
            if (!string.IsNullOrWhiteSpace(Filter_txt))
            {
                q = q.Where(t => t.MyCustomName.Contains(Filter_txt, StringComparison.CurrentCultureIgnoreCase));
            }
            if (ShowOnlyTalent)
                q = q.Where(t => t.professions.IsTalent);
            if (ShowOnlyDead)
                q = q.Where(t => t.IsDead);
            if (!ShowWithDead)
                q = q.Where(t => !t.IsDead);


            q = q.OrderBy(t => t.professions.ProfToDecode);
            StatusBarText = $"Filtered {q.Count()} chars";
            Filtered_Obj = [.. q];
        }
        public async void UnzipResources()
        {
            try
            {
                await Task.Run(() =>
                {
                    string mi = $"{App.PathToExe}Resources";
                    string local_dir = $"{mi}\\Localization\\";
                    string prof_dir = $"{mi}\\Profiles\\";

                    bool arch_loc_exist = Path.Exists($"{mi}\\Localization.yz");
                    bool arch_prof_exits = Path.Exists($"{mi}\\Profiles.yz");

                    if (arch_loc_exist)
                    {
                        if (!Path.Exists(local_dir))
                        {
                            StatusBarText = "Start extracting Localization";
                            ZipFile.ExtractToDirectory($"{mi}\\Localization.yz", local_dir);
                            StatusBarText = "End extracting Localization";
                        }
                        StatusBarText = "Set Localization";
                        SetLocale(mi + "\\Localization\\RUS\\");
                    }
                    if (arch_prof_exits)
                        if (!Path.Exists(prof_dir))
                        {
                            StatusBarText = "Start extracting Profile images";
                            ZipFile.ExtractToDirectory($"{mi}\\Profiles.yz", prof_dir);
                            StatusBarText = "End extracting Profile images";
                        }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #region Cmd
        public CommandHandler OpenFileCmd
        {
            get
            {
                return _openfile ??= new CommandHandler(async obj =>
                {
                    try
                    {
                        if ((string)obj != "OFD")
                        {
                            var ofd = new OpenFolderDialog();
                            ofd.Multiselect = false;
                            ofd.Title = "Select DIR with Locale (Hollywood Animal\\Holly_Data\\StreamingAssets\\Data\\Localization\\RUS\\)";
                            ofd.ShowHiddenItems = true;
                            if (ofd.ShowDialog() == true)
                            {
                                SetLocale(ofd.FolderName);
                            }
                        }
                        else
                        {
                            var ofdd = new OpenFileDialog();
                            ofdd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low\\Weappy\\Holly\\Saves\\Profiles";
                            ofdd.Multiselect = false;
                            ofdd.Title = "Select save file";
                            ofdd.DefaultExt = ".json";
                            ofdd.RestoreDirectory = true;
                            ofdd.Filter = "Json|*.json";
                            ofdd.ShowHiddenItems = true;
                            if (ofdd.ShowDialog() == true)
                            {
                                await Task.Run(async () =>
                                {
                                    opennedfileplace = Path.GetDirectoryName(ofdd.FileName);
                                    await ParseJson(ofdd.FileName);
                                    GC.Collect();
                                    MyStudio = Info.studioName;
                                    Filtered_Obj = Info.characters;

                                    //var q = Info.characters.Where(t=>t.whiteTagsNEW is not null).Select(t => t.whiteTagsNEW.Select(t=>t.id)).SelectMany(x=>x).Distinct().ToList();

                                    Save_Loaded = true;
                                    SelectedChar = Filtered_Obj[0];
                                    RefershLocale();
                                });
                                GC.Collect();
                            }

                        }
                    }
                    catch (Exception)
                    {
                        Save_Loaded = false;
                    }
                },
                (obj) => true);
            }
        }
        public CommandHandler SaveCmd
        {
            get
            {
                return _savefile ??= new CommandHandler(async obj =>
                {
                    Save_done = false;
                    bool t = await WriteChange();
                    Save_done = true;
                },
                (obj) => true);
            }
        }
        public CommandHandler AddTraitCmd
        {
            get
            {
                return _addtrait ??= new CommandHandler(obj =>
                {
                    SelectedChar.labels.Insert(0,(string)obj);
                }, (obj) => !string.IsNullOrEmpty((string)obj));
            }
        }
        public CommandHandler RemoveTraitCmd
        {
            get
            {
                return _removetrait ??= new CommandHandler(obj =>
                {
                    SelectedChar.labels.Remove((string)obj);
                }, (obj) => true);
            }
        }
        public CommandHandler AddSkillCmd
        {
            get
            {
                return _addskill ??= new CommandHandler(obj =>
                {
                    if (SelectedChar.whiteTagsNEW.Any(t => t.id == (string)obj))
                        return;
                    SelectedChar.whiteTagsNEW.Insert(0, new WhiteTag((string)obj, 12.0));
                }, (obj) => !string.IsNullOrEmpty((string)obj));
            }
        }
        public CommandHandler RemoveSkillCmd
        {
            get
            {
                return _removeskill ??= new CommandHandler(obj =>
                {
                    var a = SelectedChar.whiteTagsNEW.Single(t => t.id == ((WhiteTag)obj).id);
                    SelectedChar.whiteTagsNEW.Remove(a);
                }, (obj) => true);
            }
        }

        public CommandHandler SetMoodAndAttCmd
        {
            get
            {
                return _setmoodandatt ??= new CommandHandler(obj =>
                {
                    if (filtered_Obj?.Count > 0)
                        foreach (var item in Filtered_Obj)
                        {
                            item.mood = item.attitude = 1.00;
                        }
                }, (obj) => true);
            }
        }
        public CommandHandler SetMaxContrDaysCmd
        {
            get
            {
                return _setcontrdays ??= new CommandHandler(obj =>
                {
                    if (filtered_Obj?.Count > 0)
                        foreach (var item in Filtered_Obj)
                        {
                            item.contract.DaysLeft = item.contract.amount * 365;
                        }
                }, (obj) => true);
            }
        }
        public CommandHandler SetSkillToLimitCmd
        {
            get
            {
                return _setskilltolimit ??= new CommandHandler(obj =>
                {
                    if (filtered_Obj?.Count > 0)
                        foreach (var item in Filtered_Obj)
                        {
                            item.professions.Value = item.limit;
                        }
                }, (obj) => true);
            }
        }
        public CommandHandler SetLimitToMaxCmd
        {
            get
            {
                return _setskiiltocap ??= new CommandHandler(obj =>
                {
                    if (filtered_Obj?.Count > 0)
                        foreach (var item in Filtered_Obj)
                        {
                            item.limit = 1.00d;
                        }
                }, (obj) => true);
            }
        }
        public CommandHandler ShowSpawnDateCmd
        {
            get
            {
                return _showspawndate ??= new CommandHandler(obj =>
                {
                    ShowSpawn = true;
                }, (obj) => true);
            }
        }
        public CommandHandler ShowTechsCmd
        {
            get
            {
                return _showtechs ??= new CommandHandler(obj =>
                {
                    ShowTechs = true;
                }, (obj) => true);
            }
        }
        public CommandHandler UnlockTechsCmd
        {
            get
            {
                return _unlocktechs ??= new CommandHandler(obj =>
                {
                    Info.openedPerks = new ObservableCollection<string>(stateJson.PreGenPerks);
                }, (obj) => true);
            }
        }
        #endregion
        #region locale
        private async void SetLocale(string path)
        {
            try
            {
                await LoadNamesFromJson(path);
                await LoadLocaleFromJson(path);
                StatusBarText = "Loaded jsons";
                RefershLocale();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        void RefershLocale()
        {
            StatusBarText = "Refresh loacales";
            if (Info is not null && Info.characters is not null
                && LocaleNames is not null && LocaleNames.Count > 0)
                foreach (var t in Info.characters)
                {
                    t.normalLast = LocaleNames[t.lastNameId];
                    t.normalFirst = LocaleNames[t.firstNameId];
                }
            //БУЭ
            ProfList = ProfList;
            StudioList = StudioList;
            SelectedChar = SelectedChar;
            SetSearched();
            StatusBarText = "Refresh loacales done";
        }
        public async Task LoadLocaleFromJson(string path)
        {
            try
            {
                path += "\\NON_EVENT.json";
                string json = await File.ReadAllTextAsync(path);
                var map = JObject.Parse(json).SelectToken("IdMap");
                var local = JObject.Parse(json).SelectToken("locStrings");
                List<string> getout = JsonConvert.DeserializeObject<List<string>>(local.ToString());
                LocaleTranslator = new Dictionary<string, string>();
                foreach (var item in map.Children<JProperty>())
                {
                    LocaleTranslator.Add(item.Name, getout[item.Value.ToObject<int>()]);
                }
                json = null;
                local = null;
                map = null;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task LoadNamesFromJson(string path)
        {
            try
            {
                path += "\\CHARACTER_NAMES.json";
                string json = await File.ReadAllTextAsync(path);
                var dt = JObject.Parse(json).SelectToken("locStrings");
                List<string> names = JsonConvert.DeserializeObject<List<string>>(dt.ToString());
                json = null;
                dt = null;
                LocaleNames = new Dictionary<string, string>();
                int ii = 0;
                names.ForEach(t =>
                {
                    LocaleNames.Add(ii++.ToString(), t);
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        public async Task ParseJson(string path)
        {
            try
            {
                StatusBarText = "Start parsing save...";
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
                StatusBarText = "Json save readed... Start parsing";
                var aa = jobj.SelectToken("stateJson");
                Info = null;
                Info = new stateJson();
                Info.budget = (int)(aa.SelectToken("budget")?.Value<int>());
                Info.cash = (int)(aa.SelectToken("cash")?.Value<int>());
                Info.reputation = (double)(aa.SelectToken("reputation")?.Value<double>());
                Info.influence = (int)aa.SelectToken("influence")?.Value<int>();
                Info.studioName = aa.SelectToken("studioName")?.Value<string>();
                Info.timePassed = aa.SelectToken("timePassed")?.Value<string>();

                StatusBarText = "Loading milestones...";
                List<Milestones> mm = new List<Milestones>();
                foreach (var item in aa.SelectToken("milestones")?.Children())
                {
                    var q = item.ToObject<JProperty>();
                    Milestones nm = JsonConvert.DeserializeObject<Milestones>(q.Value.ToString());
                    if (!nm.id.Contains("POLICY_ENABLE_") && nm.id.Contains("POLICY_"))
                        mm.Add(nm);
                }
                Info.milestones = [.. mm];
                mm = null;
                StatusBarText = "Loading next gen timers...";
                Dictionary<string, DateTime> dt_d = new Dictionary<string, DateTime>();
                var sp_d = aa.SelectToken("nextGenCharacterTimers")?.Children();
                foreach (var item in sp_d)
                {
                    foreach (var prof in item.Children())
                    {
                        foreach (var prop in prof?.ToObject<JObject>().Properties())
                        {
                            dt_d.Add($"PROFESSION_{prop.Name.ToUpper()}", prop.Value.ToObject<DateTime>());
                        }
                    }
                }
                Info.NextSpawnDays = new Dictionary<string, DateTime>(dt_d);
                dt_d = null;

                StatusBarText = "Loading opened perks...";
                List<string> op_d = new List<string>();
                var op_p = aa.SelectToken("openedPerks")?.Children();
                foreach (var item in op_p)
                {
                    op_d.Add(item?.Value<string>()!);
                }
                Info.openedPerks = [.. op_d];
                op_d = null;

                Info.characters = [];

                int cnt = (int)aa.SelectToken("characters")?.Children().Count();
                StatusBarText = $"Loading characters lists... {cnt} char founded";
                int counter = 1;
                foreach (var item in aa.SelectToken("characters")?.Children())
                {
                    if (item is not null)
                    {
                        var charct = Character.BuildCharacter(item, Info.Now);
                        if (charct is not null)
                            Info.characters.Add(charct);
                        counter++;
                        StatusBarText = $"Loading characters lists... {(Math.Round((double)counter / (double)cnt, 2) * 100).ToString("0.0", CultureInfo.InvariantCulture)}%";
                        //Debug.WriteLine(StatusBarText);
                    }
                }
                StatusBarText = "Loading characters done!";
                StudioList = Info.characters.Select(t => t.studioId).Distinct().ToList()!;
                StudioList.Insert(0, "All");
                ProfList = Info.characters.Select(t => t.professions.ProfToDecode).Distinct().ToList()!;
                ProfList.Insert(0, "All");
                Filter_Prof = ProfList[0];
                Filter_studio = StudioList[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async Task<bool> WriteChange()
        {
            //ПОСЛЕ СОХРАНЕННИЯ НАДО БУДЕТ ПЕРЕЧИТАТЬ ТЕЛЕГУ
            try
            {
                StatusBarText = "Prepare to save";
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Select where to save";
                sfd.DefaultExt = ".json";
                sfd.InitialDirectory = opennedfileplace;
                sfd.RestoreDirectory = true;
                sfd.Filter = "Json|*.json";
                sfd.ShowHiddenItems = true;

                if (sfd.ShowDialog() == true)
                {
                    var z = jobj["stateJson"];
                    z["reputation"] = Info.reputation;
                    z["budget"] = Info.budget;
                    z["cash"] = Info.cash;
                    z["influence"] = Info.influence;

                    //milestones
                    foreach (var mil in Info.milestones)
                    {
                        var b = z["milestones"].Children().
                            SingleOrDefault(t => t.ToObject<JProperty>().Name == mil.id).ToObject<JProperty>();
                        if (b is not null)
                        {
                            b.Value["finished"] = mil.finished;
                            b.Value["locked"] = mil.locked;
                            b.Value["progress"] = mil.progress;
                        }
                    }
                    //openedPerks (без ремува)
                    foreach (var item in Info.openedPerks)
                    {
                        if (((JArray)z["openedPerks"])?.Any(x => x.ToString().Equals(item)) != true)
                        {
                            ((JArray)z["openedPerks"]).Add(item);
                        }
                    }
                    //characters
                    foreach (Character chr in Info.characters)
                    {
                        if (chr.WasChanged(Info.Now))
                        {
                            var a = z["characters"];
                            var b = a?.SingleOrDefault(t => t?["id"]?.Value<int>() == chr.id, null);
                            if (b is not null)
                            {
                                b["limit"] = chr.limit;
                                b["mood"] = chr.mood;
                                b["attitude"] = chr.attitude;
                                b["birthDate"] = chr.birthDate;
                                b["studioId"] = chr.studioId;
                                b["deathDate"] = chr.deathDate;
                                b["causeOfDeath"] = chr.causeOfDeath;
                                if (chr.CustomNameWasSetted)
                                    b["customName"] = chr.MyCustomName;
                                //contract
                                var cnt = b["contract"];
                                if (cnt is not null)
                                {
                                    if (cnt.HasValues)
                                    {
                                        if(chr.contract is null)
                                        {
                                            cnt = null;
                                        }
                                        else
                                        {
                                            cnt["amount"] = chr.contract.amount;
                                            cnt["startAmount"] = chr.contract.startAmount;
                                            cnt["initialFee"] = chr.contract.initialFee;
                                            cnt["monthlySalary"] = chr.contract.monthlySalary;
                                            cnt["weightToSalary"] = chr.contract.weightToSalary;
                                            cnt["dateOfSigning"] = chr.contract.dateOfSigning;
                                            cnt["contractType"] = chr.contract.contractType;
                                        }
                                            
                                    }
                                    else
                                    {
                                        cnt = JToken.Parse(JsonConvert.SerializeObject(chr.contract));
                                    }
                                }
                                cnt = null;
                                //proffessions
                                var prof = b["proffessions"];
                                if (prof is not null && prof.HasValues)
                                {
                                    prof[chr.professions.Name] = chr.professions.Value;
                                }
                                prof = null;
                                //labels
                                var lbl = (JArray?)b["labels"];
                                if (lbl is not null)
                                {
                                    if (chr.labels is not null)
                                    {
                                        foreach (var lablel in chr.labels) //эт на добавление
                                        {
                                            if (lbl.Any(x => x.ToString().Equals(lablel)) != true)
                                            {
                                                lbl.Add(lablel);
                                            }
                                        }
                                        List<JToken> torem = new List<JToken>();
                                        foreach (var lablel in lbl) //эт на удаление
                                        {
                                            if (!chr.labels.Contains(lablel.ToString()))
                                            {
                                                torem.Add(lablel);
                                            }
                                        }
                                        torem.ForEach(t => t.Remove());
                                        torem = null;
                                    }
                                }
                                lbl = null;
                                //whiteTagsNew
                                var wtgs = b["whiteTagsNEW"];
                                if (wtgs is not null)
                                {
                                    if (chr.whiteTagsNEW is not null)
                                    {
                                        foreach (var whiteTag in chr.whiteTagsNEW) //эт на добавление
                                        {
                                            if (wtgs.Children<JProperty>().Any(t => t.Value["id"]?.Value<string>() == whiteTag.id))
                                            {
                                                //ставим значения
                                                var tochng_p = wtgs.Children<JProperty>().Single(t => t.Value["id"]?.Value<string>() == whiteTag.id);
                                                var tochng = tochng_p.Value;
                                                tochng["id"] = whiteTag.id;
                                                tochng["dateAdded"] = whiteTag.dateAdded;
                                                tochng["movieId"] = whiteTag.movieId;
                                                tochng["value"] = whiteTag.value;
                                                tochng["IsOverall"] = whiteTag.IsOverall;
                                                //манипулируем только нулём
                                                var t_over = tochng["overallValues"].Children().Single(t =>
                                                t["movieId"]?.Value<int>() == 0 && t["sourceType"]?.Value<int>() == 0);
                                                t_over["value"] = whiteTag.ZeroPoint.value;
                                            }
                                            else //нету
                                            {
                                                //добавляем
                                                var prop = new JProperty(whiteTag.id);
                                                prop.Value = JToken.Parse(JsonConvert.SerializeObject(whiteTag));
                                                if (wtgs.HasValues)
                                                    wtgs.Last.AddAfterSelf(prop);
                                                else
                                                    ((JObject)wtgs).Add(prop);
                                            }
                                        }
                                        List<JProperty> torem = new List<JProperty>();
                                        foreach (var whitetg in wtgs.Children<JProperty>()) //эт на удаление
                                        {
                                            if (!chr.whiteTagsNEW.Any(t => t.id == whitetg.Name))
                                            {
                                                if (WhiteTag.GetEnumVal(whitetg.Name) != Skills.ELSE)
                                                    torem.Add(whitetg);
                                            }
                                        }
                                        torem.ForEach(t => t.Remove());
                                        torem = null;
                                    }
                                }
                                wtgs = null;


                            }
                        }
                    }
                    StatusBarText = "Write file";
                    await File.WriteAllTextAsync(sfd.FileName, jobj.ToString(Formatting.None)).ContinueWith(t => StatusBarText = "Save done!");

                    return true;
                }
                else
                {
                    StatusBarText = "Save canceled";
                    return false;
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }

}
