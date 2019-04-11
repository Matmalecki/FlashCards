using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace FlashCards.Custom
{
    class StreamToStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            string data = value as string;
            if (data == null)
                return null;

            byte[] stream = System.Convert.FromBase64String(data);
            return ImageSource.FromStream(() => new MemoryStream(stream));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
