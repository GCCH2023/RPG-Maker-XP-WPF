using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    // RPG.Armor
    //防具的数据类。
    //父类Object 

    public class Armor : Goods
    {
        ////属性id 
        ////ID。
        //public int id { get; set; }
        ////name 
        ////名称。
        //public string name { get; set; }
        ////icon_name 
        ////图标图像的文件名。
        //public string icon_name { get; set; }
        ////description 
        ////说明。
        //public string description { get; set; }
        //kind 
        //种类（0：盾，1：头部防具，2：身体防具，3：装饰品）。
        //public int kind { get; set; }
        //auto_state_id 
        //自动状态的 ID。
        public int auto_state_id { get; set; }
        //price 
        //价格。
        //public int price { get; set; }
        //pdef 
        //物理防御。
        public int pdef { get; set; }
        //mdef 
        //魔法防御。
        public int mdef { get; set; }
        //eva 
        //回避修正。
        public int eva { get; set; }
        //str_plus 
        //力量+。
        public int str_plus { get; set; }
        //dex_plus 
        //灵巧+。
        public int dex_plus { get; set; }
        //agi_plus 
        //速度+。
        public int agi_plus { get; set; }
        //int_plus 
        //魔力+。
        public int int_plus { get; set; }
        //guard_element_set 
        //属性防御。为属性 ID 的数组。
        public List<int> guard_element_set { get; set; }
        //guard_state_set 
        //状态防御。为状态 ID 的数组。
        public List<int> guard_state_set { get; set; }
        //定义module RPG
        public Armor()
        {

            this.id = 0;
            this.name = "";
            this.icon_name = "";
            this.description = "";
            this.kind = 0;
            this.auto_state_id = 0;
            this.price = 0;
            this.pdef = 0;
            this.mdef = 0;
            this.eva = 0;
            this.str_plus = 0;
            this.dex_plus = 0;
            this.agi_plus = 0;
            this.int_plus = 0;
            this.guard_element_set = new List<int>();
            this.guard_state_set = new List<int>();
        }
        // 附加
        public List<int> is_guard_element_set { get; set; }
    }
}
