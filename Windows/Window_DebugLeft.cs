using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Window_DebugLeft
    //------------------------------------------------------------------------------
    // 　调试画面、指定开关及变量块的窗口。
    //==============================================================================

    public class Window_DebugLeft : Window_Selectable
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Window_DebugLeft() :
            base(0, 0, 192, 480)
        {
            this.index = 0;
            refresh();
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

            this.switch_max = (Global.data_system.switches.Count - 1 + 9) / 10;
            this.variable_max = (Global.data_system.variables.Count - 1 + 9) / 10;
            this.item_max = this.switch_max + this.variable_max;
            this.contents = new Bitmap(width - 32, this.item_max * 32);
            for (var i = 0; i < this.switch_max; i++)
            {
                // %04d
                var text = string.Format("S [{0:D04}-{1:D04}]", i * 10 + 1, i * 10 + 10);
                this.contents.draw_text(4, i * 32, 152, 32, text);
            }
            for (var i = 0; i < this.variable_max; i++)
            {
                var text = string.Format("S [{0:D04}-{1:D04}]", i * 10 + 1, i * 10 + 10);
                this.contents.draw_text(4, (this.switch_max + i) * 32, 152, 32, text);
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取模式
        //--------------------------------------------------------------------------
        public int mode
        {
            get
            {
                if (this.index < this.switch_max)
                    return 0;
                else
                    return 1;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取开头显示的 ID
        //--------------------------------------------------------------------------
        public int top_id
        {
            get
            {
                if (this.index < this.switch_max)
                    return this.index * 10 + 1;
                else
                    return (this.index - this.switch_max) * 10 + 1;
            }
        }


        public int switch_max { get; set; }
        public int variable_max { get; set; }
    }
}
