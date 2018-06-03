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
        public static List<string> allClassroomsIds = new List<string>();
        public static List<string> allCoursesIds = new List<string>();
        public static List<string> allSoftwareIds = new List<string>();
        public static List<string> allSubjectsIds = new List<string>();

        public static ObservableCollection<Subject> allSubjects { get; set; }
        public static ObservableCollection<Course> allCourses { get; set; }
        public static ObservableCollection<Software> allSoftware { get; set; }
        public static ObservableCollection<Classroom> allClassrooms { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            SfSchedule schedule = new SfSchedule();
            schedule.TimeMode = TimeModes.TwentyFourHours;
            schedule.TimeInterval = TimeInterval.FifteenMin;
            schedule.ScheduleType = ScheduleType.Week;
            schedule.WorkStartHour = 6;
            schedule.WorkEndHour = 23;
            

            //initialize collections

            allSubjects = new ObservableCollection<Subject>();
            allSoftware = new ObservableCollection<Software>();
            allClassrooms = new ObservableCollection<Classroom>();
            allCourses = new ObservableCollection<Course>();

            loadData();
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
