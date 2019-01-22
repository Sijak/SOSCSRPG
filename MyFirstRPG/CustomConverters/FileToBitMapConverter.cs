using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MyFirstRPG.CustomConverters
{
    public class FileToBitMapConverter :IValueConverter
    {
        private static readonly Dictionary<string, BitmapImage> _locations
            = new Dictionary<string, BitmapImage>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is string filename))
            { return null; }

            if(!_locations.ContainsKey(filename))
            {
                _locations.Add(filename, new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}{filename}")));
            }
            return _locations[filename];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

    }
}
