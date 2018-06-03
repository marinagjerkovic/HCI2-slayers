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

        public Choose_Software(Classroom.OpSystem a)
        {
            InitializeComponent();
            MessageBox.Show(a+"");
            foreach(Software sw in MainWindow.allSoftware)
            {
                if (sw.Os.Equals(a) || sw.Os.Equals(Classroom.OpSystem.WindowsAndLinux))
                {
                    all.Add(sw);
                }
            }
            addedSoftwareDG.ItemsSource = added;
            allSoftwareDG.ItemsSource = all;
        }

        private void mouse_moved(object sender, MouseEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

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

                DataGridRow row = dep as DataGridRow;
                DataGrid dataGrid =
    ItemsControl.ItemsControlFromItemContainer(row)
    as DataGrid;

                int index = dataGrid.ItemContainerGenerator.
                    IndexFromContainer(row);

                DataGrid dg = (DataGrid)sender;

                if (dg != null && e.LeftButton == MouseButtonState.Pressed)
                {
                    DragDrop.DoDragDrop(dg, index+"", DragDropEffects.Copy);
                }
            }


        }

        private void software_removed(object sender, DragEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            if (dg != null)
            {
                if (e.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    string toe = (string)e.Data.GetData(DataFormats.StringFormat);
                    int index = Int32.Parse(toe);
                    Software sw = added.ElementAt(index);
                    added.Remove(sw);
                    all.Add(sw);
                }
            }

        }

        private void software_added(object sender, DragEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            if (dg != null)
            {
                if (e.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    string toe = (string)e.Data.GetData(DataFormats.StringFormat);
                    int index = Int32.Parse(toe);
                    Software sw = all.ElementAt(index);
                    all.Remove(sw);
                    added.Add(sw);
                }
            }
        }





    }
}
