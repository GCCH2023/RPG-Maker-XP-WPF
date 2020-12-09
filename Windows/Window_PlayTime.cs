using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Window_PlayTime
    //------------------------------------------------------------------------------
    // 　菜单画面显示游戏时间的窗口。
    //==============================================================================

    public class Window_PlayTime : Window_Base
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Window_PlayTime() :
            base(0, 0, 160, 96)
        {
            this.contents = new Bitmap(width - 32, height - 32);
            refresh();
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            this.contents.font.color = system_color;
            this.contents.draw_text(4, 0, 120, 32, "游戏时间");
            this.total_sec = Graphics.frame_count / Graphics.frame_rate;
            var hour = this.total_sec / 60 / 60;
            var min = this.total_sec / 60 % 60;
            var sec = this.total_sec % 60;
            var text = string.Format("{0:D02}:{1:D02}:{2:D02}", hour, min, sec);
            this.contents.font.color = normal_color;
            this.contents.draw_text(4, 32, 120, 32, text, 2);
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            base.update();
            if (Graphics.frame_count / Graphics.frame_rate != this.total_sec)
                refresh();
        }



        public int total_sec { get; set; }
    }
}
