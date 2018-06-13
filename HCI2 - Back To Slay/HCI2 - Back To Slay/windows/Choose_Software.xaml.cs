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

namespace HCI2___Back_To_Slay.windows
{
    /// <summary>
    /// Interaction logic for Choose_Software.xaml
    /// </summary>
    public partial class Choose_Software : Window
    {

        public ObservableCollection<Software> all = new ObservableCollection<Software>();
        public ObservableCollection<Software> added = new ObservableCollection<Software>();
        private DataGrid currentDG=null;
        private static Classroom.OpSystem currentOs;
        

        public Choose_Software(Classroom.OpSystem a)
        {
            InitializeComponent();
            MessageBox.Show(a + "");
            MessageBox.Show(currentOs + "");
            if (MainWindow.current_cr.Software.Count() == 0 || a!=currentOs) 
            {
                //os changed -> remove if something is already added
                if (a != currentOs)
                {
                    MainWindow.current_cr.Software = new List<Software>();
                }
                currentOs = a;
                MessageBox.Show("BREAK");
                MessageBox.Show(a + "");
                MessageBox.Show(currentOs + "");
                all = new ObservableCollection<Software>();
                added = new ObservableCollection<Software>();
                foreach (Software sw in MainWindow.allSoftware)
                {
                    if (sw.Os.Equals(a) || sw.Os.Equals(Classroom.OpSystem.WindowsAndLinux))
                    {
                        all.Add(sw);
                    }
                   
                }
            }else
            {
                foreach (Software sw in MainWindow.allSoftware)
                {
                    if(sw.Os.Equals(a) || sw.Os.Equals(Classroom.OpSystem.WindowsAndLinux))
                    {
                        if (MainWindow.current_cr.Software.Contains(sw))
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

        public Choose_Software()
        {
            InitializeComponent();

            foreach(Software sw in MainWindow.allSoftware)
            {
                if(sw.Os.Equals(MainWindow.current_cr.Os) || sw.Os.Equals(Classroom.OpSystem.WindowsAndLinux))
                {
                    if (MainWindow.current_cr.Software.Contains(sw))
                    {
                        added.Add(sw);
                    }
                    else
                    {
                        all.Add(sw);
                    }
                }
            }

            addedSoftwareDG.ItemsSource = added;
            allSoftwareDG.ItemsSource = all;
        }

        public static DataGridRow detect_selected_row(DependencyObject dep)
        {

            // iteratively traverse the visual tree
            while ((dep != null) &&
            !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep is DataGridCell)
            {

                DataGridCell cell = dep as DataGridCell;
                while ((dep != null) && !(dep is DataGridRow))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }
            }
            DataGridRow row = dep as DataGridRow;
            return row;

        }

        private void software_added_click(object sender, MouseButtonEventArgs e)
        {
            currentDG = (DataGrid)sender;
            DataGridRow row = detect_selected_row((DependencyObject)e.OriginalSource);
            currentDG = ItemsControl.ItemsControlFromItemContainer(row) as DataGrid;
            int index = currentDG.ItemContainerGenerator.IndexFromContainer(row);
            if (index == -1)
            {
                return;
            }
            if (currentDG != null)
            {
                Software sw = all.ElementAt(index);
                all.Remove(sw);
                added.Add(sw);
            }
        }

        private void software_removed_click(object sender, MouseButtonEventArgs e)
        {
            currentDG = (DataGrid)sender;
            DataGridRow row = detect_selected_row((DependencyObject)e.OriginalSource);
            currentDG = ItemsControl.ItemsControlFromItemContainer(row) as DataGrid;
            int index = currentDG.ItemContainerGenerator.IndexFromContainer(row);
            if (index == -1)
            {
                return;
            }
            if (currentDG != null)
            {
                Software sw = added.ElementAt(index);
                added.Remove(sw);
                all.Add(sw);
            }
        }

        private void show_software(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            currentDG = (DataGrid)btn.Parent;
            DataGridRow row = detect_selected_row((DependencyObject)e.OriginalSource);
            currentDG = ItemsControl.ItemsControlFromItemContainer(row) as DataGrid;
            int index = currentDG.ItemContainerGenerator.IndexFromContainer(row);
            if (index == -1)
            {
                return;
            }
            if (currentDG.Name.Equals("allSoftwareDG"))
            {
                Software_Info si = new Software_Info(all.ElementAt(index));
                
                si.Closed += new EventHandler((sender2, e2) => refresh_data(sender2, e2));
                si.ShowDialog();
            }
            else
            {
                Software_Info si = new Software_Info(added.ElementAt(index));
                si.Closed += new EventHandler((sender2, e2) => refresh_data(sender2, e2));
                si.ShowDialog();
            }
        }

        private void add_close(object sender, RoutedEventArgs e)
        {
            MainWindow.current_cr.Software = added.ToList();
            this.Close();
        }

        private void close_win(object sender, RoutedEventArgs e)
        {
            all = new ObservableCollection<Software>();
            added = new ObservableCollection<Software>();
            this.Close();
        }

        private void refresh_data(object sender, EventArgs e)
        {
            bool found = false;
            if (Software_Info.deleted != null)
            {

                foreach(Software sw in all)
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
                    foreach(Software sw in added)
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
    }
}
