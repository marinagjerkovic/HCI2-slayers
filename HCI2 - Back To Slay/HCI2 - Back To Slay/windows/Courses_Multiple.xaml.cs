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
using System.ComponentModel;

namespace HCI2___Back_To_Slay.windows
{
    /// <summary>
    /// Interaction logic for Courses_Multiple.xaml
    /// </summary>
    public partial class Courses_Multiple : Window
    {
        public static Course selected_crs;
        public static RoutedCommand exitDemoMode = new RoutedCommand();

        private ObservableCollection<Course> temp = new ObservableCollection<Course>();

        public Courses_Multiple(bool show)
        {
            this.DataContext = this;
            exitDemoMode.InputGestures.Add(new KeyGesture(Key.Escape));
            this.Closing += new CancelEventHandler(Window_Closing);

            InitializeComponent();
            dataGrid.ItemsSource = MainWindow.allCourses;
            kombo.Items.Add("id");
            kombo.Items.Add("name");
            kombo.Items.Add("description");

            if (show)
            {
                save_crs_btn.Visibility = System.Windows.Visibility.Visible;
                dataGrid.SelectedIndex = 2;
                
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

        private void add_new_course(object sender, RoutedEventArgs e)
        {
            Add_Course ac = new Add_Course();
            ac.ShowDialog();
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

        private void exit_demo(object sender, RoutedEventArgs e)
        {
            if (MainWindow.demoModeOn)
            {
                //radi sta se radi kad se iskljuci demo
                MainWindow.turnOfDemo = true;
                //MessageBox.Show("exit demo");
                
            }

        }

        public void Window_Closing(object sender, CancelEventArgs e)
        {
            if (MainWindow.demoModeOn)
            {
                e.Cancel = true;
                if (MainWindow.turnOfDemo)
                {
                    //MessageBox.Show("Wait until demo creating subject finishes");
                }
                else
                {
                    //MessageBox.Show("Press 'Esc' key on keyboard if you want to stop demo mode and wait until it finishes");
                }

                
            }

        }


    }
}
