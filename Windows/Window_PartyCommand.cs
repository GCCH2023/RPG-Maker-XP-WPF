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
    // ■ Window_PartyCommand
    //------------------------------------------------------------------------------
    // 　战斗画面、选择战斗与逃跑的窗口。
    //==============================================================================

    public class Window_PartyCommand : Window_Selectable
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Window_PartyCommand() :
            base(0, 0, 640, 64)
        {
            this.contents = new Bitmap(width - 32, height - 32);
            this.back_opacity = 160;
            this.commands = new string[] { "战斗", "逃跑" };
            this.item_max = 2;
            this.column_max = 2;
            draw_item(0, normal_color);
            draw_item(1, Global.game_temp.battle_can_escape ? normal_color : disabled_color);
            this.active = false;
            this.visible = false;
            this.index = 0;
        }
        //--------------------------------------------------------------------------
        // ● 描绘项目
        //     index : 项目标号
        //     color : 文字颜色
        //--------------------------------------------------------------------------
        public void draw_item(int index, Color color)
        {
            this.contents.font.color = color;
            var rect = new Rect(160 + index * 160 + 4, 0, 128 - 10, 32);
            this.contents.fill_rect(rect, Colors.Transparent);
            this.contents.draw_text(rect, this.commands[index], 1);
        }
        //--------------------------------------------------------------------------
        // ● 更新光标矩形
        //--------------------------------------------------------------------------
        public override void update_cursor_rect()
        {
            this.cursor_rect = new Rect(160 + index * 160, 0, 128, 32);
        }


        public string[] commands { get; set; }
    }
}
