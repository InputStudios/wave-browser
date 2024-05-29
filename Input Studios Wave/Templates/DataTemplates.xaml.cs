using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Input_Studios_Wave.Templates
{
    public partial class DataTemplates : ResourceDictionary
    {
        public void CloseTabButton_Click(object sender, RoutedEventArgs e)
        {
            Button closeButton = sender as Button;
            TabItem tabItem = FindVisualParent<TabItem>(closeButton);
            TabControl tabControl = FindVisualParent<TabControl>(tabItem);
            tabControl.Items.Remove(tabItem);

            if (tabControl.Items.Count == 0)
            {
                Application.Current.Shutdown();
            }
        }

        private T FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as T;
        }
    }
}
