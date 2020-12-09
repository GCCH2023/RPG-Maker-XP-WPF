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

namespace XP.Test
{
    /// <summary>
    /// AutoTile.xaml 的交互逻辑
    /// </summary>
    public partial class AutoTile : UserControl
    {
        public AutoTile()
        {
            InitializeComponent();
        }


        private void Update()
        {
            var image = this.ImageSource;
            if (image == null)
                return;

            var tile = this.Tile;
            if (tile < 0 || tile >= 48)
                return;

            var no = Internal.AutoTileInfo.Tiles[tile, 0];
            this.rect1.Fill = new ImageBrush()
            {
                ImageSource = image,
                ViewboxUnits = BrushMappingMode.Absolute,
                Viewbox = new Rect(no % 6 * 16, no / 6 * 16, 16, 16)
            };

            no = Internal.AutoTileInfo.Tiles[tile, 1];
            this.rect2.Fill = new ImageBrush()
            {
                ImageSource = image,
                ViewboxUnits = BrushMappingMode.Absolute,
                Viewbox = new Rect(no % 6 * 16, no / 6 * 16, 16, 16)
            };

            no = Internal.AutoTileInfo.Tiles[tile, 2];
            this.rect3.Fill = new ImageBrush()
            {
                ImageSource = image,
                ViewboxUnits = BrushMappingMode.Absolute,
                Viewbox = new Rect(no % 6 * 16, no / 6 * 16, 16, 16)
            };

            no = Internal.AutoTileInfo.Tiles[tile, 3];
            this.rect4.Fill = new ImageBrush()
            {
                ImageSource = image,
                ViewboxUnits = BrushMappingMode.Absolute,
                Viewbox = new Rect(no % 6 * 16, no / 6 * 16, 16, 16)
            };
        }


        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource),
            typeof(AutoTile), new PropertyMetadata(null, new PropertyChangedCallback(ImageSourceChanged)));

        private static void ImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoTile)d).Update();
        }



        public int Tile
        {
            get { return (int)GetValue(TileProperty); }
            set { SetValue(TileProperty, value); }
        }

        public static readonly DependencyProperty TileProperty =
            DependencyProperty.Register("Tile", typeof(int), typeof(AutoTile),
            new PropertyMetadata(0, new PropertyChangedCallback(TileChanged)));

        private static void TileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoTile)d).Update();
        }


    }
}
