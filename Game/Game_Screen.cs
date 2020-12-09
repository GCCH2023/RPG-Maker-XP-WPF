using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace XP
{
    //==============================================================================
    // ■ Game_Screen
    //------------------------------------------------------------------------------
    // 　更改色调以及画面闪烁、保存画面全体关系处理数据的类。本类的实例请参考
    // Global.game_screen。
    //==============================================================================

    public class Game_Screen
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public Tone tone;                     // 色调
        public Color flash_color;            // 闪烁色
        public int shake;                    // 震动位置
        public int weather_type;            // 天候 类型
        public int weather_max;             // 天候 图像的最大数
        public Tone tone_target;
        public int tone_duration;
        public int flash_duration = 0;
        public int shake_power = 0;
        public int shake_speed = 0;
        public int shake_duration = 0;
        public int shake_direction = 1;
        public List<Game_Picture> pictures;     // 图片
        public int weather_type_target = 0;
        public double weather_max_target = 0.0;
        public int weather_duration = 0;


        //--------------------------------------------------------------------------
        // ● 初试化对像
        //--------------------------------------------------------------------------
        public Game_Screen()
        {
            this.tone = new Tone(0, 0, 0, 0);
            this.tone_target = new Tone(0, 0, 0, 0);
            this.tone_duration = 0;
            this.flash_color = Colors.Transparent;
            this.shake = 0;
            this.pictures = new List<Game_Picture>(101) { null };
            for (var i = 1; i <= 100; i++)
                this.pictures.Add(new Game_Picture(i));
            this.weather_type = 0;
            this.weather_max = 0;
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
        // ● 开始画面闪烁
        //     color    : 色
        //     duration : 时间
        //--------------------------------------------------------------------------
        public void start_flash(Color color, int duration)
        {
            this.flash_color = Utility.ColorUtilities.Clone(color);
            this.flash_duration = duration;
        }
        //--------------------------------------------------------------------------
        // ● 开始震动
        //     power    : 强度
        //     speed    : 速度
        //     duration : 时间
        //--------------------------------------------------------------------------
        public void start_shake(int power, int speed, int duration)
        {
            this.shake_power = power;
            this.shake_speed = speed;
            this.shake_duration = duration;
        }
        //--------------------------------------------------------------------------
        // ● 设置天候
        //     type     : 类型
        //     power    : 强度
        //     duration : 时间
        //--------------------------------------------------------------------------
        public void weather(int type, int power, int duration)
        {
            this.weather_type_target = type;
            if (this.weather_type_target != 0)
                this.weather_type = this.weather_type_target;
            if (this.weather_type_target == 0)
                this.weather_max_target = 0.0;
            else
                this.weather_max_target = (power + 1) * 4.0;

            this.weather_duration = duration;
            if (this.weather_duration == 0)
            {
                this.weather_type = this.weather_type_target;
                this.weather_max = (int)this.weather_max_target;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public virtual void update()
        {
            if (this.tone_duration >= 1)
            {
                var d = this.tone_duration;
                this.tone.red = (this.tone.red * (d - 1) + this.tone_target.red) / d;
                this.tone.green = (this.tone.green * (d - 1) + this.tone_target.green) / d;
                this.tone.blue = (this.tone.blue * (d - 1) + this.tone_target.blue) / d;
                this.tone.gray = (this.tone.gray * (d - 1) + this.tone_target.gray) / d;
                this.tone_duration -= 1;
            }
            if (this.flash_duration >= 1)
            {
                var d = this.flash_duration;
                this.flash_color.A = (byte)(this.flash_color.A * (d - 1) / d);
                this.flash_duration -= 1;
            }
            if (this.shake_duration >= 1 || this.shake != 0)
            {
                var delta = (this.shake_power * this.shake_speed * this.shake_direction) / 10.0;
                if (this.shake_duration <= 1 && this.shake * (this.shake + delta) < 0)
                    this.shake = 0;
                else
                    this.shake += (int)delta;
                if (this.shake > this.shake_power * 2)
                    this.shake_direction = -1;

                if (this.shake < -this.shake_power * 2)
                    this.shake_direction = 1;

                if (this.shake_duration >= 1)
                    this.shake_duration -= 1;

            }
            if (this.weather_duration >= 1)
            {

                var d = this.weather_duration;
                this.weather_max = (int)( (this.weather_max * (d - 1) + this.weather_max_target) / d);
                this.weather_duration -= 1;
                if (this.weather_duration == 0)
                    this.weather_type = this.weather_type_target;
            }
            if (Global.game_temp.in_battle)
                for (var i = 51; i <= 100; i++)
                    this.pictures[i].update();
            else
                for (var i = 1; i <= 50; i++)
                    this.pictures[i].update();
        }
    }

}
