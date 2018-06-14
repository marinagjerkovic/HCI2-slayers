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
using System.Collections.ObjectModel;

namespace HCI2___Back_To_Slay.windows
{
    /// <summary>
    /// Interaction logic for Software_Multiple.xaml
    /// </summary>
    /// shows given software data; if argument is null, shows all software
    public partial class Software_Multiple : Window
    {
        private ObservableCollection<Software> Show_Data = new ObservableCollection<Software>();
        //private Classroom current_cr = null;

        private List<Software> current_sws = new List<Software>();
        private Classroom.OpSystem current_os;

        private int index;
        private bool sub;

        //constructor
        public Software_Multiple()
        {
            Show_Data = MainWindow.allSoftware;
            InitializeComponent();
            dataGrid.ItemsSource = Show_Data;
        }

        public Software_Multiple(Classroom cr)
        {
            current_sws = cr.Software;
            current_os = cr.Os;
            index = MainWindow.allClassrooms.IndexOf(cr);
            sub = false;

            foreach(Software sw in current_sws)
            {
                Show_Data.Add(sw);
            }
            InitializeComponent();
            dataGrid.ItemsSource = Show_Data;
            ar_sw.Visibility = Visibility.Visible;
        }

        public Software_Multiple(Subject subj)
        {
            current_sws = subj.Software;
            current_os = subj.Os;
            index = MainWindow.allSubjects.IndexOf(subj);
            sub = true;

            foreach (Software sw in current_sws)
            {
                Show_Data.Add(sw);
            }
            InitializeComponent();
            dataGrid.ItemsSource = Show_Data;
            ar_sw.Visibility = Visibility.Visible;
        }

        private void choose_software(object sender, RoutedEventArgs e)
        {
            Choose_Software.chosen_software = current_sws;
            Choose_Software sw = new Choose_Software(current_os);
            sw.Closed += new EventHandler((sender2, e2) => load_data(sender2, e2));
            sw.ShowDialog();
        }

        private void show_software(object sender, RoutedEventArgs e)
        {
            DataGridRow row = Helper.detect_selected_row((DependencyObject)e.OriginalSource);
            dataGrid = ItemsControl.ItemsControlFromItemContainer(row) as DataGrid;
            int index = dataGrid.ItemContainerGenerator.IndexFromContainer(row);
            if (index == -1)
            {
                return;
            }

            Software_Info si = new Software_Info(Show_Data.ElementAt(index));

            if (current_sws.Count()==0)
            {
                si.Closed += new EventHandler((sender2, e2) => refresh_data(sender2, e2));
            }
            si.ShowDialog();
        }

        private void load_data(object sender, EventArgs e)
        {
            current_sws = Choose_Software.chosen_software;
            Show_Data = new ObservableCollection<Software>();
            foreach(Software sw in current_sws)
            {
                Show_Data.Add(sw);
            }
            if (sub)
            {
                MainWindow.allSubjects.ElementAt(index).Software = current_sws;
            }
            else
            {
                MainWindow.allClassrooms.ElementAt(index).Software = current_sws;
            }
            dataGrid.ItemsSource = Show_Data;
        }

        private void refresh_data(object sender, EventArgs e)
        {
            bool found = false;
            if (Software_Info.deleted != null)
            {
                foreach(Software sw in Show_Data)
                {
                    if (sw.Id.Equals(Software_Info.deleted))
                    {
                        found = true;
                        Show_Data.Remove(sw);
                        break;
                    }
                }
            }
            if (!found && Software_Info.updated != null)
            {
                foreach (Software sw in Show_Data)
                {
                    if (sw.Id.Equals(Software_Info.updated))
                    {
                        Show_Data.Remove(sw);
                        foreach(Software s in MainWindow.allSoftware)
                        {
                            if (s.Id.Equals(sw.Id))
                            {
                                Show_Data.Add(s);
                                break;
                            }
                        }
                        break;
                    }
                }
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
    }
}
