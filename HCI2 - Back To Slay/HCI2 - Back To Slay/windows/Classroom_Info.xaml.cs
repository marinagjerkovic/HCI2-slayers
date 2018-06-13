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
    /// Interaction logic for Classroom_Info.xaml
    /// </summary>


    public partial class Classroom_Info : Window
    {
        public Classroom_Info(Classroom cr)
        {
            MainWindow.current_cr = cr;
            InitializeComponent();
            load_data();
        }

        private void back(object sender, RoutedEventArgs e)
        {
            description.BorderBrush = new SolidColorBrush(Colors.Transparent);
            num_of_seats.BorderBrush = new SolidColorBrush(Colors.Transparent);
            change_visibility();
        }

        private void change_classroom(object sender, RoutedEventArgs e)
        {
            description.BorderBrush = new SolidColorBrush(Colors.Red);
            num_of_seats.BorderBrush = new SolidColorBrush(Colors.Red);
            change_visibility();
        }

        private void update_classroom(object sender, RoutedEventArgs e)
        {
            if (description.Text.Equals(""))
            {
                MessageBox.Show("Field's can't be empty!", "Error!");
                return;
            }

            int num = 0;
            if (!Int32.TryParse(num_of_seats.Text, out num))
            {
                MessageBox.Show("Number of seats must be a number!", "Error!");
                return;
            }

            Classroom.OpSystem chosen_os;
            if (win_btn.IsChecked == true)
            {
                chosen_os = Classroom.OpSystem.Windows;
            }
            else if (lin_btn.IsChecked == true)
            {
                chosen_os = Classroom.OpSystem.Linux;
            }
            else
            {
                chosen_os = Classroom.OpSystem.WindowsAndLinux;
            }

            if (MainWindow.update_classroom_schedule(num, (bool)board_yes.IsChecked, (bool)smartb_yes.IsChecked, (bool)projector_yes.IsChecked, chosen_os))
            {
                MainWindow.current_cr.Description = description.Text;
                MainWindow.current_cr.Num_of_seats = num;
                MainWindow.current_cr.Board = (bool)board_yes.IsChecked;
                MainWindow.current_cr.Smart_board = (bool)smartb_yes.IsChecked;
                MainWindow.current_cr.Projector = (bool)projector_yes.IsChecked;

                if (!chosen_os.Equals(Classroom.OpSystem.WindowsAndLinux))
                {
                    if (MainWindow.current_cr.Os.Equals(Classroom.OpSystem.WindowsAndLinux))
                    {
                        foreach (Software sw in MainWindow.current_cr.Software.ToList())
                        {
                            if (!sw.Os.Equals(chosen_os))
                            {
                                MainWindow.current_cr.Software.Remove(sw);
                            }
                        }
                    }
                    else
                    {
                        MainWindow.current_cr.Software = new List<Software>();
                    }
                }
                MainWindow.current_cr.Os = chosen_os;

                load_data();
                description.BorderBrush = new SolidColorBrush(Colors.Transparent);
                num_of_seats.BorderBrush = new SolidColorBrush(Colors.Transparent);
                change_visibility();
            }

        }

        private void delete_classroom(object sender, RoutedEventArgs e)
        {
            //treba vidjeti sha sa ovim
            return;
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this classroom from the system?", "sinisa the best", MessageBoxButton.YesNo);
            switch (result)
            {
                case (MessageBoxResult.Yes):
                    MainWindow.allClassrooms.Remove(MainWindow.current_cr);
                    MainWindow.allClassroomsIds.Remove(MainWindow.current_cr.Id);
                    this.Close();
                    break;
                case (MessageBoxResult.No):
                    break;
            }
        }

        private void show_software(object sender, RoutedEventArgs e)
        {
            Software_Multiple sm = new Software_Multiple(MainWindow.current_cr.Software);
            sm.ShowDialog();
        }


        private void load_data()
        {
            id.Text = MainWindow.current_cr.Id;
            description.Text = MainWindow.current_cr.Description;
            num_of_seats.Text = MainWindow.current_cr.Num_of_seats + "";
            if (MainWindow.current_cr.Board)
            {
                board_tb.Text = "Yes";
                board_yes.IsChecked = true;
            }
            else
            {
                board_no.IsChecked = true;
                board_tb.Text = "No";
            }
            if (MainWindow.current_cr.Smart_board)
            {
                smartb_yes.IsChecked = true;
                smart_board_tb.Text = "Yes";
            }
            else
            {
                smartb_no.IsChecked = true;
                smart_board_tb.Text = "No";
            }
            if (MainWindow.current_cr.Projector)
            {
                projector_yes.IsChecked = true;
                projector_tb.Text = "Yes";
            }
            else
            {
                projector_no.IsChecked = true;
                projector_tb.Text = "No";
            }
            if (MainWindow.current_cr.Os.Equals(Classroom.OpSystem.Windows))
            {
                win_btn.IsChecked = true;
                os_tb.Text = Classroom.OpSystem.Windows.ToString();
            }
            else if (MainWindow.current_cr.Os.Equals(Classroom.OpSystem.Linux))
            {
                lin_btn.IsChecked = true;
                os_tb.Text = Classroom.OpSystem.Linux.ToString();
            }
            else
            {
                both_btn.IsChecked = true;
                os_tb.Text = Classroom.OpSystem.WindowsAndLinux.ToString();
            }
        }

        private Visibility isVisible(Control ctrl)
        {
            if (ctrl.Visibility == Visibility.Visible)
            {
                return Visibility.Hidden;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        private void change_visibility()
        {
            description.IsReadOnly = !description.IsReadOnly;
            num_of_seats.IsReadOnly = !num_of_seats.IsReadOnly;

            change.Visibility = isVisible(change);
            delete.Visibility = isVisible(delete);
            back_btn.Visibility = isVisible(back_btn);
            update.Visibility = isVisible(update);

            board_tb.Visibility = isVisible(board_tb);
            smart_board_tb.Visibility = isVisible(smart_board_tb);
            projector_tb.Visibility = isVisible(projector_tb);
            os_tb.Visibility = isVisible(os_tb);

            board_yes.Visibility = isVisible(board_yes);
            board_no.Visibility = isVisible(board_no);
            smartb_yes.Visibility = isVisible(smartb_yes);
            smartb_no.Visibility = isVisible(smartb_no);
            projector_yes.Visibility = isVisible(projector_yes);
            projector_no.Visibility = isVisible(projector_no);
            win_btn.Visibility = isVisible(win_btn);
            lin_btn.Visibility = isVisible(lin_btn);
            both_btn.Visibility = isVisible(both_btn);

            sw_tb.Visibility = isVisible(view_sw_btn);
            view_sw_btn.Visibility = isVisible(view_sw_btn);
        }
    }
}
