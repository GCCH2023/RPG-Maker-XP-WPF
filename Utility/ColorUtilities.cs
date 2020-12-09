using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Utility
{
    using System.Windows.Media;
    using Media = System.Windows.Media;
    public struct HsvColor
    {

        public double H;
        public double S;
        public double V;

        public HsvColor(double h, double s, double v)
        {
            this.H = h;
            this.S = s;
            this.V = v;
        }
    }
    static class ColorUtilities
    {
        /// <summary>
        /// 获取颜色对象的副本
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color Clone(Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        /// <summary>
        /// 从字节指针中创建颜色对象
        /// </summary>
        /// <param name="p"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static unsafe Color Load(byte* p, int length)
        {
            // 长度是32个字节
            // 依次对应r, g, b, a
            byte r = Convert.ToByte(*(double*)(p + 0));
            byte g = Convert.ToByte(*(double*)(p + 8));
            byte b = Convert.ToByte(*(double*)(p + 16));
            byte a = Convert.ToByte(*(double*)(p + 24));
            return Color.FromArgb(a, r, g, b);
        }
        // Converts an RGB color to an HSV color.
        public static HsvColor ConvertRgbToHsv(int r, int b, int g)
        {

            double delta, min;
            double h = 0, s, v;

            min = Math.Min(Math.Min(r, g), b);
            v = Math.Max(Math.Max(r, g), b);
            delta = v - min;

            if (v == 0.0)
            {
                s = 0;

            }
            else
                s = delta / v;

            if (s == 0)
                h = 0.0;

            else
            {
                if (r == v)
                    h = (g - b) / delta;
                else if (g == v)
                    h = 2 + (b - r) / delta;
                else if (b == v)
                    h = 4 + (r - g) / delta;

                h *= 60;
                if (h < 0.0)
                    h = h + 360;

            }

            HsvColor hsvColor = new HsvColor();
            hsvColor.H = h;
            hsvColor.S = s;
            hsvColor.V = v / 255;

            return hsvColor;

        }

        // Converts an HSV color to an RGB color.
        public static Media.Color ConvertHsvToRgb(double h, double s, double v)
        {

            double r = 0, g = 0, b = 0;

            if (s == 0)
            {
                r = v;
                g = v;
                b = v;
            }
            else
            {
                int i;
                double f, p, q, t;


                if (h == 360)
                    h = 0;
                else
                    h = h / 60;

                i = (int)Math.Truncate(h);
                f = h - i;

                p = v * (1.0 - s);
                q = v * (1.0 - (s * f));
                t = v * (1.0 - (s * (1.0 - f)));

                switch (i)
                {
                    case 0:
                        r = v;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = v;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = v;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = v;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = v;
                        break;

                    default:
                        r = v;
                        g = p;
                        b = q;
                        break;
                }

            }



            return Media.Color.FromArgb(255, (byte)(r * 255), (byte)(g * 255), (byte)(b * 255));

        }

        // Generates a list of colors with hues ranging from 0 360
        // and a saturation and value of 1. 
        public static List<Media.Color> GenerateHsvSpectrum()
        {

            List<Media.Color> colorsList = new List<Media.Color>(8);


            for (int i = 0; i < 29; i++)
            {

                colorsList.Add(
                    ColorUtilities.ConvertHsvToRgb(i * 12, 1, 1)
                );

            }
            colorsList.Add(ColorUtilities.ConvertHsvToRgb(0, 1, 1));


            return colorsList;

        }

    }
}
