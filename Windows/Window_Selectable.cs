using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace XP
{
    //==============================================================================
    // ■ Window_Selectable
    //------------------------------------------------------------------------------
    // 　拥有光标的移动以及滚动功能的窗口类。
    //==============================================================================

    public class Window_Selectable : Window_Base
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public Window_Help _help_window;             // 帮助窗口
        //--------------------------------------------------------------------------
        // ● 初始画对像
        //     x      : 窗口的 X 坐标
        //     y      : 窗口的 Y 坐标
        //     width  : 窗口的宽
        //     height : 窗口的高
        //--------------------------------------------------------------------------
        public Window_Selectable(int x, int y, int width, int height)
            : base(x, y, width, height)
        {
            this.item_max = 1;
            this.column_max = 1;
            this.index = -1;

            Global.PreviewKeyDown += OnKeyDown;
        }


        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // 可以移动光标的情况下
            if (this.active && this.item_max > 0 && this.index >= 0)
            {
                // 方向键下被按下的情况下
                if (e.Key == Key.Down)
                {
                    // 列数不是 1 并且不与方向键下的按下状态重复的情况、
                    // 或光标位置在(项目数-列数)之前的情况下
                    if ((this.column_max == 1 && Input.is_trigger(Input.DOWN)) ||
                                  this.index < this.item_max - this.column_max)
                    {

                        // 光标向下移动
                        Global.game_system.se_play(Global.data_system.cursor_se);
                        this.index = (this.index + this.column_max) % this.item_max;
                    }
                }
                // 方向键上被按下的情况下
                if (e.Key == Key.Up)
                {
                    // 列数不是 1 并且不与方向键下的按下状态重复的情况、
                    // 或光标位置在列之后的情况下
                    if ((this.column_max == 1 && Input.is_trigger(Input.UP)) ||
                                  this.index >= this.column_max)
                    // 光标向上移动
                    {
                        Global.game_system.se_play(Global.data_system.cursor_se);
                        this.index = (this.index - this.column_max + this.item_max) % this.item_max;
                    }
                }
                // 方向键右被按下的情况下
                if (e.Key == Key.Right)
                {
                    // 列数为 2 以上并且、光标位置在(项目数 - 1)之前的情况下
                    if (this.column_max >= 2 && this.index < this.item_max - 1)
                    {
                        // 光标向右移动
                        Global.game_system.se_play(Global.data_system.cursor_se);
                        this.index += 1;

                    }
                }
                // 方向键左被按下的情况下
                if (e.Key == Key.Left)
                {
                    // 列数为 2 以上并且、光标位置在 0 之后的情况下
                    if (this.column_max >= 2 && this.index > 0)
                    {
                        // 光标向左移动
                        Global.game_system.se_play(Global.data_system.cursor_se);
                        this.index -= 1;
                    }
                }
                // R 键被按下的情况下
                if (e.Key == Key.R)
                {
                    // 显示的最后行在数据中最后行上方的情况下
                    if (this.top_row + (this.page_row_max - 1) < (this.row_max - 1))
                    {
                        // 光标向后移动一页
                        Global.game_system.se_play(Global.data_system.cursor_se);
                        this.index = Math.Min(this.index + this.page_item_max, this.item_max - 1);
                        this.top_row += this.page_row_max;
                    }
                }
                // L 键被按下的情况下
                if (e.Key == Key.L)
                {
                    // 显示的开头行在位置 0 之后的情况下
                    if (this.top_row > 0)
                    {
                        // 光标向前移动一页
                        Global.game_system.se_play(Global.data_system.cursor_se);
                        this.index = Math.Max(this.index - this.page_item_max, 0);
                        this.top_row -= this.page_row_max;
                    }
                }
                // 刷新帮助文本 (update_help 定义了继承目标)
                if (this.active && this.help_window != null)
                    update_help();
                // 刷新光标矩形
                update_cursor_rect();
            }
        }
        //--------------------------------------------------------------------------
        // ● 设置光标的位置
        //     index : 新的光标位置
        //--------------------------------------------------------------------------
        int _index;
        public int index
        {
            set
            {
                this._index = value;
                // 刷新帮助文本 (update_help 定义了继承目标)
                if (this.active && this.help_window != null)
                    update_help();

                // 刷新光标矩形
                update_cursor_rect();
            }
            get { return this._index; }
        }
        //--------------------------------------------------------------------------
        // ● 获取行数
        //--------------------------------------------------------------------------
        public int row_max
        {
            get
            {
                // 由项目数和列数计算出行数
                return (this.item_max + this.column_max - 1) / this.column_max;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取开头行
        //--------------------------------------------------------------------------
        //--------------------------------------------------------------------------
        // ● 设置开头行
        //     row : 显示开头的行
        //--------------------------------------------------------------------------

        public int top_row
        {
            get
            {
                // 将窗口内容的传送源 Y 坐标、1 行的高 32 等分
                return this.oy / 32;
            }
            set
            {
                // row 未满 0 的场合更正为 0
                if (value < 0)
                    value = 0;

                // row 超过 row_max - 1 的情况下更正为 row_max - 1 
                if (value > row_max - 1)
                    value = row_max - 1;

                // row 1 行高的 32 倍、窗口内容的传送源 Y 坐标
                this.oy = value * 32;
            }
        }

        //--------------------------------------------------------------------------
        // ● 获取 1 页可以显示的行数
        //--------------------------------------------------------------------------
        public int page_row_max
        {
            get
            {
                // 窗口的高度，设置画面的高度减去 32 ，除以 1 行的高度 32 
                return (int)(this.height - 32) / 32;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取 1 页可以显示的项目数
        //--------------------------------------------------------------------------
        public int page_item_max
        {
            get
            {
                // 将行数 page_row_max 乘上列数 this.column_max
                return page_row_max * this.column_max;
            }
        }
        //--------------------------------------------------------------------------
        // ● 帮助窗口的设置
        //     help_window : 新的帮助窗口
        //--------------------------------------------------------------------------
        public Window_Help help_window
        {
            set
            {
                this._help_window = value;
                // 刷新帮助文本 (update_help 定义了继承目标)
                if (this.active && this.help_window != null)
                    update_help();
            }
            get
            {
                return this._help_window;
            }
        }
        //--------------------------------------------------------------------------
        // ● 更新光标矩形
        //--------------------------------------------------------------------------
        public override void update_cursor_rect()
        {
            // 光标位置不满 0 的情况下
            if (this.index < 0)
            {
                //this.cursor_rect.empty();
                this.cursor_rect = new Rect();
                return;
            }
            // 获取当前的行
            var row = this.index / this.column_max;
            // 当前行被显示开头行前面的情况下
            if (row < this.top_row)
                // 从当前行向开头行滚动
                this.top_row = row;

            // 当前行被显示末尾行之后的情况下
            if (row > this.top_row + (this.page_row_max - 1))
                // 从当前行向末尾滚动
                this.top_row = row - (this.page_row_max - 1);

            // 计算光标的宽度
            var cursor_width = this.width / this.column_max - 32;
            // 计算光标坐标
            var x = this.index % this.column_max * (cursor_width + 32);
            var y = this.index / this.column_max * 32 - this.oy;
            // 更新光标矩形
            // this.cursor_rect = new Rect(x, y, cursor_width, 32);
            this.cursor_rect = new Rect(x, y, cursor_width, 32);
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            base.update();
            //// 可以移动光标的情况下
            //if (this.active && this.item_max > 0 && this.index >= 0)
            //{
            //    // 方向键下被按下的情况下
            //    if (Input.is_repeat(Input.DOWN))
            //    {
            //        // 列数不是 1 并且不与方向键下的按下状态重复的情况、
            //        // 或光标位置在(项目数-列数)之前的情况下
            //        if ((this.column_max == 1 && Input.is_trigger(Input.DOWN)) ||
            //                      this.index < this.item_max - this.column_max)
            //        {

            //            // 光标向下移动
            //            Global.game_system.se_play(Global.data_system.cursor_se);
            //            this.index = (this.index + this.column_max) % this.item_max;
            //        }
            //    }
            //    // 方向键上被按下的情况下
            //    if (Input.is_repeat(Input.UP))
            //    {
            //        // 列数不是 1 并且不与方向键下的按下状态重复的情况、
            //        // 或光标位置在列之后的情况下
            //        if ((this.column_max == 1 && Input.is_trigger(Input.UP)) ||
            //                      this.index >= this.column_max)
            //        // 光标向上移动
            //        {
            //            Global.game_system.se_play(Global.data_system.cursor_se);
            //            this.index = (this.index - this.column_max + this.item_max) % this.item_max;
            //        }
            //    }
            //    // 方向键右被按下的情况下
            //    if (Input.is_repeat(Input.RIGHT))
            //    {
            //        // 列数为 2 以上并且、光标位置在(项目数 - 1)之前的情况下
            //        if (this.column_max >= 2 && this.index < this.item_max - 1)
            //        {
            //            // 光标向右移动
            //            Global.game_system.se_play(Global.data_system.cursor_se);
            //            this.index += 1;

            //        }
            //    }
            //    // 方向键左被按下的情况下
            //    if (Input.is_repeat(Input.LEFT))
            //    {
            //        // 列数为 2 以上并且、光标位置在 0 之后的情况下
            //        if (this.column_max >= 2 && this.index > 0)
            //        {
            //            // 光标向左移动
            //            Global.game_system.se_play(Global.data_system.cursor_se);
            //            this.index -= 1;
            //        }
            //    }
            //    // R 键被按下的情况下
            //    if (Input.is_repeat(Input.R))
            //    {
            //        // 显示的最后行在数据中最后行上方的情况下
            //        if (this.top_row + (this.page_row_max - 1) < (this.row_max - 1))
            //        {
            //            // 光标向后移动一页
            //            Global.game_system.se_play(Global.data_system.cursor_se);
            //            this.index = Math.Min(this.index + this.page_item_max, this.item_max - 1);
            //            this.top_row += this.page_row_max;
            //        }
            //    }
            //    // L 键被按下的情况下
            //    if (Input.is_repeat(Input.L))
            //    {
            //        // 显示的开头行在位置 0 之后的情况下
            //        if (this.top_row > 0)
            //        {
            //            // 光标向前移动一页
            //            Global.game_system.se_play(Global.data_system.cursor_se);
            //            this.index = Math.Max(this.index - this.page_item_max, 0);
            //            this.top_row -= this.page_row_max;
            //        }
            //    }
            //    // 刷新帮助文本 (update_help 定义了继承目标)
            //    if (this.active && this.help_window != null)
            //        update_help();
            //    // 刷新光标矩形
            //    update_cursor_rect();
            //}
        }
        public int item_max { get; set; }
        public int column_max { get; set; }

        public virtual void update_help()
        {

        }
    }
}
