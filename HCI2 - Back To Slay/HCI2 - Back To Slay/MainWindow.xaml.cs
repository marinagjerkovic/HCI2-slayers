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
using System.Reflection;
using System.Windows.Automation.Peers;
using System.ComponentModel;



namespace HCI2___Back_To_Slay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window
    {

        public static SfSchedule schedule;
        public DateTime mainDate;
        public static List<string> allClassroomsIds = new List<string>();
        public static List<string> allCoursesIds = new List<string>();
        public static List<string> allSoftwareIds = new List<string>();
        public static List<string> allSubjectsIds = new List<string>();
        public static List<Appointment> realApps = new List<Appointment>();

        public static ObservableCollection<Subject> allSubjects { get; set; }
        public static ObservableCollection<Course> allCourses { get; set; }
        public static ObservableCollection<Software> allSoftware { get; set; }
        public static ObservableCollection<Classroom> allClassrooms { get; set; }

        public static ObservableCollection<Subject> leftSubjects { get; set; }
        public static ObservableCollection<Subject> shownSubjects { get; set; }

        public ObservableCollection<Appointment> app2add { get; set; }

        public ResourceType resourceType;

        public static Dictionary<Appointment, ScheduleAppointment> appointments = new Dictionary<Appointment, ScheduleAppointment>();

        public static RoutedCommand exitDemoMode = new RoutedCommand();
        public static Boolean demoModeOn = false;
        public static Boolean turnOfDemo = false;

        public MainWindow()
        {
            InitializeComponent();
            //loadData();

            this.DataContext = this;
            exitDemoMode.InputGestures.Add(new KeyGesture(Key.Escape));
            this.Closing += new CancelEventHandler(Window_Closing);

            mainDate = new DateTime(2018, 6, 10, 6, 0, 0);
            schedule = new SfSchedule();
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
            schedule.PreviewMouseLeftButtonUp += schedule_PreviewMouseLeftButtonUp;
            schedule.AppointmentEndDragging += schedule_AppointmentDropped;
            this.scheduleGrid.Children.Add(schedule);

            /*
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
            */
            allSubjects = new ObservableCollection<Subject>();
            allSoftware = new ObservableCollection<Software>();
            allClassrooms = new ObservableCollection<Classroom>();
            allCourses = new ObservableCollection<Course>();

            leftSubjects = new ObservableCollection<Subject>();
            shownSubjects = new ObservableCollection<Subject>();

            loadData();

            //leftSubjects.Add(subject);
            //leftSubjects.Add(subject2);
            //leftSubjects.Add(subject3);
            Console.WriteLine(allClassrooms.Count());
            classroomsDG.ItemsSource = allClassrooms;
            subjectsDG.ItemsSource = allSubjects;

            if (allClassrooms.Count() == 0)
            {
                schedule.IsEnabled = false;
                MessageBox.Show("You can't add anything on schedule before you create classroom!");

            }
            else
            {
                resourceType.ResourceCollection.Add(create_resource(allClassrooms.First()));
            }
            //schedule.appo
        }

        public static void enableSchedule()
        {
            if (allClassrooms.Count > 0)
            {
                schedule.IsEnabled = true;
            }
        }

        public static void disableSchedule()
        {
            if (allClassrooms.Count == 0)
            {
                schedule.IsEnabled = false;
            }
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
            string display = symbol+"\nseats: " + cr.Num_of_seats + "\tProjector: " + cr.Projector + "\tBoard: " + cr.Board + "\tsmart board: " + cr.Smart_board + "\tOS: "
                + cr.Os + "\nsoftware: ";
            display += cr.softwares();
            Resource r = new Resource { DisplayName = display, ResourceName = symbol, TypeName="Classroom"};
            return r;

        }

        private Resource create_resource(Resource res)
        {
            Resource r = new Resource { DisplayName = res.DisplayName, ResourceName = res.ResourceName, TypeName = res.TypeName };
            return r;
        }

        public static bool update_classroom_schedule(string id)
        {
            bool retVal = true;
            foreach(Appointment app in realApps)
            {
                if (app.Classroom.Id == id)
                {
                    retVal = false;
                    break;
                }
            }
            return retVal;
        }

        public static bool update_subject_schedule(string id)
        {
            bool retVal = true;
            foreach (Appointment app in realApps)
            {
                if (app.Subject.Id == id)
                {
                    retVal = false;
                    break;
                }
            }
            return retVal;
        }

        public static bool update_course_schedule(string id)
        {
            bool retVal = true;
            foreach (Appointment app in realApps)
            {
                if (app.Subject.Course.Id == id)
                {
                    retVal = false;
                    break;
                }
            }
            return retVal;
        }

        public static bool update_software_schedule(string id)
        {
            bool retVal = true;
            if (realApps.Count > 0)
            {
                retVal = false;
            }
            return retVal;
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
            if (cr == null)
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
                    AllDay = false,
                    Location = cr.Id
                };
                Appointment realApp = new Appointment(cr, subject, app.StartTime,app.EndTime);
                realApps.Add(realApp);
                Console.WriteLine("Dodat 1 sad ima " + realApps.Count() + " appointmenta");
                string symbol = cr.Id;
                bool found=false;
                foreach (Resource r in resourceType.ResourceCollection.ToArray())
                {
                    if (r.ResourceName == symbol)
                    {
                        found = true;
                        break;
                    }
                }
                Resource res = create_resource(cr);
                if (!found)
                {
                    resourceType.ResourceCollection.Add(res);
                }
                app.ResourceCollection.Add(res);
                schedule.Appointments.Add(app);
                appointments[realApp] = app;
                
            }
            //Console.WriteLine("__________ZAVRSENO DODAVANJE_______________");
        }

        private void remove_subject_from_schedule(Object sender, RoutedEventArgs e)
        {
            List<ScheduleAppointment> removeApp = new List<ScheduleAppointment>();
            List<Appointment> removeRealApp = new List<Appointment>();
            Subject subject = (Subject)subjectsDG.SelectedItem;
            Console.WriteLine("__schApps___");
            foreach (ScheduleAppointment app in schedule.Appointments)
            {
                Console.WriteLine(app.StartTime.ToShortTimeString());
                if (app.Subject.Contains(subject.Name))
                {
                    Console.Write(" for remove");
                    removeApp.Add(app);
                }
            }
            foreach (ScheduleAppointment app in removeApp)
            {
                if (app.Subject.Contains(subject.Name) && app.Subject.Contains(subject.Course.Name))
                {
                    schedule.Appointments.Remove(app);
                    
                }
            }
            foreach(Appointment realApp in realApps)
            {
                if (realApp.Subject.Id == subject.Id)
                {
                    removeRealApp.Add(realApp);
                }
            }
            foreach (Appointment realApp in removeRealApp)
            {
                realApps.Remove(realApp);
                appointments.Remove(realApp);
            }
            Console.WriteLine("UKLONJEN JEDAN PREDMET! Sada ima " + realApps.Count() + " apointmenta");
        }
    
        private void show_all_software(object sender, RoutedEventArgs e)
        {
            Software_Multiple sm = new Software_Multiple();
            sm.ShowDialog();
        }

        private void show_all_classrooms(object sender, RoutedEventArgs e)
        {
            Classroom_Multiple cm = new Classroom_Multiple();
            cm.ShowDialog();
        }

        private void show_all_courses(object sender, RoutedEventArgs e)
        {
            Courses_Multiple cm = new Courses_Multiple(false);
            cm.ShowDialog();
        }

        private void show_all_subjects(object sender, RoutedEventArgs e)
        {
            Subject_Multiple sm = new Subject_Multiple();
            sm.ShowDialog();
        }

        private void menu_file_save(object sender, RoutedEventArgs e) {
            schedule.ExportICS();
        }

        private void menu_file_open(object sender, RoutedEventArgs e) {
            if (schedule != null && schedule.IsEnabled)
            {
                if (MessageBox.Show("Do you want to save first?", "New Schedule", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    schedule.ExportICS();
            }
            appointments.Clear();
            schedule.Appointments.Clear();
            realApps.Clear();
            resourceType = new ResourceType { TypeName = "Classroom" };
            schedule.ScheduleResourceTypeCollection = new ObservableCollection<ResourceType> { resourceType };
            schedule.Resource = "Classroom";
            schedule.Refresh();
            schedule.ImportICS();           
            foreach(ScheduleAppointment app in schedule.Appointments)
            {
                Appointment realApp = new Appointment(app);
                realApps.Add(realApp);
                appointments[realApp] = app;
            }
            add_resources_open();
            schedule.Refresh();
        }

        private void menu_file_new(object sender, RoutedEventArgs e) {
            if (schedule != null && schedule.IsEnabled)
            {
                if (MessageBox.Show("Do you want to save first?", "New Schedule", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    schedule.ExportICS();
            }
              
            schedule = new SfSchedule();
            schedule.Appointments.Clear();
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
            schedule.MoveToDate(mainDate);
            schedule.PreviewMouseLeftButtonUp += schedule_PreviewMouseLeftButtonUp;
            schedule.AppointmentEndDragging += schedule_AppointmentDropped;  
        }

        private void add_resources_open()
        {
            foreach(Appointment app in realApps)
            {
                Classroom cr = app.Classroom;

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
                Resource res = create_resource(cr);
                if (!found)
                {
                    resourceType.ResourceCollection.Add(res);
                }
                appointments[app].ResourceCollection.Add(res);
            }
            foreach(ScheduleAppointment app in schedule.Appointments)
            {
                foreach(Resource res in app.ResourceCollection)
                    Console.WriteLine(res.ResourceName+res.TypeName);
            }
        }

        ScheduleAppointment temp_app;
        private void schedule_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            Console.WriteLine("POCEO");
            temp_app = (ScheduleAppointment)AppointmentCloning((sender as SfSchedule).SelectedAppointment);
        }

        internal object AppointmentCloning(ScheduleAppointment app)
        {
            Console.WriteLine("POCEO");
            if (app != null)
            {
                Console.WriteLine("KLONIRA");
                ScheduleAppointment newapp = new ScheduleAppointment();
                newapp.Subject = app.Subject;
                newapp.StartTime = app.StartTime;
                newapp.EndTime = app.EndTime;
                Resource res = (Resource)app.ResourceCollection.ToArray()[0];
                newapp.ResourceCollection.Add(create_resource(res));
                return newapp;
            }
            Console.WriteLine("NULL");
            return null;
        }

        private void schedule_AppointmentDropped(object sender, AppointmentEndDraggingEventArgs args)
        {
            ScheduleAppointment app = schedule.SelectedAppointment;
            if (MessageBox.Show("Are you Sure for Drag and Drop the appointment?", "Drag and Drop", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                Console.WriteLine("usao " + temp_app.StartTime.DayOfWeek+app.StartTime.DayOfWeek);
                (sender as SfSchedule).Appointments.Remove((sender as SfSchedule).SelectedAppointment);
                schedule.Appointments.Add(temp_app);
            }
            else
            {
            }
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
            deleteDemoEntities();
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

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp(str, this);
            }
        }

        public void doThings(string param)
        {
            Title = param;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void show_help(object sender, RoutedEventArgs e)
        {
            HelpProvider.ShowHelp("index", this);        
        }

        private async void start_demo(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Press 'Esc' key on keyboard if you want to stop demo mode and wait until it finishes");
            demoModeOn = true;
            turnOfDemo = false;           
            await start();
            
        }
       

        private async Task start()
        {
            while (demoModeOn)
            {
                this.IsHitTestVisible = false;
                deleteDemoEntities();
                fillDemoEntities();
                await startingDemo();
                if (turnOfDemo)
                {
                    demoModeOn = false;
                    turnOfDemo = false;
                }
            }

            deleteDemoEntities();
            this.IsHitTestVisible = true;
            
        }

        private void fillDemoEntities()
        {
            Software s = new Software();
            s.Id = "Example_Id";
            s.Description = "Example_Description";
            s.Maker = "Example_Maker";
            s.Name = "Example_Name";
            s.Os = Classroom.OpSystem.Linux;
            s.Price = 100;
            s.Site = "Example_Site";
            s.Year = 2000;
            allSoftware.Add(s);
            allSoftwareIds.Add("Example_Id");

            Course c = new Course();
            c.Date_of_conception = System.DateTime.Now;
            c.Description = "Example_Description";
            c.Id = "Example_Id";
            c.Name = "Example_Name";
            allCourses.Add(c);
            allCoursesIds.Add("Example_Id");

            Classroom cr = new Classroom();
            cr.Board = false;
            cr.Description = "Example_Description";
            cr.Id = "Example_Id";
            cr.Num_of_seats = 15;
            cr.Os = Classroom.OpSystem.Linux;
            cr.Projector = true;
            cr.Smart_board = false;
            cr.Software.Add(s);
            allClassrooms.Add(cr);
            allClassroomsIds.Add("Example_Id");


        }

        private async Task startingDemo()
        {
            Add_Subject adds = new Add_Subject();
            adds.IsHitTestVisible = false;
            adds.Show();

            await addSubjectDemo(adds);
            demoModeOn = false;
            adds.Close();
            demoModeOn = true;
        }

        private async Task addSubjectDemo(Add_Subject adds)
        {
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            adds.id.Text = "Example_Id";
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            adds.name.Text = "Example_Name";
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            adds.description.Text = "Example_Description";
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            Brush color = adds.add_crs.Background;
            adds.add_crs.Background = SystemColors.HighlightBrush;
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            adds.add_crs.Background = color;
            Courses_Multiple cm = new Courses_Multiple(false);
            cm.IsHitTestVisible = false;
            await chooseCourseDemo(cm);            
            await Task.Delay(1500);
            
            
            adds.sel_crs.Text = ((Course)(cm.dataGrid.Items[0])).Name;
            demoModeOn = false;
            cm.Close();
            demoModeOn = true;
            if (turnOfDemo)
                return;
            adds.sel_crs.Visibility = Visibility.Visible;
            adds.change_crs.Visibility = Visibility.Visible;
            adds.add_crs.Visibility = Visibility.Hidden;
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            adds.group_size.Text = "15";
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            adds.duration.Text = "2";
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            adds.num_periods.Text = "2";
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            adds.projNo.IsChecked = true;
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            adds.boardNo.IsChecked = true;
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            adds.smartNo.IsChecked = true;
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            adds.linux.IsChecked = true;
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            Choose_Software cs = new Choose_Software(Classroom.OpSystem.Linux);
            cs.IsHitTestVisible = false;            
            adds.add_sw_btn.Background = SystemColors.HighlightBrush;
            await Task.Delay(1500);
            adds.add_sw_btn.Background = color;
            await chooseSoftwareDemo(cs);
            demoModeOn = false;
            cs.Close();
            demoModeOn = true;
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            adds.button.Background = SystemColors.HighlightBrush;
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
        }

        private async Task chooseSoftwareDemo(Choose_Software cs)
        {
            
            cs.Show();
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            cs.allSoftwareDG.SelectedIndex = 0;
            if (turnOfDemo)
                return;
            await Task.Delay(1500);            
            Software s = (Software)cs.allSoftwareDG.Items[0];
            Choose_Software.all.RemoveAt(0);
            Choose_Software.added.Add(s);
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            cs.add_btn.Background = SystemColors.HighlightBrush;
            if (turnOfDemo)
                return;
            await Task.Delay(1000);            
            Choose_Software.all.Add(s);
            Choose_Software.added.RemoveAt(0);
        }

        private async Task chooseCourseDemo(Courses_Multiple cm)
        {
            if (turnOfDemo)
                return;
            cm.save_crs_btn.Visibility = Visibility.Visible;
            cm.Show();
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            cm.dataGrid.SelectedIndex = 0;
            if (turnOfDemo)
                return;
            await Task.Delay(1500);
            cm.save_crs_btn.Background = SystemColors.HighlightBrush;
        }

        private static void deleteDemoEntities()
        {
            Classroom delclass = null;
            Subject delsubj = null;
            Course delcourse = null;
            Software delsoft = null;
            foreach (Classroom c in allClassrooms)
            {
                if (c.Id.Equals("Example_Id"))
                {
                    delclass = c;
                }
            }
            allClassrooms.Remove(delclass);
            allClassroomsIds.Remove("Example_Id");

            foreach (Subject s in allSubjects)
            {
                if (s.Id.Equals("Example_Id"))
                {
                    delsubj = s;
                }
            }
            allSubjects.Remove(delsubj);
            allSubjectsIds.Remove("Example_Id");
            
            foreach (Software s in allSoftware)
            {
                if (s.Id.Equals("Example_Id"))
                {
                    delsoft = s;
                }
            }
            allSoftware.Remove(delsoft);
            allSoftwareIds.Remove("Example_Id");

            foreach (Course c in allCourses)
            {
                if (c.Id.Equals("Example_Id"))
                {
                    delcourse = c;
                }
            }
            allCourses.Remove(delcourse);
            allCoursesIds.Remove("Example_Id");

            
        }

        private void exit_demo(object sender, RoutedEventArgs e)
        {
            if (demoModeOn)
            {
                //radi sta se radi kad se iskljuci demo
                turnOfDemo = true;
                //MessageBox.Show("exit demo");
            }
            
        }

        public void Window_Closing(object sender, CancelEventArgs e)
        {
            if (demoModeOn)
            {
                e.Cancel = true;
                if (turnOfDemo)
                {
                    //MessageBox.Show("Wait until demo creating subject finishes");
                }else
                {
                    //MessageBox.Show("Press 'Esc' key on keyboard if you want to stop demo mode and wait until it finishes");
                }

                
            }
            
        }
    }
}
