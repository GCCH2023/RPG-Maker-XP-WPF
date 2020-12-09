using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.Event
    //地图事件的数据类。

    //父类Object 
    //参照元RPG.Map 

    public class Event
    {
        //属性id 
        //ID。
        public int id { get; set; }
        //name 
        //名称。
        public string name { get; set; }
        //x 
        //地图 X 座标。
        public int x { get; set; }
        //y 
        //地图 Y 座标。
        public int y { get; set; }
        //pages 
        //事件页。RPG.Event.Page 的数组。
        public List<Page> pages { get; set; }
        //内部类RPG.Event.Page 
        //定义module RPG
        public Event()
        {
            this.id = 0;
            this.name = "";
            this.x = x;
            this.y = y;
            this.pages = new List<Page>() { new RPG.Event.Page() };
        }

        //        RPG.Event.Page
        //事件页的数据类。

        //父类Object 
        //参照元RPG.Event 
        public class Page
        {
            //属性condition 
            //条件（RPG.Event.Page.Condition）。
            public Condition condition { get; set; }
            //graphic 
            //图像（RPG.Event.Page.Graphic） 。
            public Graphic graphic { get; set; }
            //move_type 
            //移动类型。（0：固定，1：随机，2：接近，3：自定义）。
            public int move_type { get; set; }
            //move_speed 
            //移动速度。（1：Slowest，2：Slower，3：Slow，4：Fast，5：Faster，6：Fastest）。
            public int move_speed { get; set; }
            //move_frequency 
            //移动频度。（1：Lowest，2：Lower，3：Low，4：High，5：Higher，6：Highest）。
            public int move_frequency { get; set; }
            //move_route 
            //移动路线（RPG.MoveRoute）。仅限于移动类型为自定义时调用。
            public MoveRoute move_route { get; set; }
            //walk_anime 
            //选项「移动时动画」的真伪值。
            public bool walk_anime { get; set; }
            //step_anime 
            //选项「停止时动画」的真伪值。
            public bool step_anime { get; set; }
            //direction_fix 
            //选项「固定朝向」的真伪值。
            public bool direction_fix { get; set; }
            //through 
            //选项「充许穿透」的真伪值。
            public bool through { get; set; }
            //always_on_top 
            //选项「在最前面显示」的真伪值。
            public bool always_on_top { get; set; }
            //trigger 
            //触发条件（0：决定键，1：与主角接触，2：与事件接触，3：自动执行，4：并行处理）。
            public int trigger { get; set; }
            //list 
            //执行内容。RPG.EventCommand 的数组。
            public List<EventCommand> list { get; set; }
            //内部类RPG.Event.Page.Condition 
            //RPG.Event.Page.Graphic 
            //定义module RPG
            public Page()
            {
                this.condition = new RPG.Event.Page.Condition();
                this.graphic = new RPG.Event.Page.Graphic();
                this.move_type = 0; ;
                this.move_speed = 3; ;
                this.move_frequency = 3; ;
                this.move_route = new RPG.MoveRoute(); ;
                this.walk_anime = true; ;
                this.step_anime = false; ;
                this.direction_fix = false; ;
                this.through = false; ;
                this.always_on_top = false; ;
                this.trigger = 0; ;
                this.list = new List<EventCommand>() { new RPG.EventCommand() };

            }
            //RPG.Event.Page.Condition
            //事件页「条件」的数据类。

            //父类Object 
            //参照元RPG.Event.Page 
            public class Condition
            {
                //属性switch1_valid 
                //表示条件「开关」（第一个）是否有效的真伪值。
                public bool switch1_valid { get; set; }
                //switch2_valid 
                //表示条件「开关」（第二个）是否有效的真伪值。
                public bool switch2_valid { get; set; }
                //variable_valid 
                //表示条件「变量」是否有效的真伪值。
                public bool variable_valid { get; set; }
                //self_switch_valid 
                //表示条件「独立开关」是否有效的真伪值。
                public bool self_switch_valid { get; set; }
                //switch1_id 
                //条件「开关」（第一个）有效时，其开关 ID。
                public int switch1_id { get; set; }
                //switch2_id 
                //条件「开关」（第二个）有效时，其开关 ID。
                public int switch2_id { get; set; }
                //variable_id 
                //条件「变量」有效时，其变量 ID。
                public int variable_id { get; set; }
                //variable_value 
                //条件「变量」有效时，其变量的标准值（x 以上）。
                public int variable_value { get; set; }
                //self_switch_ch 
                //条件「独立开关」有效时，其文字（"A".."B"）。
                public string self_switch_ch { get; set; }
                //定义module RPG
                public Condition()
                {
                    this.switch1_valid = false;
                    this.switch2_valid = false;
                    this.variable_valid = false;
                    this.self_switch_valid = false;
                    this.switch1_id = 1;
                    this.switch2_id = 1;
                    this.variable_id = 1;
                    this.variable_value = 0;
                    this.self_switch_ch = "A";

                }
            }

            // RPG.Event.Page.Graphic
            //事件页「图像」的数据类。

            //父类Object 
            //参照元RPG.Event.Page 
            public class Graphic
            {
                //属性tile_id 
                //地图元件 ID。指定图像非地图元件的话为 0。
                public int tile_id { get; set; }
                //character_name 
                //角色图像的文件名。
                public string character_name { get; set; }
                //character_hue 
                //角色图像的色相变化值（0..360）。
                public int character_hue { get; set; }
                //direction 
                //角色的朝向（2：下，4：左，6：右，8：上）。
                public int direction { get; set; }
                //pattern 
                //角色的模式（0..3）。
                public int pattern { get; set; }
                //opacity 
                //不透明度。
                public int opacity { get; set; }
                //blend_type 
                //合成方法。
                public int blend_type { get; set; }
                //定义module RPG
                public Graphic()
                {
                    this.tile_id = 0;
                    this.character_name = "";
                    this.character_hue = 0;
                    this.direction = 2;
                    this.pattern = 0;
                    this.opacity = 255;
                    this.blend_type = 0;
                }
            }
        }
    }
}