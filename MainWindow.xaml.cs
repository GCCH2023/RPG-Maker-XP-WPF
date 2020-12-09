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
using System.Windows.Threading;
using XP.Internal;

namespace XP
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        // 游戏面板
        public Panel GameBoard
        {
            get
            {
                return this.gameBoard;
            }
        }

        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Global.data_tilesets = (List<RPG.Tileset>)Marshal.Load("Data/Tilesets.rxdata");
            //MessageBox.Show(Global.data_tilesets[1].autotile_names.Count.ToString());
            //Utility.XML<List<RPG.Tileset>>.Write(Global.data_tilesets, "Xml\\Data\\Tilesets.xml");

            //Global.data_tilesets = Utility.XML<List<RPG.Tileset>>.Read("Xml/Data/Tilesets.xml");
            ////this.tilemap.image = new BitmapImage(new Uri("Graphics/Tilesets/" + Global.data_tilesets[1].tileset_name + ".png", UriKind.Relative));
            //this.tilemap.tileset = RPG.Cache.tileset(Global.data_tilesets[1].tileset_name);
            //var map = (RPG.Map)Global.load_data("Data/Map001.rxdata");
            //this.tilemap.map_data = map.data; 

            //var control = Application.Current.FindResource(typeof(ListBoxItem));
            //using (System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(@"d:\defaultTemplate.xml", System.Text.Encoding.UTF8))
            //{
            //    writer.Formatting = System.Xml.Formatting.Indented;
            //    System.Windows.Markup.XamlWriter.Save(control, writer);
            //}

            //w.border.x = 100;
            //.RenderTransform = new TranslateTransform(100, 100);

            //Global.data_tilesets = (List<RPG.Tileset>)Global.load_data("Data/Tilesets.rxdata");
            //Utility.XML<List<RPG.Tileset>>.Write(Global.data_tilesets, "Xml\\Data\\Tilesets.xml");

            //Global.TransferGameFile();
            //MessageBox.Show("转换数据文件完成");

            sprite.flash(Colors.Red, 2);
        }
        Sprite sprite;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //sprite = new Sprite();
            //sprite.bitmap = new Bitmap(@"E:\C#项目\RPG Maker\XP\Graphics\Characters\008-Fighter08.png");
            //this.gameBoard.Children.Add(sprite);

            //plane = new Plane();
            //plane.bitmap = new Bitmap(@"E:\C#项目\RPG Maker\XP\girl.png");
            //this.gameBoard.Children.Add(plane);

            //tilemap = new Tilemap();
            //Table t = new Table(8, 18, 3);
            //var k = 0;
            //for (int i = 0; i < t.ysize; i++)
            //{
            //    for (int j = 0; j < t.xsize; j++)
            //    {
            //        t[j, i, 0] = 384 + k++;
            //    }
            //}
            //tilemap.map_data = t;
            //tilemap.tileset = new Bitmap(@"E:\C#项目\RPG Maker\XP\Graphics\Tilesets\001-Grassland01.png");
            //this.gameBoard.Children.Add(tilemap);


            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += GameTick;
            timer.Start();

            //准备过渡
            //设置系统默认字体
            Font.default_name = "黑体";
            Graphics.freeze();
            // 生成场景对像 (标题画面)
            Global.scene = new Scene_Title();

            // Global.scene 为有效的情况下调用 main 过程
            //while (Global.scene != null)
                //Global.scene.main();

            // 淡入淡出
            // Graphics.transition(20);


            //var sprite = new Internal.Sprite();
            //sprite.bitmap = new Bitmap(@"D:\RPG Maker XP\RGSS\Standard\Graphics\Titles\001-Title01.jpg");

            //var bitmap = new Bitmap(@"win.jpg");
            //var sprite1 = new Internal.Sprite(new Viewport(100, 100, 100, 100));
            //sprite1.bitmap = bitmap;
            //bitmap.clear();
            //// sprite.bitmap = bitmap;
            //sprite.bitmap.stretch_blt(new Rect(0, 0, 100, 100), bitmap, new Rect(0, 0, 200, 200), 128);
            //for(int i = 0;i<10;i++)
            //{
            //    for(int j = 0;j<10;j++)
            //    {
            //        bitmap.set_pixel(i, j, new Color(255, 0, 0));
            //    }
            //}
            //bitmap.draw_text(100, 100, 200, 50, "Hello");
            /*Main.Begin();*/
        }
        /// <summary>
        /// 处理一次游戏逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameTick(object sender, EventArgs e)
        {
            Graphics.frame_count++;
            if(Global.scene != null)
                Global.scene.main();
        }
    }
}
