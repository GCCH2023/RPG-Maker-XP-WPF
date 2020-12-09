using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP
{
    //       Tone
    //色调的类。各要素以浮点数（Float）管理。
    public class Tone
    {
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
        public int gray { get; set; }
//父类Object 
//类方法Tone.new(red, green, blue[, gray]) 
//生成 Tone 对象。如省略 gray 的话则默认为 0。
        public Tone(int red,int green, int blue, int gray = 0)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.gray = gray;
        }

//方法set(red, green, blue[, gray]) 
//设定所有属性。
        public void set(int red, int green, int blue, int gray = 0)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.gray = gray;
        }

        public Tone clone()
        {
            return new Tone(red, green, blue, gray);
        }
//属性red 
//红色分色的色彩平衡调整值（0 ～ 255）。范围外的数值会自动修正。

//green 
//绿色分色的色彩平衡调整值（0 ～ 255）。范围外的数值会自动修正。

//blue 
//蓝色分色的色彩平衡调整值（0 ～ 255）。范围外的数值会自动修正。

//gray 
//灰度过滤器的强度（0 ～ 255）。范围外的数值会自动修正。

//该值为 0 以外的话，色彩的平衡调整会花费额外的处理时间。

    }
}
