using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace FlashCards.Custom
{
    public class BoolToCardTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CardType)
            {
                return (CardType)value == CardType.Basic ? false : true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return (bool)value ? CardType.Photo : CardType.Basic;
            }

            return CardType.Basic;
        }
    }
}
