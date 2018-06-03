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
        public Add_Subject()
        {
            InitializeComponent();
        }

        private void add_new_subject(object sender, RoutedEventArgs e)
        {
            if (MainWindow.allSubjectsIds.Contains(id.Text))
            {
                MessageBox.Show("Change id - subject with the same id already exists!");
                return;
            }

            Subject sub = new Subject();

            sub.Id = id.Text;
            sub.Name = name.Text;
            sub.Description = description.Text;

            int num = 0;
            if (Int32.TryParse(group_size.Text, out num))
            {
                sub.Size_of_group = num;
            }
            else
            {
                MessageBox.Show("Size of groups must be a number!");
                return;
            }

            if (Int32.TryParse(num_periods.Text, out num))
            {
                sub.Num_of_periods = num;
            }
            else
            {
                MessageBox.Show("Number of periods must be a number!");
                return;
            }

            if (Int32.TryParse(duration.Text, out num))
            {
                sub.Duration_of_period = num;
            }
            else
            {
                MessageBox.Show("Duration of periods must be a number!");
                return;
            }

            if (projYes.IsChecked == true)
            {
                sub.Projector = true;
            }
            else
            {
                sub.Projector = false;
            }

            if (boardYes.IsChecked == true)
            {
                sub.Board = true;
            }
            else
            {
                sub.Board = false;
            }

            if (smartYes.IsChecked == true)
            {
                sub.Smart_board = true;
            }
            else
            {
                sub.Smart_board = false;
            }

            if (win.IsChecked == true)
            {
                sub.Os = Classroom.OpSystem.Windows;
            }
            else if (linux.IsChecked == true)
            {
                sub.Os = Classroom.OpSystem.Linux;
            }
            else
            {
                sub.Os = Classroom.OpSystem.Both;
            }

            //dodaj softver i kurs

            MainWindow.allSubjects.Add(sub);
            MainWindow.allSubjectsIds.Add(id.Text);
            MessageBox.Show("Successfully added a new subject!");
            this.Close();
        }

        private void cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
    }
}
