using HollyJson.ViewModels;
using System.Windows.Input;

namespace HollyJson.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainControl : ControlBase
    {
        public MainControl()
        {
            InitializeComponent();
            //this.Style = (Style)FindResource(typeof(Window));

            //string mi = $"{App.PathToExe}Resources";
            //string local_dir = $"{mi}\\Localization\\";
            //string path_to_loc = $"{mi}\\Localization.yz";
            //bool arch_loc_exist = Path.Exists(path_to_loc);
            //if (arch_loc_exist)
            //    File.Delete(path_to_loc);
            //ZipFile.CreateFromDirectory(local_dir, path_to_loc);
            //ZipFile.CreateFromDirectory("C:\\Users\\bigja\\source\\repos\\HollyJson\\Resources\\Localization", "C:\\Users\\bigja\\source\\repos\\HollyJson\\Resources\\Localization.yz");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Alt)
            {
                if (e.KeyboardDevice.IsKeyDown(Key.Down))
                {
                    (this.DataContext as MainModel).MoveInFilteredCmd.Execute("Down");
                    dgr.ScrollIntoView(dgr.SelectedItem);
                    e.Handled = true;
                }
                else if (e.KeyboardDevice.IsKeyDown(Key.Up))
                {
                    (this.DataContext as MainModel).MoveInFilteredCmd.Execute("Up");
                    dgr.ScrollIntoView(dgr.SelectedItem);
                    e.Handled = true;
                }

            }
        }
    }
}