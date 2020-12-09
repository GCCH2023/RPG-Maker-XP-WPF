using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Window_MenuStatus
    //------------------------------------------------------------------------------
    // 　显示菜单画面和同伴状态的窗口。
    //==============================================================================

    public class Window_MenuStatus : Window_Selectable
    {
        //--------------------------------------------------------------------------
        // ● 初始化目标
        //--------------------------------------------------------------------------
        public Window_MenuStatus() :
            base(0, 0, 480, 480)
        {
            this.contents = new Bitmap(width - 32, height - 32);
            refresh();
            this.active = false;
            this.index = -1;
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            this.item_max = Global.game_party.actors.Count;
            for (var i = 0; i < Global.game_party.actors.Count; i++)
            {
                var x = 64;
                var y = i * 116;
                var actor = Global.game_party.actors[i];
                draw_actor_graphic(actor, x - 40, y + 80);
                draw_actor_name(actor, x, y);
                draw_actor_class(actor, x + 144, y);
                draw_actor_level(actor, x, y + 32);
                draw_actor_state(actor, x + 90, y + 32);
                draw_actor_exp(actor, x, y + 64);
                draw_actor_hp(actor, x + 236, y + 32);
                draw_actor_sp(actor, x + 236, y + 64);
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新光标矩形
        //--------------------------------------------------------------------------
        public override void update_cursor_rect()
        {
            if (this.index < 0)
                this.cursor_rect = Rect.Empty;
            else
                this.cursor_rect = new Rect(0, this.index * 116, this.width - 32, 96);
        }
    }
}
