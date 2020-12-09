using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XP.Internal
{
    /// <summary>
    /// Window.xaml 的交互逻辑
    // Window
    // 游戏内窗口的类。在内部以大量的精灵构成。
    // 父类Object 
    /// </summary>
    public partial class Window : Canvas
    {
        // 背景
        //private DrawingVisual backgraound = new DrawingVisual();
        // 边框
        private SlicedSprite border = new SlicedSprite()
        {
            SlicedRectUnits = BrushMappingMode.Absolute,
            Viewbox = new Rect(128, 0, 64, 64),
            SlicedRect = new Thickness(16, 16, 16, 16),
            ShowCenterPart = false
        };
        // 内容
        private Canvas content = new Canvas();
        // 光标
        private SlicedSprite cursor = new SlicedSprite()
        {
            SlicedRectUnits = BrushMappingMode.Absolute,
            Viewbox = new Rect(128, 64, 32, 32),
            SlicedRect = new Thickness(4, 4, 4, 4),
            ShowCenterPart = true,
            Visibility = Visibility.Hidden
        };
        // 暂停标记
        private Rectangle pauseTag = new Rectangle()
        {
            Width = 16,
            Height = 16,
            Visibility = Visibility.Hidden
        };
        public Window()
        {
            //this.Children.Add(backgraound);
            this.Children.Add(border);
            
            // 内容与窗口同级，但是左上角相对窗口(16, 16)，z值比窗口大2
            Global.AddUIElement(this.content);

            this.Children.Add(cursor);
            //Canvas.SetZIndex(pauseTag, 1000);
            this.Children.Add(pauseTag);

            Global.AddUIElement(this);
        }

        //类方法Window.new([viewport]) 
        //生成 Window 对象。必须要指定对应的视口（Viewport）。
        public Window(Viewport viewport)
        {
            //this.Children.Add(backgraound);
            this.Children.Add(cursor);

            Global.AddUIElement(this.content);

            Canvas.SetZIndex(pauseTag, 1000);
            this.Children.Add(pauseTag);

            this.viewport = viewport;
            Global.AddUIElement(this);
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
        /// <summary>
        /// 更新窗口的视口
        /// </summary>
        private void UpdateViewort()
        {
            if (this._viewport == null || this.is_disposed)
                return;

            this.OpacityMask = this._viewport;
        }


        //dispose
        //释放窗口。如果已经释放的话则什么也不做。
        public virtual void dispose()
        {
            Global.RemoveUIElement(this);

            isDisposed = true;
        }

        bool isDisposed = false;
        //disposed? 
        //窗口已经释放的话则返回真。
        public bool is_disposed
        {
            get
            {
                return isDisposed;
            }
        }
        //update 
        //进行光标的闪烁，暂停标记的动画。该方法原则上 1 帧调用 1 次。
        public virtual void update()
        {
        }

        public virtual void refresh()
        {

        }

        private void UpdateBackgound()
        {
            if (this._windowskin == null || this.width < 0 || this.height < 0)
                return;

            //using (var dc = backgraound.RenderOpen())
            //{
            //    var rect = new Rect(0, 0, 128, 128);
            //    var brush = new VisualBrush(this._windowskin)
            //    {
            //        ViewboxUnits = BrushMappingMode.Absolute,
            //        Viewbox = rect
            //    };
            //    // 绘制背景
            //    dc.DrawRectangle(brush, null, new Rect(0, 0, this.width, this.height));
            //}
            var rect = new Rect(0, 0, 128, 128);
            this.Background = new VisualBrush(this._windowskin)
                {
                    ViewboxUnits = BrushMappingMode.Absolute,
                    Viewbox = rect
                };

        }
        private void UpdatePauseTag()
        {
            if (_windowskin == null)
                return;

            this.pauseTag.Fill = new VisualBrush(this._windowskin)
            {
                ViewboxUnits = BrushMappingMode.Absolute,
                Viewbox = new Rect(160, 64, 16, 16)
            };

            Canvas.SetLeft(this.pauseTag, (this.width - this.pauseTag.Width) / 2);
            Canvas.SetBottom(this.pauseTag, 5);
        }

        private Bitmap _windowskin;
        //属性windowskin 
        //作为窗口皮肤使用的位图（Bitmap）。
        public Bitmap windowskin
        {
            get
            {
                return this._windowskin;
            }
            set
            {
                if (this._windowskin != value)
                {
                    this._windowskin = value;
                    this.border.Bitmap = value;
                    this.cursor.Bitmap = value;
                    UpdateBackgound();
                    UpdatePauseTag();
                }
            }
        }

        Bitmap _contents;
        //contents 
        //作为窗口内容显示的位图（Bitmap）。
        public Bitmap contents
        {
            get
            {
                return this._contents;
            }
            set
            {
                if (this._contents != value)
                {
                    this._contents = value;
                    this.content.Children.Clear();
                    this.content.Children.Add(value);
                }
            }
        }

        //stretch 
        //壁纸的显示方法。真为「拉伸显示」，伪为「平铺显示」。初始值是 true。
        public bool stretch { get; set; }

        //cursor_rect 
        //光标的矩形（Rect）。以（-16,-16）的相对座标指定窗口的左上角。
        Rect _cursor_rect = Rect.Empty;
        public Rect cursor_rect
        {
            set
            {
                //Canvas.SetLeft(this.cursor, value.X + 16);
                //Canvas.SetTop(this.cursor, value.Y + 16);
                //this.cursor.Width = value.Width;
                //this.cursor.Height = value.Height;
                if (this._cursor_rect != value)
                {
                    this._cursor_rect = value;
                    this.cursor.width = (int)value.Width;
                    this.cursor.height = (int)value.Height;
                    this.cursor.x = (int)value.X + 16;
                    this.cursor.y = (int)value.Y + 16;
                }
            }
            get
            {
                //return new Rect((int)Canvas.GetLeft(this.cursor) - 16, (int)Canvas.GetTop(this.cursor) - 16,
                //    (int)this.cursor.Width, (int)this.cursor.Height);
                return _cursor_rect;
            }
        }

        //visible 
        //窗口的可见状态。真为可见。
        public bool visible
        {
            get
            {
                return this.Visibility == Visibility.Visible;
            }
            set
            {
                this.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                this.content.Visibility = this.Visibility;
            }
        }

        //pause 
        //暂停标记的的可见状态。所谓暂停标记，是消息窗口中表示等待按钮输入状态的符号。真为可见。
        public bool pause
        {
            get { return (bool)GetValue(pauseProperty); }
            set { SetValue(pauseProperty, value); }
        }

        public static readonly DependencyProperty pauseProperty =
            DependencyProperty.Register("pause", typeof(bool), typeof(Window),
            new PropertyMetadata(false, new PropertyChangedCallback(PausePropertyChanged)));

        private static void PausePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            window.UpdatePauseTagState();
        }

        //active 
        //光标的闪烁状态。真为闪烁。
        public bool active
        {
            get { return (bool)GetValue(activeProperty); }
            set { SetValue(activeProperty, value); }
        }

        public static readonly DependencyProperty activeProperty =
            DependencyProperty.Register("active", typeof(bool), typeof(Window),
            new PropertyMetadata(false, new PropertyChangedCallback(ActivePropertyChanged)));

        private static void ActivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            window.UpdateCursor();
        }

        /// <summary>
        /// 使用CursorRect更新光标的大小和位置以及闪烁动画
        /// </summary>
        private void UpdateCursor()
        {
            if (active)
            {
                this.PlayCursorAnimation();
                this.cursor.Visibility = Visibility.Visible;
            }
            else
            {
                this.StopCursorAnimation();
                this.cursor.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 显示或隐藏暂停标记及它的动画
        /// </summary>
        private void UpdatePauseTagState()
        {
            if (pause)
            {
                this.PlayPauseArrowAnimation();
                this.pauseTag.Visibility = Visibility.Visible;
            }
            else
            {
                this.StopPauseArrowAnimation();
                this.pauseTag.Visibility = Visibility.Hidden;
            }
        }
        //x 
        //窗口的 X 座标。
        public double x
        {
            set
            {
                Canvas.SetLeft(this, value);
                Canvas.SetLeft(this.content, value + 16);
            }
            get
            {
                return (int)Canvas.GetLeft(this);
            }
        }

        //y 
        //窗口的 Y 座标。
        public double y
        {
            set
            {
                Canvas.SetTop(this, value);
                Canvas.SetTop(this.content, value + 16);
            }
            get
            {
                return (int)Canvas.GetTop(this);
            }
        }


        //width 
        //窗口的宽度。
        public double width
        {
            set
            {
                this.Width = value;
                UpdateBackgound();
                this.UpdatePauseTag();
                this.border.width = (int)value;
            }
            get
            {
                return this.Width;
            }
        }

        //height 
        //窗口的高度。
        public double height
        {
            set
            {
                this.Height = value;
                UpdateBackgound();
                this.border.height = (int)value;
            }
            get
            {
                return this.Height;
            }
        }


        //z 
        // 窗口背景的 Z 座标。该值大的东西显示在上面。Z 座标相同的话，则后生成的对象显示在上面。
        // 窗口内容的 Z 座标为窗口背景的 Z 座标的值加上 2。
        public int z
        {
            set
            {
                Canvas.SetZIndex(this, value);
                Canvas.SetZIndex(this.content, value + 2);
            }
            get
            {
                return (int)Canvas.GetTop(this);
            }
        }

        //ox 
        //窗口内容传送元原点的 X 座标。根据该值变化进行滚动。
        public int ox { get; set; }
        //oy 
        //窗口内容传送元原点的 Y 座标。根据该值变化进行滚动。
        public int oy { get; set; }

        //opacity 
        //窗口的不透明度（0 ～ 255）。范围外的数值会自动修正。
        public int opacity
        {
            set
            {
                this.Opacity = value / 255.0;
            }
            get
            {
                return (int)(this.Opacity * 255);
            }
        }

        //back_opacity 
        //窗口背景的不透明度（0 ～ 255）。范围外的数值会自动修正。
        public int back_opacity
        {
            set
            {
                this.Background.Opacity = value / 255.0;
            }
            get
            {
                return (int)(this.Background.Opacity * 255);
            }
        }


        //contents_opacity 
        //窗口内容的不透明度（0 ～ 255）。范围外的数值会自动修正。
        public int contents_opacity
        {
            set
            {
                this.content.Opacity = value / 255.0;
            }
            get
            {
                return (int)(this.content.Opacity * 255);
            }
        }

        private void PlayPauseArrowAnimation()
        {
            if (this.pauseTag.Fill == null)
                return;

            //if (this.pauseArrowStoryboard.Children.Count == 0)
            //{
            RectAnimationUsingKeyFrames pauseAnimation = new RectAnimationUsingKeyFrames()
            {
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true
            };
            pauseAnimation.KeyFrames.Add(new DiscreteRectKeyFrame(new System.Windows.Rect(160, 64, 16, 16), TimeSpan.FromSeconds(0)));
            pauseAnimation.KeyFrames.Add(new DiscreteRectKeyFrame(new System.Windows.Rect(176, 64, 16, 16), TimeSpan.FromSeconds(0.1)));
            pauseAnimation.KeyFrames.Add(new DiscreteRectKeyFrame(new System.Windows.Rect(160, 80, 16, 16), TimeSpan.FromSeconds(0.2)));
            pauseAnimation.KeyFrames.Add(new DiscreteRectKeyFrame(new System.Windows.Rect(176, 80, 16, 16), TimeSpan.FromSeconds(0.3)));

            Storyboard.SetTarget(pauseAnimation, this.pauseTag.Fill);
            Storyboard.SetTargetProperty(pauseAnimation, new PropertyPath("Viewbox"));
            this.pauseTag.Fill.BeginAnimation(ImageBrush.ViewboxProperty, pauseAnimation);
            //this.pauseArrowStoryboard.Children.Add(pauseAnimation);
            //}
            //this.pauseArrowStoryboard.Begin();
        }
        /// <summary>
        /// 停止暂停标记的动画
        /// </summary>
        private void StopPauseArrowAnimation()
        {
            //this.pauseArrowStoryboard.Stop();
        }


        private Storyboard cursorStoryboard = new Storyboard();
        /// <summary>
        /// 播放光标闪烁动画
        /// </summary>
        private void PlayCursorAnimation()
        {
            if (this.cursor == null)
                return;

            if (this.cursorStoryboard.Children.Count == 0)
            {
                DoubleAnimation flash = new DoubleAnimation()
                {
                    From = 0.3,
                    To = 0.7,
                    Duration = TimeSpan.FromSeconds(0.4),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };
                Storyboard.SetTarget(flash, this.cursor);
                Storyboard.SetTargetProperty(flash, new PropertyPath(Control.OpacityProperty));
                this.cursorStoryboard.Children.Add(flash);
            }
            this.cursorStoryboard.Begin();
        }
        /// <summary>
        /// 停止光标闪烁动画
        /// </summary>
        private void StopCursorAnimation()
        {
            this.cursorStoryboard.Stop();
        }

    }
}
