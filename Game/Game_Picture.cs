using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_Picture
    //------------------------------------------------------------------------------
    // 　处理图片的类。本类在类 Game_Screen (Global.game_screen)
    // 的内部使用。
    //==============================================================================

    public class Game_Picture
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public int number;    // 图片编号
        public string name;         // 文件名
        public int origin = 0;       // 原点
        public double x = 0.0;     // X 坐标
        public double y = 0.0;     // Y 坐标
        public double zoom_x = 100.0;           // X 方向放大率
        public double zoom_y = 100.0;           // Y 方向放大率
        public double opacity = 255.0;         // 不透明度
        public int blend_type = 1;             // 合成方式
        public Tone tone;       // 色调
        public double angle;        // 旋转角度
        public int duration = 0;
        public double target_x;
        public double target_y;
        public double target_zoom_x;
        public double target_zoom_y;
        public double target_opacity;
        public Tone tone_target;
        public int tone_duration;
        public int rotate_speed;

        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     number : 图片编号
        //--------------------------------------------------------------------------
        public Game_Picture(int number)
        {
            this.number = number;
            this.target_x = this.x;
            this.target_y = this.y;
            this.target_zoom_x = this.zoom_x;
            this.target_zoom_y = this.zoom_y;
            this.target_opacity = this.opacity;
            this.tone = new Tone(0, 0, 0, 0);
            this.tone_target = new Tone(0, 0, 0, 0);
            this.tone_duration = 0;
            this.angle = 0;
            this.rotate_speed = 0;
        }
        //--------------------------------------------------------------------------
        // ● 显示图片
        //     name         : 文件名
        //     origin       : 原点
        //     x            : X 坐标
        //     y            : Y 坐标
        //     zoom_x       : X 方向放大率
        //     zoom_y       : Y 方向放大率
        //     opacity      : 不透明度
        //     blend_type   : 合成方式
        //--------------------------------------------------------------------------
        public void show(string name, int origin, double x, double y,
            double zoom_x, double zoom_y, double opacity, int blend_type)
        {
            this.name = name;
            this.origin = origin;
            this.x = x;
            this.y = y;
            this.zoom_x = zoom_x;
            this.zoom_y = zoom_y;
            this.opacity = opacity;
            this.blend_type = blend_type;
            this.duration = 0;
            this.target_x = this.x;
            this.target_y = this.y;
            this.target_zoom_x = this.zoom_x;
            this.target_zoom_y = this.zoom_y;
            this.target_opacity = this.opacity;
            this.tone = new Tone(0, 0, 0, 0);
            this.tone_target = new Tone(0, 0, 0, 0);
            this.tone_duration = 0;
            this.angle = 0;
            this.rotate_speed = 0;
        }
        //--------------------------------------------------------------------------
        // ● 移动图片
        //     duration     : 时间
        //     origin       : 原点
        //     x            : X 坐标
        //     y            : Y 坐标
        //     zoom_x       : X 方向放大率
        //     zoom_y       : Y 方向放大率
        //     opacity      : 不透明度
        //     blend_type   : 合成方式
        //--------------------------------------------------------------------------
        public void move(int duration, int origin, int x, int y, int zoom_x, int zoom_y,
            int opacity, int blend_type)
        {
            this.duration = duration;
            this.origin = origin;
            this.target_x = x;
            this.target_y = y;
            this.target_zoom_x = zoom_x;
            this.target_zoom_y = zoom_y;
            this.target_opacity = opacity;
            this.blend_type = blend_type;
        }
        //--------------------------------------------------------------------------
        // ● 更改旋转速度
        //     speed : 旋转速度
        //--------------------------------------------------------------------------
        public void rotate(int speed)
        {
            this.rotate_speed = speed;
        }
        //--------------------------------------------------------------------------
        // ● 开始更改色调
        //     tone     : 色调
        //     duration : 时间
        //--------------------------------------------------------------------------
        public void start_tone_change(Tone tone, int duration)
        {
            this.tone_target = tone.clone();
            this.tone_duration = duration;
            if (this.tone_duration == 0)
                this.tone = this.tone_target.clone();
        }
        //--------------------------------------------------------------------------
        // ● 消除图片
        //--------------------------------------------------------------------------
        public void erase()
        {
            this.name = "";
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public virtual void update()
        {
            if (this.duration >= 1)
            {
                var d = this.duration;
                this.x = (this.x * (d - 1) + this.target_x) / d;
                this.y = (this.y * (d - 1) + this.target_y) / d;
                this.zoom_x = (this.zoom_x * (d - 1) + this.target_zoom_x) / d;
                this.zoom_y = (this.zoom_y * (d - 1) + this.target_zoom_y) / d;
                this.opacity = (this.opacity * (d - 1) + this.target_opacity) / d;
                this.duration -= 1;
            }
            if (this.tone_duration >= 1)
            {
                var d = this.tone_duration;
                this.tone.red = (this.tone.red * (d - 1) + this.tone_target.red) / d;
                this.tone.green = (this.tone.green * (d - 1) + this.tone_target.green) / d;
                this.tone.blue = (this.tone.blue * (d - 1) + this.tone_target.blue) / d;
                this.tone.gray = (this.tone.gray * (d - 1) + this.tone_target.gray) / d;
                this.tone_duration -= 1;
            }
            if (this.rotate_speed != 0)
            {
                this.angle += this.rotate_speed / 2.0;
                while (this.angle < 0)
                    this.angle += 360;
                this.angle %= 360;
            }
        }
    }
}
