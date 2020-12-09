using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Internal;

namespace XP
{
    //==============================================================================
// ■ Window_DebugRight
//------------------------------------------------------------------------------
// 　调试画面、个别显示开关及变量的窗口。
//==============================================================================

    public class Window_DebugRight : Window_Selectable
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public int _mode;                    // 模式 (0:开关、1:变量)
        public int _top_id;                   // 开头显示的 ID
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Window_DebugRight() :
            base(192, 0, 448, 352)
        {
            this.contents = new Bitmap(width - 32, height - 32);
            this.index = -1;
            this.active = false;
            this.item_max = 10;
            this.mode = 0;
            this.top_id = 1;
            refresh();
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            string name;
            string status;
            for (var i = 0; i <= 9; i++)
            {
                if (this.mode == 0)
                {
                    name = Global.data_system.switches[this.top_id + i];
                    status = Global.game_switches[this.top_id + i] ? "[ON]" : "[OFF]";
                }
                else
                {
                    name = Global.data_system.variables[this.top_id + i];
                    status = Global.game_variables[this.top_id + i].ToString();
                }
                if (name == null)
                    name = "";

                var id_text = string.Format("{0:D04}:", this.top_id + i);
                var width = (int)this.contents.text_size(id_text).Width;
                this.contents.draw_text(4, i * 32, width, 32, id_text);
                this.contents.draw_text(12 + width, i * 32, 296 - width, 32, name);
                this.contents.draw_text(312, i * 32, 100, 32, status, 2);
            }
        }
        //--------------------------------------------------------------------------
        // ● 设置模式
        //     id : 新的模式
        //--------------------------------------------------------------------------
        public int mode
        {
            get { return this._mode; }
            set
            {
                if (this._mode != value)
                {
                    this._mode = value;
                    refresh();
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 设置开头显示的 ID
        //     id : 新的 ID
        //--------------------------------------------------------------------------
        public int top_id
        {
            get { return this._top_id; }
            set
            {
                if (this._top_id != value)
                {
                    this.top_id = value;
                    refresh();
                }
            }
        }
    }
}
