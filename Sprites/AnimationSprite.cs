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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XP.Sprites
{
    // 播放RPG Maker XP的动画的类
    public class AnimationSprite : Control
    {
        static AnimationSprite()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimationSprite), new FrameworkPropertyMetadata(typeof(AnimationSprite)));
        }


        public RPG.Animation Animation
        {
            get { return (RPG.Animation)GetValue(AnimationProperty); }
            set { SetValue(AnimationProperty, value); }
        }

        public static readonly DependencyProperty AnimationProperty =
            DependencyProperty.Register("Animation", typeof(RPG.Animation), typeof(AnimationSprite),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(AnimationChanged)));

        private BitmapImage bitmap;
        private static void AnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue == null)
                return;

            var sprite = d as AnimationSprite;
            var name =( (RPG.Animation)e.NewValue).animation_name;
            sprite.bitmap = RPG.Cache.Animation(name);
            sprite.Width = sprite.bitmap.PixelWidth;
            sprite.Height = sprite.bitmap.PixelHeight;
        }



        public int FrameIndex
        {
            get { return (int)GetValue(FrameIndexProperty); }
            set { SetValue(FrameIndexProperty, value); }
        }

        public static readonly DependencyProperty FrameIndexProperty =
            DependencyProperty.Register("FrameIndex", typeof(int), typeof(AnimationSprite),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(FrameIndexChanged),
                new CoerceValueCallback(CoerceFrameIndex)));

        private static object CoerceFrameIndex(DependencyObject d, object baseValue)
        {
            var sprite = d as AnimationSprite;
            if (sprite.Animation == null)
                return 0;

            var value = (int)baseValue;
            if (value < 0)
                return 0;

            if (value >= sprite.Animation.frame_max)
                return sprite.Animation.frame_max - 1;

            return value;
        }

        private static void FrameIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }


        //protected override void OnRender(DrawingContext drawingContext)
        //{
        //    base.OnRender(drawingContext);

        //    var animation = this.Animation;
        //    if (animation == null || this.bitmap == null)
        //        return;

        //    drawingContext.DrawImage(this.bitmap, new Rect(0, 0, this.Width, this.Height));

        //    var frame_index = this.FrameIndex;
        //    var cell_data = animation.frames[frame_index].cell_data;
        //    var position = animation.position;

        //    for (var i = 0; i <= 15; i++)
        //    {
        //        var pattern = cell_data[i, 0];
        //        if (pattern == null || pattern == -1)
        //            continue;
        //        var brush = new ImageBrush(this.bitmap)
        //        {
        //            ViewboxUnits = BrushMappingMode.Absolute,
        //            Viewbox = new Rect((int)pattern % 5 * 192, (int)pattern / 5 * 192, 192, 192)
        //        };

        //        double x, y;
        //        if (position == 3)
        //        {
        //            x = this.Width / 2;
        //            y = this.Height / 2;
        //        }
        //        else
        //        {
        //            x = this.Width / 2;
        //            y = this.Height / 2;
        //            if (position == 0)
        //                y -= this.Height / 4;
        //            if (position == 2)
        //                y += this.Height / 4;
        //        }
        //        x += (int)cell_data[i, 1];
        //        y += (int)cell_data[i, 2];
        //        x -= 96;
        //        y -= 96;

        //        var zoom_x = (int)cell_data[i, 3] / 100.0;
        //        var zoom_y = (int)cell_data[i, 3] / 100.0;
        //        var angle = (int)cell_data[i, 4];
        //        var mirror = ((int)cell_data[i, 5] == 1);
        //        var opacity = (int)cell_data[i, 6];
        //        var blend_type = (int)cell_data[i, 7];

        //        var transforms = new TransformGroup();

        //        transforms.Children.Add(new ScaleTransform(zoom_x, zoom_y));
        //        transforms.Children.Add(new RotateTransform(angle));
        //        brush.Opacity = opacity;
        //        brush.RelativeTransform = transforms;
        //        drawingContext.DrawRectangle(brush, null, new Rect(0, 0, 192, 192));
        //    }


        //    foreach (var timing in animation.timings)
        //    {
        //        if (timing.frame == frame_index)
        //            animation_process_timing(timing, this._animation_hit);
        //    }
        //}


    }
}
