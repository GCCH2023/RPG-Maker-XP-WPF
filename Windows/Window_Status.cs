using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Window_Status
    //------------------------------------------------------------------------------
    // 　显示状态画面、完全规格的状态窗口。
    //==============================================================================

    public class Window_Status : Window_Base
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     actor : 角色
        //--------------------------------------------------------------------------
        public Window_Status(Game_Actor actor) :
            base(0, 0, 640, 480)
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
            draw_actor_graphic(this.actor, 40, 112);
            draw_actor_name(this.actor, 4, 0);
            draw_actor_class(this.actor, 4 + 144, 0);
            draw_actor_level(this.actor, 96, 32);
            draw_actor_state(this.actor, 96, 64);
            draw_actor_hp(this.actor, 96, 112, 172);
            draw_actor_sp(this.actor, 96, 144, 172);
            draw_actor_parameter(this.actor, 96, 192, 0);
            draw_actor_parameter(this.actor, 96, 224, 1);
            draw_actor_parameter(this.actor, 96, 256, 2);
            draw_actor_parameter(this.actor, 96, 304, 3);
            draw_actor_parameter(this.actor, 96, 336, 4);
            draw_actor_parameter(this.actor, 96, 368, 5);
            draw_actor_parameter(this.actor, 96, 400, 6);
            this.contents.font.color = system_color;
            this.contents.draw_text(320, 48, 80, 32, "EXP");
            this.contents.draw_text(320, 80, 80, 32, "NEXT");
            this.contents.font.color = normal_color;
            this.contents.draw_text(320 + 80, 48, 84, 32, this.actor.exp_s, 2);
            this.contents.draw_text(320 + 80, 80, 84, 32, this.actor.next_rest_exp_s, 2);
            this.contents.font.color = system_color;
            this.contents.draw_text(320, 160, 96, 32, "装备");
            draw_item_name(Global.data_weapons[this.actor.weapon_id], 320 + 16, 208);
            draw_item_name(Global.data_armors[this.actor.armor1_id], 320 + 16, 256);
            draw_item_name(Global.data_armors[this.actor.armor2_id], 320 + 16, 304);
            draw_item_name(Global.data_armors[this.actor.armor3_id], 320 + 16, 352);
            draw_item_name(Global.data_armors[this.actor.armor4_id], 320 + 16, 400);
        }
        public void dummy()
        {
            this.contents.font.color = system_color;
            this.contents.draw_text(320, 112, 96, 32, Global.data_system.words.weapon);
            this.contents.draw_text(320, 176, 96, 32, Global.data_system.words.armor1);
            this.contents.draw_text(320, 240, 96, 32, Global.data_system.words.armor2);
            this.contents.draw_text(320, 304, 96, 32, Global.data_system.words.armor3);
            this.contents.draw_text(320, 368, 96, 32, Global.data_system.words.armor4);
            draw_item_name(Global.data_weapons[this.actor.weapon_id], 320 + 24, 144);
            draw_item_name(Global.data_armors[this.actor.armor1_id], 320 + 24, 208);
            draw_item_name(Global.data_armors[this.actor.armor2_id], 320 + 24, 272);
            draw_item_name(Global.data_armors[this.actor.armor3_id], 320 + 24, 336);
            draw_item_name(Global.data_armors[this.actor.armor4_id], 320 + 24, 400);
        }


        public Game_Actor actor { get; set; }
    }
}
