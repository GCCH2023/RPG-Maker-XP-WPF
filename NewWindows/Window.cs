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

namespace XP.NewWindows
{
    public class Window : ContentControl
    {
        static Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Window),
                new FrameworkPropertyMetadata(typeof(Window)));
        }


        /// <summary>
        /// 窗口的皮肤图像
        /// </summary>
        public ImageSource WindowSkin
        {
            get { return (ImageSource)GetValue(WindowSkinProperty); }
            set { SetValue(WindowSkinProperty, value); }
        }

        public static readonly DependencyProperty WindowSkinProperty =
            DependencyProperty.Register("WindowSkin", typeof(ImageSource),
            typeof(Window), new PropertyMetadata(null));

        /// <summary>
        /// 是否暂停状态，暂停状态则显示暂停动画
        /// </summary>
        public bool IsPause
        {
            get { return (bool)GetValue(pauseProperty); }
            set { SetValue(pauseProperty, value); }
        }

        public static readonly DependencyProperty pauseProperty =
            DependencyProperty.Register("IsPause", typeof(bool), typeof(Window),
            new PropertyMetadata(false, new PropertyChangedCallback(PausePropertyChanged)));

        private static void PausePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            window.UpdatePauseArrow();
        }

        //IsActive 
        //光标的闪烁状态。真为闪烁。
        public bool IsActive
        {
            get { return (bool)GetValue(activeProperty); }
            set { SetValue(activeProperty, value); }
        }

        public static readonly DependencyProperty activeProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(Window),
            new PropertyMetadata(false, new PropertyChangedCallback(ActivePropertyChanged)));

        private static void ActivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            window.UpdateCursor();
        }


        /// <summary>
        /// 光标的位置和大小
        /// </summary>
        public System.Windows.Rect CursorRect
        {
            get { return (System.Windows.Rect)GetValue(CursorRectProperty); }
            set { SetValue(CursorRectProperty, value); }
        }

        public static readonly DependencyProperty CursorRectProperty =
            DependencyProperty.Register("CursorRect", typeof(System.Windows.Rect),
            typeof(Window), new PropertyMetadata(new System.Windows.Rect(0, 0, 0, 0),
                new PropertyChangedCallback(OnCursorRectChanged)));

        private static void OnCursorRectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;

            if (window.cursor == null)
                return;

            window.UpdateCursor();
        }
        /// <summary>
        /// 使用CursorRect更新光标的大小和位置以及闪烁动画
        /// </summary>
        private void UpdateCursor()
        {
            var rect = CursorRect;

            this.cursor.Width = rect.Width;
            this.cursor.Height = rect.Height;
            Canvas.SetLeft(this.cursor, rect.X);
            Canvas.SetTop(this.cursor, rect.Y);

            if (IsActive)
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
        private void UpdatePauseArrow()
        {
            if (IsPause)
            {
                this.PlayPauseArrowAnimation();
                this.pauseArrow.Visibility = Visibility.Visible;
            }
            else
            {
                this.StopPauseArrowAnimation();
                this.pauseArrow.Visibility = Visibility.Hidden;
            }
        }
        private Storyboard pauseArrowStoryboard = new Storyboard();
        /// <summary>
        /// 播放光标闪烁动画
        /// </summary>
        private void PlayPauseArrowAnimation()
        {
            if (this.pauseImageBrush == null)
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

                Storyboard.SetTarget(pauseAnimation, this.pauseImageBrush);
                Storyboard.SetTargetProperty(pauseAnimation, new PropertyPath("Viewbox"));
                this.pauseImageBrush.BeginAnimation(ImageBrush.ViewboxProperty, pauseAnimation);
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


        // 光标元素
        private Internal.SlicedSprite cursor = null;
        // 暂停标记
        private Rectangle pauseArrow = null;
        private ImageBrush pauseImageBrush = null;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.cursor = GetTemplateChild("cursor") as Internal.SlicedSprite;

            UpdateCursor();

            this.pauseArrow = GetTemplateChild("pauseArrow") as Rectangle;
            this.pauseImageBrush = GetTemplateChild("pauseBrush") as ImageBrush;

            UpdatePauseArrow();
        }
    }
}
