using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Internal;

namespace XP
{
    //==============================================================================
// ■ Window_Gold
//------------------------------------------------------------------------------
// 　显示金钱的窗口。
//==============================================================================

    public class Window_Gold : Window_Base
    {
        //--------------------------------------------------------------------------
        // ● 初始化窗口
        //--------------------------------------------------------------------------
        public Window_Gold()
            : base(0, 0, 160, 64)
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
            int cx = (int)contents.text_size(Global.data_system.words.gold.ToString()).Width;
            this.contents.font.color = normal_color;
            this.contents.draw_text(4, 0, 120 - cx - 2, 32, Global.game_party.gold.ToString(), 2);
            this.contents.font.color = system_color;
            this.contents.draw_text(124 - cx, 0, cx, 32, Global.data_system.words.gold.ToString(), 2);
        }
    }
}
