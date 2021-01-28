using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace XP.RPG
{
    //RPG.Animation
    //动画的数据类。
    //父类Object 
    public class Animation
    {
        //属性id 
        //ID。
        public int id { get; set; }
        //name 
        //名称。
        public string name { get; set; }
        //animation_name 
        //动画图像的文件名。
        public string animation_name { get; set; }
        //animation_hue 
        //动画图像的色相变化值（0..360）。
        public int animation_hue { get; set; }
        //position 
        //位置（0：上，1：中，2：下，3：画面）。
        public int position { get; set; }
        //frame_max 
        //帧数。
        public int frame_max { get; set; }
        //frames 
        //帧的内容。RPG.Animation.Frame 的数组。
        public List<Frame> frames { get; set; }
        //timings 
        //SE 与闪烁的时机。RPG.Animation.Timing 的数组。
        public List<Timing> timings { get; set; }
        //内部类RPG.Animation.Frame 
        //RPG.Animation.Timing 
        //定义module RPG

        public Animation()
        {
            this.id = 0;
            this.name = "";
            this.animation_name = "";
            this.animation_hue = 0;
            this.position = 1;
            this.frame_max = 1;
            this.frames = new List<Frame>();
            this.timings = new List<Timing>();
        }

        //RPG.Animation.Frame
        //动画帧的数据类。
        //定义module RPG

        public class Frame
        {

            //父类Object 
            //参照元RPG.Animation 

            //属性cell_max 
            //单元的数。与帧中存在的最大的单元号码相同。
            public int cell_max { get; set; }
            //cell_data 
            //包含了单元内容的二维数组（Table）。
            //具体来说应该是 cell_data[cell_index, data_index] 的形式。
            //data_index 的范围是 0..7，表示单元的各信息（0：式样、1：X 座标，2：Y 座标，3：放大率，4：旋转角度，5：左右反转，6：不透明度，7：合成方式）。式样是 Maker 显示的号码减去 1 的数字。-1 意味着该单元缺如。
            Table _cell_data;

            public Table cell_data
            {
                get { return _cell_data; }
                set { _cell_data = value; }
            }

            public Frame()
            {
                this.cell_max = 0;
                this.cell_data = new Table(0, 0);
            }
        }

        //RPG.Animation.Timing
        //动画「SE 与闪烁的时机」的数据类。

        //父类Object 
        public class Timing
        {
            //参照元RPG.Animation 
            //属性frame 
            //帧号码。是 Maker 显示的号码减去 1 的数字。
            public int frame { get; set; }
            //se 
            //SE（RPG.AudioFile）。
            public RPG.AudioFile se { get; set; }
            //flash_scope 
            //闪烁的范围（0：无，1：对象，2：画面，3：对象消失）。
            public int flash_scope { get; set; }
            //flash_color 
            //闪烁的颜色（Color）。
            public Color flash_color { get; set; }
            //flash_duration 
            //闪烁的持续时间。
            public int flash_duration { get; set; }
            //condition 
            //条件（0：无，1：击中，2失败）。
            public int condition { get; set; }
            //定义module RPG
            public Timing()
            {
                this.frame = 0;
                this.se = new AudioFile("", 80);
                this.flash_scope = 0;
                this.flash_color = Colors.White;
                this.flash_duration = 5;
                this.condition = 0;
            }
        }
    }
}
