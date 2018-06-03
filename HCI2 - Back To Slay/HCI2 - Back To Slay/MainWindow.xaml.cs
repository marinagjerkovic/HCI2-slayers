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
using System.Collections.ObjectModel;


namespace HCI2___Back_To_Slay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window
    {
        public static List<string> allClassroomsIds = new List<string>();
        public static List<string> allCoursesIds = new List<string>();
        public static List<string> allSoftwareIds = new List<string>();
        public static List<string> allSubjectsIds = new List<string>();

        public static ObservableCollection<Subject> allSubjects { get; set; }
        public static ObservableCollection<Course> allCourses { get; set; }
        public static ObservableCollection<Software> allSoftware { get; set; }
        public static ObservableCollection<Classroom> allClassrooms { get; set; }

       
        public ObservableCollection<Appointment> app2add { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            schedule.TimeMode = TimeModes.TwentyFourHours;
            schedule.TimeInterval = TimeInterval.FifteenMin;
            schedule.ScheduleType = ScheduleType.Week;
            schedule.FirstDayOfWeek = DayOfWeek.Monday;
            schedule.WorkStartHour = 6;
            schedule.WorkEndHour = 23;
            schedule.ShowNonWorkingHours = false;
            ResourceType resourceType = new ResourceType { TypeName = "Classroom" };
            resourceType.ResourceCollection.Add(new Resource { DisplayName = "Ucionica 1", ResourceName = "Ucionica1", });
            resourceType.ResourceCollection.Add(new Resource { DisplayName = "Ucionica 2", ResourceName = "UCionica2" });
            schedule.ScheduleResourceTypeCollection = new ObservableCollection<ResourceType> { resourceType };
            schedule.Resource = "Classroom";

            //initialize collections

            allSubjects = new ObservableCollection<Subject>();
            allSoftware = new ObservableCollection<Software>();
            allClassrooms = new ObservableCollection<Classroom>();
            allCourses = new ObservableCollection<Course>();

            for (int i = 1; i < 6; i++)
            {
                Software s = new Software();
                s.Id = i + "";
                s.Name = "name" + i;
                s.Os = Classroom.OpSystem.Windows;
                allSoftware.Add(s);
                allSoftwareIds.Add(s.Id);
            }


            this.LayoutRoot.Children.Add(schedule);
            List<Software> ss = new List<Software>()
            {
                new Software("soft_id","soft_name",Classroom.OpSystem.Linux,"soft_maker", "soft_site",1999,200.09,"soft_desc"),
                new Software("soft_id2","soft_name2",Classroom.OpSystem.Linux,"soft_maker2","soft_site2",1999,220.09,"soft_2desc")
            };
            Course course = new Course("1","ime","opis",DateTime.Now);
            Subject subject = new Subject("1", "name", "description", course, 30, 2, 2, true, true, true, Classroom.OpSystem.Linux, ss);
            Subject subject2 = new Subject("2", "name2", "description2", course, 30, 2, 2, true, true, true, Classroom.OpSystem.Linux, ss);
            Appointment app1 = new Appointment();
            app1.Subject = subject;
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
