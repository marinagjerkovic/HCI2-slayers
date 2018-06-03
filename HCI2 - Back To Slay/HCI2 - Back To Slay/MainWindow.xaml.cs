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
using System.IO;


namespace HCI2___Back_To_Slay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window
    {
        public DateTime mainDate;
        public static List<string> allClassroomsIds = new List<string>();
        public static List<string> allCoursesIds = new List<string>();
        public static List<string> allSoftwareIds = new List<string>();
        public static List<string> allSubjectsIds = new List<string>();

        public static ObservableCollection<Subject> allSubjects { get; set; }
        public static ObservableCollection<Course> allCourses { get; set; }
        public static ObservableCollection<Software> allSoftware { get; set; }
        public static ObservableCollection<Classroom> allClassrooms { get; set; }

        public static ObservableCollection<Subject> leftSubjects { get; set; }
        public static ObservableCollection<Subject> shownSubjects { get; set; }

        public ObservableCollection<Appointment> app2add { get; set; }

        public ResourceType resourceType;

        public MainWindow()
        {
            InitializeComponent();
            mainDate = new DateTime(2018, 6, 10, 6, 0, 0);
            schedule.TimeMode = TimeModes.TwentyFourHours;
            schedule.TimeInterval = TimeInterval.FifteenMin;
            schedule.ScheduleType = ScheduleType.Week;
            schedule.FirstDayOfWeek = DayOfWeek.Monday;
            schedule.WorkStartHour = 6;
            schedule.WorkEndHour = 23;
            schedule.ShowNonWorkingHours = false;
            resourceType = new ResourceType { TypeName = "Classroom" };

            schedule.ScheduleResourceTypeCollection = new ObservableCollection<ResourceType> { resourceType };
            schedule.Resource = "Classroom";

            

            //initialize collections

            schedule.ScheduleResourceTypeCollection = new ObservableCollection<ResourceType> { resourceType };
            schedule.Resource = "Classroom";
            schedule.MoveToDate(mainDate);

            List<Software> ss = new List<Software>()
            {
                new Software("soft_id","soft_name",Classroom.OpSystem.Linux,"soft_maker", "soft_site",1999,200.09,"soft_desc"),
                new Software("soft_id2","soft_name2",Classroom.OpSystem.Linux,"soft_maker2","soft_site2",1999,220.09,"soft_2desc")
            };
            Course course = new Course("1", "ime", "opis", DateTime.Now);
            Subject subject = new Subject("1", "name", "description", course, 10, 2, 2, true, true, true, Classroom.OpSystem.Linux, ss);
            Subject subject2 = new Subject("2", "name2", "description2", course, 15, 3, 2, true, true, true, Classroom.OpSystem.Linux, ss);
            Subject subject3 = new Subject("3", "name3", "description3", course, 20, 2, 3, true, true, true, Classroom.OpSystem.Linux, ss);

            Appointment app1 = new Appointment();
            app1.Subject = subject;

            allSubjects = new ObservableCollection<Subject>();
            allSoftware = new ObservableCollection<Software>();
            allClassrooms = new ObservableCollection<Classroom>();
            allCourses = new ObservableCollection<Course>();

            leftSubjects = new ObservableCollection<Subject>();
            shownSubjects = new ObservableCollection<Subject>();

            leftSubjects.Add(subject);
            leftSubjects.Add(subject2);
            leftSubjects.Add(subject3);

            for (int i = 1; i < 6; i++)
            {
                Software s = new Software();
                s.Id = i + "";
                s.Name = "name" + i;
                s.Os = Classroom.OpSystem.Windows;
                allSoftware.Add(s);
                allSoftwareIds.Add(s.Id);
                Classroom cr = new Classroom("id1" + i, "desc" + i, 30, true, false, true, Classroom.OpSystem.Both, ss);
                //Console.Write(cr.Description);
                allClassrooms.Add(cr);
            }
            Console.WriteLine(allClassrooms.Count());
            classroomsDG.ItemsSource = allClassrooms;
            subjectsDG.ItemsSource = leftSubjects;

            if (allClassrooms.Count() == 0)
            {
                schedule.IsEnabled = false;
                MessageBox.Show("You can't add anything on schedule before you create classroom!");

            }
            else
            {
                resourceType.ResourceCollection.Add(create_resource(allClassrooms.First()));
            }
            //this.LayoutRoot.Children.Add(schedule);
            loadData();
            this.LayoutRoot.Children.Add(schedule);
        }

        private void dataGridClassroom_MouseDoubleClick(object sender, MouseButtonEventArgs e) { }

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

        private void add_classroom_on_schedule(Object sender, RoutedEventArgs e)
        {
            Classroom cr = (Classroom)classroomsDG.SelectedItem;
            string symbol = cr.Id;
            bool found = false;
            foreach (Resource r in resourceType.ResourceCollection.ToArray())
            {
                if (r.ResourceName == symbol)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                resourceType.ResourceCollection.Add(create_resource(cr));
            }
        }

        private Resource create_resource(Classroom cr)
        {
            string symbol = cr.Id;
            string display = "seats: " + cr.Num_of_seats + "\tProjector: " + cr.Projector + "\tBoard: " + cr.Board + "\tsmart board: " + cr.Smart_board + "\tOS: "
                + cr.Os + "\nsoftware: ";
            display += cr.softwares();
            Resource r = new Resource { DisplayName = display, ResourceName = symbol };
            return r;

        }

        private void remove_classroom_from_schedule(Object sender, RoutedEventArgs e)
        {
            if (resourceType.ResourceCollection.Count <= 1)
            {
                MessageBox.Show("There must be at least one classroom on schedule!");
                return;
            }
            string symbol = ((Classroom)classroomsDG.SelectedItem).Id;
            foreach (Resource r in resourceType.ResourceCollection.ToArray())
            {
                if (r.ResourceName == symbol)
                {
                    resourceType.ResourceCollection.Remove(r);
                    break;
                }
            }
        }
        private void add_subject_on_schedule(Object sender, RoutedEventArgs e)
        {
            Subject subject = (Subject)subjectsDG.SelectedItem;
            Classroom cr = (Classroom)classroomsDG.SelectedItem;

            if (allClassrooms.Count == 0)
            {
                MessageBox.Show("You can't add anything on schedule before you create classroom!");
                return;
            }
            if (cr is null)
            {
                MessageBox.Show("You must select classroom first!");
                return;
            }
            for (int i = 0; i < subject.Num_of_periods; i++)
            {
                ScheduleAppointment app = new ScheduleAppointment()
                {
                    StartTime = mainDate,
                    EndTime = mainDate.AddMinutes(subject.Duration_of_period * 45),
                    Subject = subject.Name + "\n" + subject.Course.Name,
                    AllDay = false
                };
                string symbol = cr.Id;
                string display = "seats: " + cr.Num_of_seats + "\tProjector: " + cr.Projector + "\tBoard: " + cr.Board + "\tsmart board: " + cr.Smart_board + "\tOS: "
                    + cr.Os + "\nsoftware: ";
                bool found = false;
                foreach (Resource r in resourceType.ResourceCollection.ToArray())
                {
                    if (r.ResourceName == symbol)
                    {
                        found = true;
                        break;
                    }
                }
                display += cr.softwares();
                Resource res = new Resource { TypeName = "Classroom", ResourceName = symbol, DisplayName = display };
                if (!found)
                {
                    resourceType.ResourceCollection.Add(res);
                }
                app.ResourceCollection.Add(res);
                schedule.Appointments.Add(app);
            }
        }
        private void remove_subject_from_schedule(Object sender, RoutedEventArgs e) { }
        private void return_subject_to_placeholder(Object sender, RoutedEventArgs e) { }

    }




        private void loadData()
        {
            
            using (StreamReader cit = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\files\\savings.txt"))
            {
                string line = cit.ReadLine();
                if (line != null)
                {
                    string[] lista;

                    while ((line = cit.ReadLine()) != "courses")
                    {
                        lista = line.Split(',');
                        Software s = new Software();
                        s.Id = lista[0];
                        s.Os = (Classroom.OpSystem)Enum.Parse(typeof(Classroom.OpSystem),lista[1]);
                        s.Name = lista[2];
                        s.Maker = lista[3];
                        s.Site = lista[4];
                        s.Year = int.Parse(lista[5]);
                        s.Price = double.Parse(lista[6]);
                        s.Description = lista[7];

                        allSoftware.Add(s);
                        allSoftwareIds.Add(lista[0]);
                        /*foreach (CurrencyClass cur in digitalCurrenciesList)
                        {
                            if (cur.Symbol.Equals(cc.Symbol))
                            {
                                cur.CheckedBox = true;
                            }
                        }

                        chosenDigitalList.Insert(0, cc);*/
                    }
                    while ((line = cit.ReadLine()) != "subjects")
                    {
                        lista = line.Split(',');
                        Course c = new Course();
                        c.Id = lista[0];
                        c.Name = lista[1];
                        c.Description = lista[2];
                        c.Date_of_conception = DateTime.Parse(lista[3]);

                        allCourses.Add(c);
                    }
                    while ((line = cit.ReadLine()) != "classrooms")
                    {
                        lista = line.Split(',');
                        Subject s = new Subject();
                        s.Id = lista[0];
                        s.Name = lista[1];
                        s.Description = lista[2];
                        s.Board = Boolean.Parse(lista[3]);
                        s.Smart_board = Boolean.Parse(lista[4]);
                        s.Projector = Boolean.Parse(lista[5]);
                        s.Duration_of_period = int.Parse(lista[6]);
                        s.Num_of_periods = int.Parse(lista[7]);
                        s.Size_of_group = int.Parse(lista[8]);
                        
                        foreach (Course c in allCourses)
                        {
                            if (c.Id.Equals(lista[9]))
                            {
                                s.Course = c;
                            }
                        }

                        s.Os = (Classroom.OpSystem)Enum.Parse(typeof(Classroom.OpSystem), lista[10]);

                        s.Software = new List<Software>();
                        string[] listica = lista[11].Split('|');
                        foreach (string soft in listica)
                        {
                            foreach (Software software in allSoftware)
                            {
                                if (soft.Equals(software.Id)){
                                    s.Software.Add(software);
                                }
                            }
                        }

                        allSubjects.Add(s);
                    }
                    while ((line = cit.ReadLine()) != null)
                    {
                        lista = line.Split(',');
                        Classroom c = new Classroom();
                        c.Id = lista[0];
                        c.Description = lista[1];
                        c.Num_of_seats = int.Parse(lista[2]);
                        c.Board = Boolean.Parse(lista[3]);
                        c.Smart_board = Boolean.Parse(lista[4]);
                        c.Projector = Boolean.Parse(lista[5]);
                        c.Os = (Classroom.OpSystem)Enum.Parse(typeof(Classroom.OpSystem), lista[6]);

                        c.Software = new List<Software>();
                        string[] listica = lista[7].Split('|');
                        foreach (string soft in listica)
                        {
                            foreach (Software software in allSoftware)
                            {
                                if (soft.Equals(software.Id))
                                {
                                    c.Software.Add(software);
                                }
                            }
                        }

                        allClassrooms.Add(c);
                    }
                   
                }

                foreach (Software s in allSoftware)
                {
                    Console.WriteLine(s);
                }

                foreach (Course s in allCourses)
                {
                    Console.WriteLine(s);
                }

                foreach (Subject s in allSubjects)
                {
                    Console.WriteLine(s);
                }

                foreach (Classroom s in allClassrooms)
                {
                    Console.WriteLine(s);
                }
            }
        }

            
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            using (StreamWriter pis = new StreamWriter(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\files\\savings.txt"))
            {
                //pis.WriteLine(updateInterval + "," + cbUpdateInterval.SelectedIndex);
                //pis.WriteLine(historyInterval + "," + cbHistoryInterval.SelectedIndex);
                //pis.WriteLine(referentCurrency.Symbol + "," + referentCurrency.Name + "," + cbRefCurrencies.SelectedIndex);

                pis.WriteLine("software");
                foreach (Software s in allSoftware)
                {
                    pis.WriteLine(s.Id + "," + s.Os.ToString() + "," + s.Name + "," + s.Maker + "," + s.Site + "," + s.Year + "," + s.Price + "," + s.Description);
                }

                pis.WriteLine("courses");
                foreach (Course c in allCourses)
                {
                    pis.WriteLine(c.Id+","+c.Name+","+c.Description+","+c.Date_of_conception.ToString());
                }

                pis.WriteLine("subjects");
                foreach (Subject s in allSubjects)
                {
                    string softwareList = "";
                    bool first = true;
                    foreach (Software soft in s.Software)
                    {
                        if (first)
                        {
                            softwareList = softwareList + soft.Id;
                            first = false;
                        }else
                        {
                            softwareList = softwareList + "|" + soft.Id;
                        }
                    }
                    pis.WriteLine(s.Id+","+s.Name+","+s.Description+","+s.Board+","+s.Smart_board+","+s.Projector+","+s.Duration_of_period+","+s.Num_of_periods+","+s.Size_of_group+","+s.Course.Id+","+s.Os+","+softwareList);
                }


                pis.WriteLine("classrooms");
                foreach (Classroom c in allClassrooms)
                {
                    string softwareList = "";
                    bool first = true;
                    foreach (Software soft in c.Software)
                    {
                        if (first)
                        {
                            softwareList = softwareList + soft.Id;
                            first = false;
                        }
                        else
                        {
                            softwareList = softwareList + "|" + soft.Id;
                        }
                    }
                    pis.WriteLine(c.Id+","+c.Description+","+c.Num_of_seats+","+c.Board+","+c.Smart_board+","+c.Projector+","+c.Os.ToString()+","+softwareList);
                }

                /*pis.WriteLine("ChosenDigital");
                foreach (CurrencyClass cc in chosenDigitalList)
                {
                    pis.WriteLine(cc.Symbol + "," + cc.Name);
                }
                pis.WriteLine("ChosenPhysical");
                foreach (CurrencyClass cc in chosenPhysicalList)
                {
                    pis.WriteLine(cc.Symbol + "," + cc.Name);
                }
                pis.WriteLine("ChosenStock");
                foreach (CurrencyClass cc in chosenStockList)
                {
                    pis.WriteLine(cc.Symbol + "," + cc.Name);
                }
                pis.WriteLine("Shown");
                foreach (ShowingCurrencyClass scc in shownCurrenciesList)
                {
                    pis.WriteLine(scc.Metadata.ElementAt(1).Value + "," + scc.Type);
                }*/
            }
        }
    }
}
