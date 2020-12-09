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
    // ■ Window_ShopBuy
    //------------------------------------------------------------------------------
    // 　商店画面、浏览显示可以购买的商品的窗口。
    //==============================================================================

    public class Window_ShopBuy : Window_Selectable
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     shop_goods : 商品
        //--------------------------------------------------------------------------
        public Window_ShopBuy(List<List<object>> shop_goods) :
            base(0, 128, 368, 352)
        {
            this.shop_goods = shop_goods;
            refresh();
            this.index = 0;
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
            RPG.Goods item = null;
            foreach (var goods_item in this.shop_goods)
            {
                switch ((int)goods_item[0])
                {
                    case 0:
                        item = Global.data_items[(int)goods_item[1]]; break;
                    case 1:
                        item = Global.data_weapons[(int)goods_item[1]]; break;
                    case 2:
                        item = Global.data_armors[(int)goods_item[1]]; break;
                }

                if (item != null)
                    this.data.Add(item);
            }
            // 如果项目数不是 0 就生成位图、描绘全部项目
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
            // 获取物品所持数
            int number = 0;
            if (item is RPG.Item)
                number = Global.game_party.item_number(item.id);
            else if (item is RPG.Weapon)
                number = Global.game_party.weapon_number(item.id);
            else if (item is RPG.Armor)
                number = Global.game_party.armor_number(item.id);

            // 价格在所持金以下、并且所持数不是 99 的情况下为普通文字颜色
            // 除此之外的情况设置为无效文字色
            if (item.price <= Global.game_party.gold && number < 99)
                this.contents.font.color = normal_color;
            else
                this.contents.font.color = disabled_color;

            var x = 4;
            var y = index * 32;
            var rect = new Rect(x, y, this.width - 32, 32);
            this.contents.fill_rect(rect, Colors.Transparent);
            var bitmap = RPG.Cache.icon(item.icon_name);
            var opacity = this.contents.font.color == normal_color ? 255 : 128;
            this.contents.blt(x, y + 4, bitmap, new Rect(0, 0, 24, 24), opacity);
            this.contents.draw_text(x + 28, y, 212, 32, item.name, 0);
            this.contents.draw_text(x + 240, y, 88, 32, item.price.ToString(), 2);
        }
        //--------------------------------------------------------------------------
        // ● 刷新帮助文本
        //--------------------------------------------------------------------------
        public override void update_help()
        {
            this.help_window.set_text(this.item == null ? "" : this.item.description);
        }


        public List<List<object>> shop_goods { get; set; }
        public List<RPG.Goods> data { get; set; }
    }
}
