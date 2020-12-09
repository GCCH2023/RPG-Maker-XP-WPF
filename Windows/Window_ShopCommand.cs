using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Window_ShopCommand
    //------------------------------------------------------------------------------
    // 　商店画面、选择要做的事的窗口
    //==============================================================================

    public class Window_ShopCommand : Window_Selectable
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Window_ShopCommand()
            : base(0, 64, 480, 64)
        {
            this.contents = new Bitmap(width - 32, height - 32);
            this.item_max = 3;
            this.column_max = 3;
            this.commands = new string[] { "买", "卖", "取消" };
            refresh();
            this.index = 0;
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            for (var i = 0; i < this.item_max; i++)
                draw_item(i);
        }
        //--------------------------------------------------------------------------
        // ● 描绘项目
        //     index : 项目编号
        //--------------------------------------------------------------------------
        public void draw_item(int index)
        {
            var x = 4 + index * 160;
            this.contents.draw_text(x, 0, 128, 32, this.commands[index]);
        }

        public string[] commands { get; set; }
    }

}
