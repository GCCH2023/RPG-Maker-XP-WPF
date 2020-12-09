using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Window_SkillStatus
    //------------------------------------------------------------------------------
    // 　显示特技画面、特技使用者的窗口。
    //==============================================================================

    public class Window_SkillStatus : Window_Base
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     actor : 角色
        //--------------------------------------------------------------------------
        public Window_SkillStatus(Game_Actor actor) :
            base(0, 64, 640, 64)
        {
            this.contents = new Bitmap(width - 32, height - 32);
            this.actor = actor;
            refresh();
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            draw_actor_name(this.actor, 4, 0);
            draw_actor_state(this.actor, 140, 0);
            draw_actor_hp(this.actor, 284, 0);
            draw_actor_sp(this.actor, 460, 0);
        }


        public Game_Actor actor { get; set; }
    }
}
