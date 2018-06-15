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
    /// Interaction logic for Subject_Multiple.xaml
    /// </summary>
    public partial class Subject_Multiple : Window
    {

        private ObservableCollection<Subject> temp = new ObservableCollection<Subject>();

        public Subject_Multiple()
        {
            InitializeComponent();
            dataGrid.ItemsSource = MainWindow.allSubjects;

            kombo.Items.Add("id");
            kombo.Items.Add("name");
            kombo.Items.Add("description");
        }

        private void show_subject(object sender, RoutedEventArgs e)
        {
            Subject_Info si = new Subject_Info((Subject)dataGrid.SelectedItem);
            si.Closed += new EventHandler((sender2, e2) => check_data(sender2, e2));
            si.ShowDialog();
        }


        private void show_software(object sender, RoutedEventArgs e)
        {
            Software_Multiple sm = new Software_Multiple((Subject)dataGrid.SelectedItem);
            sm.ShowDialog();
        }

        private void search_subjects(object sender, RoutedEventArgs e)
        {
            string search_text = search_tb.Text.ToLower();
            string search_crit = (string)kombo.SelectedItem;
            if (search_text.Equals(""))
            {
                temp = null;
                dataGrid.ItemsSource = MainWindow.allSubjects;
            }
            else
            {
                load_temp(search_crit, search_text, true);
                dataGrid.ItemsSource = temp;
            }
        }

        private void load_temp(string search_crit, string search_text, bool reload)
        {
            temp = new ObservableCollection<Subject>();
            switch (search_crit)
            {
                case ("id"):
                    foreach (Subject sub in MainWindow.allSubjects)
                    {
                        if (sub.Id.ToLower().Contains(search_text))
                        {
                            temp.Add(sub);
                        }
                    }
                    break;
                case ("description"):
                    foreach (Subject sub in MainWindow.allSubjects)
                    {
                        if (sub.Description.ToLower().Contains(search_text))
                        {
                            temp.Add(sub);
                        }
                    }
                    break;
                case (null):
                    if (reload)
                    {
                        MessageBox.Show("No search criterium selected!");
                    }
                    temp = MainWindow.allSubjects;
                    break;
            }
            if (temp.Count() == 0)
            {
                MessageBox.Show("No subjects found!");
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

        private void add_new_subject(object sender, RoutedEventArgs e)
        {
            Add_Subject asub = new Add_Subject();
            asub.ShowDialog();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int indeks = 0;
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i].Title.Equals("Subject_Multiple"))
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
