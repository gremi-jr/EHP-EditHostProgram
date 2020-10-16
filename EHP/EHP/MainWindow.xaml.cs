using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using Path = System.IO.Path;

namespace EHP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string PathToHostFile = "C:\\Windows\\System32\\drivers\\etc\\hosts";
        private List<string> ListApplication = new List<string>();
        private List<string> ListIPs = new List<string>();
        private List<string> ListDescription = new List<string>();

        private bool AlreadyHasEntry = false;

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            string ConfigPath = ".\\config.csv";

            using (var reader = new StreamReader(ConfigPath))
            {
                //Reading the frist line to skip it in while header
                string headerLine = reader.ReadLine();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(';');
                    ListApplication.Add(values[0]);
                    ListIPs.Add(values[1]);
                    ListDescription.Add(values[2]);
                }

                
                //Add applications to combobox
                foreach (var applicationItem in ListApplication)
                {
                    //To check if combobox already have the application
                    if (!(ComboBoxApplication.Items.Contains(applicationItem.ToString().ToUpper())))
                    {
                        ComboBoxApplication.Items.Add(applicationItem.ToUpper());
                    }
                }
            }
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxApplication.SelectedItem == null || ComboBoxSystem.SelectedItem == null)
            {
                MessageBox.Show("Please select a application and IP-Address!", "Select a Application", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string[] ContentOfHostFile = System.IO.File.ReadAllLines(PathToHostFile);
                var ListOfContent = new List<string>(ContentOfHostFile);

                for (int i = 0; i < ListOfContent.Count; i++)
                {

                    if (ListOfContent[i].Contains(ComboBoxApplication.SelectedItem.ToString().ToLower()))
                    {

                        AlreadyHasEntry = true;
                        ListOfContent[i] = LabelSystemDescription.Content + " " + ComboBoxApplication.SelectedItem.ToString().ToLower();
                        Console.WriteLine(AlreadyHasEntry);
                    }
                }
                if (AlreadyHasEntry != true)
                {

                    if (String.IsNullOrEmpty(ListOfContent[ListOfContent.Count - 1]))
                    {

                        ListOfContent[ListOfContent.Count - 1] += LabelSystemDescription.Content + " " + ComboBoxApplication.SelectedItem.ToString().ToLower();
                    }
                    else
                    {

                        ListOfContent.Add(LabelSystemDescription.Content + " " + ComboBoxApplication.SelectedItem.ToString().ToLower());
                    }

                }

                try
                {
                    System.IO.File.WriteAllLines(PathToHostFile, ListOfContent);
                    
                    MessageBox.Show("Changes successfully made!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.UnauthorizedAccessException)
                {
                    MessageBox.Show("Unauthorized access! Please restart the application as administrator!", "Unauthorized access!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ComboBoxSystem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(ComboBoxSystem.SelectedIndex == -1))
            {
                int IndexOfDescription = ListDescription.IndexOf(ComboBoxSystem.SelectedItem.ToString());

                LabelSystemDescription.Content = ListIPs[IndexOfDescription];
            }
        }

        private void ComboBoxApplication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Clears combobox so that only the description of the selectet application is shown
            ComboBoxSystem.Items.Clear();
            //Reset of descrition so that after changing the application the description label is emty
            LabelSystemDescription.Content = "";

            for (int i=0; i <= ListApplication.Count-1; i++)
            {
                if (ComboBoxApplication.SelectedItem.ToString().ToLower() == ListApplication[i])
                {
                    ComboBoxSystem.Items.Add(ListDescription[i]);
                }
            }
        }

        private void ButtonStandard_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxApplication.SelectedItem == null)
            {
                MessageBox.Show("Please select a application!", "Select a Application", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var ContentOfHostFile = System.IO.File.ReadAllLines(PathToHostFile);
                var ListOfContent = new List<string>(ContentOfHostFile);

                for (int i = 0; i < ListOfContent.Count; i++)
                {
                    if (ListOfContent[i].Contains(ComboBoxApplication.SelectedItem.ToString().ToLower()))
                    {
                        ListOfContent[i] = "";
                    }
                }
                try
                {
                    System.IO.File.WriteAllLines(PathToHostFile, ListOfContent);
                    AlreadyHasEntry = false;
                    MessageBox.Show("Entries restored!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch(System.UnauthorizedAccessException)
                {
                    MessageBox.Show("Unauthorized access! Please restart the application as administrator!", "Unauthorized access!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void MenuItemHelp_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow help = new HelpWindow();
            help.Show();
            
        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Show();
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
