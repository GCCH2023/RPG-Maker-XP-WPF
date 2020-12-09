using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.MoveRoute
    //移动路线的数据类。

    //父类Object 

    public class MoveRoute
    {
        //参照元RPG.EventPage 
        //属性repeat 
        //选项「重复运作」的真伪值。
        public bool repeat { get; set; }
        //skippable 
        //选项「忽略不能移动的场合」的真伪值。
        public bool skippable { get; set; }
        //list 
        //执行内容。RPG.MoveCommand 的数组。
        public List<MoveCommand> list { get; set; }
        //定义module RPG
        public MoveRoute()
        {
            this.repeat = true;
            this.skippable = false;
            this.list = new List<MoveCommand>() { new RPG.MoveCommand() };
        }


    }
}
