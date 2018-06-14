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
    /// Interaction logic for Course_Info.xaml
    /// </summary>
    public partial class Course_Info : Window
    {

        private Course current_crs;

        public Course_Info(Course crs)
        {
            current_crs = crs;
            InitializeComponent();
            id.Text = current_crs.Id;
            name.Text = current_crs.Name;
            description.Text = current_crs.Description;
            date.Text = Convert.ToString(current_crs.Date_of_conception);
            date_picker.SelectedDate = current_crs.Date_of_conception;
        }

        private void delete(object sender, RoutedEventArgs e)
        {

        }

        private void change_data(object sender, RoutedEventArgs e)
        {
            name.BorderBrush = Helper.colors[0];
            description.BorderBrush = Helper.colors[0];
            change_visibility();
        }

        private void update(object sender, RoutedEventArgs e)
        {
            if(name.Text.Equals("") || description.Text.Equals(""))
            {
                MessageBox.Show("Field's can't be empty!", "Error!");
                return;
            }

            current_crs.Name = name.Text;
            current_crs.Description = description.Text;
            current_crs.Date_of_conception = DateTime.Parse(date_picker.Text);

            date.Text = Convert.ToString(current_crs.Date_of_conception);
            MainWindow.allCourses.Remove(current_crs);
            MainWindow.allCourses.Add(current_crs);

            name.BorderBrush = Helper.colors[1];
            description.BorderBrush = Helper.colors[1];
            change_visibility();
        }

        private void back(object sender, RoutedEventArgs e)
        {
            name.BorderBrush = Helper.colors[1];
            description.BorderBrush = Helper.colors[1];
            change_visibility();
        }

        private void change_visibility()
        {
            name.IsReadOnly = !name.IsReadOnly; 
            description.IsReadOnly = !description.IsReadOnly;

            change_btn.Visibility = Helper.isVisible(change_btn);
            delete_btn.Visibility = Helper.isVisible(delete_btn);
            update_btn.Visibility = Helper.isVisible(update_btn);
            back_btn.Visibility = Helper.isVisible(back_btn);
            date.Visibility = Helper.isVisible(date);
            date_picker.Visibility = Helper.isVisible(date_picker);
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int indeks = 0;
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i].Title.Equals("Course_Info"))
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
