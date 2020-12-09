using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Window_EquipLeft
    //------------------------------------------------------------------------------
    // 　装备画面的、显示角色能力值变化的窗口。
    //==============================================================================

    public class Window_EquipLeft : Window_Base
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     actor : 角色
        //--------------------------------------------------------------------------
        public Window_EquipLeft(Game_Actor actor) :
            base(0, 64, 272, 192)
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
            draw_actor_level(this.actor, 4, 32);
            draw_actor_parameter(this.actor, 4, 64, 0);
            draw_actor_parameter(this.actor, 4, 96, 1);
            draw_actor_parameter(this.actor, 4, 128, 2);
            if (this.new_atk != null)
            {
                this.contents.font.color = system_color;
                this.contents.draw_text(160, 64, 40, 32, "→", 1);
                this.contents.font.color = normal_color;
                this.contents.draw_text(200, 64, 36, 32, this.new_atk.ToString(), 2);
            }

            if (this.new_pdef != null)
            {
                this.contents.font.color = system_color;
                this.contents.draw_text(160, 96, 40, 32, "→", 1);
                this.contents.font.color = normal_color;
                this.contents.draw_text(200, 96, 36, 32, this.new_pdef.ToString(), 2);
            }

            if (this.new_mdef != null)
            {
                this.contents.font.color = system_color;
                this.contents.draw_text(160, 128, 40, 32, "→", 1);
                this.contents.font.color = normal_color;
                this.contents.draw_text(200, 128, 36, 32, this.new_mdef.ToString(), 2);
            }
        }
        //--------------------------------------------------------------------------
        // ● 变更装备后的能力值设置
        //     new_atk  : 变更装备后的攻击力
        //     new_pdef : 变更装备后的物理防御
        //     new_mdef : 变更装备后的魔法防御
        //--------------------------------------------------------------------------
        public void set_new_parameters(int new_atk, int new_pdef, int new_mdef)
        {
            if (this.new_atk != new_atk || this.new_pdef != new_pdef || this.new_mdef != new_mdef)
            {
                this.new_atk = new_atk;
                this.new_pdef = new_pdef;
                this.new_mdef = new_mdef;
                refresh();
            }
        }

        public Game_Actor actor { get; set; }
        public int new_atk { get; set; }
        public int new_pdef { get; set; }
        public int new_mdef { get; set; }
    }
}
