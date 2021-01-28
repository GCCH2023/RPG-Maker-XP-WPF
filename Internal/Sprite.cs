using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace XP.Internal
{
    // Sprite
    // 精灵的类。所谓精灵，是为了在游戏画面上显示人物等的基本概念。
    //父类Object 
    public class Sprite : Base.ViewportObject
    {
        DrawingVisual visual = new DrawingVisual();
        SolidColorBrush colorBrush = new SolidColorBrush(Colors.White);
        //类方法new Sprite([viewport]) 
        //生成 Sprite 对象。必须要指定对应的视口（Viewport）。
        public Sprite(Viewport viewport = null) : 
            base(viewport)
        {
            // 将精灵添加到游戏面板中
            this.AddVisual(this.visual);
           Global.AddUIElement(this);
        }
        // 方法dispose 
        // 释放精灵。如果已经释放的话则什么也不做。
        public override void dispose()
        {
           Global.RemoveUIElement(this);

           base.dispose();
        }
        //flash(color, duration) 
        //开始精灵的闪烁。duration 是闪烁的帧数。
        //color 为闪烁的颜色，如指定为 null，闪烁时则消去精灵本身。
        // 修改：duration是闪烁的时间，单位为秒
        public void flash(Color? color, double duration)
        {
            if(color != null)
            {
                if(this.Effect == null)
                {
                    // 添加一个改变颜色的效果
                    this.Effect = new Effects.ColorEffect();
                }

                ColorAnimation colorAnimation = new ColorAnimation()
                {
                    From = color,
                    To = this.color,
                    Duration = TimeSpan.FromSeconds(duration)
                };
                colorAnimation.Completed += (o, e) =>
                    {
                        var effect = (Effects.ColorEffect)this.Effect;
                        effect.Color = this.color;
                        // 去掉动画
                        effect.BeginAnimation(Effects.ColorEffect.ColorProperty, null);
                    };
                ((Effects.ColorEffect)this.Effect).BeginAnimation(Effects.ColorEffect.ColorProperty, colorAnimation);
            }
        }

        //update 
        //进行精灵的闪烁。该方法原则上 1 帧调用 1 次。
        //不需要闪烁的情况下不必调用。
        public virtual void update()
        {

        }

        private void UpdateVisual()
        {
            if (this.bitmap == null)
                return;

            using(var dc = this.visual.RenderOpen())
            {
                var brush = new VisualBrush(this.bitmap)
                {
                    ViewboxUnits = BrushMappingMode.Absolute,
                    Viewbox = _src_rect,
                    Stretch = Stretch.None,
                    TileMode = TileMode.None,
                };
                dc.DrawRectangle(brush, null, new Rect(0, 0, _src_rect.Width, _src_rect.Height));
            }
        }
        Bitmap _bitmap = null;
        //属性bitmap 
        //作为传送元的位图（Bitmap）。
        public Bitmap bitmap
        {
            get
            {
                return this._bitmap;
            }
            set
            {
                if(this._bitmap != value)
                {
                    this._bitmap = value;

                    if (!this.is_disposed)
                    {
                        this.Width = _bitmap.Width;
                        this.Height = _bitmap.Height;

                        if (this.src_rect == Rect.Empty)
                            this.src_rect = new Rect(0, 0, _bitmap.width, _bitmap.height);
                        UpdateVisual();
                    }
                }
            }
        }
        //src_rect 
        //传送位图的矩形（Rect）。
        Rect _src_rect = Rect.Empty;

        public Rect src_rect
        {
            get { return _src_rect; }
            set
            {
                _src_rect = value;
                if (bitmap != null)
                    UpdateVisual();
            }
        }

        //visible 
        //精灵的可见状态。真为可见。
        //public bool visible { get; set; }
        //x 
        //精灵的 X 座标。
        double _x;
        public override double x
        {
            get { return _x; }
            set
            {
                _x = value;
                base.x = _x - ox;
            }
        }
        //y 
        //精灵的 Y 座标。
        double _y;
        public override double y
        {
            get { return _y; }
            set
            {
                _y = value;
                base.y = _y - oy;
            }
        }
        //z 
        //精灵的 Z 座标。该值大的东西显示在上面。Z 座标相同的话，则后生成的对象显示在上面。
        //public int z { get; set; }

        //ox 
        //精灵传送元原点的 X 座标。
        double _ox;

        public double ox
        {
            get { return _ox; }
            set { _ox = value; }
        }
        //oy 
        //精灵传送元原点的 Y 座标。
        double _oy;

        public double oy
        {
            get { return _oy; }
            set { _oy = value; }
        }
        //zoom_x 
        //精灵的 X 轴方向的放大率。1.0 为等倍。
        //public double zoom_x { get; set; }
        //zoom_y 
        //精灵的 Y 轴方向的放大率。1.0 为等倍。
        //public double zoom_y { get; set; }
        //angle 
        //精灵的旋转角度。以逆时针方向 360 度系统来指定。在旋转图画时会花费时间，所以要尽量避免。
        //public double angle { get; set; }
        //mirror 
        //精灵的左右反转特征。真时反转图画。
        //public bool mirror { get; set; }
        //bush_depth 
        //精灵的草木繁茂处深度。所谓草木繁茂处深度，是半透明显示精灵下部的点数。根据这个效果，能简单地表现人物脚下象是隐藏在草木繁茂处一样。
        public int bush_depth { get; set; }
        //opacity 
        //精灵的不透明度。范围为 0 ～ 255。范围外的数值会自动修正。
        //public double opacity { get; set; }
        //blend_type 
        //精灵的合成方法（0：正常，1：加法，2：减法）。
        public int blend_type { get; set; }
        // color 
        // 在精灵中混合颜色（Color）。混合的比例使用 alpha 值。
        // flash 中混合颜色是另外的管理。但是，显示的时候 alpha 值大的颜色会优先混合。
        // 不明白
        protected Color _color = Colors.Transparent;
        public Color color
        {
            get { return _color; }
            set 
            {
                if (_color != value)
                {
                    _color = value;

                    if (this.Effect == null)
                    {
                        // 添加一个改变颜色的效果
                        this.Effect = new Effects.ColorEffect();
                    }
                    ((Effects.ColorEffect)this.Effect).Color = value;
                }
            }
        }
        // tone 
        // 精灵的色调（Tone）。
        // 也不明白
        Tone _tone = new Tone(0, 0, 0);
        public Tone tone
        {
            get { return _tone; }
            set { _tone = value; }
        }
    }
}
