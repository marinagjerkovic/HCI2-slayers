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

        private Classroom current_cr;

        public Classroom_Info(Classroom cr)
        {
            current_cr = cr;
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
                current_cr.Description = description.Text;
                current_cr.Num_of_seats = num;
                current_cr.Board = (bool)board_yes.IsChecked;
                current_cr.Smart_board = (bool)smartb_yes.IsChecked;
                current_cr.Projector = (bool)projector_yes.IsChecked;

                if (!chosen_os.Equals(Classroom.OpSystem.WindowsAndLinux))
                {

                        foreach (Software sw in current_cr.Software.ToList())
                        {
                            if (!sw.Os.Equals(chosen_os) && !sw.Os.Equals(Classroom.OpSystem.WindowsAndLinux))
                            {
                                current_cr.Software.Remove(sw);
                            }
                        }

                }
                current_cr.Os = chosen_os;

                MainWindow.allClassrooms.Remove(current_cr);
                MainWindow.allClassroomsIds.Remove(current_cr.Id);
                MainWindow.allClassrooms.Add(current_cr);
                MainWindow.allClassroomsIds.Add(current_cr.Id);

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
                    MainWindow.allClassrooms.Remove(current_cr);
                    MainWindow.allClassroomsIds.Remove(current_cr.Id);
                    this.Close();
                    break;
                case (MessageBoxResult.No):
                    break;
            }
        }

        private void show_software(object sender, RoutedEventArgs e)
        {
            Software_Multiple sm = new Software_Multiple(current_cr);
            sm.ShowDialog();
        }


        private void load_data()
        {
            id.Text = current_cr.Id;
            description.Text = current_cr.Description;
            num_of_seats.Text = current_cr.Num_of_seats + "";
            if (current_cr.Board)
            {
                board_tb.Text = "Yes";
                board_yes.IsChecked = true;
            }
            else
            {
                board_no.IsChecked = true;
                board_tb.Text = "No";
            }
            if (current_cr.Smart_board)
            {
                smartb_yes.IsChecked = true;
                smart_board_tb.Text = "Yes";
            }
            else
            {
                smartb_no.IsChecked = true;
                smart_board_tb.Text = "No";
            }
            if (current_cr.Projector)
            {
                projector_yes.IsChecked = true;
                projector_tb.Text = "Yes";
            }
            else
            {
                projector_no.IsChecked = true;
                projector_tb.Text = "No";
            }
            if (current_cr.Os.Equals(Classroom.OpSystem.Windows))
            {
                win_btn.IsChecked = true;
                os_tb.Text = Classroom.OpSystem.Windows.ToString();
            }
            else if (current_cr.Os.Equals(Classroom.OpSystem.Linux))
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

        private void change_visibility()
        {
            description.IsReadOnly = !description.IsReadOnly;
            num_of_seats.IsReadOnly = !num_of_seats.IsReadOnly;

            change.Visibility = Helper.isVisible(change);
            delete.Visibility = Helper.isVisible(delete);
            back_btn.Visibility = Helper.isVisible(back_btn);
            update.Visibility = Helper.isVisible(update);

            board_tb.Visibility = Helper.isVisible(board_tb);
            smart_board_tb.Visibility = Helper.isVisible(smart_board_tb);
            projector_tb.Visibility = Helper.isVisible(projector_tb);
            os_tb.Visibility = Helper.isVisible(os_tb);

            board_yes.Visibility = Helper.isVisible(board_yes);
            board_no.Visibility = Helper.isVisible(board_no);
            smartb_yes.Visibility = Helper.isVisible(smartb_yes);
            smartb_no.Visibility = Helper.isVisible(smartb_no);
            projector_yes.Visibility = Helper.isVisible(projector_yes);
            projector_no.Visibility = Helper.isVisible(projector_no);
            win_btn.Visibility = Helper.isVisible(win_btn);
            lin_btn.Visibility = Helper.isVisible(lin_btn);
            both_btn.Visibility = Helper.isVisible(both_btn);

            sw_tb.Visibility = Helper.isVisible(view_sw_btn);
            view_sw_btn.Visibility = Helper.isVisible(view_sw_btn);
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int indeks = 0;
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i].Title.Equals("Classroom_Info"))
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
