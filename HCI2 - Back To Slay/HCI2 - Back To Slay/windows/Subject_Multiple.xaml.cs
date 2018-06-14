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
    /// Interaction logic for Subject_Multiple.xaml
    /// </summary>
    public partial class Subject_Multiple : Window
    {
        public Subject_Multiple()
        {
            InitializeComponent();
            dataGrid.ItemsSource = MainWindow.allSubjects;
        }

        private void show_subject(object sender, RoutedEventArgs e)
        {

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

            Software_Multiple sm = new Software_Multiple(MainWindow.allSubjects.ElementAt(index));
            sm.ShowDialog();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int indeks = 0;
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i].Title.Equals("Subject_Multiple"))
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
