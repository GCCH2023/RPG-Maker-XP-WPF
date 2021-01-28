using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XP
{
    //==============================================================================
    // ■ Arrow_Base
    //------------------------------------------------------------------------------
    // 　在战斗画面使用的箭头光标的活动块。本类作为
    // Arrow_Enemy 类与 Arrow_Actor 类的超级类使用。
    //==============================================================================

    public class Arrow_Base : XP.Internal.Sprite
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public int _index;                   // 光标位置
        public Window_Help _help_window;              // 帮助窗口
        public int blink_count;
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     viewport : 显示端口
        //--------------------------------------------------------------------------
        public Arrow_Base(Viewport viewport)
            : base(viewport)
        {
            this.bitmap = RPG.Cache.windowskin(Global.game_system.windowskin_name);
            this.ox = 16;
            this.oy = 64;
            this.z = 2500;
            this.blink_count = 0;
            this.index = 0;
            this.help_window = null;
            update();
        }
        //--------------------------------------------------------------------------
        // ● 设置光标位置
        //     index : 新的光标位置
        //--------------------------------------------------------------------------
        public virtual int index
        {
            set
            {
                this._index = value;
                //update();
            }
            get { return this._index; }
        }
        //--------------------------------------------------------------------------
        // ● 设置帮助窗口
        //     help_window : 新的帮助窗口
        //--------------------------------------------------------------------------
        public Window_Help help_window
        {
            set
            {
                this._help_window = value;
                // 刷新帮助文本 (update_help 定义了继承目标)
                if (this.help_window != null)
                    update_help();
            }
            get { return this._help_window; }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 刷新闪烁记数
            this.blink_count = (this.blink_count + 1) % 8;
            // 设置传送源矩形
            if (this.blink_count < 4)
                this.src_rect = new Rect(128, 96, 32, 32);
            else
                this.src_rect = new Rect(160, 96, 32, 32);

            // 刷新帮助文本 (update_help 定义了继承目标)
            if (this.help_window != null)
                update_help();
        }
        public virtual void update_help()
        {

        }
    }
}
