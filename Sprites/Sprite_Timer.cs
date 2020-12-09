using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Sprite_Timer
    //------------------------------------------------------------------------------
    // 　显示计时器用的活动块。监视 Global.game_system 、活动块状态
    // 自动变化。
    //==============================================================================

    public class Sprite_Timer : XP.Internal.Sprite
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Sprite_Timer()
        {
            this.bitmap = new Bitmap(88, 48);
            this.bitmap.font.name = "Arial";
            this.bitmap.font.size = 32;
            this.x = 640 - this.bitmap.width;
            this.y = 0;
            this.z = 500;
            update();
        }
        //--------------------------------------------------------------------------
        // ● 释放
        //--------------------------------------------------------------------------
        public override void dispose()
        {
            if (this.bitmap != null)
                this.bitmap.dispose();
            base.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            base.update();
            // 设置计时器执行中为可见
            this.visible = Global.game_system.timer_working;
            // 如果有必要再次描绘计时器
            if (Global.game_system.timer / Graphics.frame_rate != this.total_sec)
            {
                // 清除窗口内容
                this.bitmap.clear();
                // 计算总计秒数
                this.total_sec = Global.game_system.timer / Graphics.frame_rate;
                // 生成计时器显示用字符串
                var min = this.total_sec / 60;
                var sec = this.total_sec % 60;
                var text = string.Format("%02d:%02d", min, sec);
                // 描绘计时器
                this.bitmap.font.color = Colors.White;
                this.bitmap.draw_text(this.bitmap.rect, text, 1);
            }
        }

        public int total_sec { get; set; }
    }

}
