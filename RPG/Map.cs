using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.Map
    //地图的数据类。

    //父类Object 
    public class Map
    {
        public Map()
        {

        }
        //属性tileset_id 
        //地图所使用图块的 ID。
        public int tileset_id { get; set; }
        //width 
        //地图的宽度。
        public int width { get; set; }
        //height 
        //地图的高度。
        public int height { get; set; }
        //autoplay_bgm 
        //表示自动切换 RPG.AudioFile 是否有效的真伪值。
        public bool autoplay_bgm { get; set; }
        //bgm 
        //自动切换 RPG.AudioFile 有效时，其 RPG.AudioFile（RPG.AudioFile）。
        public RPG.AudioFile bgm { get; set; }
        //autoplay_bgs 
        //表示自动切换 BGS 是否有效的真伪值。
        public bool autoplay_bgs { get; set; }
        //bgs 
        //自动切换 BGS 有效时，其 BGS（RPG.AudioFile）。
        public RPG.AudioFile bgs { get; set; }
        //encounter_list 
        //遇敌目录。为队伍 ID 的数组。
        public List<int> encounter_list { get; set; }
        //encounter_step 
        //遇敌步数。
        public int encounter_step { get; set; }
        //data 
        //地图数据本体。是地图元件 ID 的三维数组（Table）。
        Table _data;
        public Table data
        {
            get { return _data; }
            set { _data = value; }
        }
        //events 
        //地图事件。是以事件 ID 为主键，RPG.Event 的实例为数值的哈希表。
        public Dictionary<int, RPG.Event> events { get; set; }
        //定义module RPG
        public Map(int width, int height)
        {
            this.tileset_id = 1;
            this.width = width;
            this.height = height;
            this.autoplay_bgm = false;
            this.bgm = new RPG.AudioFile();
            this.autoplay_bgs = false;
            this.bgs = new RPG.AudioFile("", 80);
            this.encounter_list = new List<int>();
            this.encounter_step = 30;
            this.data = new Table(width, height, 3);
            this.events = new Dictionary<int, Event>();
        }

    }
}
