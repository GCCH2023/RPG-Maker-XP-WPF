using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace XP
{
    //Viewport
    //视口的类。在画面的一部分中显示精灵，而不影响其他部分的情况下使用。
    //父类Object 
    public class Viewport
    {
        // 用于遮罩控件的画刷
        private Brush opacityMaskBrush;

        //类方法Viewport.new(x, y, width, height) 
        public Viewport(double x, double y, double width, double height)
        {
            this.rect = new Rect(x, y, width, height);
        }

        //Viewport.new(rect) 
        //生成 Viewport 对象。
        public Viewport(Rect rect)
        {
            this.rect = rect;
        }

        /// <summary>
        /// 定义到System.Windows.Rect的隐式转换
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static implicit operator Brush(Viewport viewport)
        {
            return viewport.opacityMaskBrush;
        }

        //方法dispose 
        //释放视口。如果已经释放的话则什么也不做。
        public virtual void dispose()
        {
            this.opacityMaskBrush = null;
        }

        //disposed? 
        //视口已经释放的话则返回真。
        public bool is_dispose()
        {
            return this.opacityMaskBrush == null;
        }

        //flash(color, duration) 
        //开始视口的闪烁。duration 是闪烁的帧数。
        //color 为闪烁的颜色，如指定为 null，闪烁时则消去视口本身。
        public void flash(Color color, int duration)
        {

        }
        //update 
        //进行视口的闪烁。该方法原则上 1 帧调用 1 次。

        //不需要闪烁的情况下不必调用。
        public virtual void update()
        {

        }
        private Rect _rect;
        //属性rect 
        //作为视口设定的矩形（Rect）。

        public Rect rect
        {
            get
            {
                return _rect;
            }
            set
            {
                if (this._rect != value)
                {
                    this._rect = value;
                    this.opacityMaskBrush = new DrawingBrush()
                    {
                        ViewportUnits = BrushMappingMode.Absolute,
                        Viewport = new System.Windows.Rect(0, 0, value.Width, value.Height),
                        Drawing = Global.ViewportDrawing
                    };
                }
            }
        }
        bool _visible = true;
        // visible 
        // 视口的可见状态。真为可见。

        public bool visible
        {
            get
            {
                return _visible;
            }
            set 
            {
                if (_visible != value)
                {
                    _visible = value;

                    this.opacityMaskBrush.Opacity = _visible ? 1 : 0;
                }
            }
        }

        //z 
        //视口的 Z 座标。该值大的东西显示在上面。Z 座标相同的话，则后生成的对象显示在上面。
        int _z = 0;
        public int z
        {
            get { return _z; }
            set { _z = value; }
        }
        //ox 
        //视口传送元原点的 X 座标。根据该值变化进行滚动。
        int _ox;

        public int ox
        {
            get { return _ox; }
            set { _ox = value; }
        }
        //oy 
        //视口传送元原点的 Y 座标。根据该值变化进行滚动。
        int _oy;

        public int oy
        {
            get { return _oy; }
            set { _oy = value; }
        }
        //color 
        //在视口中混合颜色（Color）。混合的比例使用 alpha 值。
        //flash 中混合颜色是另外的管理。
        Color _color = new Color();

        public Color color
        {
            get { return _color; }
            set { _color = value; }
        }

        //tone 
        //视口的色调（Tone）。
        Tone _tone = new Tone(0, 0, 0);
        public Tone tone
        {
            get { return _tone; }
            set { _tone = value; }
        }
    }
}
