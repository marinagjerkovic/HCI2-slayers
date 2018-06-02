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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HCI2___Back_To_Slay.windows;
using Syncfusion.UI.Xaml.Schedule;

namespace HCI2___Back_To_Slay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window
    {
        public static Dictionary<string, Classroom> allClassrooms = new Dictionary<string, Classroom>();
        public static Dictionary<string, Course> allCourses = new Dictionary<string, Course>();
        public static Dictionary<string, Software> allSoftware = new Dictionary<string, Software>();
        public static Dictionary<string, Subject> allSubjects = new Dictionary<string, Subject>();

        public MainWindow()
        {
            InitializeComponent();
            SfSchedule schedule = new SfSchedule();
            schedule.TimeMode = TimeModes.TwentyFourHours;
            schedule.TimeInterval = TimeInterval.FifteenMin;
            schedule.ScheduleType = ScheduleType.Week;
            schedule.WorkStartHour = 6;
            schedule.WorkEndHour = 23;

            this.LayoutRoot.Children.Add(schedule);
        }

        private void add_new_classroom(object sender, RoutedEventArgs e)
        {
            Add_Classroom add_classroom_window = new Add_Classroom();
            add_classroom_window.Show();
        }

        private void add_new_course(object sender, RoutedEventArgs e)
        {
            Add_Course add_course_window = new Add_Course();
            add_course_window.Show();
        }

        private void add_new_subject(object sender, RoutedEventArgs e)
        {
            Add_Subject add_subject_window = new Add_Subject();
            add_subject_window.Show();
        }

        private void add_new_software(object sender, RoutedEventArgs e)
        {
            Add_Software add_software_window = new Add_Software();
            add_software_window.Show();
        }
    }
}
