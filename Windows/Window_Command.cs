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
    // ■ Window_Command
    //------------------------------------------------------------------------------
    // 　一般的命令选择行窗口。
    //==============================================================================

    public class Window_Command : Window_Selectable
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     width    : 窗口的宽
        //     commands : 命令字符串序列
        //--------------------------------------------------------------------------
        public Window_Command(int width, string[] commands)
            : base(0, 0, width, commands.Length * 32 + 32)
        {
            // 由命令的个数计算出窗口的高
            this.item_max = commands.Length;
            this.commands = commands;
            this.contents = new Bitmap(width - 32, this.item_max * 32);
            refresh();
            this.index = 0;
            this.active = true;
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            for (var i = 0; i < this.item_max; i++)
                draw_item(i, normal_color);
        }
        //--------------------------------------------------------------------------
        // ● 描绘项目
        //     index : 项目编号
        //     color : 文字色
        //--------------------------------------------------------------------------
        public void draw_item(int index, Color color)
        {
            this.contents.font.color = color;
            var rect = new Rect(4, 32 * index, this.contents.width - 8, 32);
            //this.contents.fill_rect(rect, new Color(0, 0, 0, 0));
            this.contents.draw_text(rect, this.commands[index]);
        }
        //--------------------------------------------------------------------------
        // ● 项目无效化
        //     index : 项目编号
        //--------------------------------------------------------------------------
        public void disable_item(int index)
        {
            draw_item(index, disabled_color);
        }

        public string[] commands { get; set; }
    }
}
