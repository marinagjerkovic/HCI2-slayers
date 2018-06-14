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

namespace HCI2___Back_To_Slay.windows
{
    /// <summary>
    /// Interaction logic for Add_Subject.xaml
    /// </summary>
    public partial class Add_Subject : Window
    {
        private Subject current_sub;

        public Add_Subject()
        {
            InitializeComponent();
            current_sub = new Subject();
        }

        private void add_new_subject(object sender, RoutedEventArgs e)
        {
            if (MainWindow.allSubjectsIds.Contains(id.Text))
            {
                MessageBox.Show("Change id - subject with the same id already exists!");
                return;
            }

            current_sub.Id = id.Text;
            current_sub.Name = name.Text;
            current_sub.Description = description.Text;

            int num = 0;
            if (Int32.TryParse(group_size.Text, out num))
            {
                current_sub.Size_of_group = num;
            }
            else
            {
                MessageBox.Show("Size of groups must be a number!");
                return;
            }

            if (Int32.TryParse(num_periods.Text, out num))
            {
                current_sub.Num_of_periods = num;
            }
            else
            {
                MessageBox.Show("Number of periods must be a number!");
                return;
            }

            if (Int32.TryParse(duration.Text, out num))
            {
                current_sub.Duration_of_period = num;
            }
            else
            {
                MessageBox.Show("Duration of periods must be a number!");
                return;
            }

            if (projYes.IsChecked == true)
            {
                current_sub.Projector = true;
            }
            else
            {
                current_sub.Projector = false;
            }

            if (boardYes.IsChecked == true)
            {
                current_sub.Board = true;
            }
            else
            {
                current_sub.Board = false;
            }

            if (smartYes.IsChecked == true)
            {
                current_sub.Smart_board = true;
            }
            else
            {
                current_sub.Smart_board = false;
            }

            if (win.IsChecked == true)
            {
                current_sub.Os = Classroom.OpSystem.Windows;
            }
            else if (linux.IsChecked == true)
            {
                current_sub.Os = Classroom.OpSystem.Linux;
            }
            else
            {
                current_sub.Os = Classroom.OpSystem.WindowsAndLinux;
            }

            //dodaj softver i kurs

            MainWindow.allSubjects.Add(current_sub);
            MainWindow.allSubjectsIds.Add(id.Text);
            MessageBox.Show("Successfully added a new subject!");

            current_sub = new Subject();
            id.Text = "";
            name.Text = "";
            description.Text = "";
            group_size.Text = "";
            num_periods.Text = "";
            duration.Text = "";
            add_crs.Visibility = System.Windows.Visibility.Visible;
            sel_crs.Visibility = System.Windows.Visibility.Hidden;
            change_crs.Visibility = System.Windows.Visibility.Hidden;                        
            Courses_Multiple.selected_crs = null;
            //this.Close();
        }

        private void cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void add_software(object sender, RoutedEventArgs e)
        {
            Classroom.OpSystem a;
            if (win.IsChecked == true)
            {
                a = Classroom.OpSystem.Windows;
            }
            else if (linux.IsChecked == true)
            {
                a = Classroom.OpSystem.Linux;
            }
            else
            {
                a = Classroom.OpSystem.WindowsAndLinux;
            }
            Choose_Software.chosen_software = current_sub.Software;
            Choose_Software cs = new Choose_Software(a);
            cs.Closed += new EventHandler((sender2, e2) => load_data(sender2, e2));
            cs.ShowDialog();
        }

        private void load_data(object sender, EventArgs e)
        {
            current_sub.Software = Choose_Software.chosen_software;
        }

        private void select_course(object sender, EventArgs e)
        {
            Courses_Multiple cm = new Courses_Multiple(true);
            cm.Closed += new EventHandler((sender2, e2) => take_selected(sender2, e2));
            cm.ShowDialog();
        }


        private void take_selected(object sender, EventArgs e)
        {
            if (Courses_Multiple.selected_crs == null)
            {
                return;
            }
            add_crs.Visibility = System.Windows.Visibility.Hidden;
            sel_crs.Visibility = System.Windows.Visibility.Visible;
            change_crs.Visibility = System.Windows.Visibility.Visible;
            current_sub.Course = Courses_Multiple.selected_crs;
            sel_crs.Text = current_sub.Course.Name;
            Courses_Multiple.selected_crs = null;
        }


        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int indeks = 0;
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i].Title.Equals("Add_Subject"))
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
