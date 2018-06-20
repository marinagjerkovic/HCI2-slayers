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
    /// Interaction logic for Software_Info.xaml
    /// </summary>
    public partial class Software_Info : Window
    {

        private Software current_sw;
        public static string deleted = null;
        public static string updated = null;

        public Software_Info(Software sw)
        {
            current_sw = sw;
            InitializeComponent();
            load_data();
        }

        private void change_software(object sender, RoutedEventArgs e)
        {
            if (MainWindow.update_software_schedule(current_sw.Id))
            {
                change_visibility();
                description.BorderBrush = Helper.colors[0];
                name.BorderBrush = Helper.colors[0];
                price.BorderBrush = Helper.colors[0];
                year.BorderBrush = Helper.colors[0];
                maker.BorderBrush = Helper.colors[0];
                site.BorderBrush = Helper.colors[0];
            }
            else
            {
                MessageBox.Show("This software is used in current schedule - please remove it before updating it!");
            }
        }

        private void update_software(object sender, RoutedEventArgs e)
        {
            if (name.Text.Equals("") || description.Text.Equals("") || maker.Text.Equals("") || site.Text.Equals(""))
            {
                MessageBox.Show("Field's can't be empty!", "Error!");
                return;
            }

            current_sw.Name = name.Text;
            current_sw.Description = description.Text;
            current_sw.Maker = maker.Text;
            current_sw.Site = site.Text;

            int num = 0;
            if (Int32.TryParse(year.Text, out num) && num > 1900 && num < 2018)
            {
                current_sw.Year = num;
            }
            else
            {
                MessageBox.Show("Wrong year! Must be in 1900-2018 interval!", "Error!");
                return; 
            }

            double pr = 0;
            if (Double.TryParse(price.Text, out pr))
            {
                current_sw.Price = pr;
            }
            else
            {
                MessageBox.Show("Price must be a number!", "Error!");
                return;
            }

            if (win_btn.IsChecked == true)
            {
                current_sw.Os = Classroom.OpSystem.Windows;
            }else if (lin_btn.IsChecked == true)
            {
                current_sw.Os = Classroom.OpSystem.Linux;
            }else
            {
                current_sw.Os = Classroom.OpSystem.WindowsAndLinux;
            }

            MainWindow.allSoftware.Remove(current_sw);
            MainWindow.allSoftwareIds.Remove(current_sw.Id);
            MainWindow.allSoftware.Add(current_sw);
            MainWindow.allSoftwareIds.Add(current_sw.Id);
            updated = current_sw.Id;
               

            change_visibility();
            description.BorderBrush = Helper.colors[1];
            name.BorderBrush = Helper.colors[1];
            price.BorderBrush = Helper.colors[1];
            year.BorderBrush = Helper.colors[1];
            maker.BorderBrush = Helper.colors[1];
            site.BorderBrush = Helper.colors[1];
        }

        private void delete_software(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this software from the system?", "sinisa the best", MessageBoxButton.YesNo);
            switch (result)
            {
                case (MessageBoxResult.Yes):
                    if (MainWindow.update_software_schedule(current_sw.Id))
                    {
                        MainWindow.allSoftware.Remove(current_sw);
                        MainWindow.allSoftwareIds.Remove(current_sw.Id);
                        deleted = current_sw.Id;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("This software is used in current schedule - please remove it before deleting it!");
                    }
                    break;
                case (MessageBoxResult.No):
                    break;
            }
        }
        
        private void back(object sender, RoutedEventArgs e)
        {
            description.BorderBrush = Helper.colors[1];
            name.BorderBrush = Helper.colors[1];
            price.BorderBrush = Helper.colors[1];
            year.BorderBrush = Helper.colors[1];
            maker.BorderBrush = Helper.colors[1];
            site.BorderBrush = Helper.colors[1];
            change_visibility();
        }


        private void load_data()
        {
            id.Text = current_sw.Id;
            name.Text = current_sw.Name;
            description.Text = current_sw.Description;
            maker.Text = current_sw.Maker;
            site.Text = current_sw.Site;
            year.Text = current_sw.Year + "";
            price.Text = current_sw.Price + "";
            if (current_sw.Os.Equals(Classroom.OpSystem.Windows))
            {
                win_btn.IsChecked = true;
                os_tb.Text = Classroom.OpSystem.Windows.ToString();
            }
            else if (current_sw.Os.Equals(Classroom.OpSystem.Linux))
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
            name.IsReadOnly = !name.IsReadOnly;
            description.IsReadOnly = !description.IsReadOnly;
            maker.IsReadOnly = !maker.IsReadOnly;
            site.IsReadOnly = !site.IsReadOnly;
            year.IsReadOnly = !year.IsReadOnly;
            price.IsReadOnly = !price.IsReadOnly;

            change.Visibility = Helper.isVisible(change);
            delete.Visibility = Helper.isVisible(delete);
            back_btn.Visibility = Helper.isVisible(back_btn);
            update.Visibility = Helper.isVisible(update);
            win_btn.Visibility = Helper.isVisible(win_btn);
            lin_btn.Visibility = Helper.isVisible(lin_btn);
            both_btn.Visibility = Helper.isVisible(both_btn);
            os_tb.Visibility = Helper.isVisible(os_tb);
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int indeks = 0;
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i].Title.Equals("Software_Info"))
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
