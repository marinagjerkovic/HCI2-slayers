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
    public partial class Software_Multiple : Window
    {
        private ObservableCollection<Software> Show_Data;
        public Software_Multiple(List<Software> show)
        {
            if (show == null)
            {
                Show_Data = MainWindow.allSoftware;
            }else
            {
                foreach(Software sw in show)
                {
                    Show_Data.Add(sw);
                }
            }
            InitializeComponent();
            dataGrid.ItemsSource = Show_Data;
        }

        private void show_software(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            DataGridRow row = Choose_Software.detect_selected_row((DependencyObject)e.OriginalSource);
            dataGrid = ItemsControl.ItemsControlFromItemContainer(row) as DataGrid;
            int index = dataGrid.ItemContainerGenerator.IndexFromContainer(row);
            if (index == -1)
            {
                return;
            }

            Software_Info si = new Software_Info(Show_Data.ElementAt(index));

            si.Closed += new EventHandler((sender2, e2) => refresh_data(sender2, e2));
            si.ShowDialog();
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
    }
}
