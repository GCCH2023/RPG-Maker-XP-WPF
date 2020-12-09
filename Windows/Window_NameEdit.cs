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
    // ■ Window_NameEdit
    //------------------------------------------------------------------------------
    // 　名称输入画面、编辑名称的窗口。
    //==============================================================================

    public class Window_NameEdit : Window_Base
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public string name;                  // 名称
        public int index;                   // 光标位置
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     actor    : 角色
        //     max_char : 最大字数
        //--------------------------------------------------------------------------
        public Window_NameEdit(Game_Actor actor, int max_char) :
            base(0, 0, 640, 128)
        {
            this.contents = new Bitmap(width - 32, height - 32);
            this.actor = actor;
            this.name = actor.name;
            this.max_char = max_char;
            // 控制名字在最大字数以内
            var name_array = this.name.Substring(0, this.max_char);
            this.name = "";
            for (var i = 0; i < name_array.Length; i++)
                this.name += name_array[i];

            this.default_name = this.name;
            this.index = name_array.Length;
            refresh();
            update_cursor_rect();
        }
        //--------------------------------------------------------------------------
        // ● 还原为默认的名称
        //--------------------------------------------------------------------------
        public void restore_default()
        {
            this.name = this.default_name;
            this.index = this.name.Length;
            refresh();
            update_cursor_rect();
        }
        //--------------------------------------------------------------------------
        // ● 添加文字
        //     character : 要添加的文字
        //--------------------------------------------------------------------------
        public void add(string character)
        {
            if (this.index < this.max_char && character != "")
            {
                this.name += character;
                this.index += 1;
                refresh();
                update_cursor_rect();
            }
        }
        //--------------------------------------------------------------------------
        // ● 删除文字
        //--------------------------------------------------------------------------
        public void back()
        {
            if (this.index > 0)
            {
                // 删除一个字
                var name_array = this.name;
                this.name = "";
                for (var i = 0; i < name_array.Length - 1; i++)
                    this.name += name_array[i];
                this.index -= 1;
                refresh();
                update_cursor_rect();
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            // 描绘名称
            var name_array = this.name;
            for (var i = 0; i < this.max_char; i++)
            {
                var c = name_array[i];
                //if( c == null)
                //  c = "＿";

                var x = 320 - this.max_char * 14 + i * 28;
                this.contents.draw_text(x, 32, 28, 32, c.ToString(), 1);
            }
            // 描绘图形
            draw_actor_graphic(this.actor, 320 - this.max_char * 14 - 40, 80);
        }
        //--------------------------------------------------------------------------
        // ● 刷新光标矩形
        //--------------------------------------------------------------------------
        public override void update_cursor_rect()
        {
            var x = 320 - this.max_char * 14 + this.index * 28;
            this.cursor_rect = new Rect(x, 32, 28, 32);
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            base.update();
            update_cursor_rect();
        }


        public Game_Actor actor { get; set; }
        public int max_char { get; set; }
        public string default_name { get; set; }
    }
}
