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
    // ■ Window_InputNumber
    //------------------------------------------------------------------------------
    // 　信息窗口内部使用、输入数值的窗口。
    //==============================================================================

    public class Window_InputNumber : Window_Base
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     digits_max : 位数
        //--------------------------------------------------------------------------
        public Window_InputNumber(int digits_max)
            : base(0, 0, 0, 0)
        {
            this.digits_max = digits_max;
            this.number = 0;
            // 从数字的幅度计算(假定与 0～9 等幅)光标的幅度
            var dummy_bitmap = new Bitmap(32, 32);
            this.cursor_width = (int)dummy_bitmap.text_size("0").Width + 8;
            base.SetRect(0, 0, this.cursor_width * this.digits_max + 32, 64);
            dummy_bitmap.dispose();
            this.contents = new Bitmap(width - 32, height - 32);
            this.z += 9999;
            this.opacity = 0;
            this.index = 0;
            refresh();
            update_cursor_rect();
        }
        //--------------------------------------------------------------------------
        // ● 取得数值
        //--------------------------------------------------------------------------
        //--------------------------------------------------------------------------
        // ● 设置数值
        //     number : 新的数值
        //--------------------------------------------------------------------------
        int _number;

        public int number
        {
            get { return _number; }
            set
            {
                _number = Math.Min(Math.Max(number, 0), (int)Math.Pow(10, this.digits_max - 1));
                refresh();
            }
        }
        //--------------------------------------------------------------------------
        // ● 更新光标矩形
        //--------------------------------------------------------------------------
        public override void update_cursor_rect()
        {
            this.cursor_rect = new Rect(this.index * this.cursor_width, 0, this.cursor_width, 32);
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            base.update();
            // 按下方向键上与下的情况下
            if (Input.is_repeat(Input.UP) || Input.is_repeat(Input.DOWN))
            {
                Global.game_system.se_play(Global.data_system.cursor_se);
                // 取得现在位置的数字位数
                var place = (int)Math.Pow(10, (this.digits_max - 1 - this.index));
                var n = this.number / place % 10;
                this.number -= n * place;
                // 上为 +1、下为 -1
                if (Input.is_repeat(Input.UP))
                    n = (n + 1) % 10;
                if (Input.is_repeat(Input.DOWN))
                    n = (n + 9) % 10;
                // 再次设置现在位的数字
                this.number += n * place;
                refresh();
            }
            // 光标右
            if (Input.is_repeat(Input.RIGHT))
            {
                if (this.digits_max >= 2)
                {
                    Global.game_system.se_play(Global.data_system.cursor_se);
                    this.index = (this.index + 1) % this.digits_max;
                }
            }
            // 光标左
            if (Input.is_repeat(Input.LEFT))
            {
                if (this.digits_max >= 2)
                {
                    Global.game_system.se_play(Global.data_system.cursor_se);
                    this.index = (this.index + this.digits_max - 1) % this.digits_max;
                }
            }

            update_cursor_rect();
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            this.contents.font.color = normal_color;
            var s = string.Format("%0*d", this.digits_max, this.number);
            for (var i = 0; i < this.digits_max; i++)
                this.contents.draw_text(i * this.cursor_width + 4, 0, 32, 32, s.Substring(i, 1));
        }


        public int digits_max { get; set; }
        public int index { get; set; }
        public int cursor_width { get; set; }
    }
}
