using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.Tileset
    //图块的数据类。

    //父类Object 

    public class Tileset
    {
        //属性id 
        //ID。
        public int id { get; set; }
        //name 
        //名称。
        public string name { get; set; }
        //tileset_name 
        //图块图像的文件名。
        public string tileset_name { get; set; }
        //autotile_names 
        //自动地图元件图像的文件名数组（[0]..[6]）。
        private List<string> _autotile_names;

        public List<string> autotile_names
        {
            get { return _autotile_names; }
            set { _autotile_names = value; }
        }
        //panorama_name 
        //远景图像的文件名。
        public string panorama_name { get; set; }
        //panorama_hue 
        //远景图像的色相变化值（0..360）。
        public int panorama_hue { get; set; }
        //fog_name 
        //雾图像的文件名。
        public string fog_name { get; set; }
        //fog_hue 
        //雾图像的色相变化值（..360）。
        public int fog_hue { get; set; }
        //fog_opacity 
        //雾的不透明度。
        public int fog_opacity { get; set; }
        //fog_blend_type 
        //雾的合成方式。
        public int fog_blend_type { get; set; }
        //fog_zoom 
        //雾的放大率。
        public int fog_zoom { get; set; }
        //fog_sx 
        //雾的 SX（X 方向自动滚动速度）。
        public int fog_sx { get; set; }
        //fog_sy 
        //雾的 SY（Y 方向自动滚动速度）。
        public int fog_sy { get; set; }
        //battleback_name 
        //战斗背景图像的文件名。
        public string battleback_name { get; set; }
        //passages 
        //通行表。是包含了通行标记，草木繁茂处标记和柜台标记的一维数组（Table）。
        //取地图元件 ID 作为索引。各位的对应如下所示。

        //0x01 : 下方向通行不能。 
        //0x02 : 左方向通行不能。 
        //0x04 : 右方向通行不能。 
        //0x08 : 上方向通行不能。 
        //0x40 : 草木繁茂处标记。 
        //0x80 : 柜台标记。 
        public Table passages { get; set; }
        //priorities 
        //优先级表。是包含了优先级数据的一维数组（Table）。

        //取地图元件 ID 作为索引。

        private Table _priorities;

        public Table priorities
        {
            get { return _priorities; }
            set { _priorities = value; }
        }
        //terrain_tags 
        //地形标志表。是包含了地形标志数据的一维数组（Table）。

        //取地图元件 ID 作为索引。
        public Table terrain_tags { get; set; }
        //定义module RPG
        public Tileset()
        {
            this.id = 0;
            this.name = "";
            this.tileset_name = "";
            this.autotile_names = new List<string>();
            this.panorama_name = "";
            this.panorama_hue = 0;
            this.fog_name = "";
            this.fog_hue = 0;
            this.fog_opacity = 64;
            this.fog_blend_type = 0;
            this.fog_zoom = 200;
            this.fog_sx = 0;
            this.fog_sy = 0;
            this.battleback_name = "";
            this.passages = new Table(384);
            this.priorities = new Table(384);
            this.priorities[0] = 5;
            this.terrain_tags = new Table(384);
        }

    }
}
