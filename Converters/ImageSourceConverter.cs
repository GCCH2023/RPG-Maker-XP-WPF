using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace XP
{
    public class ImageSourceConverter : IValueConverter
    {
        public static BitmapImage LoadImage(string folderName, string fileName)
        {
            string path;
            // 判断是在设计时还是运行时
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                path = string.Format("{0}{1}{2}", Global.DesignResourceFolder, folderName, fileName);
            }
            else
            {
                path = string.Format("{0}{1}", folderName, fileName);
            }
            // 获取文件的完整名称
            try
            {
                var fullPath = Utility.Util.GetFullFileName(path);
                var bitmap = new BitmapImage(new Uri(fullPath, UriKind.Relative));
                return bitmap;
            }catch(Exception e)
            {
                System.Windows.MessageBox.Show("找不到文件 " + path + e.Message);
            }
            return null;
        }

        public object Convert(object value, Type targetType, object parameter, global::System.Globalization.CultureInfo culture)
        {
            if (parameter == null)
                return LoadImage("", (string)value);

            var filename = (string)value;
            switch ((string)parameter)
            {
                case "animation":
                    return LoadImage("Graphics/Animations/", filename);

                case "autotile":
                    return LoadImage("Graphics/Autotiles/", filename);
                case "battleback":
                    return LoadImage("Graphics/Battlebacks/", filename);
                case "battler":
                    return LoadImage("Graphics/Battlers/", filename);
                case "character":
                    return LoadImage("Graphics/Characters/", filename);
                case "fog":
                    return LoadImage("Graphics/Fogs/", filename);
                case "gameover":
                    return LoadImage("Graphics/Gameovers/", filename);
                case "icon":
                    return LoadImage("Graphics/Icons/", filename);
                case "panorama":
                    return LoadImage("Graphics/Panoramas/", filename);
                case "picture":
                    return LoadImage("Graphics/Pictures/", filename);
                case "tileset":
                    return LoadImage("Graphics/Tilesets/", filename);
                case "title":
                    return LoadImage("Graphics/Titles/", filename);
                case "windowskin":
                    return LoadImage("Graphics/Windowskins/", filename);
            }
            return LoadImage("", filename);
        }

        public object ConvertBack(object value, Type targetType, object parameter, global::System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
