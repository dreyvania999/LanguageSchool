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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LanguageSchool
{
    /// <summary>
    /// Логика взаимодействия для Header.xaml
    /// </summary>
    public partial class Header : Page
    {
        public static string Code;
        public Header()
        {
            InitializeComponent();
            if (Code == "0000")
            {
                Record.Visibility = Visibility.Visible;
            }
            else
            {
                Record.Visibility = Visibility.Collapsed;
            }
        }

        private void Services_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frame.Navigate(new ListOfServices());
        }

        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            Admin windowPerson = new Admin();
            windowPerson.ShowDialog();
            ClassFrame.headerFrame.Navigate(new Header());
            ClassFrame.frame.Navigate(new ListOfServices());
        }

        private void Record_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frame.Navigate(new NearRecord());
        }
    }
}
