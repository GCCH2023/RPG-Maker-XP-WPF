using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Window_Help
    //------------------------------------------------------------------------------
    // 　特技及物品的说明、角色的状态显示的窗口。
    //==============================================================================

    public class Window_Help : Window_Base
    {
        public Game_Actor actor;
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Window_Help()
            : base(0, 0, 640, 64)
        {
            this.contents = new Bitmap(width - 32, height - 32);
        }
        //--------------------------------------------------------------------------
        // ● 设置文本
        //     text  : 窗口显示的字符串
        //     align : 对齐方式 (0..左对齐、1..中间对齐、2..右对齐)
        //--------------------------------------------------------------------------
        public void set_text(string text, int align = 0)
        {
            // 如果文本和对齐方式的至少一方与上次的不同
            if (text != this.text || align != this.align)
            {
                // 再描绘文本
                this.contents.clear();
                this.contents.font.color = normal_color;
                this.contents.draw_text(4, 0, this.width - 40, 32, text, align);
                this.text = text;
                this.align = align;
                this.actor = null;
            }
            this.visible = true;
        }
        //--------------------------------------------------------------------------
        // ● 设置角色
        //     actor : 要显示状态的角色
        //--------------------------------------------------------------------------
        public void set_actor(Game_Actor actor)
        {
            if (actor != this.actor)
            {
                this.contents.clear();
                draw_actor_name(actor, 4, 0);
                draw_actor_state(actor, 140, 0);
                draw_actor_hp(actor, 284, 0);
                draw_actor_sp(actor, 460, 0);
                this.actor = actor;
                this.text = null;
                this.visible = true;
            }
        }
        //--------------------------------------------------------------------------
        // ● 设置敌人
        //     enemy : 要显示名字和状态的敌人
        //--------------------------------------------------------------------------
        public void set_enemy(Game_Enemy enemy)
        {
            var text = enemy.name;
            var state_text = make_battler_state_text(enemy, 112, false);
            if (state_text != "")
                text += "  " + state_text;

            set_text(text, 1);
        }


        public string text { get; set; }

        public int align { get; set; }
    }
}
