using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace HCI2___Back_To_Slay.windows
{
    public static class Helper
    {

        public static SolidColorBrush[] colors = {new SolidColorBrush(Colors.Red), new SolidColorBrush(Colors.Transparent)};

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

        public static Visibility isVisible(Control ctrl)
        {
            if (ctrl.Visibility == Visibility.Visible)
            {
                return Visibility.Hidden;
            }
            else
            {
                return Visibility.Visible;
            }
        }
        
    }
}
