using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Internal;

namespace XP
{
   //==============================================================================
// ■ Window_Steps
//------------------------------------------------------------------------------
// 　菜单画面显示步数的窗口。
//==============================================================================

    public class Window_Steps : Window_Base
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Window_Steps() :
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
            this.contents.draw_text(4, 0, 120, 32, "步数");
            this.contents.font.color = normal_color;
            this.contents.draw_text(4, 32, 120, 32, Global.game_party.steps.ToString(), 2);
        }
    }
}
