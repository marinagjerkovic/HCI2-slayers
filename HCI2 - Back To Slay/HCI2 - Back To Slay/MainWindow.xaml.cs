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



namespace HCI2___Back_To_Slay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window
    {

        //public static SfSchedule schedule;
        public DateTime mainDate;
        public static bool enable = true;
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

        public MainWindow()
        {
            InitializeComponent();
            //loadData();

            this.DataContext = this;
            exitDemoMode.InputGestures.Add(new KeyGesture(Key.Escape));

            mainDate = new DateTime(2018, 6, 10, 6, 0, 0);
            //schedule = new SfSchedule();
            schedule.TimeMode = TimeModes.TwentyFourHours;
            schedule.TimeInterval = TimeInterval.ThirtyMin;
            schedule.ScheduleType = ScheduleType.Week;
            schedule.FirstDayOfWeek = DayOfWeek.Monday;
            schedule.WorkStartHour = 6;
            schedule.WorkEndHour = 23;
            schedule.ShowNonWorkingHours = false;
            resourceType = new ResourceType { TypeName = "Classroom" };
            schedule.ScheduleResourceTypeCollection = new ObservableCollection<ResourceType> { resourceType };
            schedule.Resource = "Classroom";
            schedule.MoveToDate(mainDate);
            //schedule.PreviewMouseLeftButtonUp += schedule_PreviewMouseLeftButtonUp;
            schedule.AppointmentEndDragging += schedule_AppointmentDropped;
            
            schedule.AppointmentStartResizing += schedule_start_resize;
            schedule.AppointmentEndResizing += schedule_end_resize;
            schedule.AppointmentResizing += schedule_resize;
            //schedule.AppointmentEditorOpening += schedule_editor_open;
            //schedule.AppointmentEditorClosed += schedule_editor_closed;
            
            schedule.IsEnabled = enable;
            
            //schedule.AppointmentEditorOpening -= schedule_edit;
            //this.scheduleGrid.Children.Add(schedule);
            
            
            
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
            //Console.WriteLine(allClassrooms.Count());
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

        private void schedule_end_resize(object sender, AppointmentEndResizingEventArgs args)
        {
            args.Cancel = true;
        }

        private void schedule_start_resize(object sender, AppointmentStartResizingEventArgs args)
        {
            args.Cancel = true;
        }

        private void schedule_resize(object sender, AppointmentResizingEventArgs args)
        {
            
        }

        private void schedule_editor_open(object sender, AppointmentEditorOpeningEventArgs args)
        {
            args.Cancel = true;
        }

       

        public void enableSchedule()
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
            }
        }
        private void disable_schedule()
        {
            schedule.IsEnabled = false;
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
            if (cr == null)
            {
                MessageBox.Show("You must select classroom first!");
                return;
            }
            if(!classroom_drag_check(cr, subject))
            {
                MessageBox.Show("Subject don't fit in this classroom, check seat number/equipment/software");
                return;
            }
            

            if (allClassrooms.Count == 0)
            {
                MessageBox.Show("You can't add anything on schedule before you create classroom!");
                return;
            }
            
            foreach(ScheduleAppointment app in schedule.Appointments)
            {
                if (app.Subject.Contains(subject.Name))
                {
                    MessageBox.Show("This subject is allready added!");
                    return;
                }
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
                //Console.WriteLine("Dodat 1 sad ima " + realApps.Count() + " appointmenta");
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
                Console.WriteLine("____dodat predmet_____");
                realApp.printApp();
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
                Console.WriteLine(app.StartTime.Day);
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
            schedule.Refresh();
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
            //schedule.PreviewMouseLeftButtonUp += schedule_PreviewMouseLeftButtonUp;
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
                foreach (Resource res in app.ResourceCollection) { }

                   // Console.WriteLine(res.ResourceName+res.TypeName);
            }
        }

        private bool drop_check(Appointment app, string newCrId, DateTime newStartTime, DateTime newEndTime)
        {
            //DateTime newEndTime = newStartTime.AddMinutes
            
            if (app==null || app.Subject==null || app.Classroom == null)
            {
                MessageBox.Show("000Moving is forbidden because meanwhile some of the loaded subject/classroom/course are deleted");
                return false;
            }
            else 
            {
                MessageBox.Show(app.Classroom.Id+ newCrId);
                if(app.Classroom.Id != newCrId )
                {
                    Classroom newCr = get_new_classroom(newCrId);
                    if(!classroom_drag_check(newCr, app.Subject))
                    {
                        MessageBox.Show("111Moving is forbidden because selected subject can't fit in chosen classroom\nCheck group size/equipment/software");
                        return false;
                    }
                }
                if(!appointment_time_check(newCrId, newStartTime,newEndTime)){
                    {
                        MessageBox.Show("222Appointment can't fit in the choosen time!");
                        return false;
                    }
                }
            }
                
            return true;
        }

        private bool appointment_time_check(string newCrId, DateTime newStartTime, DateTime newEndTime)
        {
          
            ScheduleAppointment selApp = schedule.SelectedAppointment;
            if (selApp == null)
            {
                MessageBox.Show("time_check0");
                return false;
            }
            if(newStartTime.Hour < 6 || newEndTime.Hour > 23)
            {
                MessageBox.Show("time_check1");
                return false;
            }
            foreach(ScheduleAppointment app in schedule.Appointments)
            {
                //da ne proverava sa sammim sobom
                if (selApp.StartTime != app.StartTime && app.StartTime.DayOfWeek!=DayOfWeek.Sunday)
                {
                    if ((app.EndTime < newEndTime && app.EndTime > newStartTime) ||
                        (app.StartTime > newStartTime && app.StartTime < newEndTime))
                    {
                        MessageBox.Show("timecheck2");
                        return false;
                    }
                }
            }
            return true;
        }

        private bool classroom_drag_check(Classroom cr, Subject sub)
        {
            bool sofware_check = true;
            foreach(Software sw in sub.Software)
            {
                if(!cr.Software.Contains(sw)){
                    MessageBox.Show("classroom check sw");
                    return false;
                }
            }
            return (cr.Num_of_seats >= sub.Size_of_group) &&
                   !(cr.Projector == false && sub.Projector == true) &&
                   !(cr.Board == false && sub.Board == true) &&
                   !(cr.Smart_board == false && sub.Smart_board == true) &&
                   !(cr.Os != Classroom.OpSystem.WindowsAndLinux && cr.Os != sub.Os)&&
                   sofware_check;
        }

        private void schedule_AppointmentDropped(object sender, AppointmentEndDraggingEventArgs args)
        {
            schedule.Refresh();
            

            Console.WriteLine(args.To + "\n" + args.From);
            ScheduleAppointment app = (ScheduleAppointment)args.Appointment;
            if (app != null)
            {
                Console.WriteLine("___________________pomeranje______________________\n" + app.StartTime+" "+app.EndTime.DayOfWeek+" "+app.EndTime);
                string newCr = "";
                foreach (Resource res in app.ResourceCollection)
                {
                    newCr = res.ResourceName;
                    break;
                }
                Appointment realApp = get_real_appointment(app);
                DateTime to = args.To;
                DateTime end = to.AddMinutes(realApp.Subject.Duration_of_period * 45);
                realApp.printApp();
                Console.WriteLine(newCr + " get real app: " + realApp.Classroom.Id);
                if (!drop_check(realApp, newCr, to, end))
                {
                    app.ResourceCollection.Clear();
                    app.ResourceCollection.Add(create_resource(realApp.Classroom));
                    schedule.Refresh();
                    args.Cancel = true;
                    app.ResourceCollection.Clear();
                    app.ResourceCollection.Add(create_resource(realApp.Classroom));
                    schedule.Refresh();
                }
                else
                {
                    foreach (Resource res in app.ResourceCollection)
                    {
                        //Console.WriteLine("novi resurs: " + res.ResourceName);
                        appointments.Remove(get_real_appointment(app));
                        Appointment newReal = new Appointment(app);
                        app.Location = res.ResourceName;
                        appointments[newReal] = app;
                        break;
                    }
                }
            }
            schedule.Refresh();
        }

        private Appointment get_real_appointment(ScheduleAppointment app)
        {
            foreach(Appointment realApp in appointments.Keys)
            {
                if(appointments[realApp] == app)
                {
                    return realApp;
                }
                
       
            }
            MessageBox.Show("nema odg realApp!!");
            return null;
        }

        private Classroom get_new_classroom(string id)
        {
            foreach(Classroom cr in allClassrooms)
            {
                if(cr.Id == id)
                {
                    return cr;
                }
            }
            return null;
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
            
            demoModeOn = true;            
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
            adds.Close();
        }

        private async Task addSubjectDemo(Add_Subject adds)
        {
            await Task.Delay(1500);
            adds.id.Text = "Example_Id";
            await Task.Delay(1500);
            adds.name.Text = "Example_Name";
            await Task.Delay(1500);
            adds.description.Text = "Example_Description";
            await Task.Delay(1500);
            Brush color = adds.add_crs.Background;
            adds.add_crs.Background = Brushes.Blue;
            await Task.Delay(1500);
            adds.add_crs.Background = color;
            Courses_Multiple cm = new Courses_Multiple(false);
            cm.IsHitTestVisible = false;
            await chooseCourseDemo(cm);
            await Task.Delay(1500);
            
            cm.Close();
            await Task.Delay(1500);
            adds.group_size.Text = "15";
            await Task.Delay(1500);
            adds.duration.Text = "2";
            await Task.Delay(1500);
            adds.num_periods.Text = "2";
            await Task.Delay(1500);
            adds.projNo.IsChecked = true;
            await Task.Delay(1500);
            adds.boardNo.IsChecked = true;
            await Task.Delay(1500);
            adds.smartNo.IsChecked = true;
            await Task.Delay(1500);
            adds.linux.IsChecked = true;
            await Task.Delay(1500);
            Choose_Software cs = new Choose_Software(Classroom.OpSystem.Linux);
            cs.IsHitTestVisible = false;
            await Task.Delay(1500);
            adds.add_sw_btn.Background = Brushes.Blue;
            await Task.Delay(1500);
            adds.add_sw_btn.Background = color;
            await chooseSoftwareDemo(cs);            
            await Task.Delay(1500);
            adds.button.Background = Brushes.Blue;
            await Task.Delay(1500);
        }

        private async Task chooseSoftwareDemo(Choose_Software cs)
        {
            cs.Show();
            cs.allSoftwareDG.SelectedIndex = 0;
            await Task.Delay(1500);            
            Software s = (Software)cs.allSoftwareDG.Items[0];
            Choose_Software.all.RemoveAt(0);
            Choose_Software.added.Add(s);
            await Task.Delay(1500);
            cs.add_btn.Background = Brushes.Blue;
            await Task.Delay(1000);
            cs.Close();
            Choose_Software.all.Add(s);
            Choose_Software.added.RemoveAt(0);
        }

        private async Task chooseCourseDemo(Courses_Multiple cm)
        {
            cm.save_crs_btn.Visibility = Visibility.Visible;
            cm.Show();
            cm.dataGrid.SelectedIndex = 0;

            await Task.Delay(1500);
            cm.save_crs_btn.Background = Brushes.Blue;
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
                demoModeOn = false;
                MessageBox.Show("exit demo");
            }
            
        }
    }
}
