using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LanguageSchool
{
    public partial class ClientService
    {
        public SolidColorBrush EditColor // Изменение цвета для оставшегося времени до записи клиента
        {
            get
            {
                TimeSpan dateTime = StartTime - DateTime.Now;
                TimeSpan hour = TimeSpan.FromHours(1);
                if (dateTime < hour)
                {
                    return Brushes.Red;
                }
                return Brushes.Black;
            }
        }
        public string RemainingTime // Оставшееся время до записи
        {
            get
            {
                TimeSpan dateTime = StartTime - DateTime.Now;
                int hour = 0;
                int minute = 0;
                if (dateTime.Days > 0)
                {
                    hour += 24 * Convert.ToInt32(dateTime.Days);
                }
                hour += Convert.ToInt32(dateTime.ToString("hh"));
                minute = Convert.ToInt32(dateTime.ToString("mm"));
                string textHour = "час";
                if (hour == 0 || hour >= 5)
                {
                    textHour = "часов";
                }
                else
                {
                    if (hour >= 2)
                    {
                        textHour = "часа";
                    }
                }
                string textMinute = "минута";
                if (minute == 0 || minute >= 5)
                {
                    textMinute = "минут";
                }
                else
                {
                    if (minute >= 2)
                    {
                        textMinute = "минуты";
                    }
                }
                return "" + hour + " " + textHour + " " + minute + " " + textMinute;
            }
        }

    }
}