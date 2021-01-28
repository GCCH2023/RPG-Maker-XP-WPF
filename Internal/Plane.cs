using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace XP.Internal
{
    // Plane
    // 平面的类。所谓平面，是在整个画面中排列显示位图图形的特殊的精灵，用来显示全景和雾。
    // 实际上就是一个大小始终是屏幕大小的精灵，该精灵使用指定图像平铺满屏幕
    //父类Object 
    public class Plane : Border
    {
        //类方法Plane.new([viewport]) 
        //生成 Plane 对象。必须要指定对应的视口（Viewport）。
        public Plane(Viewport viewport = null)
        {
            this.viewport = viewport;
            this.Width = Global.ScreenWidth;
            this.Height = Global.ScreenHeight;

            this.RenderTransform = new ScaleTransform(1, 1);

            //Global.AddUIElement(this);
        }

        /// <summary>
        /// 使用bitmap来更新平面
        /// </summary>
        private void Update()
        {
            if(this.bitmap == null)
                return;

            var visualBrush = this.Background as VisualBrush;
            visualBrush.Viewport = new Rect(-ox, -oy, this.bitmap.width, this.bitmap.height);
        }


        bool _is_disposed = false;
        //方法dispose 
        //释放平面。如果已经释放的话则什么也不做。
        public virtual void dispose()
        {
            _is_disposed = true;
        }

        //disposed? 
        //平面已经释放的话则返回真。
        public bool is_disposed
        {
            get { return this._is_disposed; }
        }

        //属性bitmap 
        //对平面使用位图（Bitmap）的参照。
        Bitmap _bitmap = null;

        public Bitmap bitmap
        {
            get { return _bitmap; }
            set
            {
                if (this._bitmap != value)
                {
                    _bitmap = value;

                    this.Background = new VisualBrush(value)
                    {
                        ViewportUnits = BrushMappingMode.Absolute,
                        TileMode = TileMode.Tile,
                        Stretch = Stretch.None
                    };

                    Update();
                }
            }
        }

        //visible 
        //平面的可见状态。真为可见。
        public bool visible
        {
            get
            {
                return this.Visibility == Visibility.Visible;
            }
            set
            {
                this.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            }
        }
        //ox 
        //平面传送元原点的 X 座标。根据该值变化进行滚动。
        double _ox = 0;

        public double ox
        {
            get { return _ox; }
            set
            {
                if (_ox != value)
                {
                    _ox = value;
                    Update();
                }
            }
        }
       
        //oy 
        //平面传送元原点的 Y 座标。根据该值变化进行滚动。
        double _oy = 0;

        public double oy
        {
            get { return _oy; }
            set
            {
                if (_oy != value)
                {
                    _oy = value;
                    Update();
                }
            }
        }
        //zoom_x 
        //平面的 X 轴方向的放大率。1.0 为等倍。
        public double zoom_x
        {
            get
            {
                return ((ScaleTransform)this.RenderTransform).ScaleX;
            }
            set
            {
                ((ScaleTransform)this.RenderTransform).ScaleX = value;
            }
        }
        //zoom_y 
        //平面的 Y 轴方向的放大率。1.0 为等倍。
        public double zoom_y
        {
            get
            {
                return ((ScaleTransform)this.RenderTransform).ScaleY;
            }
            set
            {
                ((ScaleTransform)this.RenderTransform).ScaleY = value;
            }
        }
        //opacity 
        //平面的不透明度。范围为 0 ～ 255。范围外的数值会自动修正。
        public double opacity
        {
            get
            {
                return (int)(this.Opacity * 255);
            }
            set
            {
                this.Opacity = value / 255.0;
            }
        }
        //blend_type 
        //平面的合成方法（0：正常，1：加法，2：减法）。
        public int blend_type { get; set; }
        //color 
        //在平面中混合颜色（Color）。混合的比例使用 alpha 值。
        public Color color { get; set; }
        //tone 
        //平面的色调（Tone）。
        public Tone tone { get; set; }


        private void UpdateViewort()
        {
            if (this._viewport == null || this.is_disposed)
                return;

            this.OpacityMask = this._viewport;
            // 可能要更新z值
            this.z = this.z;
        }

        Viewport _viewport = null;
        //viewport 
        //取得生成时指定的视口（Viewport）。
        public Viewport viewport
        {
            get
            {
                return this._viewport;
            }
            set
            {
                if (this._viewport != value)
                {
                    this._viewport = value;
                    this.UpdateViewort();
                }
            }
        }

        //z 
        //平面的 Z 座标。该值大的东西显示在上面。Z 座标相同的话，则后生成的对象显示在上面。
        int _z = 0;
        public virtual int z
        {
            get
            {
                return _z;
            }
            set
            {
                this._z = value;

                if (this.viewport != null && value < this.viewport.z)
                    value = this.viewport.z;

                Canvas.SetZIndex(this, value);
            }
        }

    }
}
