using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using static System.Net.Mime.MediaTypeNames;

namespace LanguageSchool
{
    /// <summary>
    /// Логика взаимодействия для AddService.xaml
    /// </summary>
    public partial class AddService : Page
    {
        private Service service;
        private bool IsCreate = true;
        private string Path;
        private int id;

        public AddService()
        {
            InitializeComponent();

            Title.Text = "Добавление записи";
            AddPhotos.Visibility = Visibility.Collapsed;
            service = new Service();
           
        }
        public AddService(Service service)
        {
            InitializeComponent();

            Title.Text = "Изменение записи";
            IsCreate = false;
            this.service = service;

            IdService.Visibility = Visibility.Visible;
            IdService.Text = service.ID.ToString();
            NameServices.Text = service.Title;
            Description.Text = service.Description;
            PriceServices.Text = service.Cost.ToString();
            TimeServices.Text = service.TimeLesson.ToString();

            if (service.Discount == null)
            {
                Sale.Text = "0";
            }
            else
            {
                Sale.Text = (service.Discount * 100).ToString();
            }

            Path = service.MainImagePath;
            ImageService.Source = new BitmapImage(new Uri(Path, UriKind.Relative));

            
            List<ServicePhoto> photos = Base.DB.ServicePhoto.Where(x => x.ServiceID == service.ID).ToList();
            ListPhoto.ItemsSource = photos;

            if (photos.Count == 0)
            {
                MessageBox.Show("Дополнительных изображений не найдено", "Услуги", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            ChangePhoto.Visibility = Visibility.Visible;
           
        }
        public bool NameService()
        {
            if (NameServices.Text.Length == 0)
            {
                MessageBox.Show("Введите название услуги", "Услуги", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            else if (Base.DB.Service.FirstOrDefault(x => x.Title.ToLower() == NameServices.Text.ToLower()) != null && IsCreate)
            {
                MessageBox.Show("Данная услуга уже существует", "Услуги", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            else return true;
        }

        public bool SaleService()
        {
            try
            {        
                if (Sale.Text.Length != 0)
                {
                    if (Convert.ToInt32(Sale.Text) > 0)
                    {

                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Укажите скидку цифрами", "Ошибка", MessageBoxButton.OK);
                        return false;
                    }                    
                }
                else return true;

            }
            catch
            {
                MessageBox.Show("Укажите скидку корректно", "Ошибка", MessageBoxButton.OK);
                return false;
            }
        }

        public bool PriceService()
        {
            try
            {
                if (PriceServices.Text.Length != 0)
                {
                    if (Convert.ToInt32(PriceServices.Text) > 0)
                    {

                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Укажите цену цифрами", "Ошибка", MessageBoxButton.OK);
                        return false;
                    }
                }
                else return false;

            }
            catch
            {
                MessageBox.Show("Укажите цену в цифрах", "Ошибка", MessageBoxButton.OK);
                return false;
            }
        }
        public bool TimeService()
        {
            try
            {
                if (TimeServices.Text.Length != 0)
                {
                    if (Convert.ToInt32(TimeServices.Text) > 0)
                    {
                        if (Convert.ToInt32(TimeServices.Text) < 240)
                        {

                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Время выполнения услуги не может превышать 4 - х часов", "Ошибка", MessageBoxButton.OK);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Укажите время цифрами", "Ошибка", MessageBoxButton.OK);
                        return false;
                    }
                }
                else return false;             

            }
            catch
            {
                MessageBox.Show("Некорректно введено значение. Укажите количество часов цифрой", "Ошибка", MessageBoxButton.OK);
                return false;
            }
        }
        private void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 9) + "images\\";
                ofd.ShowDialog();
                string[] array = ofd.FileName.Split('\\');

                if (array.Length != 1)
                {
                    Path = "\\" + array[array.Length - 2] + "\\" + array[array.Length - 1];
                    ImageService.Source = new BitmapImage(new Uri(Path, UriKind.Relative));
                }
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так", "Ошибка", MessageBoxButton.OK);
            }
        }
        int n = 0;
        private void ChangePhoto_Click(object sender, RoutedEventArgs e)
        {
                       
            List<ServicePhoto> servicePhoto = Base.DB.ServicePhoto.Where(x => x.ServiceID == service.ID).ToList();
            if (servicePhoto.Count >= 1)
            {

                BitmapImage img = new BitmapImage(new Uri(servicePhoto[n].PhotoPath, UriKind.RelativeOrAbsolute));
                ImageService.Source = img;

                AddPhoto.Visibility = Visibility.Collapsed;
                ChangePhoto.Visibility = Visibility.Collapsed;              
                AddPhotos.Visibility = Visibility.Collapsed;
                SavePhoto.Visibility = Visibility.Visible;
                DeletPhoto.Visibility = Visibility.Visible;

            }
            else
            {
                MessageBox.Show("Нет дополнительных фотографий", "Ошибка", MessageBoxButton.OK);
            }        

        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> servicePhoto = Base.DB.ServicePhoto.Where(x => x.ServiceID == service.ID).ToList();

            n++;
            if (Back.IsEnabled == false)
            {
                Back.IsEnabled = true;
            }
            if (servicePhoto != null)  // если объект не пустой, начинает переводить байтовый массив в изображение
            {

                BitmapImage img = new BitmapImage(new Uri(servicePhoto[n].PhotoPath, UriKind.RelativeOrAbsolute));
                ImageService.Source = img;
            }
            if (n == servicePhoto.Count - 1)
            {
                Next.IsEnabled = false;
            }
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> u = Base.DB.ServicePhoto.Where(x => x.ServiceID == service.ID).ToList();

            n--;
            if (Next.IsEnabled == false)
            {
                Next.IsEnabled = true;
            }
            if (u != null)  // если объект не пустой, начинает переводить байтовый массив в изображение
            {

                BitmapImage img = new BitmapImage(new Uri(u[n].PhotoPath, UriKind.RelativeOrAbsolute));
                ImageService.Source = img;
            }
           
        }
        private void SavePhoto_Click(object sender, RoutedEventArgs e)
        {

            List<ServicePhoto> u = Base.DB.ServicePhoto.Where(x => x.ServiceID == service.ID).ToList();
            service.MainImagePath = u[n].PhotoPath;
            Base.DB.SaveChanges();
            MessageBox.Show("Фотография изменена");          
            SavePhoto.Visibility = Visibility.Collapsed;
            AddPhoto.Visibility = Visibility.Visible;            
            ChangePhoto.Visibility = Visibility.Visible;
            AddPhotos.Visibility = Visibility.Visible;
            DeletPhoto.Visibility = Visibility.Collapsed;
            ClassFrame.frame.Navigate(new ListOfServices());
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NameService())
                {
                    if (TimeService())
                    {
                        if (PriceService())
                        {
                            if (SaleService())
                            {
                                double? discount;
                                if (Sale.Text == "" || Convert.ToInt32(Sale.Text) == 0)
                                {
                                    discount = null;
                                }
                                else
                                {
                                    discount = Convert.ToInt32(Sale.Text) / 100.0;
                                }

                                service.Title = NameServices.Text;
                                service.Cost = Convert.ToDecimal(PriceServices.Text);
                                service.DurationInSeconds = Convert.ToInt32(TimeServices.Text) * 60;
                                service.Description = Description.Text;
                                service.Discount = discount;
                                service.MainImagePath = Path;

                                if (IsCreate == true)
                                {

                                    Base.DB.Service.Add(service);
                                    Base.DB.SaveChanges();
                                    MessageBox.Show("Услуга успешно добавлена", "Услуги", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ClassFrame.frame.Navigate(new ListOfServices());
                                }
                                else
                                {
                                    Base.DB.SaveChanges();
                                    MessageBox.Show("Услуга успешно изменена", "Услуги", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ClassFrame.frame.Navigate(new ListOfServices());
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так. Не удалось добавить запись", "Ошибка", MessageBoxButton.OK);
            }
            
        }

        private void AddPhotos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;

                if ((bool)ofd.ShowDialog())
                {
                    foreach (string path in ofd.FileNames)
                    {
                        string[] array = path.Split('\\');

                        try
                        {
                            File.Copy(path, Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 9) + "images\\" + array[array.Length - 1]);
                        }
                        catch
                        {

                        }

                        ServicePhoto photo = new ServicePhoto()
                        {
                            ServiceID = service.ID,
                            PhotoPath = "\\images\\" + array[array.Length - 1]
                        };

                        Base.DB.ServicePhoto.Add(photo);
                    }

                   Base.DB.SaveChanges();
                   MessageBox.Show("Фото успешно добавлены", "Услуги", MessageBoxButton.OK, MessageBoxImage.Information);
                
                }
                           
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так. Часть файлов не удалось загрузить", "Услуги", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image image = (System.Windows.Controls.Image)sender;
            int id = Convert.ToInt32(image.Uid);

            string fileName = Base.DB.ServicePhoto.FirstOrDefault(x => x.ID == id).PhotoPath;
            string path = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 10) + fileName;
            image.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
        } 

        private void DeletPhoto_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> photos = Base.DB.ServicePhoto.Where(x => x.ServiceID == service.ID).ToList();
            if (photos[n].PhotoPath != service.MainImagePath)
            {
                ServicePhoto photo = photos.FirstOrDefault(x => x.PhotoPath == photos[n].PhotoPath);
                Base.DB.ServicePhoto.Remove(photo);
                Base.DB.SaveChanges();
                ClassFrame.frame.Navigate(new AddService(service));
            }
            else
            {
                MessageBox.Show("Выбранное фото нельзя удалить", "Ошибка", MessageBoxButton.OK);
            }
        }
    }
}
