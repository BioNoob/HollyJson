using Newtonsoft.Json;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace HollyJson.ViewModels
{
    public class DoubleJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsSubclassOf(typeof(double));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return double.Parse((string)reader.Value, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is not null)
            {
                string val = ((double)value).ToString("#0.000", CultureInfo.InvariantCulture);
                writer.WriteValue(val);
            }
            else
                writer.WriteNull();
        }
    }
    public class IntJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsSubclassOf(typeof(int));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return int.Parse((string)reader.Value, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is not null)
            {
                string val = ((int)value).ToString("#0", CultureInfo.InvariantCulture);
                writer.WriteValue(val);
            }
            else
                writer.WriteNull();
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
        //public event EventHandler? CanExecuteChanged;
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public CommandHandler(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            //if (_canExecute == null)
            //    return true;
            //return _canExecute(parameter!);
            return _canExecute == null || _canExecute(parameter!);
        }

        public void Execute(object? parameter)
        {
            _execute?.Invoke(parameter!);
        }
    }
}
