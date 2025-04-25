using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.Caching;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace HollyJson
{

    [AddINotifyPropertyChangedInterface]
    public class MainModel
    {
        private bool names_loaded = false;
        private string opennedfileplace = string.Empty;
        CommandHandler _savefile;
        CommandHandler _openfile;
        private string search_txt;
        JObject? jobj = null;
        private Character selectedChar;

        public static Dictionary<string, string> LocaleNames { get; set; } = [];
        public static Dictionary<string, string> LocaleTranslator { get; set; } = [];
        public stateJson Info { get; set; }
        public ObservableCollection<Character> FilteredObj { get; set; }
        public Character SelectedChar { get => selectedChar; set => selectedChar = value; }
        public List<string> StudioListForChar { get; set; }
        public bool Save_Loaded { get; set; } = false;
        public bool Save_done { get; set; } = false;

        public List<string> StudioList { get; set; }
        public List<string> ProfList { get; set; }
        public string Filter_Prof { get; set; }
        public string Filter_studio { get; set; }
        public string Filter_txt { get => search_txt; set => search_txt = value; }

        private async void SetLocale(string path)
        {
            await LoadNamesFromJson(path);
            await LoadLocaleFromJson(path);

            if (Save_Loaded | names_loaded)
                foreach (var t in Info.characters)
                {
                    t.normalLast = LocaleNames[t.lastNameId];
                    t.normalFirst = LocaleNames[t.firstNameId];
                }

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
                            var ofd = new OpenFileDialog();
                            ofd.Multiselect = false;
                            ofd.Title = "Select names file (Hollywood Animal\\Holly_Data\\StreamingAssets\\Data\\Localization\\RUS\\)";
                            ofd.DefaultExt = ".json";
                            ofd.FileName = "CHARACTER_NAMES.json";
                            ofd.RestoreDirectory = true;
                            ofd.Filter = "Json|*.json";
                            ofd.ShowHiddenItems = true;
                            if (ofd.ShowDialog() == true)
                            {
                                await LoadNamesFromJson(ofd.FileName);
                                names_loaded = true;
                                if (Save_Loaded)
                                    foreach (var t in Info.characters)
                                    {
                                        t.normalLast = LocaleNames[t.lastNameId];
                                        t.normalFirst = LocaleNames[t.firstNameId];
                                    }
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
                                    FilteredObj = Info.characters;
                                    Save_Loaded = true;
                                    //SelectedChar = Info.Mycharacters[0];
                                    if (names_loaded)
                                        foreach (var t in Info.characters)
                                        {
                                            t.normalLast = LocaleNames[t.lastNameId];
                                            t.normalFirst = LocaleNames[t.firstNameId];
                                        }
                                });
                                GC.Collect();
                            }

                        }
                    }
                    catch (System.Exception)
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
        public async Task LoadLocaleFromJson(string path)
        {
            string json = await File.ReadAllTextAsync(path); //"PROTAGONIST_WARRIOR": 26,
            var map = JObject.Parse(json).SelectToken("IdMap");
            List<string> maplbl = JsonConvert.DeserializeObject<List<string>>(map.ToString());
            var local = JObject.Parse(json).SelectToken("locStrings");//array[int] = answer
                                                                      //должны получить на выходе - вирд строка : нормальная строка
            List<string> getout = JsonConvert.DeserializeObject<List<string>>(local.ToString());
            LocaleTranslator = new Dictionary<string, string>();
            int ii = 0;
            maplbl.ForEach(t =>
            {
                LocaleTranslator.Add(t, getout[ii++]);
            });
        }
        public async Task LoadNamesFromJson(string path)
        {
            try
            {
                string json = await File.ReadAllTextAsync(path);
                var dt = JObject.Parse(json).SelectToken("locStrings");
                List<string> names = JsonConvert.DeserializeObject<List<string>>(dt.ToString());
                LocaleNames = new Dictionary<string, string>();
                int ii = 0;
                names.ForEach(t =>
                {
                    LocaleNames.Add(ii++.ToString(), t);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                
                //var reader = new JsonTextReader(new StringReader(jsonstr));
                var aa = jobj.SelectToken("stateJson");
                //Info = JsonConvert.DeserializeObject<stateJson>(aa.ToString());
                //переезжаем на дессирализцаию в ручную...
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
                            Info.NextSpawnDays.Add(prop.Name,prop.Value.ToObject<DateTime>());
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
                        var q_val = qqq.Value;
                        //qqq[0].
                        //z.professions.PropertyProf = ;
                        if (z is not null)
                        {
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
                                    if (whiteTag.Tagtype == Tags.ELSE) //срезаем то что не отслеживаем, ибо нафиг
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
                StudioList = Info.characters.Where(t => t.studioId is not null).Select(t => t.studioId).Distinct().ToList()!;
                //jobj = null;
                //StudioList.Insert(0, "All");
                //ProfList = Info.characters.Where(t => t.professions.GetProfession != Professions.Profession.Else).Select(t => t.professions.ProfAsString).Distinct().ToList()!;
                //ProfList.Insert(0, "All");
                //Filter_Prof = ProfList[0];
                //Filter_studio = StudioList[0];
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
            return MainModel.LocaleTranslator.ContainsKey((string)value) ? MainModel.LocaleTranslator[(string)value] : value;
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
