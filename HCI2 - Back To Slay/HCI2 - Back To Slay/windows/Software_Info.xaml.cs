﻿using System;
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

        public Software_Info(Software sw)
        {
            current_sw = sw;
            InitializeComponent();
            this.Title = current_sw.Name;
            id.Text = sw.Id;
            name.Text = sw.Name;
            description.Text = sw.Description;
            maker.Text = sw.Maker;
            site.Text = sw.Site;
            year.Text = sw.Year+"";
            price.Text = sw.Price + "";
            if (current_sw.Os.Equals(Classroom.OpSystem.Windows))
            {
                win_btn.IsChecked = true;
            }else if (current_sw.Os.Equals(Classroom.OpSystem.Linux))
            {
                lin_btn.IsChecked = true;
            }else
            {
                both_btn.IsChecked = true;
            }

        }

        private void change_software(object sender, RoutedEventArgs e)
        {
            change_visibility();
        }

        private void update_software(object sender, RoutedEventArgs e)
        {

            if(name.Text.Equals("") || description.Text.Equals("") || maker.Text.Equals("") || site.Text.Equals(""))
            {
                MessageBox.Show("Field's can't be empty!", "Error!");
                return;
            }
            MainWindow.allSoftware.Remove(current_sw);
            MainWindow.allSoftwareIds.Remove(current_sw.Id);
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
                current_sw.Os = Classroom.OpSystem.Both;
            }
            MainWindow.allSoftware.Add(current_sw);
            MainWindow.allSoftwareIds.Add(current_sw.Id);
            change_visibility();
            MessageBox.Show("Info updated!", "Info!");
        }

        private void delete_software(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this software from the system?", "sinisa the best", MessageBoxButton.YesNo);
            switch (result)
            {
                case (MessageBoxResult.Yes):
                    MainWindow.allSoftware.Remove(current_sw);
                    MainWindow.allSoftwareIds.Remove(current_sw.Id);
                    this.Close();
                    break;
                case (MessageBoxResult.No):
                    break;
            }
        }
        
        private void back(object sender, RoutedEventArgs e)
        {
            change_visibility();
        }

        private void change_visibility()
        {
            name.IsReadOnly = !name.IsReadOnly;
            description.IsReadOnly = !description.IsReadOnly;
            maker.IsReadOnly = !maker.IsReadOnly;
            site.IsReadOnly = !site.IsReadOnly;
            year.IsReadOnly = !year.IsReadOnly;
            price.IsReadOnly = !price.IsReadOnly;

            if (change.Visibility == System.Windows.Visibility.Visible)
            {
                change.Visibility = System.Windows.Visibility.Hidden;
                delete.Visibility = System.Windows.Visibility.Hidden;
                back_btn.Visibility = System.Windows.Visibility.Visible;
                update.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                change.Visibility = System.Windows.Visibility.Visible;
                delete.Visibility = System.Windows.Visibility.Visible;
                back_btn.Visibility = System.Windows.Visibility.Hidden;
                update.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
