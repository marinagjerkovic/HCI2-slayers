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
    /// Interaction logic for Classroom_Multiple.xaml
    /// </summary>
    public partial class Classroom_Multiple : Window
    {

        private ObservableCollection<Classroom> temp = new ObservableCollection<Classroom>();

        public Classroom_Multiple()
        {
            InitializeComponent();
            dataGrid.ItemsSource = MainWindow.allClassrooms;
            kombo.Items.Add("id");
            kombo.Items.Add("description");
        }

        private void show_classroom(object sender, RoutedEventArgs e)
        {
            Classroom_Info ci = new Classroom_Info((Classroom)dataGrid.SelectedItem);
            ci.Closed += new EventHandler((sender2, e2) => check_data(sender2, e2));
            ci.ShowDialog();
        }

        private void show_software(object sender, RoutedEventArgs e)
        {
            Software_Multiple sm = new Software_Multiple((Classroom)dataGrid.SelectedItem);
            sm.ShowDialog();
        }

        private void search_classrooms(object sender, RoutedEventArgs e)
        {
            string search_text = search_tb.Text.ToLower();
            string search_crit = (string) kombo.SelectedItem;
            if (search_text.Equals(""))
            {
                temp = null;
                dataGrid.ItemsSource = MainWindow.allClassrooms;
            }else
            {
                load_temp(search_crit, search_text, true);
                dataGrid.ItemsSource = temp;
            }
        }

        private void load_temp(string search_crit, string search_text, bool reload)
        {
            temp = new ObservableCollection<Classroom>();
            switch (search_crit)
            {
                case ("id"):
                    foreach (Classroom cr in MainWindow.allClassrooms)
                    {
                        if (cr.Id.ToLower().Contains(search_text))
                        {
                            temp.Add(cr);
                        }
                    }
                    break;
                case ("description"):
                    foreach (Classroom cr in MainWindow.allClassrooms)
                    {
                        if (cr.Description.ToLower().Contains(search_text))
                        {
                            temp.Add(cr);
                        }
                    }
                    break;
                case (null):
                    if (reload)
                    {
                        MessageBox.Show("No search criterium selected!");
                    }
                    temp = MainWindow.allClassrooms;
                    break;
            }
            if (temp.Count() == 0)
            {
                MessageBox.Show("No classrooms found!");
                return;
            }
        }

        private void check_data(object sender, EventArgs e)
        {
            if (temp != null)
            {
                string search_text = search_tb.Text.ToLower();
                string search_crit = (string)kombo.SelectedItem;
                load_temp(search_crit, search_text, false);
                dataGrid.ItemsSource = temp;
            }
        }

        private void criteria_changed(object sender, EventArgs e)
        {
            search_tb.Text = "";
        }

        private void add_new_classroom(object sender, RoutedEventArgs e)
        {
            Add_Classroom ac = new Add_Classroom();
            ac.ShowDialog();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int indeks = 0;
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i].Title.Equals("Classroom_Multiple"))
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
