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
    /// Interaction logic for Courses_Multiple.xaml
    /// </summary>
    public partial class Courses_Multiple : Window
    {
        public static Course selected_crs;

        public Courses_Multiple(bool show)
        {
            
            InitializeComponent();
            dataGrid.ItemsSource = MainWindow.allCourses;
            if (show)
            {
                save_crs_btn.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void show_course(object sender, RoutedEventArgs e)
        {
            DataGridRow row = Helper.detect_selected_row((DependencyObject)e.OriginalSource);
            dataGrid = ItemsControl.ItemsControlFromItemContainer(row) as DataGrid;
            if (row == null)
            {
                MessageBox.Show("hmm");
                return;
            }
            int index = dataGrid.ItemContainerGenerator.IndexFromContainer(row);
            if (index == -1)
            {
                return;
            }

            Course_Info ci = new Course_Info(MainWindow.allCourses.ElementAt(index));
            ci.ShowDialog();
        }

        private void save_course(object sender, RoutedEventArgs e)
        {
            selected_crs = (Course) dataGrid.SelectedItem;
            this.Close();
        }
    }
}
