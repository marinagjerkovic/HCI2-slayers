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
        public Add_Classroom()
        {
            InitializeComponent();
        }

        private void add_classroom(object sender, RoutedEventArgs e)
        {
            if (MainWindow.allClassrooms.ContainsKey(id.Text))
            {
                MessageBox.Show("Change id - classroom with same id already exists!");
                return;
            }

            Classroom cr = new Classroom();

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
                cr.Os = Classroom.OpSystem.Both;
            }

            //dodaj softver


            MainWindow.allClassrooms.Add(cr.Id, cr);
            MessageBox.Show("Successfully added a new classroom!");
            this.Close();
        }

        private void cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
