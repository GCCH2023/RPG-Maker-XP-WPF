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
    // ■ Window_Target
    //------------------------------------------------------------------------------
    // 　物品画面与特技画面的、使用对像角色选择窗口。
    //==============================================================================

    public class Window_Target : Window_Selectable
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Window_Target() :
            base(0, 0, 336, 480)
        {
            this.contents = new Bitmap(width - 32, height - 32);
            this.z += 10;
            this.item_max = Global.game_party.actors.Count;
            refresh();
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            for (var i = 0; i < Global.game_party.actors.Count; i++)
            {
                var x = 4;
                var y = i * 116;
                var actor = Global.game_party.actors[i];
                draw_actor_name(actor, x, y);
                draw_actor_class(actor, x + 144, y);
                draw_actor_level(actor, x + 8, y + 32);
                draw_actor_state(actor, x + 8, y + 64);
                draw_actor_hp(actor, x + 152, y + 32);
                draw_actor_sp(actor, x + 152, y + 64);
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新光标矩形
        //--------------------------------------------------------------------------
        public override void update_cursor_rect()
        {
            // 光标位置 -1 为全选、-2 以下为单独选择 (使用者自身)
            if (this.index <= -2)
                this.cursor_rect = new Rect(0, (this.index + 10) * 116, this.width - 32, 96);
            else if (this.index == -1)
                this.cursor_rect = new Rect(0, 0, this.width - 32, this.item_max * 116 - 20);
            else
                this.cursor_rect = new Rect(0, this.index * 116, this.width - 32, 96);
        }
    }
}
