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
using Syncfusion.UI.Xaml.Grid;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HCI2___Back_To_Slay.windows
{
    /// <summary>
    /// Interaction logic for Choose_Software.xaml
    /// </summary>
    public partial class Choose_Software : Window
    {

        public static ObservableCollection<Software> all = new ObservableCollection<Software>();
        public static ObservableCollection<Software> added = new ObservableCollection<Software>();
        public static List<Software> chosen_software = new List<Software>();
        private DataGrid currentDG = null;

        private ObservableCollection<Software> temp_all = new ObservableCollection<Software>();
        private ObservableCollection<Software> temp_added = new ObservableCollection<Software>();
        private Classroom.OpSystem os;

        public static RoutedCommand exitDemoMode = new RoutedCommand();

        public Choose_Software(Classroom.OpSystem a)
        {
            this.DataContext = this;
            exitDemoMode.InputGestures.Add(new KeyGesture(Key.Escape));
            this.Closing += new CancelEventHandler(Window_Closing);

            os = a;
            InitializeComponent();
            all = new ObservableCollection<Software>();
            added = new ObservableCollection<Software>();
            if (chosen_software.Count() == 0)
            {
                foreach (Software sw in MainWindow.allSoftware)
                {
                    if (a.Equals(Classroom.OpSystem.WindowsAndLinux) || sw.Os.Equals(a) || sw.Os.Equals(Classroom.OpSystem.WindowsAndLinux))
                    {
                        all.Add(sw);
                    }
                }
            }
            else
            {
                foreach (Software sw in MainWindow.allSoftware)
                {
                    if (a.Equals(Classroom.OpSystem.WindowsAndLinux) || sw.Os.Equals(a) || sw.Os.Equals(Classroom.OpSystem.WindowsAndLinux))
                    {
                        if (chosen_software.Contains(sw))
                        {
                            added.Add(sw);
                        }
                        else
                        {
                            all.Add(sw);
                        }
                    }

                }
            }
            addedSoftwareDG.ItemsSource = added;
            allSoftwareDG.ItemsSource = all;
        }

        private void software_added_click(object sender, MouseButtonEventArgs e)
        {
            currentDG = (DataGrid)sender;
            if (currentDG != null)
            {
                Software sw = (Software)allSoftwareDG.SelectedItem;
                if (sw == null)
                {
                    return;
                }
                all.Remove(sw);
                added.Add(sw);
                if (temp_all.Contains(sw))
                {
                    temp_all.Remove(sw);
                    temp_added.Add(sw);
                }
                search_added_tb.Text = "";
            }
        }

        private void software_removed_click(object sender, MouseButtonEventArgs e)
        {
            currentDG = (DataGrid)sender;
            if (currentDG != null)
            {
                Software sw = (Software)addedSoftwareDG.SelectedItem;
                if (sw == null)
                {
                    return;
                }
                added.Remove(sw);
                all.Add(sw);
                if (temp_added.Contains(sw))
                {
                    temp_added.Remove(sw);
                    temp_all.Add(sw);
                }
                search_all_tb.Text = "";
            }
        }

        private void show_software(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            currentDG = (DataGrid)btn.Parent;
            DataGridRow row = Helper.detect_selected_row((DependencyObject)e.OriginalSource);
            currentDG = ItemsControl.ItemsControlFromItemContainer(row) as DataGrid;
            int index = currentDG.ItemContainerGenerator.IndexFromContainer(row);
            if (index == -1)
            {
                return;
            }
            if (currentDG.Name.Equals("allSoftwareDG"))
            {
                Software_Info si = new Software_Info(all.ElementAt(index));

                si.Closed += new EventHandler((sender2, e2) => check_data(sender2, e2));
                si.ShowDialog();
            }
            else
            {
                Software_Info si = new Software_Info(added.ElementAt(index));
                si.Closed += new EventHandler((sender2, e2) => check_data_added(sender2, e2));
                si.ShowDialog();
            }
        }

        private void add_close(object sender, RoutedEventArgs e)
        {
            chosen_software = added.ToList();
            this.Close();
        }

        private void close_win(object sender, RoutedEventArgs e)
        {
            all = new ObservableCollection<Software>();
            added = new ObservableCollection<Software>();
            this.Close();
        }

        /*private void refresh_data(object sender, EventArgs e)
        {
            bool found = false;
            if (Software_Info.deleted != null)
            {

                foreach (Software sw in all)
                {
                    if (sw.Id.Equals(Software_Info.deleted))
                    {
                        found = true;
                        all.Remove(sw);
                        break;
                    }
                }
                if (!found)
                {
                    foreach (Software sw in added)
                    {
                        if (sw.Id.Equals(Software_Info.deleted))
                        {
                            added.Remove(sw);
                            break;
                        }
                    }
                }
                return;
            }
            if (Software_Info.updated != null)
            {
                foreach (Software sw in all)
                {
                    if (sw.Id.Equals(Software_Info.updated))
                    {
                        found = true;
                        all.Remove(sw);
                        foreach (Software s in MainWindow.allSoftware)
                        {
                            if (s.Id.Equals(sw.Id))
                            {
                                all.Add(s);
                                break;
                            }
                        }
                        break;
                    }
                }
                if (!found)
                {
                    foreach (Software sw in added)
                    {
                        if (sw.Id.Equals(Software_Info.updated))
                        {
                            added.Remove(sw);
                            foreach (Software s in MainWindow.allSoftware)
                            {
                                if (s.Id.Equals(sw.Id))
                                {
                                    added.Add(s);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                return;
            }
        }*/

        private void add_new_software(object sender, RoutedEventArgs e)
        {
            Add_Software ac = new Add_Software(os);
            search_all_tb.Text = "";
            ac.Closed += new EventHandler((sender2, e2) => added_new(sender2, e2));
            ac.ShowDialog();
        }

        private void added_new(object sender, EventArgs e)
        {
            foreach(int idx in Add_Software.added)
            {
                all.Add(MainWindow.allSoftware.ElementAt(idx));
            }
            Add_Software.added = null;
            check_data(sender, e);
        }


        private void search_all_software(object sender, TextChangedEventArgs e)
        {
            string search_text = search_all_tb.Text.ToLower();
            if (search_text.Equals(""))
            {
                //temp_all = null;
                allSoftwareDG.ItemsSource = all;
            }else
            {
                load_temp_all(search_text, true);
                allSoftwareDG.ItemsSource = temp_all;
            }
        }

        private void search_added_software(object sender, TextChangedEventArgs e)
        {
            string search_text = search_added_tb.Text.ToLower();
            if (search_text.Equals(""))
            {
                //temp_all = null;
                addedSoftwareDG.ItemsSource = added;
            }
            else
            {
                load_temp_added(search_text, true);
                addedSoftwareDG.ItemsSource = temp_added;
            }
        }

        private void load_temp_all(string search_text, bool reload)
        {
            temp_all = new ObservableCollection<Software>();

            if (search_text.Equals(""))
            {
                temp_all = new ObservableCollection<Software>();
                foreach (Software sw in MainWindow.allSoftware)
                {
                    if (all.Contains(sw))
                    {
                        temp_all.Add(sw);
                    }
                }
                return;
            }
            foreach (Software sw in all)
            {
                if (sw.Name.ToLower().Contains(search_text))
                {
                    temp_all.Add(sw);
                }
            }
            if (temp_all.Count() == 0)
            {
                MessageBox.Show("No software found!");
                return;
            }
        }

        private void load_temp_added(string search_text, bool reload)
        {
            temp_added = new ObservableCollection<Software>();

            if (search_text.Equals(""))
            {
                temp_added = new ObservableCollection<Software>();
                foreach (Software sw in MainWindow.allSoftware)
                {
                    if (added.Contains(sw))
                    {
                        temp_added.Add(sw);
                    }
                }
                return;
            }
            foreach (Software sw in added)
            {
                if (sw.Name.ToLower().Contains(search_text))
                {
                    temp_added.Add(sw);
                }
            }
            if (temp_added.Count() == 0)
            {
                MessageBox.Show("No software found!");
                return;
            }
        }

        private void check_data(object sender, EventArgs e)
        {
            if (temp_all != null)
            {
                string search_text = search_all_tb.Text;
                load_temp_all(search_text, false);
                allSoftwareDG.ItemsSource = temp_all;
            }
        }

        private void check_data_added(object sender, EventArgs e)
        {
            if (temp_added != null)
            {
                string search_text = search_added_tb.Text;
                load_temp_added(search_text, false);
                addedSoftwareDG.ItemsSource = temp_added;
            }
        }


        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int indeks = 0;
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i].Title.Equals("Choose_Software"))
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

        

        private void exit_demo(object sender, RoutedEventArgs e)
        {
            if (MainWindow.demoModeOn)
            {
                //radi sta se radi kad se iskljuci demo
                MainWindow.turnOfDemo = true;
                //MessageBox.Show("exit demo");
            }

        }

        public void Window_Closing(object sender, CancelEventArgs e)
        {
            if (MainWindow.demoModeOn)
            {
                e.Cancel = true;
                if (MainWindow.turnOfDemo)
                {
                    //MessageBox.Show("Wait until demo creating subject finishes");
                }
                else
                {
                    //MessageBox.Show("Press 'Esc' key on keyboard if you want to stop demo mode and wait until it finishes");
                }

                
            }

        }
    }
}
