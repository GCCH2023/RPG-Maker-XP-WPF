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
    // ■ Window_EquipItem
    //------------------------------------------------------------------------------
    // 　装备画面、显示浏览变更装备的候补物品的窗口。
    //==============================================================================

    public class Window_EquipItem : Window_Selectable
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     actor      : 角色
        //     equip_type : 装备部位 (0～3)
        //--------------------------------------------------------------------------
        public Window_EquipItem(Game_Actor actor, int equip_type)
            : base(0, 256, 640, 224)
        {
            this.actor = actor;
            this.equip_type = equip_type;
            this.column_max = 2;
            refresh();
            this.active = false;
            this.index = -1;
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
            if (this.contents != null)
            {
                this.contents.dispose();
                this.contents = null;
            }

            this.data = new List<RPG.Goods>();
            // 添加可以装备的武器
            if (this.equip_type == 0)
            {
                var weapon_set = Global.data_classes[this.actor.class_id].weapon_set;
                for (var i = 1; i < Global.data_weapons.Count; i++)
                {
                    if (Global.game_party.weapon_number(i) > 0 && weapon_set.Contains(i))
                        this.data.Add(Global.data_weapons[i]);
                }
            }
            // 添加可以装备的防具
            if (this.equip_type != 0)
            {
                var armor_set = Global.data_classes[this.actor.class_id].armor_set;
                for (var i = 1; i < Global.data_armors.Count; i++)
                {
                    if (Global.game_party.armor_number(i) > 0 && armor_set.Contains(i))
                    {
                        if (Global.data_armors[i].kind == this.equip_type - 1)
                            this.data.Add(Global.data_armors[i]);
                    }
                }
            }
            // 添加空白
            this.data.Add(null);
            // 生成位图、描绘全部项目
            this.item_max = this.data.Count;
            this.contents = new Bitmap(width - 32, row_max * 32);
            for (var i = 0; i < this.item_max - 1; i++)
                draw_item(i);
        }
        //--------------------------------------------------------------------------
        // ● 项目的描绘
        //     index : 项目符号
        //--------------------------------------------------------------------------
        public void draw_item(int index)
        {
            var item = this.data[index];
            var x = 4 + index % 2 * (288 + 32);
            var y = index / 2 * 32;
            int number = 0;
            if (item is RPG.Weapon)
            {
                number = Global.game_party.weapon_number(item.id);
            }
            else if (item is RPG.Armor)
            {
                number = Global.game_party.armor_number(item.id);
            }

            var bitmap = RPG.Cache.icon(item.icon_name);
            this.contents.blt(x, y + 4, bitmap, new Rect(0, 0, 24, 24));
            this.contents.font.color = normal_color;
            this.contents.draw_text(x + 28, y, 212, 32, item.name, 0);
            this.contents.draw_text(x + 240, y, 16, 32, ":", 1);
            this.contents.draw_text(x + 256, y, 24, 32, number.ToString(), 2);
        }
        //--------------------------------------------------------------------------
        // ● 刷新帮助文本
        //--------------------------------------------------------------------------
        public override void update_help()
        {
            this.help_window.set_text(this.item == null ? "" : this.item.description);
        }


        public List<RPG.Goods> data { get; set; }
        public int equip_type { get; set; }
        public Game_Actor actor { get; set; }
    }
}
