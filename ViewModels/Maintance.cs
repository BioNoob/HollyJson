using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace HollyJson.ViewModels
{
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
