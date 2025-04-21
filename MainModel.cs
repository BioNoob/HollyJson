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

namespace HollyJson
{

    [AddINotifyPropertyChangedInterface]
    public class MainModel
    {
        private bool names_loaded = false;
        private string opennedfileplace = string.Empty;
        JObject? jobj = null;
        public Dictionary<string, string> DictNames { get; set; }
        public stateJson Info { get; set; }
        public Character SelectedChar { get; set; }
        public bool Save_Loaded { get; set; } = false;

        CommandHandler _savefile;
        CommandHandler _openfile;
        public CommandHandler OpenFileCmd
        {
            get
            {
                return _openfile ??= new CommandHandler(async obj =>
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
                                    t.normalLast = DictNames[t.lastNameId];
                                    t.normalFirst = DictNames[t.firstNameId];
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
                            opennedfileplace = Path.GetDirectoryName(ofdd.FileName);
                            await ParseJson(ofdd.FileName);
                            Save_Loaded = true;
                            SelectedChar = Info.Mycharacters[0];
                            if (names_loaded)
                                foreach (var t in Info.characters)
                                {
                                    t.normalLast = DictNames[t.lastNameId];
                                    t.normalFirst = DictNames[t.firstNameId];
                                }
                        }

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
                    await WriteChange();
                },
                (obj) => true);
            }
        }
        public async Task LoadNamesFromJson(string path)
        {
            try
            {
                string json = await File.ReadAllTextAsync(path);
                var dt = JObject.Parse(json).SelectToken("locStrings");
                List<string> names = JsonConvert.DeserializeObject<List<string>>(dt.ToString());
                DictNames = new Dictionary<string, string>();
                int ii = 0;
                names.ForEach(t =>
                {
                    DictNames.Add(ii++.ToString(), t);
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
                var reader = new JsonTextReader(new StringReader(jsonstr));
                reader.FloatParseHandling = FloatParseHandling.Decimal;
                jobj = JObject.Load(reader);
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
                Info.characters = new ObservableCollection<Character>();
                foreach (var item in aa.SelectToken("characters")?.Children())
                {
                    if (item is not null)
                    {
                        var z = JsonConvert.DeserializeObject<Character>(item.ToString());
                        if (z is not null)
                        {
                            z.JsonString = item.ToString();
                            z.SetFullAge(Info.Now);
                            if (z.contract is not null)
                                z.contract.SetCalcDaysLeft(Info.Now);
                            Info.characters.Add(z);
                        }
                    }
                }
                Info.Mycharacters = [.. Info.characters.Where(t => t.studioId == "PL" && t.professions.GetProfession != Professions.Profession.Else)];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
        public async Task WriteChange()
        {
            Info.reputation += 10;
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
                        if(cnt is not null && cnt.HasValues)
                        {
                            cnt["amount"] = item.contract.amount;
                            cnt["startAmount"] = item.contract.startAmount;
                            cnt["initialFee"] = item.contract.initialFee;
                            cnt["monthlySalary"] = item.contract.monthlySalary;
                            cnt["weightToSalary"] = item.contract.weightToSalary;
                            cnt["dateOfSigning"] = item.contract.dateOfSigning;
                        }
                        b["professions"][item.professions.ProfAsString] = item.professions.SetterVal;
                    }
                        
                }
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Select where to save";
            sfd.DefaultExt = ".json";
            sfd.InitialDirectory = opennedfileplace;
            sfd.RestoreDirectory = true;
            sfd.Filter = "Json|*.json";
            sfd.ShowHiddenItems = true;
            if(sfd.ShowDialog() == true)
            {
                File.WriteAllText(sfd.FileName, jobj.ToString());
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
    public class CommandHandler : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public CommandHandler(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }
    }
}
