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
            id.Text = cr.Id;
            description.Text = cr.Description;
            num_of_seats.Text = cr.Num_of_seats + "";
            if (cr.Board)
            {
                board_tb.Text = "Yes";
            }else
            {
                board_tb.Text = "No";
            }
            if (cr.Smart_board)
            {
                smart_board_tb.Text = "Yes";
            }else
            {
                smart_board_tb.Text = "No";
            }
            if (cr.Projector)
            {
                projector_tb.Text = "Yes";
            }else
            {
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

        private void back(object sender, RoutedEventArgs e)
        {
        }

        private void change_classroom(object sender, RoutedEventArgs e)
        {
        }

        private void update_classroom(object sender, RoutedEventArgs e)
        {
        }

        private void delete_classroom(object sender, RoutedEventArgs e)
        {
        }
    }
}
