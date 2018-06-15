using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace HCI2___Back_To_Slay.windows
{
    /// <summary>
    /// Interaction logic for Software_Multiple.xaml
    /// </summary>
    /// shows given software data; if argument is null, shows all software
    public partial class Software_Multiple : Window
    {
        private ObservableCollection<Software> Show_Data = new ObservableCollection<Software>();
        private ObservableCollection<Software> temp = new ObservableCollection<Software>();
        //private Classroom current_cr = null;

        //trenutno predstavljen softver
        private List<Software> current_sws = new List<Software>();
        private Classroom.OpSystem current_os;

        private int index = -1;
        private bool sub;

        //constructor
        public Software_Multiple()
        {
            Show_Data = MainWindow.allSoftware;
            InitializeComponent();
            dataGrid.ItemsSource = Show_Data;

            load_kombo();
        }

        public Software_Multiple(Classroom cr)
        {
            current_sws = cr.Software;
            current_os = cr.Os;
            index = MainWindow.allClassrooms.IndexOf(cr);
            sub = false;

            foreach(Software sw in current_sws)
            {
                Show_Data.Add(sw);
            }
            InitializeComponent();
            dataGrid.ItemsSource = Show_Data;
            ar_sw.Visibility = Visibility.Visible;

            load_kombo();
        }

        public Software_Multiple(Subject subj)
        {
            current_sws = subj.Software;
            current_os = subj.Os;
            index = MainWindow.allSubjects.IndexOf(subj);
            sub = true;

            foreach (Software sw in current_sws)
            {
                Show_Data.Add(sw);
            }
            InitializeComponent();
            dataGrid.ItemsSource = Show_Data;
            ar_sw.Visibility = Visibility.Visible;

            load_kombo();
        }

        private void load_kombo()
        {
            kombo.Items.Add("id");
            kombo.Items.Add("description");
            kombo.Items.Add("name");
            kombo.Items.Add("maker");
        }

        private void choose_software(object sender, RoutedEventArgs e)
        {
            Choose_Software.chosen_software = current_sws;
            Choose_Software sw = new Choose_Software(current_os);
            sw.Closed += new EventHandler((sender2, e2) => load_data(sender2, e2));
            sw.ShowDialog();
        }

        private void show_software(object sender, RoutedEventArgs e)
        {
            Software_Info si = new Software_Info((Software) dataGrid.SelectedItem);

            si.Closed += new EventHandler((sender2, e2) => check_data(sender2, e2));
            
            si.ShowDialog();
        }

        private void load_data(object sender, EventArgs e)
        {
            current_sws = Choose_Software.chosen_software;
            Show_Data = new ObservableCollection<Software>();
            foreach(Software sw in current_sws)
            {
                Show_Data.Add(sw);
            }
            if (sub)
            {
                MainWindow.allSubjects.ElementAt(index).Software = current_sws;
            }
            else
            {
                MainWindow.allClassrooms.ElementAt(index).Software = current_sws;
            }
            if (temp != null)
            {
                string search_crit = (string)kombo.SelectedItem;
                string search_text = search_tb.Text;
                load_temp(search_crit, search_text, false);
                dataGrid.ItemsSource = temp;
                return;
            }
            dataGrid.ItemsSource = Show_Data;
        }


        private void criteria_changed(object sender, RoutedEventArgs e)
        {
            search_tb.Text = "";
        }

        private void search_software(object sender, RoutedEventArgs e)
        {
            string search_text = search_tb.Text.ToLower();
            string search_crit = (string)kombo.SelectedItem;
            if (search_text.Equals(""))
            {
                temp = null;
                dataGrid.ItemsSource = Show_Data;
            }
            else
            {
                load_temp(search_crit, search_text, true);
                dataGrid.ItemsSource = temp;
            }
        }

        private void load_temp(string search_crit, string search_text, bool reload)
        {
            temp = new ObservableCollection<Software>();

            switch (search_crit)
            {
                case ("id"):
                    foreach (Software sw in Show_Data)
                    {
                        if (sw.Id.ToLower().Contains(search_text))
                        {
                            temp.Add(sw);
                        }
                    }
                    break;
                case ("description"):
                    foreach (Software sw in Show_Data)
                    {
                        if (sw.Description.ToLower().Contains(search_text))
                        {
                            temp.Add(sw);
                        }
                    }
                    break;
                case ("name"):
                    foreach (Software sw in Show_Data)
                    {
                        if (sw.Name.ToLower().Contains(search_text))
                        {
                            temp.Add(sw);
                        }
                    }
                    break;
                case ("maker"):
                    foreach(Software sw in Show_Data)
                    {
                        if (sw.Maker.ToLower().Contains(search_text))
                        {
                            temp.Add(sw);
                        }
                    }
                    break;
                case (null):
                    if (reload)
                    {
                        MessageBox.Show("No search criterium selected!");
                    }
                    if (index != -1)
                    {
                        reload_showing();
                    }else
                    {
                        temp = Show_Data;
                    }
                    break;
            }
            if (temp.Count() == 0)
            {
                MessageBox.Show("No software found!");
                return;
            }
        }

        private void reload_showing()
        {
            temp = new ObservableCollection<Software>();
            foreach(Software sw in MainWindow.allSoftware)
            {
                if (Show_Data.Contains(sw))
                {
                    temp.Add(sw);
                }
            }
        }

        private void check_data(object sender, EventArgs e)
        {
            if (temp != null)
            {
                string search_crit = (string)kombo.SelectedItem;
                string search_text = search_tb.Text;
                load_temp(search_crit, search_text, false);
                dataGrid.ItemsSource = temp;
            }
        }


        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int indeks = 0;
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i].Title.Equals("Software_Multiple"))
                {
                    indeks = i;
                }
            }

            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[indeks]);
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp(str, this);
            }
        }
    }
}
