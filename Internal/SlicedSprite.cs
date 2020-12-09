using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace XP.Internal
{
    /// <summary>
    /// SlicedSprite.xaml 的交互逻辑
    /// 九宫格精灵
    /// 也就是把源图片的一部分用显示成9个部分，适合用用作边框
    /// 注意：！！！
    /// 如果使用的是绝对模式，则SlicedRect的区域应该包含于Viewbox的区域
    /// 如果使用的是相对模式，则SlicedRect的各个值应该属于[0, 1]
    /// 否则，结果不准确
    /// </summary>
    public class SlicedSprite : Base.GameObject
    {
        DrawingVisual visual = new DrawingVisual();
        public SlicedSprite()
        {
            this.AddVisual(visual);
        }


        /// <summary>
        /// 图像源
        /// </summary>
        public Bitmap Bitmap
        {
            get { return (Bitmap)GetValue(BitmapProperty); }
            set { SetValue(BitmapProperty, value); }
        }

        public static readonly DependencyProperty BitmapProperty =
            DependencyProperty.Register("Bitmap", typeof(Bitmap),
            typeof(SlicedSprite), new PropertyMetadata(null, new PropertyChangedCallback(SlicedSpriteChanged)));


        /// <summary>
        /// 指示选取图片的哪个区域来显示
        /// </summary>
        public Rect Viewbox
        {
            get { return (Rect)GetValue(ViewboxProperty); }
            set { SetValue(ViewboxProperty, value); }
        }

        public static readonly DependencyProperty ViewboxProperty =
            TileBrush.ViewboxProperty.AddOwner(typeof(SlicedSprite),
            new PropertyMetadata(new Rect(0, 0, 1, 1),
                new PropertyChangedCallback(SlicedSpriteChanged)));


        /// <summary>
        /// 指示分割坐标是绝对坐标还是相对坐标
        /// </summary>
        public BrushMappingMode SlicedRectUnits
        {
            get { return (BrushMappingMode)GetValue(SlicedRectUnitsProperty); }
            set { SetValue(SlicedRectUnitsProperty, value); }
        }

        public static readonly DependencyProperty SlicedRectUnitsProperty =
            DependencyProperty.Register("SlicedRectUnits", typeof(BrushMappingMode), typeof(SlicedSprite),
            new PropertyMetadata(BrushMappingMode.RelativeToBoundingBox, new PropertyChangedCallback(SlicedSpriteChanged)));



        // 图片源是一个矩形，要把该矩形切成9个部分，需要4刀
        // 横两刀，竖两刀
        // 九宫格实际部分的Viewbox需要根据坐标模式，Viewbox和SlicedRect计算
        // SlicedRect定义了这4到分别距离左上右下多远，相当于Padding
        public Thickness SlicedRect
        {
            get { return (Thickness)GetValue(SlicedRectProperty); }
            set { SetValue(SlicedRectProperty, value); }
        }

        public static readonly DependencyProperty SlicedRectProperty =
            DependencyProperty.Register("SlicedRect", typeof(Thickness), typeof(SlicedSprite),
            new PropertyMetadata(new Thickness(0.25, 0.25, 0.25, 0.25), new PropertyChangedCallback(SlicedSpriteChanged)));

        // 是否显示中间部分
        public bool ShowCenterPart
        {
            get { return (bool)GetValue(ShowCenterPartProperty); }
            set { SetValue(ShowCenterPartProperty, value); }
        }

        public static readonly DependencyProperty ShowCenterPartProperty =
            DependencyProperty.Register("ShowCenterPart", typeof(bool), typeof(SlicedSprite),
            new PropertyMetadata(true, new PropertyChangedCallback(SlicedSpriteChanged)));


        public override int width
        {
            get
            {
                return base.width;
            }
            set
            {
                base.width = value;
                Update();
            }
        }

        public override int height
        {
            get
            {
                return base.height;
            }
            set
            {
                base.height = value;
                Update();
            }
        }


        private static void SlicedSpriteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SlicedSprite)d).Update();
        }

        private Rect GetRealViewbox(BrushMappingMode mode, Thickness slicedRect, Rect viewbox, int part)
        {
            switch (part)
            {
                case 0:
                    if (mode == BrushMappingMode.Absolute)
                    {
                        var rect = new Rect(viewbox.Left,
                             viewbox.Top,
                             slicedRect.Left,
                             slicedRect.Top);
                        return rect;
                    }
                    else
                    {
                        var dx = viewbox.Right - viewbox.Left;
                        var dy = viewbox.Bottom - viewbox.Top;
                        var rect = new Rect(viewbox.Left, viewbox.Top,
                            slicedRect.Left * dx, slicedRect.Top * dy);
                        return rect;
                    }
                case 1:
                    if (mode == BrushMappingMode.Absolute)
                    {
                        return new Rect(viewbox.Left + slicedRect.Left,
                           viewbox.Top,
                           viewbox.Right - viewbox.Left - slicedRect.Right - slicedRect.Left,
                           slicedRect.Top);
                    }
                    else
                    {
                        var dx = viewbox.Right - viewbox.Left;
                        var dy = viewbox.Bottom - viewbox.Top;
                        return new Rect(viewbox.Left + slicedRect.Left * dx, viewbox.Top,
                            (1 - slicedRect.Right - slicedRect.Left) * dx, slicedRect.Top * dy);
                    }
                case 2:
                    if (mode == BrushMappingMode.Absolute)
                    {
                        return new Rect(viewbox.Right - slicedRect.Right,
                           viewbox.Top,
                           slicedRect.Right,
                           slicedRect.Top);
                    }
                    else
                    {
                        var dx = viewbox.Right - viewbox.Left;
                        var dy = viewbox.Bottom - viewbox.Top;
                        return new Rect(viewbox.Right - slicedRect.Right * dx,
                            viewbox.Top,
                            slicedRect.Right * dx, slicedRect.Top * dy);
                    }
                case 3:
                    if (mode == BrushMappingMode.Absolute)
                    {
                        return new Rect(viewbox.Left,
                           viewbox.Top + slicedRect.Top,
                           slicedRect.Left,
                           viewbox.Bottom - viewbox.Top - slicedRect.Bottom - slicedRect.Top);
                    }
                    else
                    {
                        var dx = viewbox.Right - viewbox.Left;
                        var dy = viewbox.Bottom - viewbox.Top;
                        return new Rect(viewbox.Left,
                            viewbox.Top + slicedRect.Top * dy,
                            slicedRect.Left * dx, (1 - slicedRect.Bottom - slicedRect.Top) * dy);
                    }
                case 4:
                    if (mode == BrushMappingMode.Absolute)
                    {
                        return new Rect(viewbox.Left + slicedRect.Left,
                           viewbox.Top + slicedRect.Top,
                           viewbox.Right - viewbox.Left - slicedRect.Right - slicedRect.Left,
                           viewbox.Bottom - viewbox.Top - slicedRect.Bottom - slicedRect.Top);
                    }
                    else
                    {
                        var dx = viewbox.Right - viewbox.Left;
                        var dy = viewbox.Bottom - viewbox.Top;
                        return new Rect(viewbox.Left + slicedRect.Left * dx,
                            viewbox.Top + slicedRect.Top * dy,
                            (1 - slicedRect.Right - slicedRect.Left) * dx, (1 - slicedRect.Bottom - slicedRect.Top) * dy);
                    }
                case 5:
                    if (mode == BrushMappingMode.Absolute)
                    {
                        return new Rect(viewbox.Right - slicedRect.Right,
                           viewbox.Top + slicedRect.Top,
                           slicedRect.Right,
                           viewbox.Bottom - viewbox.Top - slicedRect.Bottom - slicedRect.Top);
                    }
                    else
                    {
                        var dx = viewbox.Right - viewbox.Left;
                        var dy = viewbox.Bottom - viewbox.Top;
                        return new Rect(viewbox.Right - slicedRect.Right * dx,
                            viewbox.Top + slicedRect.Top * dy,
                            slicedRect.Right * dx, (1 - slicedRect.Bottom - slicedRect.Top) * dy);
                    }
                case 6:
                    if (mode == BrushMappingMode.Absolute)
                    {
                        return new Rect(viewbox.Left,
                            viewbox.Bottom - slicedRect.Bottom,
                             slicedRect.Left,
                             slicedRect.Bottom);
                    }
                    else
                    {
                        var dx = viewbox.Right - viewbox.Left;
                        var dy = viewbox.Bottom - viewbox.Top;
                        return new Rect(viewbox.Left,
                            viewbox.Bottom - slicedRect.Bottom * dy,
                            slicedRect.Left * dx, slicedRect.Bottom * dy);
                    }
                case 7:
                    if (mode == BrushMappingMode.Absolute)
                    {
                        return new Rect(viewbox.Left + slicedRect.Left,
                           viewbox.Bottom - slicedRect.Bottom,
                           viewbox.Right - viewbox.Left - slicedRect.Right - slicedRect.Left,
                           slicedRect.Bottom);
                    }
                    else
                    {
                        var dx = viewbox.Right - viewbox.Left;
                        var dy = viewbox.Bottom - viewbox.Top;
                        return new Rect(viewbox.Left + slicedRect.Left * dx,
                            viewbox.Bottom - slicedRect.Right * dy,
                            (1 - slicedRect.Right - slicedRect.Left) * dx, slicedRect.Bottom * dy);
                    }
                case 8:
                    if (mode == BrushMappingMode.Absolute)
                    {
                        return new Rect(viewbox.Right - slicedRect.Right,
                           viewbox.Bottom - slicedRect.Bottom,
                          slicedRect.Right,
                          slicedRect.Bottom);
                    }
                    else
                    {
                        var dx = viewbox.Right - viewbox.Left;
                        var dy = viewbox.Bottom - viewbox.Top;
                        return new Rect(viewbox.Right - slicedRect.Right * dx,
                             viewbox.Bottom - slicedRect.Bottom * dy,
                             slicedRect.Right * dx, slicedRect.Bottom * dy);
                    }
                default:
                    return Rect.Empty;
            }
        }

        private void Update()
        {
            if (this.Bitmap == null || this.width <= 0 || this.height <= 0)
                return;

            BrushMappingMode mode = this.SlicedRectUnits;
            Thickness slicedRect = this.SlicedRect;
            Rect viewbox = this.Viewbox;
            var bitmap = this.Bitmap;

            // 目标区域的宽高
            double width1, width2, width3, height1, height2, height3;

            if (mode == BrushMappingMode.Absolute)
            {
                width1 = slicedRect.Left;
                width3 = slicedRect.Right;
                width2 = this.width - width1 - width3;

                height1 = slicedRect.Top;
                height3 = slicedRect.Bottom;
                height2 = this.height - height1 - height3;

            }
            else
            {
                double dw = bitmap.width * (viewbox.Right - viewbox.Left);
                double dh = bitmap.width * (viewbox.Bottom - viewbox.Top);
                width1 = slicedRect.Left * dw;
                width3 = slicedRect.Right * dw;
                width2 = this.width - width1 - width3;

                height1 = slicedRect.Top * dh;
                height3 = slicedRect.Bottom * dh;
                height2 = this.height - height1 - height3;
            }

            var destWidth = new double[9]
                {
                   width1, width2, width3,
                   width1, width2, width3,
                   width1, width2, width3,
                };
            var destHeight = new double[9]
                {
                   height1, height1, height1, 
                   height2, height2, height2, 
                   height3, height3, height3, 
                };

            var destX = new double[9]
                {
                   0, width1, width2 + width1,
                   0, width1, width2 + width1,
                   0, width1, width2 + width1,
                };
            var destY = new double[9]
                {
                   0, 0, 0,
                   height1, height1, height1, 
                   height1 + height2, height1 + height2,height1 + height2,
                };

            using (var dc = this.visual.RenderOpen())
            {
                for (int i = 0; i < 4; i++)
                {
                    var brush = new VisualBrush(bitmap)
                    {
                        ViewboxUnits = mode,
                        Viewbox = GetRealViewbox(mode, slicedRect, viewbox, i)
                    };
                    dc.DrawRectangle(brush, null, new Rect(destX[i], destY[i], destWidth[i], destHeight[i]));
                }
                if (ShowCenterPart)
                {
                    var brush = new VisualBrush(bitmap)
                    {
                        ViewboxUnits = mode,
                        Viewbox = GetRealViewbox(mode, slicedRect, viewbox, 4)
                    };
                    dc.DrawRectangle(brush, null, new Rect(destX[4], destY[4], destWidth[4], destHeight[4]));
                }
                for (int i = 5; i < 9; i++)
                {
                    var brush = new VisualBrush(bitmap)
                    {
                        ViewboxUnits = mode,
                        Viewbox = GetRealViewbox(mode, slicedRect, viewbox, i)
                    };
                    dc.DrawRectangle(brush, null, new Rect(destX[i], destY[i], destWidth[i], destHeight[i]));
                }

            }

        }
    }
}
