using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.MapInfo
    //地图信息的数据类。

    //父类Object 

    public class MapInfo
    {
        //属性name 
        //名称。
        public string name { get; set; }
        //parent_id 
        //母地图的 ID。
        public int parent_id { get; set; }
        //order 
        //Maker 内部使用的地图树表示顺序。
        public int order { get; set; }
        //expanded 
        //Maker 内部使用的地图树展开标记。
        public bool expanded { get; set; }
        //scroll_x 
        //Maker 内部使用的 X 方向的滚动位置。
        public int scroll_x { get; set; }
        //scroll_y 
        //Maker 内部使用的 Y 方向的滚动位置。
        public int scroll_y { get; set; }
        //定义module RPG
        public MapInfo()
        {
            this.name = "";
            this.parent_id = 0;
            this.order = 0;
            this.expanded = false;
            this.scroll_x = 0;
            this.scroll_y = 0;
        }

    }
}
