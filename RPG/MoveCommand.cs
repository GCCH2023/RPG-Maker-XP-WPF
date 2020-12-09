using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    // RPG.MoveCommand
    // 移动指令的数据类。
    // 父类Object 
    // 参照元RPG.MoveRoute 
    public class MoveCommand
    {
        public MoveCommand()
        {

        }
        //属性code 
        //移动指令代码。
        public int code { get; set; }
        //parameters 
        //包含了移动指令参数的数组。其内容每个指令都不同。
        public List<object> parameters { get; set; }
        //定义module RPG
        public MoveCommand(int code = 0, List<object> parameters = null)
        {
            if (parameters == null)
                parameters = new List<object>();
            this.code = code;
            this.parameters = parameters;
        }
    }
}
