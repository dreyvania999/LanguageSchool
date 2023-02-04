using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LanguageSchool
{
    public partial class Service
    {
        public static System.Windows.Visibility Btn_Admin = System.Windows.Visibility.Collapsed;
        public static System.Windows.Visibility btn_admin
        {
            get
            {
                return Btn_Admin;
            }
            set
            {
                Btn_Admin = value;
            }
        }
        public string Sale
        {
            get
            {
                if (Discount == null)
                {
                    return "";
                }
                return "* скидка " + Discount * 100 + "%";
            }
        }

        public SolidColorBrush TextBrush
        {
            get
            {
                var brushConverter = new BrushConverter();

                if (Discount == null)
                {
                    return Brushes.White;
                }
                else
                {
                    return (SolidColorBrush)(Brush)brushConverter.ConvertFrom("#e7fabf");

                }

            }
        }
        public int TimeLesson
        {
            get
            {
                return DurationInSeconds / 60;
            }
        }
        public string CostLesson

        {
            get
            {
                if (Discount != null)
                {
                    double a = Convert.ToDouble(Cost);
                    return a + " ";
                }
                return "";

            }
        }
        public string PriceLesson
        {
            get
            {
                if (Discount != null)
                {
                    double b = (Convert.ToDouble(Cost) / 100) * (100 - (Discount.Value * 100));
                    int time = DurationInSeconds / 60;
                    return b + " рублей на " + time + " минут";
                }
                else
                {
                    double a = Convert.ToDouble(Cost);
                    int time = DurationInSeconds / 60;
                    return a + " рублей на " + time + " минут";
                }
            }
        }
     
    }
}