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
    /// Interaction logic for Courses_Multiple.xaml
    /// </summary>
    public partial class Courses_Multiple : Window
    {
        public static Course selected_crs;

        private ObservableCollection<Course> temp = new ObservableCollection<Course>();

        public Courses_Multiple(bool show)
        {
            
            InitializeComponent();
            dataGrid.ItemsSource = MainWindow.allCourses;
            kombo.Items.Add("id");
            kombo.Items.Add("name");
            kombo.Items.Add("description");

            if (show)
            {
                save_crs_btn.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void show_course(object sender, RoutedEventArgs e)
        {
            Course_Info ci = new Course_Info((Course) dataGrid.SelectedItem);
            ci.Closed += new EventHandler((sender2, e2) => check_data(sender2, e2));
            ci.ShowDialog();
        }

        private void save_course(object sender, RoutedEventArgs e)
        {
            selected_crs = (Course) dataGrid.SelectedItem;
            this.Close();
        }

        private void criteria_changed(object sender, RoutedEventArgs e)
        {
            search_tb.Text = "";
        }

        private void search_courses(object sender, RoutedEventArgs e)
        {
            string search_text = search_tb.Text.ToLower();
            string search_crit = (string)kombo.SelectedItem;
            if (search_text.Equals(""))
            {
                temp = null;
                dataGrid.ItemsSource = MainWindow.allCourses;
            }
            else
            {
                load_temp(search_crit, search_text, true);
                dataGrid.ItemsSource = temp;
            }
        }

        private void load_temp(string search_crit, string search_text, bool reload)
        {
            temp = new ObservableCollection<Course>();

            switch (search_crit)
            {
                case ("id"):
                    foreach (Course crs in MainWindow.allCourses)
                    {
                        if (crs.Id.ToLower().Contains(search_text))
                        {
                            temp.Add(crs);
                        }
                    }
                    break;
                case ("description"):
                    foreach (Course crs in MainWindow.allCourses)
                    {
                        if (crs.Description.ToLower().Contains(search_text))
                        {
                            temp.Add(crs);
                        }
                    }
                    break;
                case ("name"):
                    foreach (Course crs in MainWindow.allCourses)
                    {
                        if (crs.Name.ToLower().Contains(search_text))
                        {
                            temp.Add(crs);
                        }
                    }
                    break;
                case (null):
                    if (reload)
                    {
                        MessageBox.Show("No search criterium selected!");
                    }
                    temp = MainWindow.allCourses;
                    break;
            }
            if (temp.Count() == 0)
            {
                MessageBox.Show("No courses found!");
                return;
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
                if (Application.Current.Windows[i].Title.Equals("Courses_Multiple"))
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
