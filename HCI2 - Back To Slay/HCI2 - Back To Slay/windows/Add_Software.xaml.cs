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
    /// Interaction logic for Add_Software.xaml
    /// </summary>
    public partial class Add_Software : Window
    {
        public Add_Software()
        {
            InitializeComponent();
        }

        private void add_new_software(object sender, RoutedEventArgs e)
        {
            if (MainWindow.allSoftwareIds.Contains(id.Text))
            {
                MessageBox.Show("Change id - software with the same id already exists!");
                return;
            }

            Software sw = new Software();
            sw.Id = id.Text;
            sw.Name = name.Text;
            sw.Description = description.Text;
            sw.Maker = maker.Text;
            sw.Site = site.Text;

            int num = 0;
            if (Int32.TryParse(year.Text, out num) && num>1900 && num<2018)
            {
                sw.Year = num;
            }
            else
            {
                MessageBox.Show("Wrong year! Must be in 1900-2018 interval!");
                return;
            }

            double pr = 0;
            if(Double.TryParse(price.Text, out pr))
            {
                sw.Price = pr;
            }
            else
            {
                MessageBox.Show("Price must be a number!");
                return;
            }

            if (win.IsChecked == true)
            {
                sw.Os = Classroom.OpSystem.Windows;
            }
            else if (lin.IsChecked == true)
            {
                sw.Os = Classroom.OpSystem.Linux;
            }
            else
            {
                sw.Os = Classroom.OpSystem.WindowsAndLinux;
            }

            MainWindow.allSoftware.Add(sw);
            MainWindow.allSoftwareIds.Add(id.Text);
            MessageBox.Show("Successfully added a new software!");

            id.Text = "";
            name.Text = "";
            description.Text = "";
            maker.Text = "";
            site.Text = "";
            year.Text = "";
            price.Text = "";
            //this.Close();
        }

        private void cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int indeks = 0;
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i].Title.Equals("Add Software"))
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
