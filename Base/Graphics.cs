using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    // Graphics
    //进行有关全体图像处理的模块。
    public class Graphics
    {

        //模块方法Graphics.update 
        //更新游戏画面，前进 1 帧时间。这个方法必须要定期调用。

        //while(true){
        //  Graphics.update
        //  Input.update
        //  do_something
        //end
        //这个方法 10 秒以上未运行，会被认为脚本失控而将其强制结束。

        public static void update()
        {

        }

        //Graphics.freeze 
        //准备渐变，固定现在的画面。

        //从这之后一直到调用 transition 方法，其间禁止一切画面更换。
        public static void freeze()
        {

        }
        //Graphics.transition([duration[, filename[, vague]]]) 
        //进行从以 freeze 方法固定的画面到现在画面的渐变。

        //duration 是渐变的帧数。省略时默认为 8。

        //filename 指定渐变图形的文件名（未指定文件名的话通常为画面淡出）。也会自动搜索 RGSS-RTP、加密档案文件中包含的文件。可以省略文件扩展名。

        //vague 是传送元和传送处边界的模糊度，数值越大越模糊。省略时默认为 40。
        public static void transition(int duration = 8, string filename = "", int vague = 40)
        {

        }
        //Graphics.frame_reset 
        //重设画面的更新时间。调用该方法花费时间处理后，能避免严重的跳帧现象。
        public static void frame_reset()
        {

        }

        static int _frame_rate = 50;
        //模块属性Graphics.frame_rate 
        //「平滑模式」下 1 秒钟更新画面的次数。值越大就会占用更多的 CPU 资源。通常为 40。未选择「平滑模式」时更新次数为其一半，每 1 帧都会跳帧显示。
        //不推荐更改该属性，如要更改请在 10 ～ 120 的范围内指定数值。范围外的数值会自动修正。
        public static int frame_rate
        {
            get
            {
                return _frame_rate;
            }
            set
            {
                if (value < 10)
                    value = 10;
                if (value > 120)
                    value = 120;
                _frame_rate = value;
            }
        }
        //Graphics.frame_count 
        //是画面更新次数的计数。游戏开始时这个属性预先设定为 0，通过 frame_rate 属性的值，就能算出游戏的运行时间（秒数）。
        public static int frame_count = 0;
    }
}
