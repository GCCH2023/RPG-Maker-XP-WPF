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

namespace XP
{
    //==============================================================================
    // ■ ArrowBase
    //------------------------------------------------------------------------------
    // 　在战斗画面使用的箭头光标的活动块。本类作为
    // ArrowEnemy 类与 ArrowActor 类的超级类使用。
    //==============================================================================
    public class ArrowBase : Control
    {
        static ArrowBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ArrowBase), new FrameworkPropertyMetadata(typeof(ArrowBase)));
        }

        public ArrowBase()
        {
            this.Loaded += ArrowBaseLoaded;
        }

        private void ArrowBaseLoaded(object sender, RoutedEventArgs e)
        {
            if(Global.game_system != null)
                this.ImageSource = RPG.Cache.WindowSkin(Global.game_system.windowskin_name);
        }

        /// <summary>
        /// 提供箭头图案的图像（窗口皮肤图片文件）
        /// </summary>
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource),
            typeof(ArrowBase), new PropertyMetadata(null));


        /// <summary>
        /// 是否播放动画
        /// </summary>
        public bool AnimationActive
        {
            get { return (bool)GetValue(AnimationActiveProperty); }
            set { SetValue(AnimationActiveProperty, value); }
        }

        public static readonly DependencyProperty AnimationActiveProperty =
            DependencyProperty.Register("AnimationActive", typeof(bool), typeof(ArrowBase),
            new PropertyMetadata(false));
    }
}
