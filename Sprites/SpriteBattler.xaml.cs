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

namespace XP
{
    /// <summary>
    /// SpriteBattler.xaml 的交互逻辑
    /// </summary>
    public partial class SpriteBattler : UserControl
    {
        public SpriteBattler()
        {
            InitializeComponent();
        }

         public Game_Battler battler;                  // 战斗者

        public int battler_hue { get; set; }

        public int state_animation_id { get; set; }

        bool _battler_visible;

        public bool battler_visible
        {
            get { return _battler_visible; }
            set { _battler_visible = value; }
        }

        public SpriteBattler(Viewport viewport, Game_Battler battler = null)
        {
            Global.AddUIElement(this);

            this.battler = battler;

            this.battler_visible = false;

            InitializeComponent();
        }

         //--------------------------------------------------------------------------
        // ● 释放
        //--------------------------------------------------------------------------
        public void dispose()
        {
           
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public void update()
        {
            //base.update();
            // 战斗者为 null 的情况下
            if (this.battler == null)
            {
                //this.bitmap = null;
                //loop_animation(null);
                return;
            }
            // 动画 ID 与当前的情况有差异的情况下
            if (this.battler.damage == null && this.battler.state_animation_id != this.state_animation_id)
            {
                this.state_animation_id = this.battler.state_animation_id;
                
                //loop_animation(Global.data_animations[this.state_animation_id]);
            }
            // 应该被显示的角色的情况下
            if ((this.battler is Game_Actor) && this.battler_visible)
            {
                // 不是主状态的时候稍稍降低点透明度
                if (Global.game_temp.battle_main_phase)
                {
                    this.Opacity += 0.01;
                    //if (this.opacity < 255)
                    //    this.opacity += 3;
                }
                else
                {
                    //if (this.opacity > 207)
                    //    this.opacity -= 3;
                    this.Opacity -= 0.01;
                }
            }
            // 明灭
            //if (this.battler.blink)
            //    BlinkOn();
            //else
            //    BlinkOff();

            // 不可见的情况下
            if (!this.battler_visible)
            {
                // 出现
                if (!this.battler.hidden && !this.battler.is_dead &&
                            (this.battler.damage == null || this.battler.damage_pop))
                {
                    //appear();
                    PlayStoryboard("appear");
                    this.battler_visible = true;
                }
            }
            // 可见的情况下
            if (this.battler_visible)
            {
                // 逃跑
                if (this.battler.hidden)
                {
                    Global.game_system.se_play(Global.data_system.escape_se);
                    // escape();
                    PlayStoryboard("escape");
                    this.battler_visible = false;
                }
                // 白色闪烁
                if (this.battler.white_flash)
                {
                    // whiten();
                    PlayStoryboard("appear");
                    this.battler.white_flash = false;
                }
                // 动画
                if (this.battler.animation_id != 0)
                {
                    //var animation1 = Global.data_animations[this.battler.animation_id];
                    //animation(animation1, this.battler.animation_hit);
                    //this.battler.animation_id = 0;
                }
                // 伤害
                if (this.battler.damage_pop)
                {
                    DamageAnimation(this.battler.damage, this.battler.critical);
                }
                // korapusu
                if (this.battler.damage == null && this.battler.is_dead)
                {
                    if (this.battler is Game_Enemy)
                        Global.game_system.se_play(Global.data_system.enemy_collapse_se);
                    else
                        Global.game_system.se_play(Global.data_system.actor_collapse_se);
                    // collapse();
                     ((Storyboard)this.Resources["collapse"]).Begin();
                    this.battler_visible = false;
                }
            }
            // 设置活动块的坐标
            //this.x = this.battler.screen_x;
            //this.y = this.battler.screen_y;
            //this.z = this.battler.screen_z;
            Canvas.SetLeft(this, this.battler.screen_x);
            Canvas.SetTop(this, this.battler.screen_y);
            Canvas.SetZIndex(this, this.battler.screen_z);
        }
        // 伤害动画
        private void DamageAnimation(object value, bool critical)
        {
            string damage_string;
            if (value is double)
                damage_string = Math.Abs((double)value).ToString();
            else
                damage_string = value.ToString();


            if ((value is double) && (double)value < 0)
                this.damageTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(176, 255, 144));
            else
                this.damageTextBlock.Foreground = Brushes.White;

            // 暴击
            if (critical)
                this.damageTextBlock.Text = "暴击\n" + damage_string;
            else
                this.damageTextBlock.Text = damage_string;

            Canvas.SetLeft(this.damageTextBlock, 20);
            Canvas.SetTop(this.damageTextBlock, 40);
            this.damageTextBlock.Visibility = Visibility.Visible;
            PlayStoryboard("damage");

            this.battler.damage = null;
            this.battler.critical = false;
            this.battler.damage_pop = false;
        }
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            Image.SourceProperty.AddOwner(typeof(SpriteBattler));

