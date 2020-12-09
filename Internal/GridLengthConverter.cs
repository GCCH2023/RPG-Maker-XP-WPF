using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace XP.Internal
{
    class GridLengthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BrushMappingMode mode = (BrushMappingMode)values[0];
            double value = (double)values[1];
            
            if(mode == BrushMappingMode.Absolute)
            {
                return new GridLength(value, GridUnitType.Pixel);
            }
           
            // 相对模式需要计算出对应的图片像素宽高
            var image = (ImageSource)values[2];
            if (image == null)
                return null;

            var viewbox = (System.Windows.Rect)values[3];

            switch((string)parameter)
            {
                case "left":
                case "right":
                    return new GridLength(image.Width * (viewbox.Right - viewbox.Left) * value, GridUnitType.Pixel);
                case "top":
                case "bottom":
                    return new GridLength(image.Width * (viewbox.Bottom - viewbox.Top) * value, GridUnitType.Pixel);
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
