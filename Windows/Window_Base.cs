using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace XP
{
    //==============================================================================
    // ■ Window_Base
    //------------------------------------------------------------------------------
    // 　游戏中全部窗口的超级类。
    //==============================================================================

    public class Window_Base : XP.Internal.Window
    {
        public string windowskin_name;
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     x      : 窗口的 X 坐标
        //     y      : 窗口的 Y 坐标
        //     width  : 窗口的宽
        //     height : 窗口的宽
        //--------------------------------------------------------------------------
        public Window_Base(int x, int y, int width, int height)
        {
            this.windowskin_name = Global.game_system.windowskin_name;
            this.windowskin = RPG.Cache.windowskin(this.windowskin_name);
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.z = 100;
        }

        public virtual void update_cursor_rect()
        {

        }

        public void SetRect(int x, int y, int width, int heght)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
        //--------------------------------------------------------------------------
        // ● 释放
        //--------------------------------------------------------------------------
        public override void dispose()
        {
            // 如果窗口的内容已经被设置就被释放
            if (this.contents != null)
                this.contents.dispose();
            base.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 获取文字色
        //     n : 文字色编号 (0～7)
        //--------------------------------------------------------------------------
        public Color text_color(int n)
        {
            switch (n)
            {
                case 0: return Colors.White;
                case 1:return Color.FromRgb(128, 128, 255);
                case 2:return Color.FromRgb(255, 128, 128);
                case 3:return Color.FromRgb(128, 255, 128);
                case 4:return Color.FromRgb(128, 255, 255);
                case 5:return Color.FromRgb(255, 128, 255);
                case 6:return Color.FromRgb(255, 255, 128);
                case 7:return Color.FromRgb(192, 192, 192);
                default:return normal_color;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取普通文字色
        //--------------------------------------------------------------------------
        public Color normal_color
        {
            get
            {
                return Colors.White;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取无效文字色
        //--------------------------------------------------------------------------
        public Color disabled_color
        {
            get
            {
                // return Color.FromArgb(128, 255, 255, 255);
                return Colors.Gray;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取系统文字色
        //--------------------------------------------------------------------------
        public Color system_color
        {
            get { return Color.FromRgb(192, 224, 255); }
        }

        //--------------------------------------------------------------------------
        // ● 获取危机文字色
        //--------------------------------------------------------------------------
        public Color crisis_color
        {
            get { return Color.FromRgb(255, 255, 64); }
        }

        //--------------------------------------------------------------------------
        // ● 获取战斗不能文字色
        //--------------------------------------------------------------------------
        public Color knockout_color
        {
            get { return Color.FromRgb(255, 64, 0); }
        }

        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            base.update();
            // 如果窗口的外观被变更了、再设置
            if (Global.game_system.windowskin_name != this.windowskin_name)
            {
                this.windowskin_name = Global.game_system.windowskin_name;
                this.windowskin = RPG.Cache.windowskin(this.windowskin_name);
            }
        }
        //--------------------------------------------------------------------------
        // ● 图形的描绘
        //     actor : 角色
        //     x     : 描画目标 X 坐标
        //     y     : 描画目标 Y 坐标
        //--------------------------------------------------------------------------
        public void draw_actor_graphic(Game_Actor actor, int x, int y)
        {
            var bitmap = RPG.Cache.character(actor.character_name, actor.character_hue);
            var cw = bitmap.width / 4;
            var ch = bitmap.height / 4;
            var src_rect = new Rect(0, 0, cw, ch);
            this.contents.blt(x - cw / 2, y - ch, bitmap, src_rect);
        }
        //--------------------------------------------------------------------------
        // ● 名称的描绘
        //     actor : 角色
        //     x     : 描画目标 X 坐标
        //     y     : 描画目标 Y 坐标
        //--------------------------------------------------------------------------
        public void draw_actor_name(Game_Actor actor, int x, int y)
        {
            this.contents.font.color = normal_color;
            this.contents.draw_text(x, y, 120, 32, actor.name);
        }

        //--------------------------------------------------------------------------
        // ● 职业的描绘
        //     actor : 角色
        //     x     : 描画目标 X 坐标
        //     y     : 描画目标 Y 坐标
        //--------------------------------------------------------------------------
        public void draw_actor_class(Game_Actor actor, int x, int y)
        {
            this.contents.font.color = normal_color;
            this.contents.draw_text(x, y, 236, 32, actor.class_name);
        }
        //--------------------------------------------------------------------------
        // ● 等级的描绘
        //     actor : 角色
        //     x     : 描画目标 X 坐标
        //     y     : 描画目标 Y 坐标
        //--------------------------------------------------------------------------
        public void draw_actor_level(Game_Actor actor, int x, int y)
        {
            this.contents.font.color = system_color;
            this.contents.draw_text(x, y, 32, 32, "Lv");
            this.contents.font.color = normal_color;
            this.contents.draw_text(x + 32, y, 24, 32, actor.level.ToString(), 2);
        }
        //--------------------------------------------------------------------------
        // ● 生成描绘用的状态字符串
        //     actor       : 角色
        //     width       : 描画目标的宽度
        //     need_normal : [正常] 是否为必须 (true / false)
        //--------------------------------------------------------------------------
        public string make_battler_state_text(Game_Battler battler, int width, bool need_normal)
        {
            // 获取括号的宽
            var brackets_width = (int)this.contents.text_size("[]").Width;
            // 生成状态名字符串
            var text = "";
            foreach (var i in battler.states)
            {
                if (Global.data_states[i].rating >= 1)
                {
                    if (text == "")
                        text = Global.data_states[i].name;
                    else
                    {
                        var new_text = text + "/" + Global.data_states[i].name;
                        var text_width = (int)this.contents.text_size(new_text).Width;
                        if (text_width > width - brackets_width)
                            break;
                        text = new_text;
                    }
                }
            }
            // 状态名空的字符串是 "[正常]" 的情况下
            if (text == "")
            {
                if (need_normal)
                    text = "[正常]";
            }
            else
            {
                // 加上括号
                text = "[" + text + "]";
            }
            // 返回完成后的文字类
            return text;
        }
        //--------------------------------------------------------------------------
        // ● 描绘状态
        //     actor : 角色
        //     x     : 描画目标 X 坐标
        //     y     : 描画目标 Y 坐标
        //     width : 描画目标的宽
        //--------------------------------------------------------------------------
        public void draw_actor_state(Game_Actor actor, int x, int y, int width = 120)
        {
            var text = make_battler_state_text(actor, width, true);
            this.contents.font.color = actor.hp == 0 ? knockout_color : normal_color;
            this.contents.draw_text(x, y, width, 32, text);
        }
        //--------------------------------------------------------------------------
        // ● 描绘 EXP
        //     actor : 角色
        //     x     : 描画目标 X 坐标
        //     y     : 描画目标 Y 坐标
        //--------------------------------------------------------------------------
        public void draw_actor_exp(Game_Actor actor, int x, int y)
        {
            this.contents.font.color = system_color;
            this.contents.draw_text(x, y, 24, 32, "E");
            this.contents.font.color = normal_color;
            this.contents.draw_text(x + 24, y, 84, 32, actor.exp_s, 2);
            this.contents.draw_text(x + 108, y, 12, 32, "/", 1);
            this.contents.draw_text(x + 120, y, 84, 32, actor.next_exp_s);
        }
        //--------------------------------------------------------------------------
        // ● 描绘 HP
        //     actor : 角色
        //     x     : 描画目标 X 坐标
        //     y     : 描画目标 Y 坐标
        //     width : 描画目标的宽
        //--------------------------------------------------------------------------
        public void draw_actor_hp(Game_Actor actor, int x, int y, int width = 144)
        {
            // 描绘字符串 "HP"
            this.contents.font.color = system_color;
            this.contents.draw_text(x, y, 32, 32, Global.data_system.words.hp);
            // 计算描绘 MaxHP 所需的空间 
            int hp_x = 0;
            bool flag = false;
            if (width - 32 >= 108)
            {
                hp_x = x + width - 108;
                flag = true;
            }
            else if (width - 32 >= 48)
            {
                hp_x = x + width - 48;
                flag = false;
            }
            // 描绘 HP
            this.contents.font.color = actor.hp == 0 ? knockout_color :
              actor.hp <= actor.maxhp / 4 ? crisis_color : normal_color;
            this.contents.draw_text(hp_x, y, 48, 32, actor.hp.ToString(), 2);
            // 描绘 MaxHP
            if (flag)
            {
                this.contents.font.color = normal_color;
                this.contents.draw_text(hp_x + 48, y, 12, 32, "/", 1);
                this.contents.draw_text(hp_x + 60, y, 48, 32, actor.maxhp.ToString());
            }
        }
        //--------------------------------------------------------------------------
        // ● 描绘 SP
        //     actor : 角色
        //     x     : 描画目标 X 坐标
        //     y     : 描画目标 Y 坐标
        //     width : 描画目标的宽
        //--------------------------------------------------------------------------
        public void draw_actor_sp(Game_Actor actor, int x, int y, int width = 144)
        {
            // 描绘字符串 "SP" 
            this.contents.font.color = system_color;
            this.contents.draw_text(x, y, 32, 32, Global.data_system.words.sp);
            // 计算描绘 MaxSP 所需的空间
            int sp_x = 0;
            bool flag = false;
            if (width - 32 >= 108)
            {
                sp_x = x + width - 108;
                flag = true;
            }
            else if (width - 32 >= 48)
            {
                sp_x = x + width - 48;
                flag = false;
            }
            // 描绘 SP
            this.contents.font.color = actor.sp == 0 ? knockout_color :
              actor.sp <= actor.maxsp / 4 ? crisis_color : normal_color;
            this.contents.draw_text(sp_x, y, 48, 32, actor.sp.ToString(), 2);
            // 描绘 MaxSP
            if (flag)
            {
                this.contents.font.color = normal_color;
                this.contents.draw_text(sp_x + 48, y, 12, 32, "/", 1);
                this.contents.draw_text(sp_x + 60, y, 48, 32, actor.maxsp.ToString());
            }
        }
        //--------------------------------------------------------------------------
        // ● 描绘能力值
        //     actor : 角色
        //     x     : 描画目标 X 坐标
        //     y     : 描画目标 Y 坐标
        //     type  : 能力值种类 (0～6)
        //--------------------------------------------------------------------------
        public void draw_actor_parameter(Game_Actor actor, int x, int y, int type)
        {
            string parameter_name = "";
            double parameter_value = 0;
            switch (type)
            {
                case 0:
                    parameter_name = Global.data_system.words.atk;
                    parameter_value = actor.atk;
                    break;
                case 1:
                    parameter_name = Global.data_system.words.pdef;
                    parameter_value = actor.pdef;
                    break;

                case 2:
                    parameter_name = Global.data_system.words.mdef;
                    parameter_value = actor.mdef; break;

                case 3:
                    parameter_name = Global.data_system.words.str;
                    parameter_value = actor.str; break;

                case 4:
                    parameter_name = Global.data_system.words.dex;
                    parameter_value = actor.dex; break;

                case 5:
                    parameter_name = Global.data_system.words.agi;
                    parameter_value = actor.agi; break;

                case 6:
                    parameter_name = Global.data_system.words.int1;
                    parameter_value = actor.int1; break;
            }
            this.contents.font.color = system_color;
            this.contents.draw_text(x, y, 120, 32, parameter_name);
            this.contents.font.color = normal_color;
            this.contents.draw_text(x + 120, y, 36, 32, parameter_value.ToString(), 2);
        }
        //--------------------------------------------------------------------------
        // ● 描绘物品名
        //     item : 物品
        //     x    : 描画目标 X 坐标
        //     y    : 描画目标 Y 坐标
        //--------------------------------------------------------------------------
        public void draw_item_name(RPG.Goods item, int x, int y)
        {
            if (item == null)
                return;
            var bitmap = RPG.Cache.icon(item.icon_name);
            this.contents.blt(x, y + 4, bitmap, new Rect(0, 0, 24, 24));
            this.contents.font.color = normal_color;
            this.contents.draw_text(x + 28, y, 212, 32, item.name);
        }
    }
}
