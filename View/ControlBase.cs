using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HollyJson.View
{
    public partial class ControlBase : UserControl
    {
        protected bool CheckDouble(string text) => Regex.IsMatch(text, @"^[0-9\.]$");
        protected bool CheckInteger(string text) => Regex.IsMatch(text, @"^[0-9]$");
        protected bool CheckString(string text) => Regex.IsMatch(text, @"^[\p{L} ]+$");
        protected bool CheckDoubleFull(string text) => Regex.IsMatch(text, @"^(\d+(\.\d+)?)$");
        protected bool CheckIntegerFull(string text) => Regex.IsMatch(text, @"^([0-9]+)$");
        protected bool CheckLimitOneFull(string text) => Regex.IsMatch(text, @"^((0\.\d+)|(1\.0)|([1,0]))$");
        protected bool CheckAgeFull(string text) => Regex.IsMatch(text, @"^[0-1]?[0-9][0-9]$");
        protected void PastingHandler(object sender, DataObjectPastingEventArgs e)
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
                            if (tags == "AGE")
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
                        break;
                    default:
                        break;
                }
                if (!valid) e.CancelCommand();
            }
        }
        protected void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
        protected void TextBox_TextChanged(object sender, TextChangedEventArgs e)
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
                    if (e.Handled)
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
}