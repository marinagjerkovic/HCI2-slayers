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

namespace HCI2___Back_To_Slay.windows
{
    /// <summary>
    /// Interaction logic for Classroom_Multiple.xaml
    /// </summary>
    public partial class Classroom_Multiple : Window
    {

        public Classroom_Multiple()
        {
            InitializeComponent();
            dataGrid.ItemsSource = MainWindow.allClassrooms;
        }

        private void show_classroom(object sender, RoutedEventArgs e)
        {
            DataGridRow row = Helper.detect_selected_row((DependencyObject)e.OriginalSource);
            dataGrid = ItemsControl.ItemsControlFromItemContainer(row) as DataGrid;
            int index = dataGrid.ItemContainerGenerator.IndexFromContainer(row);
            if (index == -1)
            {
                return;
            }

            Classroom_Info ci = new Classroom_Info(MainWindow.allClassrooms.ElementAt(index));

            //si.Closed += new EventHandler((sender2, e2) => refresh_data(sender2, e2));
            ci.ShowDialog();
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

            Software_Multiple sm = new Software_Multiple(MainWindow.allClassrooms.ElementAt(index));
            sm.ShowDialog();
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
