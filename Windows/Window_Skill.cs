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
    // ■ Window_Skill
    //------------------------------------------------------------------------------
    // 　特技画面、战斗画面、显示可以使用的特技浏览的窗口。
    //==============================================================================

    public class Window_Skill : Window_Selectable
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     actor : 角色
        //--------------------------------------------------------------------------
        public Window_Skill(Game_Actor actor) :
            base(0, 128, 640, 352)
        {
            this.actor = actor;
            this.column_max = 2;
            refresh();
            this.index = 0;
            // 战斗中的情况下将窗口移至中央并将其半透明化
            if (Global.game_temp.in_battle)
            {
                this.y = 64;
                this.height = 256;
                this.back_opacity = 160;
            }
            this.active = true;
        }
        //--------------------------------------------------------------------------
        // ● 获取特技
        //--------------------------------------------------------------------------
        public RPG.Skill skill
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

            this.data = new List<RPG.Skill>();
            for (var i = 0; i < this.actor.skills.Count; i++)
            {
                var skill = Global.data_skills[this.actor.skills[i]];
                if (skill != null)
                    this.data.Add(skill);
            }
            // 如果项目数不是 0 就生成位图、重新描绘全部项目
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
            var skill = this.data[index];
            if (this.actor.is_skill_can_use(skill.id))
                this.contents.font.color = normal_color;
            else
                this.contents.font.color = disabled_color;

            var x = 4 + index % 2 * (288 + 32);
            var y = index / 2 * 32;
            var rect = new Rect(x, y, this.width / this.column_max - 32, 32);
            this.contents.fill_rect(rect, Colors.Transparent);
            var bitmap = RPG.Cache.icon(skill.icon_name);
            var opacity = this.contents.font.color == normal_color ? 255 : 128;
            this.contents.blt(x, y + 4, bitmap, new Rect(0, 0, 24, 24), opacity);
            this.contents.draw_text(x + 28, y, 204, 32, skill.name, 0);
            this.contents.draw_text(x + 232, y, 48, 32, skill.sp_cost.ToString(), 2);
        }
        //--------------------------------------------------------------------------
        // ● 刷新帮助文本
        //--------------------------------------------------------------------------
        public override void update_help()
        {
            this.help_window.set_text(this.skill == null ? "" : this.skill.description);
        }


        public Game_Actor actor { get; set; }
        public List<RPG.Skill> data { get; set; }
    }
}
