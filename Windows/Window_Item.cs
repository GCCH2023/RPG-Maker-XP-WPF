using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Window_Item
    //------------------------------------------------------------------------------
    // 　物品画面、战斗画面、显示浏览物品的窗口。
    //==============================================================================

    public class Window_Item : Window_Selectable
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Window_Item() :
            base(0, 64, 640, 416)
        {
            this.column_max = 2;
            refresh();
            this.index = 0;
            // 战斗中的情况下将窗口移至中央并将其半透明化
            if (Global.game_temp.in_battle)
            {
                this.y = 64;
                this.height = 256;
                this.back_opacity = 160;
            }
            this.active = true;
        }
        //--------------------------------------------------------------------------
        // ● 获取物品
        //--------------------------------------------------------------------------
        public RPG.Goods item
        {
            get
            {
                if (this.index >= this.data.Count)
                    return null;
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
            // 添加物品
            for (var i = 1; i < Global.data_items.Count; i++)
                if (Global.game_party.item_number(i) > 0)
                    this.data.Add(Global.data_items[i]);

            // 在战斗中以外添加武器、防具
            if (!Global.game_temp.in_battle)
            {
                for (var i = 1; i < Global.data_weapons.Count; i++)
                    if (Global.game_party.weapon_number(i) > 0)
                        this.data.Add(Global.data_weapons[i]);

                for (var i = 1; i < Global.data_armors.Count; i++)
                    if (Global.game_party.armor_number(i) > 0)
                        this.data.Add(Global.data_armors[i]);

            }
            // 如果项目数不是 0 就生成位图、重新描绘全部项目
            this.item_max = this.data.Count;
            if (this.item_max > 0)
            {
                this.contents = new Bitmap(width - 32, row_max * 32);
                for (var i = 0; i < this.item_max; i++)
                    draw_item(i);
            }
        }
        //--------------------------------------------------------------------------
        // ● 描绘项目
        //     index : 项目编号
        //--------------------------------------------------------------------------
        public void draw_item(int index)
        {
            var item = this.data[index];
            int number = 0;
            if (item is RPG.Item)
                number = Global.game_party.item_number(item.id);
            else if (item is RPG.Weapon)
                number = Global.game_party.weapon_number(item.id);
            else if (item is RPG.Armor)
                number = Global.game_party.armor_number(item.id);

            if (item is RPG.Item &&
                      Global.game_party.is_item_can_use(item.id))

                this.contents.font.color = normal_color;
            else
                this.contents.font.color = disabled_color;

            var x = 4 + index % 2 * (288 + 32);
            var y = index / 2 * 32;
            var rect = new Rect(x, y, this.width / this.column_max - 32, 32);
            this.contents.fill_rect(rect, Colors.Transparent);
            var bitmap = RPG.Cache.icon(item.icon_name);
            var opacity = this.contents.font.color == normal_color ? 255 : 128;
            this.contents.blt(x, y + 4, bitmap, new Rect(0, 0, 24, 24), opacity);
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
    }
}
