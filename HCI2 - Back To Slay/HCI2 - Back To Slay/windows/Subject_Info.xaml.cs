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
    /// Interaction logic for Subject_Info.xaml
    /// </summary>
    public partial class Subject_Info : Window
    {
        public Subject_Info()
        {
            InitializeComponent();
        }

        private void change(object sender, RoutedEventArgs e)
        {

        }

        private void update(object sender, RoutedEventArgs e)
        {

        }

        private void delete(object sender, RoutedEventArgs e)
        {

        }

        private void back(object sender, RoutedEventArgs e)
        {

        }

        private void show_software(object sender, RoutedEventArgs e)
        {

        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int indeks = 0;
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i].Title.Equals("Subject_Info"))
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
