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
    // ■ Window_ShopStatus
    //------------------------------------------------------------------------------
    // 　商店画面、显示物品所持数与角色装备的窗口。
    //==============================================================================

    public class Window_ShopStatus : Window_Base
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Window_ShopStatus() :
            base(368, 128, 272, 352)
        {
            this.contents = new Bitmap(width - 32, height - 32);
            this.item = null;
            refresh();
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            if (this.item == null)
                return;

            int number = 0;
            if (this.item is RPG.Item)
                number = Global.game_party.item_number(this.item.id);
            else if (this.item is RPG.Weapon)
                number = Global.game_party.weapon_number(this.item.id);
            else if (this.item is RPG.Armor)
                number = Global.game_party.armor_number(this.item.id);

            this.contents.font.color = system_color;
            this.contents.draw_text(4, 0, 200, 32, "所持数");
            this.contents.font.color = normal_color;
            this.contents.draw_text(204, 0, 32, 32, number.ToString(), 2);
            if (this.item is RPG.Item)
                return;

            // 添加装备品信息
            for (var i = 0; i < Global.game_party.actors.Count; i++)
            {
                // 获取角色
                var actor = Global.game_party.actors[i];
                // 可以装备为普通文字颜色、不能装备设置为无效文字颜色
                if (actor.is_equippable(this.item))
                    this.contents.font.color = normal_color;
                else
                    this.contents.font.color = disabled_color;

                // 描绘角色名字
                this.contents.draw_text(4, 64 + 64 * i, 120, 32, actor.name);
                RPG.Goods item1 = null;
                // 获取当前的装备品
                if (this.item is RPG.Weapon)
                    item1 = Global.data_weapons[actor.weapon_id];
                else if (this.item.kind == 0)
                    item1 = Global.data_armors[actor.armor1_id];
                else if (this.item.kind == 1)
                    item1 = Global.data_armors[actor.armor2_id];
                else if (this.item.kind == 2)
                    item1 = Global.data_armors[actor.armor3_id];
                else
                    item1 = Global.data_armors[actor.armor4_id];

                int atk1, atk2, change = 0;
                int pdef1, mdef1, pdef2, mdef2;
                // 可以装备的情况
                if (actor.is_equippable(this.item))
                { // 武器的情况
                    if (this.item is RPG.Weapon)
                    {
                        var item2 = (RPG.Weapon)this.item;
                        atk1 = item1 != null ? item2.atk : 0;
                        atk2 = item2 != null ? item2.atk : 0;
                        change = atk2 - atk1;
                    }
                    // 防具的情况
                    if (this.item is RPG.Armor)
                    {
                        var item2 = (RPG.Armor)this.item;
                        pdef1 = item2 != null ? item2.pdef : 0;
                        mdef1 = item2 != null ? item2.mdef : 0;
                        pdef2 = item != null ? item2.pdef : 0;
                        mdef2 = item != null ? item2.mdef : 0;
                        change = pdef2 - pdef1 + mdef2 - mdef1;
                    }
                    // 描绘能力值变化
                    this.contents.draw_text(124, 64 + 64 * i, 112, 32,
                      string.Format("%+d", change), 2);

                    // 描绘物品
                    if (item1 != null)
                    {
                        var x = 4;
                        var y = 64 + 64 * i + 32;
                        var bitmap = RPG.Cache.icon(item1.icon_name);
                        var opacity = this.contents.font.color == normal_color ? 255 : 128;
                        this.contents.blt(x, y + 4, bitmap, new Rect(0, 0, 24, 24), opacity);
                        this.contents.draw_text(x + 28, y, 212, 32, item1.name);
                    }
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 设置物品
        //     item : 新的物品
        //--------------------------------------------------------------------------
        RPG.Goods _item;
        public RPG.Goods item
        {
            set
            {
                if (this._item != value)
                    this._item = item;
                refresh();
            }
            get { return this._item; }
        }
    }
}
