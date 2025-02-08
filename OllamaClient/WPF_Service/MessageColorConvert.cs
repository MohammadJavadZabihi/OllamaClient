using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace OllamaClient.Windows.WPF_Service
{
    public class MessageColorConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool isUserChat)
            {
                return isUserChat ? new SolidColorBrush(Color.FromRgb(0, 122, 255))
                                  : new SolidColorBrush(Color.FromRgb(55 ,55, 55));
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
