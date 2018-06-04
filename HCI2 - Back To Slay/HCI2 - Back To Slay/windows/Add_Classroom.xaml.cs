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
        private Classroom cr;

        public Add_Classroom()
        {
            cr = new Classroom();
            InitializeComponent();
        }

        private void add_classroom(object sender, RoutedEventArgs e)
        {
            if (MainWindow.allClassroomsIds.Contains(id.Text))
            {
                MessageBox.Show("Change id - classroom with same id already exists!");
                return;
            }

            cr.Id = id.Text;
            cr.Description = description.Text;

            int num = 0;
            if (Int32.TryParse(num_of_seats.Text, out num))
            {
                cr.Num_of_seats = num;
            }
            else
            {
                MessageBox.Show("Number of seats must be a number!");
                return;
            }

            if (projYes.IsChecked==true)
            {
                cr.Projector = true;
            }
            else
            {
                cr.Projector = false;
            }

            if (boardYes.IsChecked == true)
            {
                cr.Board = true;
            }
            else
            {
                cr.Board = false;
            }

            if (smartYes.IsChecked == true)
            {
                cr.Smart_board = true;
            }
            else
            {
                cr.Smart_board = false;
            }

            if (win.IsChecked == true)
            {
                cr.Os = Classroom.OpSystem.Windows;
            }
            else if (lin.IsChecked == true)
            {
                cr.Os = Classroom.OpSystem.Linux;
            }
            else
            {
                cr.Os = Classroom.OpSystem.WindowsAndLinux;
            }

            //dodaj softver

            MainWindow.allClassroomsIds.Add(id.Text);
            MainWindow.allClassrooms.Add(cr);
            MainWindow.enableSchedule();

            MessageBox.Show("Successfully added a new classroom!");
            this.Close();
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
            Choose_Software cs = new Choose_Software(a);
            cs.Show();
            cs.Closed += new EventHandler((sender2, e2) => close(sender2, e2, ""));
        }

        void close(object sender, EventArgs e, string title)
        {
            //get added software
        }

    }
}
