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
            name.BorderBrush = Helper.colors[0];
            description.BorderBrush = Helper.colors[0];
            number.BorderBrush = Helper.colors[0];
            duration.BorderBrush = Helper.colors[0];
            size.BorderBrush = Helper.colors[0];
            change_visibility();
        }

        private void update(object sender, RoutedEventArgs e)
        {
            name.BorderBrush = Helper.colors[1];
            description.BorderBrush = Helper.colors[1];
            number.BorderBrush = Helper.colors[1];
            duration.BorderBrush = Helper.colors[1]; 
            size.BorderBrush = Helper.colors[1];
            change_visibility();
        }

        private void delete(object sender, RoutedEventArgs e)
        {

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

        }

        private void change_crs(object sender, RoutedEventArgs e)
        {

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
