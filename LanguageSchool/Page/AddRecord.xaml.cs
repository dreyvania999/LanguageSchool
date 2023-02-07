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
    /// Логика взаимодействия для AddRecord.xaml
    /// </summary>
    public partial class AddRecord : Page
    {
        Service service;
        ClientService client;
        public AddRecord(Service service)
        {
            InitializeComponent();
            this.service = service;
            Title.Text = "Название услуги: " + service.Title + " | " + "Длительность услуги: " + service.TimeLesson + " минут";
            List<Client> clients = Base.DB.Client.ToList();
            for (int i = 0; i < clients.Count; i++)
            {
                FullName.Items.Add(clients[i].FullName);
            }

            hh.Text = DateTime.Now.ToString("HH");
            mm.Text = DateTime.Now.ToString("MM");
            int HH = Convert.ToInt32(DateTime.Now.ToString("HH"));
            int MM = Convert.ToInt32(DateTime.Now.ToString("MM"));
            DateTime date = new DateTime(2000, 2, 2, HH, MM, 0);
            DateTime data = date.AddMinutes(Convert.ToInt32(service.TimeLesson));
            TimeEnd.Text = data.ToShortTimeString();
        }
        private void hh_TextChanged(object sender, TextChangedEventArgs e)
        {
            TimeCheck();
        }

        private void mm_TextChanged(object sender, TextChangedEventArgs e)
        {
            TimeCheck();
        }
        void TimeCheck()
        {
            try
            {
                if (mm.Text=="")
                {
                    return;
                }
                int h = Convert.ToInt32(hh.Text);
                int m = Convert.ToInt32(mm.Text);
                if ((h < 24) && (m < 60))
                {

                    int HH = Convert.ToInt32(h);
                    int MM = Convert.ToInt32(m);
                    DateTime date = new DateTime(2000, 2, 2, HH, MM, 0);
                    DateTime data = date.AddMinutes(Convert.ToInt32(service.TimeLesson));
                    TimeEnd.Text = data.ToShortTimeString();
                }
                else
                {
                    MessageBox.Show("Введите корректное значение", "Ошибка", MessageBoxButton.OK);

                }
            }
            catch
            {

                MessageBox.Show("Что-то пошло не так", "Ошибка", MessageBoxButton.OK);
            }
        }
      


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (FullName.Text == "" || hh.Text == "" || mm.Text == "" || StartDate.Text == "")
            {
                MessageBox.Show("Обязательные поля не заполнены", "Ошибка", MessageBoxButton.OK);
            }
            else
            {
                client = new ClientService();
                client.ServiceID = service.ID;
                client.ClientID = FullName.SelectedIndex + 1;
                string date = StartDate.Text;
                string[] Dat = date.Split('.');
                int h = Convert.ToInt32(hh.Text);
                int m = Convert.ToInt32(mm.Text);
                DateTime dateStar = new DateTime(Convert.ToInt32(Dat[2]), Convert.ToInt32(Dat[1]), Convert.ToInt32(Dat[0]), h, m, 0);
                client.StartTime = dateStar;
                Base.DB.ClientService.Add(client);

                Base.DB.SaveChanges();
                MessageBox.Show("Клиент записан");

                ClassFrame.frame.Navigate(new ListOfServices());
            }
        }
    }
}

