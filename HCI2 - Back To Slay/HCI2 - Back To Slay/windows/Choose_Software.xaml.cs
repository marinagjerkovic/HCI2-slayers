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

        private ObservableCollection<Software> all = new ObservableCollection<Software>();
        private ObservableCollection<Software> added = new ObservableCollection<Software>();
        private DataGrid currentDG;

        public Choose_Software(Classroom.OpSystem a)
        {
            InitializeComponent();
            MessageBox.Show(a + "");
            foreach (Software sw in MainWindow.allSoftware)
            {
                if (sw.Os.Equals(a) || sw.Os.Equals(Classroom.OpSystem.WindowsAndLinux))
                {
                    all.Add(sw);
                }
            }
            addedSoftwareDG.ItemsSource = added;
            allSoftwareDG.ItemsSource = all;
        }

        private int detect_selected_row(DependencyObject dep)
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
            if (row == null)
            {
                return -1;
            }
            currentDG = ItemsControl.ItemsControlFromItemContainer(row) as DataGrid;
            return currentDG.ItemContainerGenerator.IndexFromContainer(row);
        }

        private void software_added_click(object sender, MouseButtonEventArgs e)
        {
            currentDG = (DataGrid)sender;
            int index = detect_selected_row((DependencyObject)e.OriginalSource);
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
            int index = detect_selected_row((DependencyObject)e.OriginalSource);
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
            int index = detect_selected_row((DependencyObject)e.OriginalSource);
            if (index == -1)
            {
                return;
            }
            if (currentDG.Name.Equals("allSoftwareDG"))
            {
                Software_Info si = new Software_Info(all.ElementAt(index));
                si.Show();
                si.Closed += new EventHandler((sender2, e2)=>refresh(sender2, e2));
            }
            else
            {
                Software_Info si = new Software_Info(added.ElementAt(index));
                si.Show();
            }
        }

        private void refresh(object sender, EventArgs e)
        {

        }

    }
}