        public bool Blink
        {
            get { return (bool)GetValue(BlinkProperty); }
            set { SetValue(BlinkProperty, value); }
        }

        public static readonly DependencyProperty BlinkProperty =
            DependencyProperty.Register("Blink", typeof(bool), typeof(SpriteBattler),
            new PropertyMetadata(false, new PropertyChangedCallback(BlinkChanged)));

        private static void BlinkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sprite = d as SpriteBattler;
            if ((bool)e.NewValue)
                sprite.BlinkOn();
            else
                sprite.BlinkOff();
        }



        public string BattlerFileName
        {
            get { return (string)GetValue(BattlerFileNameProperty); }
            set { SetValue(BattlerFileNameProperty, value); }
        }

        public static readonly DependencyProperty BattlerFileNameProperty =
            DependencyProperty.Register("BattlerFileName", typeof(string), typeof(SpriteBattler),
            new PropertyMetadata(null, new PropertyChangedCallback(BattlerFileNameChanged)));

        private static void BattlerFileNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sprite = d as SpriteBattler;

            // 获取、设置位图
           var battler_name = (string)e.NewValue;
            sprite.battler_hue = sprite.battler.battler_hue;
            var bitmap = RPG.Cache.Battler(battler_name);
            sprite.Background = new ImageBrush()
            {
                ImageSource = bitmap
            };
            sprite.ImageSource = bitmap;

            sprite.Width = bitmap.PixelWidth;
            sprite.Height = bitmap.PixelHeight;

            sprite.RenderTransform = new TranslateTransform(-sprite.Width / 2, -sprite.Height);

            // 如果是战斗不能或者是隐藏状态就把透明度设置成 0
            if (sprite.battler.is_dead || sprite.battler.hidden)
            {
                //sprite.opacity = 0;
                sprite.Visibility = Visibility.Hidden;
            }
        }



        private void PlayStoryboard(string name)
        {
            storyboardCompleted = false;
            storyboard = (Storyboard)this.Resources[name];
            storyboard.Completed += StoryboardCompleted;
            storyboard.Begin();
        }
        Storyboard storyboard = null;
        bool storyboardCompleted = true;
        private void StoryboardCompleted(object sender, EventArgs e)
        {
            storyboardCompleted = true;
            storyboard.Completed -= StoryboardCompleted;

            this.damageTextBlock.Visibility = Visibility.Hidden;
        }

        private void BlinkOn()
        {
            ((Storyboard)this.Resources["blink"]).Begin();
        }

        private void BlinkOff()
        {
            ((Storyboard)this.Resources["blink"]).Stop();
        }

        public bool is_effect
        {
            get
            {
                return !this.storyboardCompleted;
            }
        }

        private void SpriteBattler_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetBinding(BlinkProperty, new Binding() { Source = this.battler, Path = new PropertyPath("blink") });
            this.SetBinding(BattlerFileNameProperty, new Binding() { Source = this.battler, Path = new PropertyPath("battler_name") });
        }
    }
}
