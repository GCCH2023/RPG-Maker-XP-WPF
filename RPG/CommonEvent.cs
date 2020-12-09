using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.CommonEvent
    //公共事件的数据类。

    //父类Object 

    public class CommonEvent
    {
        //属性id 
        //ID。
        public int id { get; set; }
        //name 
        //名称。
        public string name { get; set; }
        //trigger 
        //触发（0：无，1：自动执行，2：并行处理）。
        public int trigger { get; set; }
        //switch_id 
        //条件开关的 ID。
        public int switch_id { get; set; }
        //list 
        //执行内容。RPG.EventCommand 的数组。
        public List<EventCommand> list { get; set; }
        //定义module RPG
        public CommonEvent()
        {
            this.id = 0;
            this.name = "";
            this.trigger = 0;
            this.switch_id = 1;
            this.list = new List<EventCommand>() { new RPG.EventCommand() };
        }

    }
}
