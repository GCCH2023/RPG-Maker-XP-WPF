using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using XP.Internal;

namespace XP.RPG
{
    //        RPG.Weather
    //用 RPGXP 事件指令显示天气效果（雨，风，雪）的类。

    //父类Object 
    public class Weather
    {

        //类方法RPG.Weather.new([viewport]) 
        //生成 RPG.Weather 对象。必须要指定对应的视口（Viewport）。
        public Weather(Viewport viewport = null)
        {
            this.type = 0;
            this.max = 0;
            this.ox = 0;
            this.oy = 0;
            var color1 = Colors.White;
            var color2 = Color.FromArgb(128, 255, 255, 255);
            this.rain_bitmap = new Bitmap(7, 56);
            for (var i = 0; i <= 6; i++)
                this.rain_bitmap.fill_rect(6 - i, i * 8, 1, 8, color1);

            this.storm_bitmap = new Bitmap(34, 64);
            for (var i = 0; i <= 31; i++)
            {
                this.storm_bitmap.fill_rect(33 - i, i * 2, 1, 2, color2);
                this.storm_bitmap.fill_rect(32 - i, i * 2, 1, 2, color1);
                this.storm_bitmap.fill_rect(31 - i, i * 2, 1, 2, color2);
            }

            this.snow_bitmap = new Bitmap(6, 6);
            this.snow_bitmap.fill_rect(0, 1, 6, 4, color2);
            this.snow_bitmap.fill_rect(1, 0, 4, 6, color2);
            this.snow_bitmap.fill_rect(1, 2, 4, 2, color1);
            this.snow_bitmap.fill_rect(2, 1, 2, 4, color1);
            this.sprites = new List<Sprite>();
            for (var i = 0; i <= 40; i++)
            {
                var sprite = new Sprite(viewport);
                sprite.z = 1000;
                sprite.visible = false;
                sprite.opacity = 0;
                this.sprites.Add(sprite);
            }
        }
        //方法dispose 
        //释放天气效果。
        public virtual void dispose()
        {
            foreach (var sprite in this.sprites)
                sprite.dispose();

            this.rain_bitmap.dispose();
            this.storm_bitmap.dispose();
            this.snow_bitmap.dispose();
        }





        //定义module RPG
        //属性type 
        //天气类型（0：无，1：雨，2：风，3：雪）。

        int _type;
        public int type
        {
            set
            {
                if (this._type == value)
                    return;
                this._type = value;
                Bitmap bitmap = null;
                switch (this.type)
                {
                    case 1:
                        bitmap = this.rain_bitmap; break;
                    case 2:
                        bitmap = this.storm_bitmap; break;
                    case 3:
                        bitmap = this.snow_bitmap; break;
                    default:
                        bitmap = null;
                        break;
                }
                for (var i = 1; i <= 40; i++)
                {
                    var sprite = this.sprites[i];
                    if (sprite != null)
                    {
                        sprite.visible = (i <= this.max);
                        sprite.bitmap = bitmap;
                    }
                }
            }
            get
            {
                return this._type;
            }
        }
        //ox 
        //原点的 X 座标。其滚动追踪元件地图的原点。

        int _ox;
        public int ox
        {
            get { return this._ox; }
            set
            {
                if (this._ox == value)
                    return;
                this._ox = value;
                foreach (var sprite in this.sprites)
                    sprite.ox = this._ox;
            }
        }
        //oy 
        //原点的 Y 座标。其滚动追踪元件地图的原点。

        int _oy;
        public int oy
        {
            get { return this._oy; }
            set
            {
                if (this._oy == value)
                    return;
                this._oy = value;
                foreach (var sprite in this.sprites)
                    sprite.oy = this._oy;
            }
        }
        //max 
        //同时显示天气效果图像的量（0..40）。

        int _max;
        public int max
        {
            get { return this._max; }
            set
            {
                if (this._max == value)
                    return;
                this._max = Math.Min(Math.Max(value, 0), 40);
                for (var i = 1; i <= 40; i++)
                {
                    var sprite = this.sprites[i];
                    if (sprite != null)
                        sprite.visible = (i <= this._max);
                }
            }
        }
        //update 
        //进行天气效果。该方法原则上 1 帧调用 1 次。

        public virtual void update()
        {
            if (this.type == 0)
                return;
            for (var i = 1; i <= this.max; i++)
            {
                var sprite = this.sprites[i];
                if (sprite == null)
                    break;
                if (this.type == 1)
                {
                    sprite.x -= 2;
                    sprite.y += 16;
                    sprite.opacity -= 8;
                }
                if (this.type == 2)
                {
                    sprite.x -= 8;
                    sprite.y += 16;
                    sprite.opacity -= 12;
                }
                if (this.type == 3)
                {
                    sprite.x -= 2;
                    sprite.y += 8;
                    sprite.opacity -= 8;
                }
                var x = sprite.x - this.ox;
                var y = sprite.y - this.oy;
                if (sprite.opacity < 64 || x < -50 || x > 750 || y < -300 || y > 500)
                {
                    sprite.x = Global.rand(800) - 50 + this.ox;
                    sprite.y = Global.rand(800) - 200 + this.oy;
                    sprite.opacity = 255;
                }
            }

        }

        public Bitmap rain_bitmap { get; set; }

        public Bitmap storm_bitmap { get; set; }

        public Bitmap snow_bitmap { get; set; }

        public List<Sprite> sprites { get; set; }
    }
}
