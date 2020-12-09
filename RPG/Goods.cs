using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    public class Goods
    {
        //属性id 
        //ID。
        public int id { get; set; }
        //name 
        //名称。
        public string name { get; set; }
        //icon_name 
        //图标图像的文件名。
        public string icon_name { get; set; }
        //description 
        //说明。
        public string description { get; set; }
        //price 
        //价格。
        public int price { get; set; }

        public int kind { get; set; }
    }
}
