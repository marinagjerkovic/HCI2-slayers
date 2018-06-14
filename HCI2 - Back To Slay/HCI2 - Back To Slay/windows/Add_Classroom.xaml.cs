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
using System.IO;

namespace HCI2___Back_To_Slay.windows
{
    /// <summary>
    /// Interaction logic for Add_Classroom.xaml
    /// </summary>
    public partial class Add_Classroom : Window
    {

        private Classroom current_cr;

        public Add_Classroom()
        {
            current_cr = new Classroom();
            InitializeComponent();
        }

        private void add_classroom(object sender, RoutedEventArgs e)
        {
            if (MainWindow.allClassroomsIds.Contains(id.Text))
            {
                MessageBox.Show("Change id - classroom with same id already exists!");
                return;
            }

            if(id.Text.Equals("") || description.Text.Equals("") || current_cr.Software.Count() == 0)
            {
                MessageBox.Show("You didn't input all fields - check again!");
                return;
            }

            current_cr.Id = id.Text;
            current_cr.Description = description.Text;

            int num = 0;
            if (Int32.TryParse(num_of_seats.Text, out num))
            {
                current_cr.Num_of_seats = num;
            }
            else
            {
                MessageBox.Show("Number of seats must be a number!");
                return;
            }

            if (projYes.IsChecked==true)
            {
                current_cr.Projector = true;
            }
            else
            {
                current_cr.Projector = false;
            }

            if (boardYes.IsChecked == true)
            {
                current_cr.Board = true;
            }
            else
            {
                current_cr.Board = false;
            }

            if (smartYes.IsChecked == true)
            {
                current_cr.Smart_board = true;
            }
            else
            {
                current_cr.Smart_board = false;
            }

            if (win.IsChecked == true)
            {
                current_cr.Os = Classroom.OpSystem.Windows;
            }
            else if (lin.IsChecked == true)
            {
                current_cr.Os = Classroom.OpSystem.Linux;
            }
            else
            {
                current_cr.Os = Classroom.OpSystem.WindowsAndLinux;
            }

            //dodaj softver

            MainWindow.allClassroomsIds.Add(id.Text);
            MainWindow.allClassrooms.Add(current_cr);
            MainWindow.enableSchedule();

            MessageBox.Show("Successfully added a new classroom!");
            id.Text = "";
            description.Text = "";
            num_of_seats.Text = "";
            current_cr = new Classroom();
            //this.Close();
        }

        private void cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void choose_software(object sender, RoutedEventArgs e)
        {
            Classroom.OpSystem a;
            if (win.IsChecked == true)
            {
                a = Classroom.OpSystem.Windows;
            }else if(lin.IsChecked==true){
                a = Classroom.OpSystem.Linux;
            }else
            {
                a = Classroom.OpSystem.WindowsAndLinux;
            }
            Choose_Software.chosen_software = current_cr.Software;
            Choose_Software cs = new Choose_Software(a);
            cs.Closed += new EventHandler((sender2, e2) => load_data(sender2, e2));
            cs.ShowDialog();
        }

        private void load_data(object sender, EventArgs e)
        {
            current_cr.Software = Choose_Software.chosen_software;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int indeks=0;
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i].Title.Equals("Add Classroom"))
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
