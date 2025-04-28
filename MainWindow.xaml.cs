using System.IO.Compression;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO.Packaging;
using System.Diagnostics;

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
        //ZipFile.CreateFromDirectory("C:\\Users\\bigja\\source\\repos\\HollyJson\\Resources\\Localization", "C:\\Users\\bigja\\source\\repos\\HollyJson\\Resources\\Localization.yz");
    }
    private void NumberValidation(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex(@"[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
    private void DoubleValidation(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex(@"^\d+(?:\.\d+)$");
        e.Handled = regex.IsMatch(e.Text);
    }
    private void LimitValidation(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex(@"^[0](?:\.\d+)|(1\.0)$");
        e.Handled = regex.IsMatch(e.Text);
    }
    private bool IsTextAllowed(string text)
    {

        Regex regex = new Regex(@"^\d+(\.\d+)?$");
        return regex.IsMatch(text);
    }
    private void PastingHandler(object sender, DataObjectPastingEventArgs e)
    {
        if (sender.GetType().Name == "TextBox")
        {
            var t = (TextBox)sender;
            if (t.Tag is not null && t.Tag.ToString() == "NoChecking")
                return;
        }
        if (e.DataObject.GetDataPresent(typeof(String)))
        {
            String text = (String)e.DataObject.GetData(typeof(String));
            if (!IsTextAllowed(text)) e.CancelCommand();
        }
        else e.CancelCommand();
    }
    private void TextBox_KeyDownHandler(object sender, KeyEventArgs e)
    {
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var z = (sender as TextBox);
        if (z.Tag is not null && z.Tag.ToString() == "NoChecking")
            return;
        if (string.IsNullOrEmpty(z.Text))
        {
            e.Handled = true;
            z.Text = "0";
        }

    }
}