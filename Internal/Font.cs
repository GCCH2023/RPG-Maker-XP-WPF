using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace XP
{
    public class Font
    {
        //        Font
        //字体的类。字体是 Bitmap 类的属性。

        //父类Object 
        //类方法Font.new([name[, size]]) 
        //生成 Font 对象。
        public Font(string name, double size)
        {

        }

        public Font()
        {

        }
        //Font.is_exist(name) 
        //系统中存在指定名称的字体时返回真。
        public bool is_exist
        {
            get { return Fonts.SystemFontFamilies.Contains(new FontFamily(this.name)); }
        }
        //属性name 
        //字体名称。初始值是“MS PGothic”（莫尼卡汉化版初始值是“黑体”）。

        //想要按顺序指定多个字体的话，可以设定成字符串的排列。

        //font.name = ["华文行楷", "黑体"]

        //上述示例，如果系统中不存在第一指定字体“华文行楷”的话，会使用第二指定字体“黑体”。
        string _name = default_name;
        public string name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        //size 
        //字体的大小。初期值是 22。
        public double size = default_size;
        //bold 
        //粗体特征。初始值是 false。
        public bool bold = default_bold;
        //italic 
        //斜体特征。初始值是 false。
        public bool italic = default_italic;
        //color 
        //字体的颜色（Color）。alpha 值也有效。初期值是（255，255，255，255）。
        public Color color = Colors.White;
        //类属性default_name 
        public static string default_name = "Arial";
        //default_size 
        public static double default_size = 22;
        //default_bold 
        public static bool default_bold = true;
        //default_italic 
        public static bool default_italic = false;
        //default_color 
        //更改新建 Font 对象时各属性设定的默认值。
        public static Color default_color = Colors.White;
        //Font.default_name = "华文行楷"
        //Font.default_bold = true

        static Font _default_font = new Font();
        /// <summary>
        /// 默认字体
        /// </summary>
        public static Font default_font
        {
            get { return _default_font;}
        }
    }
}
