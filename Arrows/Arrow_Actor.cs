using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
// ■ Arrow_Actor
//------------------------------------------------------------------------------
// 　选择角色的箭头光标。本类继承 Arrow_Base 
// 类。
//==============================================================================

    public class Arrow_Actor : Arrow_Base
    {
        public Arrow_Actor(Viewport viewport)
            : base(viewport)
        {

        }
        //--------------------------------------------------------------------------
        // ● 获取光标指向的角色
        //--------------------------------------------------------------------------
        public Game_Actor actor
        {
            get
            {
                return Global.game_party.actors[this.index];
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            base.update();
            // 光标右
            if (Input.is_repeat(Input.RIGHT))
            {
                Global.game_system.se_play(Global.data_system.cursor_se);
                this.index += 1;
                this.index %= Global.game_party.actors.Count;
            }
            // 光标左
            if (Input.is_repeat(Input.LEFT))
            {
                Global.game_system.se_play(Global.data_system.cursor_se);
                this.index += Global.game_party.actors.Count - 1;
                this.index %= Global.game_party.actors.Count;
            }

            // 设置活动块坐标
            if (this.actor != null)
            {
                this.x = this.actor.screen_x;
                this.y = this.actor.screen_y;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新帮助文本
        //--------------------------------------------------------------------------
        public override void update_help()
        {
            // 帮助窗口显示角色的状态
            this.help_window.set_actor(this.actor);
        }
    }

}
