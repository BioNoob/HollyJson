using HollyJson.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

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
        private string search_txt;
        JObject? jobj = null;
        private Character selectedChar;
        private string filter_Prof;
        private string filter_studio;

        public static Dictionary<string, string> LocaleNames { get; set; } = [];
        public static Dictionary<string, string> LocaleTranslator { get; set; } = [];
        public static string MyStudio { get; set; }
        public stateJson Info { get; set; }
        public ObservableCollection<Character> Filtered_Obj { get; set; }
        public Character SelectedChar { get => selectedChar; set => selectedChar = value; }
        public bool Save_Loaded { get; set; } = false;
        public bool Save_done { get; set; } = false;
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

            Filtered_Obj = [.. q];
        }


        private async void SetLocale(string path)
        {
            try
            {
                await LoadNamesFromJson(path);
                await LoadLocaleFromJson(path);
                RefershLocale();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        void RefershLocale()
        {
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
        }
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
                    SelectedChar.labels.Add((string)obj);
                }, (obj) => true);
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
        public async Task LoadLocaleFromJson(string path)
        {
            try
            {
                path += "\\NON_EVENT.json";
                string json = await File.ReadAllTextAsync(path); //"PROTAGONIST_WARRIOR": 26,
                var map = JObject.Parse(json).SelectToken("IdMap");
                var local = JObject.Parse(json).SelectToken("locStrings");//array[int] = answer
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
        public async Task ParseJson(string path)
        {
            try
            {
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
                var aa = jobj.SelectToken("stateJson");
                Info = new stateJson();
                Info.budget = (int)(aa.SelectToken("budget")?.Value<int>());
                Info.cash = (int)(aa.SelectToken("cash")?.Value<int>());
                Info.reputation = (double)(aa.SelectToken("reputation")?.Value<double>());
                Info.influence = (int)aa.SelectToken("influence")?.Value<int>();
                Info.studioName = aa.SelectToken("studioName")?.Value<string>();
                Info.timePassed = aa.SelectToken("timePassed")?.Value<string>();
                Info.NextSpawnDays = [];
                var sp_d = aa.SelectToken("nextGenCharacterTimers")?.Children();
                foreach (var item in sp_d)
                {
                    foreach (var prof in item.Children())
                    {
                        foreach (var prop in prof?.ToObject<JObject>().Properties())
                        {
                            Info.NextSpawnDays.Add(prop.Name, prop.Value.ToObject<DateTime>());
                        }
                    }
                }
                Info.characters = [];
                foreach (var item in aa.SelectToken("characters")?.Children())
                {
                    if (item is not null)
                    {
                        var z = JsonConvert.DeserializeObject<Character>(item.ToString());
                        var qqq = item.SelectToken("professions").ToObject<JObject>().Properties().ElementAt(0);
                        var q_prop = qqq.Name;
                        var q_val = qqq.Value.ToObject<double>();
                        if (z is not null)
                        {
                            z.professions = new Professions() { Name = q_prop, Value = q_val };
                            z.JsonString = item.ToString();
                            z.SetFullAge(Info.Now);
                            if (z.contract is not null)
                                z.contract.SetCalcDaysLeft(Info.Now);
                            var tags = item.SelectToken("whiteTagsNEW");
                            if (tags?.Children().Count() > 0)
                            {
                                z.whiteTagsNEW = [];
                                foreach (var tag in tags.Children())
                                {
                                    WhiteTag whiteTag = new WhiteTag();
                                    var in_tag = tag.First();
                                    whiteTag.id = in_tag.SelectToken("id")?.Value<string>();
                                    if (whiteTag.Tagtype == Skills.ELSE) //срезаем то что не отслеживаем, ибо нафиг
                                        continue;
                                    whiteTag.dateAdded = (DateTime)in_tag.SelectToken("dateAdded")?.Value<DateTime>();
                                    whiteTag.movieId = (int)in_tag.SelectToken("movieId")?.Value<int>();
                                    whiteTag.value = (double)in_tag.SelectToken("value")?.Value<double>();
                                    whiteTag.IsOverall = (bool)in_tag.SelectToken("IsOverall")?.Value<bool>();
                                    whiteTag.overallValues = JsonConvert.DeserializeObject<List<OverallValue>>(in_tag.SelectToken("overallValues").ToString());
                                    z.whiteTagsNEW.Add(whiteTag);
                                }
                            }
                            Info.characters.Add(z);
                        }
                    }
                }
                StudioList = Info.characters.Select(t => t.studioId).Distinct().ToList()!;
                StudioList.Insert(0, "All");
                ProfList = Info.characters.Select(t => t.professions.ProfToDecode).Distinct().ToList()!;
                ProfList.Insert(0, "All");
                Filter_Prof = ProfList[0];
                Filter_studio = StudioList[0];
                //Info.Mycharacters = [.. Info.characters.Where(t => t.studioId == "PL" && t.professions.GetProfession != Professions.Profession.Else)];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async Task<bool> WriteChange()
        {
            try
            {
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
                    z["reputation"] = Info.reputation;//.ToString("0.0", CultureInfo.InvariantCulture);
                    z["budget"] = Info.budget;
                    z["cash"] = Info.cash;
                    z["influence"] = Info.influence;

                    foreach (var item in Info.characters)
                    {
                        if (item.WasChanged)
                        {
                            var a = z["characters"];
                            var b = a?.SingleOrDefault(t => t?["id"]?.Value<int>() == item.id, null);
                            if (b is not null)
                            {
                                b["limit"] = item.limit;
                                b["mood"] = item.mood;
                                b["attitude"] = item.attitude;
                                b["birthDate"] = item.birthDate;
                                b["studioId"] = item.studioId;
                                var cnt = b["contract"];
                                if (cnt is not null && cnt.HasValues)
                                {
                                    cnt["amount"] = item.contract.amount;
                                    cnt["startAmount"] = item.contract.startAmount;
                                    cnt["initialFee"] = item.contract.initialFee;
                                    cnt["monthlySalary"] = item.contract.monthlySalary;
                                    cnt["weightToSalary"] = item.contract.weightToSalary;
                                    cnt["dateOfSigning"] = item.contract.dateOfSigning;
                                }
                                if (item.CustomNameWasSetted)
                                    b["customName"] = item.MyCustomName;
                                //b["professions"][item.professions.ProfAsString] = item.professions.SetterVal;
                            }

                        }
                    }

                    await File.WriteAllTextAsync(sfd.FileName, jobj.ToString(Formatting.None));
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


        }
    }
    public class DateTimeToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToString("dd.MM.yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    public class LangStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = (string?)value;
            if (str == "COM" | str == "ART")
                str = $"STATUS_{str}_SORT";
            if (str == "INDOOR" | str == "OUTDOOR")
                str = $"SKILL_{str}_SORT";
            string str_out = MainModel.LocaleTranslator.ContainsKey(str) ? MainModel.LocaleTranslator[str] : str;

            if (str_out is not null)
                if (str_out.Contains("PROFESSION_"))
                    return str_out.Replace("PROFESSION_", "").ToLower();
                else if (str_out == "PL")
                    return MainModel.MyStudio;
            if (string.IsNullOrWhiteSpace(str_out))
                return str;
            return str_out!;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    public class CommandHandler : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public CommandHandler(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
                return true;
            return _canExecute(parameter!);
        }

        public void Execute(object? parameter)
        {
            _execute?.Invoke(parameter!);
        }
    }
}
