using System.IO.Compression;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO.Packaging;
using System.Diagnostics;
using System.Globalization;
using HollyJson.ViewModels;

namespace HollyJson;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Style = (Style)FindResource(typeof(Window));
        //string mi = $"{App.PathToExe}Resources";
        //string local_dir = $"{mi}\\Localization\\";
        //string path_to_loc = $"{mi}\\Localization.yz";
        //bool arch_loc_exist = Path.Exists(path_to_loc);
        //if (arch_loc_exist)
        //    File.Delete(path_to_loc);
        //ZipFile.CreateFromDirectory(local_dir, path_to_loc);
        //ZipFile.CreateFromDirectory("C:\\Users\\bigja\\source\\repos\\HollyJson\\Resources\\Localization", "C:\\Users\\bigja\\source\\repos\\HollyJson\\Resources\\Localization.yz");
    }

    //на ввод посимвольно
    //целык
    private void NumberValidation(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex(@"[^0-9]");
        e.Handled = regex.IsMatch(e.Text);
    }
    //с точкой
    private void DoubleValidation(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex(@"[^0-9\.\s]");//(@"^\d+(?:\.\d+)$");
        e.Handled = regex.IsMatch(e.Text);
    }

    private bool CheckDouble(string text) => Regex.IsMatch(text, @"^[0-9\.]$");
    private bool CheckInteger(string text) => Regex.IsMatch(text, @"^[0-9]$");
    private bool CheckString(string text) => Regex.IsMatch(text, @"^[\p{L} ]+$");
    private bool CheckDoubleFull(string text) => Regex.IsMatch(text, @"^(\d+(\.\d+)?)$");
    private bool CheckIntegerFull(string text) => Regex.IsMatch(text, @"^([0-9]+)$");
    private bool CheckLimitOneFull(string text) => Regex.IsMatch(text, @"^((0\.\d+)|(1\.0)|([1,0]))$");
    private bool CheckAgeFull(string text) => Regex.IsMatch(text, @"^[0-1]?[0-9][0-9]$");

    private void PastingHandler(object sender, DataObjectPastingEventArgs e)
    {
        if (sender.GetType().Name == "TextBox")
        {
            var z = (TextBox)sender;
            string tags = z.Tag?.ToString();
            string val = (string)e.DataObject.GetData(typeof(string));
            bool valid = false;

            switch (tags)
            {
                case "STR":
                    if (!string.IsNullOrEmpty(val))
                        valid = CheckString(val);
                    break;
                case "INT":
                case "AGE":
                    double w = 0;
                    if (double.TryParse(val, CultureInfo.InvariantCulture, out w))
                    {
                        int ans = (int)Math.Round(w, 0, MidpointRounding.AwayFromZero);
                        if(tags == "AGE")
                            if (ans > 150)
                                ans = 90;
                        val = ans.ToString("0");
                        DataObject d = new DataObject();
                        d.SetData(DataFormats.Text, val);
                        e.DataObject = d;
                    }
                    if (tags == "AGE")
                        valid = CheckAgeFull(val);
                    else
                        valid = CheckIntegerFull(val);
                    break;
                case "DBL":
                    valid = CheckDoubleFull(val);
                    break;
                case "LMT":
                    valid = CheckLimitOneFull(val);
                    //if (e.Handled)
                    //{
                    //    double val = 0.0d;
                    //    if (double.TryParse(z.Text, CultureInfo.InvariantCulture, out val))
                    //    {
                    //        if (val > 1.0d)
                    //            z.Text = 1.0d.ToString("0.00", CultureInfo.InvariantCulture);
                    //    }
                    //}
                    break;
                default:
                    break;
            }
            if (!valid) e.CancelCommand();
        }
    }


    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var z = (sender as TextBox);
        string tags = z.Tag?.ToString();
        switch (tags)
        {
            case "STR":
                e.Handled = !CheckString(e.Text);
                break;
            case "INT":
            case "AGE":
                e.Handled = !CheckInteger(e.Text);
                break;
            case "DBL":
            case "LMT":
                e.Handled = !CheckDouble(e.Text);
                break;
            default:
                break;
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var z = (sender as TextBox);
        //все таки по тагу смотреть... и переключаться на нужные проверки...
        string tags = z.Tag?.ToString();
        if (z.Text == "∞")
            return;
        switch (tags)
        {
            case "STR":
                if (!string.IsNullOrEmpty(z.Text))
                    e.Handled = !CheckString(z.Text);
                break;
            case "INT":
                e.Handled = !CheckIntegerFull(z.Text);
                break;
            case "AGE":
                e.Handled = !CheckAgeFull(z.Text);
                if(e.Handled)
                {
                    int val = 0;
                    int.TryParse(z.Text, CultureInfo.InvariantCulture, out val);
                    if (val > 150)
                        z.Text = 90.ToString();
                }
                break;
            case "DBL":
                e.Handled = !CheckDoubleFull(z.Text);
                break;
            case "LMT":
                e.Handled = !CheckLimitOneFull(z.Text);
                if (e.Handled)
                {
                    double val = 0.0d;
                    if (double.TryParse(z.Text, CultureInfo.InvariantCulture, out val))
                    {
                        if (val > 1.0d)
                            z.Text = 1.0d.ToString("0.00", CultureInfo.InvariantCulture);
                    }
                }
                break;
            default:
                break;
        }

    }


}