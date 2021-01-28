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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace XP.Test
{
    /// <summary>
    /// TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
        }

        RPG.Animation animation;
        RPG.Sprite sprite;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //animation = (Utility.XML<List<RPG.Animation>>.Read("Xml/Data/Animations.xml"))[1];
            //sprite = new RPG.Sprite();
            //sprite.bitmap = RPG.Cache.battler("019-Thief04.png", 0);
            //sprite.width = 120;
            //sprite.height = 165;

            //Border border = new Border()
            //{
            //    BorderThickness = new Thickness(2),
            //    BorderBrush = Brushes.White,
            //    Child = sprite,
            //};
            //this.grid.Children.Add(border);

            //var  timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromMilliseconds(40);
            //timer.Tick += GameTick;
            //timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sprite.animation(animation, false);
        }

        private void GameTick(object sender, EventArgs e)
        {
            var count = this.grid.Children.Count;
            var vi = this.sprite.Visibility;
            var op = this.sprite.Opacity;
            sprite.update();
        }
    }
}
