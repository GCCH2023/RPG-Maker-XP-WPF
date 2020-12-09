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
    // ■ Window_ShopNumber
    //------------------------------------------------------------------------------
    // 　商店画面、输入买卖数量的窗口。
    //==============================================================================

    public class Window_ShopNumber : Window_Base
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Window_ShopNumber() :
            base(0, 128, 368, 352)
        {
            this.contents = new Bitmap(width - 32, height - 32);
            this.item = null;
            this.max = 1;
            this.price = 0;
            this.number = 1;
        }
        //--------------------------------------------------------------------------
        // ● 设置物品、最大个数、价格
        //--------------------------------------------------------------------------
        public void set(RPG.Goods item, int max, int price)
        {
            this.item = item;
            this.max = max;
            this.price = price;
            this.number = 1;
            refresh();
        }
        //--------------------------------------------------------------------------
        // ● 被输入的件数设置
        //--------------------------------------------------------------------------
        public int number
        {
            get
            {
                return this._number;
            }
            set { this._number = value; }
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            draw_item_name(this.item, 4, 96);
            this.contents.font.color = normal_color;
            this.contents.draw_text(272, 96, 32, 32, "×");
            this.contents.draw_text(308, 96, 24, 32, this.number.ToString(), 2);
            this.cursor_rect = new Rect(304, 96, 32, 32);
            // 描绘合计价格和货币单位
            var domination = Global.data_system.words.gold;
            var cx = (int)contents.text_size(domination).Width;
            var total_price = this.price * this.number;
            this.contents.font.color = normal_color;
            this.contents.draw_text(4, 160, 328 - cx - 2, 32, total_price.ToString(), 2);
            this.contents.font.color = system_color;
            this.contents.draw_text(332 - cx, 160, cx, 32, domination, 2);
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            base.update();
            if (this.active)
            {
                // 光标右 (+1)
                if (Input.is_repeat(Input.RIGHT) && this.number < this.max)
                {
                    Global.game_system.se_play(Global.data_system.cursor_se);
                    this.number += 1;
                    refresh();
                }
                // 光标左 (-1)
                if (Input.is_repeat(Input.LEFT) && this.number > 1)
                {
                    Global.game_system.se_play(Global.data_system.cursor_se);
                    this.number -= 1;
                    refresh();
                }
                // 光标上 (+10)
                if (Input.is_repeat(Input.UP) && this.number < this.max)
                {
                    Global.game_system.se_play(Global.data_system.cursor_se);
                    this.number = Math.Min(this.number + 10, this.max);
                    refresh();
                }
                // 光标下 (-10)
                if (Input.is_repeat(Input.DOWN) && this.number > 1)
                {
                    Global.game_system.se_play(Global.data_system.cursor_se);
                    this.number = Math.Max(this.number - 10, 1);
                    refresh();
                }
            }
        }


        public RPG.Goods item { get; set; }
        public int max { get; set; }
        public int price { get; set; }
        private int _number;
    }
}
