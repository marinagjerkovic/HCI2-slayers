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
    /// Interaction logic for Subject_Info.xaml
    /// </summary>
    public partial class Subject_Info : Window
    {

        private Subject current_sub;
        private Course temp = null;

        public Subject_Info(Subject sub)
        {
            current_sub = sub;
            InitializeComponent();
            id.Text = sub.Id;
            name.Text = sub.Name;
            description.Text = sub.Description;
            course.Text = sub.Course.Name;
            number.Text = sub.Num_of_periods+"";
            duration.Text = sub.Duration_of_period+"";
            size.Text = sub.Size_of_group+"";
            load_data();

        }

        private void change(object sender, RoutedEventArgs e)
        {
            if (MainWindow.update_subject_schedule(current_sub.Id))
            {
                name.BorderBrush = Helper.colors[0];
                description.BorderBrush = Helper.colors[0];
                number.BorderBrush = Helper.colors[0];
                duration.BorderBrush = Helper.colors[0];
                size.BorderBrush = Helper.colors[0];
                change_visibility();
                temp = current_sub.Course;
            }
            else
            {
                MessageBox.Show("This subject is used in current schedule - please remove it before updating it!");
            }
        }

        private void update(object sender, RoutedEventArgs e)
        {
            if(name.Text.Equals("") || description.Text.Equals(""))
            {
                MessageBox.Show("Field's can't be empty!", "Error!");
                return;
            }
            int num_periods = 0;
            if (!Int32.TryParse(number.Text, out num_periods))
            {
                MessageBox.Show("Number of periods must be a number!", "Error!");
                return;
            }
            int duration_period = 0;
            if (!Int32.TryParse(duration.Text, out duration_period))
            {
                MessageBox.Show("Duration of periods must be a number!", "Error!");
                return;
            }
            int group_size = 0;
            if (!Int32.TryParse(number.Text, out group_size))
            {
                MessageBox.Show("Size of groups must be a number!", "Error!");
                return;
            }

            current_sub.Name = name.Text;
            current_sub.Description = description.Text;
            current_sub.Num_of_periods = num_periods;
            current_sub.Duration_of_period = duration_period;
            current_sub.Size_of_group = group_size;
            current_sub.Course = temp;

            MainWindow.allSubjects.Remove(current_sub);
            MainWindow.allSubjects.Add(current_sub);


            name.BorderBrush = Helper.colors[1];
            description.BorderBrush = Helper.colors[1];
            number.BorderBrush = Helper.colors[1];
            duration.BorderBrush = Helper.colors[1]; 
            size.BorderBrush = Helper.colors[1];
            change_visibility();
        }

        private void delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this subject from the system?", "sinisa the best", MessageBoxButton.YesNo);
            switch (result)
            {
                case (MessageBoxResult.Yes):
                    if (MainWindow.update_classroom_schedule(current_sub.Id))
                    {
                        MainWindow.allSubjects.Remove(current_sub);
                        MainWindow.allSubjectsIds.Remove(current_sub.Id);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("This subject is used in current schedule - please remove it before deleting it!");
                    }
                    break;
                case (MessageBoxResult.No):
                    break;
            }
        }

        private void back(object sender, RoutedEventArgs e)
        {
            name.BorderBrush = Helper.colors[1];
            description.BorderBrush = Helper.colors[1];
            number.BorderBrush = Helper.colors[1];
            duration.BorderBrush = Helper.colors[1];
            size.BorderBrush = Helper.colors[1];
            change_visibility();
        }

        private void show_software(object sender, RoutedEventArgs e)
        {
            Software_Multiple sm = new Software_Multiple(current_sub);
            sm.ShowDialog();
        }

        private void change_crs(object sender, RoutedEventArgs e)
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
            temp = Courses_Multiple.selected_crs;
            course.Text = temp.Name;
            Courses_Multiple.selected_crs = null;
        }

        private void change_visibility()
        {
            change_btn.Visibility = Helper.isVisible(change_btn);
            delete_btn.Visibility = Helper.isVisible(delete_btn);
            update_btn.Visibility = Helper.isVisible(update_btn);
            back_btn.Visibility = Helper.isVisible(back_btn);
            show_sw_btn.Visibility = Helper.isVisible(show_sw_btn);
            change_crs_btn.Visibility = Helper.isVisible(change_crs_btn);

            board_yes.Visibility = Helper.isVisible(board_yes);
            board_no.Visibility = Helper.isVisible(board_no);
            smart_board_yes.Visibility = Helper.isVisible(smart_board_yes);
            smart_board_no.Visibility = Helper.isVisible(smart_board_no);
            projector_yes.Visibility = Helper.isVisible(projector_yes);
            projector_no.Visibility = Helper.isVisible(projector_no);
            win_btn.Visibility = Helper.isVisible(win_btn);
            lin_btn.Visibility = Helper.isVisible(lin_btn);
            both_btn.Visibility = Helper.isVisible(both_btn);


            name.IsReadOnly = !name.IsReadOnly;
            description.IsReadOnly = !description.IsReadOnly;
            size.IsReadOnly = !size.IsReadOnly;
            duration.IsReadOnly = !duration.IsReadOnly;
            number.IsReadOnly = !number.IsReadOnly;

            projector_tb.Visibility = Helper.isVisible(projector_tb);
            board_tb.Visibility = Helper.isVisible(board_tb);
            smart_board_tb.Visibility = Helper.isVisible(smart_board_tb);
            os_tb.Visibility = Helper.isVisible(os_tb);

        }

        private void load_data()
        {
            if (current_sub.Board)
            {
                board_yes.IsChecked = true;
                board_tb.Text = "Yes";
            }else
            {
                board_no.IsChecked = true;
                board_tb.Text = "No";
            }
            if (current_sub.Smart_board)
            {
                smart_board_yes.IsChecked = true;
                smart_board_tb.Text = "Yes";
            }else
            {
                smart_board_no.IsChecked = true;
                smart_board_tb.Text = "No";
            }
            if (current_sub.Projector)
            {
                projector_yes.IsChecked = true;
                projector_tb.Text = "Yes";
            }else
            {
                projector_no.IsChecked = true;
                projector_tb.Text = "No";
            }
            if (current_sub.Os.Equals(Classroom.OpSystem.Windows))
            {
                win_btn.IsChecked = true;
            }else if (current_sub.Os.Equals(Classroom.OpSystem.Linux))
            {
                lin_btn.IsChecked = true;
            }else
            {
                both_btn.IsChecked = true;
            }
            os_tb.Text = current_sub.Os.ToString();
        }
    }
}
