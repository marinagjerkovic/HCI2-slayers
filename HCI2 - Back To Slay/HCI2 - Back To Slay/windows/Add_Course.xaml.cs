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
    /// Interaction logic for Add_Course.xaml
    /// </summary>
    public partial class Add_Course : Window
    {
        public Add_Course()
        {
            InitializeComponent();
        }

        private void add_new_course(object sender, RoutedEventArgs e)
        {
            if (MainWindow.allCourses.ContainsKey(id.Text))
            {
                MessageBox.Show("Change id - course with same id already exists!");
                return;
            }

            Course course = new Course();

            course.Id = id.Text;
            course.Name = name.Text;
            course.Description = description.Text;
            course.Date_of_conception = DateTime.Parse(datepicker.Text);

            MainWindow.allCourses.Add(course.Id, course);
            MessageBox.Show("Successfully added a new course!");
            this.Close();
        }

        private void cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
