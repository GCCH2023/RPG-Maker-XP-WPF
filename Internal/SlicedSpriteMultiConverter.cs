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
    class SlicedSpriteMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BrushMappingMode mode = (BrushMappingMode)values[0];
            Thickness slicedRect = (Thickness)values[1];
            System.Windows.Rect viewbox = (System.Windows.Rect)values[2];

            try
            {
                switch ((string)parameter)
                {
                    case "part0":
                        if (mode == BrushMappingMode.Absolute)
                        {
                            var rect = new System.Windows.Rect(viewbox.Left,
                                 viewbox.Top,
                                 slicedRect.Left,
                                 slicedRect.Top);
                            return rect;
                        }
                        else
                        {
                            var dx = viewbox.Right - viewbox.Left;
                            var dy = viewbox.Bottom - viewbox.Top;
                            var rect = new System.Windows.Rect(viewbox.Left, viewbox.Top,
                                slicedRect.Left * dx, slicedRect.Top * dy);
                            return rect;
                        }
                    case "part1":
                        if (mode == BrushMappingMode.Absolute)
                        {
                            return new System.Windows.Rect(viewbox.Left + slicedRect.Left,
                               viewbox.Top,
                               viewbox.Right - viewbox.Left - slicedRect.Right - slicedRect.Left,
                               slicedRect.Top);
                        }
                        else
                        {
                            var dx = viewbox.Right - viewbox.Left;
                            var dy = viewbox.Bottom - viewbox.Top;
                            return new System.Windows.Rect(viewbox.Left + slicedRect.Left * dx, viewbox.Top,
                                (1 - slicedRect.Right - slicedRect.Left) * dx, slicedRect.Top * dy);
                        }
                    case "part2":
                        if (mode == BrushMappingMode.Absolute)
                        {
                            return new System.Windows.Rect(viewbox.Right - slicedRect.Right,
                               viewbox.Top,
                               slicedRect.Right,
                               slicedRect.Top);
                        }
                        else
                        {
                            var dx = viewbox.Right - viewbox.Left;
                            var dy = viewbox.Bottom - viewbox.Top;
                            return new System.Windows.Rect(viewbox.Right - slicedRect.Right * dx,
                                viewbox.Top,
                                slicedRect.Right * dx, slicedRect.Top * dy);
                        }
                    case "part3":
                        if (mode == BrushMappingMode.Absolute)
                        {
                            return new System.Windows.Rect(viewbox.Left,
                               viewbox.Top + slicedRect.Top,
                               slicedRect.Left,
                               viewbox.Bottom - viewbox.Top - slicedRect.Bottom - slicedRect.Top);
                        }
                        else
                        {
                            var dx = viewbox.Right - viewbox.Left;
                            var dy = viewbox.Bottom - viewbox.Top;
                            return new System.Windows.Rect(viewbox.Left,
                                viewbox.Top + slicedRect.Top * dy,
                                slicedRect.Left * dx, (1 - slicedRect.Bottom - slicedRect.Top) * dy);
                        }
                    case "part4":
                        if (mode == BrushMappingMode.Absolute)
                        {
                            return new System.Windows.Rect(viewbox.Left + slicedRect.Left,
                               viewbox.Top + slicedRect.Top,
                               viewbox.Right - viewbox.Left - slicedRect.Right - slicedRect.Left,
                               viewbox.Bottom - viewbox.Top - slicedRect.Bottom - slicedRect.Top);
                        }
                        else
                        {
                            var dx = viewbox.Right - viewbox.Left;
                            var dy = viewbox.Bottom - viewbox.Top;
                            return new System.Windows.Rect(viewbox.Left + slicedRect.Left * dx,
                                viewbox.Top + slicedRect.Top * dy,
                                (1 - slicedRect.Right - slicedRect.Left) * dx, (1 - slicedRect.Bottom - slicedRect.Top) * dy);
                        }
                    case "part5":
                        if (mode == BrushMappingMode.Absolute)
                        {
                            return new System.Windows.Rect(viewbox.Right - slicedRect.Right,
                               viewbox.Top + slicedRect.Top,
                               slicedRect.Right,
                               viewbox.Bottom - viewbox.Top - slicedRect.Bottom - slicedRect.Top);
                        }
                        else
                        {
                            var dx = viewbox.Right - viewbox.Left;
                            var dy = viewbox.Bottom - viewbox.Top;
                            return new System.Windows.Rect(viewbox.Right - slicedRect.Right * dx,
                                viewbox.Top + slicedRect.Top * dy,
                                slicedRect.Right * dx, (1 - slicedRect.Bottom - slicedRect.Top) * dy);
                        }
                    case "part6":
                        if (mode == BrushMappingMode.Absolute)
                        {
                            return new System.Windows.Rect(viewbox.Left,
                                viewbox.Bottom - slicedRect.Bottom,
                                 slicedRect.Left,
                                 slicedRect.Bottom);
                        }
                        else
                        {
                            var dx = viewbox.Right - viewbox.Left;
                            var dy = viewbox.Bottom - viewbox.Top;
                            return new System.Windows.Rect(viewbox.Left,
                                viewbox.Bottom - slicedRect.Bottom * dy,
                                slicedRect.Left * dx, slicedRect.Bottom * dy);
                        }
                    case "part7":
                        if (mode == BrushMappingMode.Absolute)
                        {
                            return new System.Windows.Rect(viewbox.Left + slicedRect.Left,
                               viewbox.Bottom - slicedRect.Bottom,
                               viewbox.Right - viewbox.Left - slicedRect.Right - slicedRect.Left,
                               slicedRect.Bottom);
                        }
                        else
                        {
                            var dx = viewbox.Right - viewbox.Left;
                            var dy = viewbox.Bottom - viewbox.Top;
                            return new System.Windows.Rect(viewbox.Left + slicedRect.Left * dx,
                                viewbox.Bottom - slicedRect.Right * dy,
                                (1 - slicedRect.Right - slicedRect.Left) * dx, slicedRect.Bottom * dy);
                        }
                    case "part8":
                        if (mode == BrushMappingMode.Absolute)
                        {
                            return new System.Windows.Rect(viewbox.Right - slicedRect.Right,
                               viewbox.Bottom - slicedRect.Bottom,
                              slicedRect.Right,
                              slicedRect.Bottom);
                        }
                        else
                        {
                            var dx = viewbox.Right - viewbox.Left;
                            var dy = viewbox.Bottom - viewbox.Top;
                            return new System.Windows.Rect(viewbox.Right - slicedRect.Right * dx,
                                 viewbox.Bottom - slicedRect.Bottom * dy,
                                 slicedRect.Right * dx, slicedRect.Bottom * dy);
                        }
                }
            }
            catch
            {
                // 可能会先修改模式，然后再设计值，就导致Rect中出现负值
                return null;
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
