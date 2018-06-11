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

        public static ObservableCollection<Software> all = new ObservableCollection<Software>();
        public static ObservableCollection<Software> added = new ObservableCollection<Software>();
        private DataGrid currentDG=null;
        private Classroom.OpSystem currentOs;
        

        public Choose_Software(Classroom.OpSystem a)
        {
            InitializeComponent();
            if (all.Count() == 0 || a!=currentOs) 
            {
                //os changed -> remove if something is already added
                if (a != currentOs)
                {
                    Add_Classroom.cr.Software = new List<Software>();
                }
                currentOs = a;
                all = new ObservableCollection<Software>();
                added = new ObservableCollection<Software>();
                foreach (Software sw in MainWindow.allSoftware)
                {
                    if (sw.Os.Equals(a) || sw.Os.Equals(Classroom.OpSystem.WindowsAndLinux))
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
                si.ShowDialog();
            }
        }

        private void add_close(object sender, RoutedEventArgs e)
        {
            if (added.Count() == 0)
            {
                MessageBox.Show("Nothing chosen!");
                return;
            }
            Add_Classroom.cr.Software = added.ToList();
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
            MessageBox.Show(Software_Info.updated);
            if (Software_Info.updated != null)
            {
                MessageBox.Show(Software_Info.updated);
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
                    MessageBox.Show("u ifu");
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
    }
}
