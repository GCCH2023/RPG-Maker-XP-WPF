using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Window_EquipRight
    //------------------------------------------------------------------------------
    // 　装备画面、显示角色现在装备的物品的窗口。
    //==============================================================================

    public class Window_EquipRight : Window_Selectable
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     actor : 角色
        //--------------------------------------------------------------------------
        public Window_EquipRight(Game_Actor actor) :
            base(272, 64, 368, 192)
        {
            this.contents = new Bitmap(width - 32, height - 32);
            this.actor = actor;
            refresh();
            this.index = 0;
            this.active = true;
        }
        //--------------------------------------------------------------------------
        // ● 获取物品
        //--------------------------------------------------------------------------
        public RPG.Goods item
        {
            get
            {
                return this.data[this.index];
            }
        }

        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            this.data = new List<RPG.Goods>();
            this.data.Add(Global.data_weapons[this.actor.weapon_id]);
            this.data.Add(Global.data_armors[this.actor.armor1_id]);
            this.data.Add(Global.data_armors[this.actor.armor2_id]);
            this.data.Add(Global.data_armors[this.actor.armor3_id]);
            this.data.Add(Global.data_armors[this.actor.armor4_id]);
            this.item_max = this.data.Count;
            this.contents.font.color = system_color;
            this.contents.draw_text(4, 32 * 0, 92, 32, Global.data_system.words.weapon);
            this.contents.draw_text(4, 32 * 1, 92, 32, Global.data_system.words.armor1);
            this.contents.draw_text(4, 32 * 2, 92, 32, Global.data_system.words.armor2);
            this.contents.draw_text(4, 32 * 3, 92, 32, Global.data_system.words.armor3);
            this.contents.draw_text(5, 32 * 4, 92, 32, Global.data_system.words.armor4);
            draw_item_name(this.data[0], 92, 32 * 0);
            draw_item_name(this.data[1], 92, 32 * 1);
            draw_item_name(this.data[2], 92, 32 * 2);
            draw_item_name(this.data[3], 92, 32 * 3);
            draw_item_name(this.data[4], 92, 32 * 4);
        }
        //--------------------------------------------------------------------------
        // ● 刷新帮助文本
        //--------------------------------------------------------------------------
        public override void update_help()
        {
            this.help_window.set_text(this.item == null ? "" : this.item.description);
        }

        public Game_Actor actor { get; set; }
        public List<RPG.Goods> data { get; set; }
    }
}
