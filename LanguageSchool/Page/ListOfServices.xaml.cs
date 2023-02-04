using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для ListOfServices.xaml
    /// </summary>
    public partial class ListOfServices: Page
    {
        public static string admin;
        public ListOfServices()
        {
            InitializeComponent();
            ListServices.ItemsSource = Base.DB.Service.ToList();
            Sorting.SelectedIndex = 0;
            Filtering.SelectedIndex = 0;
            count.Text = Base.DB.Service.ToList().Count + "/" + Base.DB.Service.ToList().Count;
            if (admin == "0000")
            {
                Service.btn_admin = Visibility.Visible;
                AddService.Visibility = Visibility.Visible;
            }
            else
            {
                Service.btn_admin = Visibility.Collapsed;
                AddService.Visibility = Visibility.Collapsed;
            }
        }
        void Filter()
        {
            List<Service> services = new List<Service>();
            services = Base.DB.Service.ToList();
            //поиск название

            if (!string.IsNullOrWhiteSpace(SearchName.Text))  // если строка не пустая и если она не состоит из пробелов
            {
                services = services.Where(x => x.Title.ToLower().Contains(SearchName.Text.ToLower())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(SearchDescription.Text))  // если строка не пустая и если она не состоит из пробелов
            {

                List<Service> trl = services.Where(x => x.Description != null).ToList();
                if (trl.Count > 0)
                {

                    services = trl.Where(x => x.Description.ToLower().Contains(SearchDescription.Text.ToLower())).ToList();

                }
                else
                {
                    MessageBox.Show("нет описания");
                    SearchDescription.Text = "";
                }

            }
            //Фильтрация

            switch (Filtering.SelectedIndex)
            {
                 
                case 0:                   
                    services = services.ToList();                    
                    break;
                case 1:
                    services = services.Where(x => x.Discount >= 0 && x.Discount < 0.05).ToList();
                    break;
                case 2:
                    services = services.Where(x => x.Discount >= 0.05 && x.Discount < 0.15).ToList();
                    break;
                case 3:
                    services = services.Where(x => x.Discount >= 0.15 && x.Discount < 0.3).ToList();
                    break;
                case 4:
                    services = services.Where(x => x.Discount >= 0.3 && x.Discount < 0.7).ToList();
                    break;
                case 5:
                    services = services.Where(x => x.Discount >= 0.7 && x.Discount < 1).ToList();
                    break;
            }

            //сортировка

            switch (Sorting.SelectedIndex)
            {
                case 0:
                    {
                        services.Sort((x, y) => x.Cost.CompareTo(y.Cost));
                    }
                    break;
                case 1:
                    {
                        services.Sort((x, y) => x.Cost.CompareTo(y.Cost));
                        services.Reverse();
                    }
                    break;
            }

            ListServices.ItemsSource = services;
            if (services.Count == 0)
            {
                MessageBox.Show("нет записей");
                count.Text = Base.DB.Service.ToList().Count + "/" + Base.DB.Service.ToList().Count;
                SearchName.Text = "";
                SearchDescription.Text = "";
                Sorting.SelectedIndex = 0;
                Filtering.SelectedIndex = 0;

            }
            count.Text = services.Count + "/" + Base.DB.Service.ToList().Count;

        }
        private void SearchName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void SearchDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void Filtering_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void Sorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int id = Convert.ToInt32(btn.Uid);
            Service serv = Base.DB.Service.FirstOrDefault(x => x.ID == id);
            List<ClientService> clientservices = Base.DB.ClientService.Where(x => x.ServiceID == serv.ID).ToList();
            if (clientservices.Count > 0)
            {
                MessageBox.Show("Данную услугу нельзя удалить");
            }
            else
            {
                Base.DB.Service.Remove(serv);
                Base.DB.SaveChanges();
                ClassFrame.frame.Navigate(new ListOfServices());
            }

        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frame.Navigate(new AddService());
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int id = Convert.ToInt32(btn.Uid);
            Service service = Base.DB.Service.FirstOrDefault(x => x.ID == id);
            ClassFrame.frame.Navigate(new AddService(service));
        }

        private void NewRecord_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int id = Convert.ToInt32(btn.Uid);
            Service service = Base.DB.Service.FirstOrDefault(x => x.ID == id);
            ClassFrame.frame.Navigate(new AddRecord(service));
        }
    }
}
