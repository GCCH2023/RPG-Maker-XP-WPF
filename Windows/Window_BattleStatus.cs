using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Window_BattleStatus
    //------------------------------------------------------------------------------
    // 　显示战斗画面同伴状态的窗口。
    //==============================================================================

    public class Window_BattleStatus : Window_Base
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Window_BattleStatus() :
            base(0, 320, 640, 160)
        {
            this.contents = new Bitmap(width - 32, height - 32);
            this.level_up_flags = new bool[] { false, false, false, false };
            refresh();
        }
        //--------------------------------------------------------------------------
        // ● 释放
        //--------------------------------------------------------------------------
        public override void dispose()
        {
            base.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 设置升级标志
        //     actor_index : 角色索引
        //--------------------------------------------------------------------------
        public void level_up(int actor_index)
        {
            this.level_up_flags[actor_index] = true;
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
                var actor = Global.game_party.actors[i];
                var actor_x = i * 160 + 4;
                draw_actor_name(actor, actor_x, 0);
                draw_actor_hp(actor, actor_x, 32, 120);
                draw_actor_sp(actor, actor_x, 64, 120);
                if (this.level_up_flags[i])
                {
                    this.contents.font.color = normal_color;
                    this.contents.draw_text(actor_x, 96, 120, 32, "LEVEL UP!");

                }
                else
                    draw_actor_state(actor, actor_x, 96);
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            base.update();
            // 主界面的不透明度下降
            if (Global.game_temp.battle_main_phase)
            {
                if (this.contents_opacity > 191)
                    this.contents_opacity -= 4;
            }
            else
            {
                if (this.contents_opacity < 255)
                    this.contents_opacity += 4;
            }
        }
        private bool[] level_up_flags;


        public int item_max { get; set; }
    }
}
