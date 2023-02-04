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
using System.Windows.Threading;

namespace LanguageSchool
{
    /// <summary>
    /// Логика взаимодействия для NearRecord.xaml
    /// </summary>
    public partial class NearRecord : Page
    {
        public NearRecord()
        {
            InitializeComponent(); 
            DateTime date = DateTime.Today;
            DateTime data = date.AddDays(2);
            List<ClientService> ser = Base.DB.ClientService.Where(x => x.StartTime >= DateTime.Today && x.StartTime < data).ToList();
            ListRecord.ItemsSource = ser.OrderBy(x => x.StartTime).ToList();
            Load();
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(30);
            dispatcherTimer.Tick += Time;
            dispatcherTimer.Start();
        }
        private void Load()
        {
            List<ClientService> clientServices = Base.DB.ClientService.ToList();
            clientServices = clientServices.Where(x => x.StartTime >= DateTime.Now).ToList(); // Фильтрация по дате начала
            DateTime endDateTime = DateTime.Today.AddDays(2).AddTicks(-1); // Конец завтрашнего дня
            clientServices = clientServices.Where(x => x.StartTime < endDateTime).ToList(); // Фильтрация по дате окончания
            clientServices.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));
            ListRecord.ItemsSource = clientServices;
        }
        private void Time(object sender, EventArgs e)
        {
            Load();
        }
    }
}
