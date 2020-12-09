using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.EventCommand
    //事件指令的数据类。

    //父类Object 
    //参照元RPG.Event.Page 
    //RPG.Troop.Page 
    //RPG.CommonEvent 
    public class EventCommand
    {

        //属性code 
        //事件代码。
        public int code { get; set; }

        //indent 
        //缩进的深度。通常为 0，「条件分歧」等指令每加深一层其值 +1。
        public int indent { get; set; }
        //parameters 
        //包含了事件指令参数的数组。其内容每个指令都不同。
        public List<object> parameters { get; set; }

        public EventCommand()
        {

        }
        //定义module RPG
        public EventCommand(int code = 0, int indent = 0, List<object> parameters = null)
        {
            if (parameters == null)
                parameters = new List<object>();
            this.code = code;
            this.indent = indent;
            this.parameters = parameters;
        }
    }
}
