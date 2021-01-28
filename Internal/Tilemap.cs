using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace XP.Internal
{
    // Tilemap
    // 管理元件地图的类。元件地图是 2D 游戏为显示地图特殊化了的一个概念，在内部以大量的精灵构成。
    // 需要根据ox和oy以及屏幕大小来计算需要显示的画面
    // 使用Plane来做出平移的效果
    // 先将地图绘制到DrawingVisual上，显示到Plane中
    public class Tilemap
    {
        // 六个图层
        //private DrawingVisual[] layers;
        // 使用六个平面来显示六层地图，以便滚动
        private Plane[] planes = new Plane[]
        {
            new Plane(), new Plane(), new Plane(), new Plane(), new Plane(), new Plane()
        };
        // 图块大小
        private const int tileLength = 32;
        // 横向需要显示的图块个数
        private int tileWidth = (int)Math.Ceiling(Global.ScreenWidth / tileLength);
        // 纵向需要显示的图块个数
        private int tileHeight = (int)Math.Ceiling(Global.ScreenWidth / tileLength);

        private void OnRender()
        {
            if (tileset == null || map_data == null)
                return;

            var layers = new DrawingVisual[this.planes.Length];

            // 地图图块位图
            var bitmap = this.tileset;
            // 开始绘制的左上角坐标
            double x = 0, y = 0;
            // 打开所有图层
            var dc = new DrawingContext[layers.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new DrawingVisual();
                dc[i] = layers[i].RenderOpen();
            }

            var priorities = this.priorities;
            // 根据优先级进行绘制
            for (int i = 0; i < map_data.ysize; i++)
            {
                for (int j = 0; j < map_data.xsize; j++)
                {
                    for (int k = 0; k < map_data.zsize; k++)
                    {
                        var value = (int)map_data[j, i, k];

                        if (value == 0)
                            continue;

                        // 当前元件绘制在第几层
                        var layerIndex = (int)priorities[value];

                        if (value >= 384)
                        {
                            var tileValue = value - 384;
                            var imageBrush = new VisualBrush(bitmap)
                            {
                                ViewboxUnits = BrushMappingMode.Absolute,
                                Viewbox = new Rect(tileValue % 8 * tileLength, tileValue / 8 * tileLength, tileLength, tileLength),
                            };
                            dc[layerIndex].DrawRectangle(imageBrush, null, new Rect(x, y, tileLength, tileLength));
                        }
                        else
                        {
                            // 自动图块
                            var autoTileIndex = value / 48 - 1;
                            value %= 48;
                            for (int t = 0; t < 4; t++)
                            {
                                var no = AutoTileInfo.Tiles[value, t];
                                var imageBrush = new VisualBrush(this.autotiles[autoTileIndex])
                                {
                                    ViewboxUnits = BrushMappingMode.Absolute,
                                    Viewbox = new Rect(no % 6 * 16, no / 6 * 16, 16, 16)
                                };
                                dc[layerIndex].DrawRectangle(imageBrush, null, new Rect(x + t % 2 * 16, y + t / 2 * 16, 16, 16));
                            }
                        }
                    }

                    x += 32;
                }
                x = 0;
                y += 32;
            }

            // 关闭所有图层
            for (int i = 0; i < layers.Length; i++)
            {
                // 必须在左上角和右下角绘制两点，要不然WPF会忽略掉空白区域
                dc[i].DrawRectangle(Brushes.Black, null, new Rect(0, 0, 1, 1));
                dc[i].DrawRectangle(Brushes.Black, null,
                    new Rect(this.map_data.xsize * tileLength - 1, this.map_data.ysize * tileLength - 1, 1, 1));

                dc[i].Close();
                this.planes[i].bitmap = new Bitmap(layers[i], this.map_data.xsize * tileLength, this.map_data.ysize * tileLength);
            }
        }

        //父类Object 
        //类方法Tilemap.new([viewport]) 
        //生成 Tilemap 对象。必须要指定对应的视口（Viewport）。
        public Tilemap(Viewport viewport = null)
        {
            this._viewport = viewport;

            int i = 32;
            foreach (var plane in this.planes)
            {
                plane.z = i;
                Global.AddUIElement(plane);
                i += 32;
            }
            // 从第二层开始，每层z值递增32，第二层的z值为64
            // 第一层的z值为0
            this.planes[0].z = 0;

            Global.PreviewKeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var key = (int)System.Windows.Input.Key.F1;
            switch(e.Key)
            {
                case System.Windows.Input.Key.F1:
                case System.Windows.Input.Key.F2:
                case System.Windows.Input.Key.F3:
                case System.Windows.Input.Key.F4:
                case System.Windows.Input.Key.F5:
                case System.Windows.Input.Key.F6:
                    this.planes[(int)e.Key - key].visible = !this.planes[(int)e.Key - key].visible;
                    break;
            }
        }


        //方法dispose 
        //释放元件地图。如果已经释放的话则什么也不做。
        public void dispose()
        {
            foreach (var plane in this.planes)
                Global.RemoveUIElement(plane);
        }

        //disposed? 
        //元件地图已经释放的话则返回真。
        public bool is_disposed { get; set; }

        private Viewport _viewport;
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
                this._viewport = value;
            }
        }

        //update 
        //进行自动地图元件的动画等。该方法原则上 1 帧调用 1 次。
        public virtual void update()
        {

        }

        //属性tileset 
        //作为图块使用的位图（Bitmap）。
        Bitmap _tileset;

        public Bitmap tileset
        {
            get { return _tileset; }
            set { _tileset = value; OnRender(); }
        }
        //autotiles[index] 
        //作为号码 index（0 ～ 6）的自动地图元件使用的位图（Bitmap）。
        Bitmap[] _autotiles = new Bitmap[7];

        public Bitmap[] autotiles
        {
            get { return _autotiles; }
            set { _autotiles = value; }
        }
        //map_data 
        //对地图数据（Table）的参照。设定为横尺寸 * 纵尺寸 * 3 的 3 维数组。
        Table _map_data;

        public Table map_data
        {
            get { return _map_data; }
            set { _map_data = value; OnRender(); }
        }
        // flash_data 
        // 在模拟游戏移动范围的显示等中使用，是对闪烁数据（Table）的参照。
        // 设定为横尺寸 * 纵尺寸的 2 维数组。必须与地图数据相同尺寸。
        // 各单元是以 RGB 各 4 位显示地图元件的闪烁色。例如 0xf84，是 RGB（15,8,4）色的闪烁。
        public Table flash_data { get; set; }
        //priorities 
        //对优先级表（Table）的参照。设定为单元与地图元件 ID 相对应的 1 维数组。
        public Table priorities { get; set; }
        //visible 
        //元件地图的可见状态。真为可见。
        //public bool visible { get; set; }

        //ox 
        //元件地图传送元原点的 X 座标。根据该值变化进行滚动。
        public double ox
        {
            get
            {
                return this.planes[0].ox;
            }
            set
            {
                foreach (var plane in this.planes)
                    plane.ox = value;
            }
        }

        //oy 
        //元件地图传送元原点的 Y 座标。根据该值变化进行滚动。

        public double oy
        {
            get
            {
                return this.planes[0].oy;
            }
            set
            {
                foreach (var plane in this.planes)
                    plane.oy = value;
            }
        }

        // 备注构成元件地图的各精灵的 Z 座标是特定的固定值。
        // 优先级为 0 的地图元件的 Z 座标必定是 0。 
        // 画面上端位置的优先级为 1 的地图元件的 Z 座标是 64。 
        // 优先级每增加 1，Z 座标增加 32。 
        // 元件地图纵向滚动的话，Z 座标也一起变化。 
        // 地图上显示的人物 Z 座标，必须先决定地图元件的 Z 座标。
    }
}
