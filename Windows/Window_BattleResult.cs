using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Window_BattleResult
    //------------------------------------------------------------------------------
    // 　战斗结束时、显示获得的 EXP 及金钱的窗口。
    //==============================================================================

    public class Window_BattleResult : Window_Base
    {
        private int exp = 0;
        private int gold = 0;
        private List<RPG.Goods> treasures;
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     exp       : EXP
        //     gold      : 金钱
        //     treasures : 宝物
        //--------------------------------------------------------------------------
        public Window_BattleResult(int exp, int gold, List<RPG.Goods> treasures) :
            base(160, 0, 320, treasures.Count * 32 + 64)
        {
            this.exp = exp;
            this.gold = gold;
            this.treasures = treasures;
            this.contents = new Bitmap(width - 32, height - 32);
            this.y = 160 - height / 2;
            this.back_opacity = 160;
            this.visible = false;
            refresh();
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            var x = 4;
            this.contents.font.color = normal_color;
            var cx = (int)contents.text_size(this.exp.ToString()).Width;
            this.contents.draw_text(x, 0, cx, 32, this.exp.ToString());
            x += cx + 4;
            this.contents.font.color = system_color;
            cx = (int)contents.text_size("EXP").Width;
            this.contents.draw_text(x, 0, 64, 32, "EXP");
            x += cx + 16;
            this.contents.font.color = normal_color;
            cx = (int)contents.text_size(this.gold.ToString()).Width;
            this.contents.draw_text(x, 0, cx, 32, this.gold.ToString());
            x += cx + 4;
            this.contents.font.color = system_color;
            this.contents.draw_text(x, 0, 128, 32, Global.data_system.words.gold);
            var y = 32;
            foreach (var item in this.treasures)
            {
                draw_item_name(item, 4, y);
                y += 32;
            }
        }
    }

}
