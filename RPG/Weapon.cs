using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.Weapon
    //武器的数据类。

    //父类Object 
    public class Weapon : Goods
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
        //animation1_id 
        //攻击方的动画 ID。
        public int animation1_id { get; set; }
        //animation2_id 
        //对象方的动画 ID。
        public int animation2_id { get; set; }
        //price 
        //价格。
        //public int price { get; set; }
        //atk 
        //攻击力。
        public int atk { get; set; }
        //pdef 
        //物理防御。
        public int pdef { get; set; }
        //mdef 
        //魔法防御。
        public int mdef { get; set; }
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
        //element_set 
        //属性。为属性 ID 的数组。
        public List<int> element_set { get; set; }
        //plus_state_set 
        //附加状态。为状态 ID 的数组。
        public List<int> plus_state_set { get; set; }
        //minus_state_set 
        //解除状态。为状态 ID 的数组。
        public List<int> minus_state_set { get; set; }
        //定义module RPG
        public Weapon()
        {
            this.id = 0;
            this.name = "";
            this.icon_name = "";
            this.description = "";
            this.animation1_id = 0;
            this.animation2_id = 0;
            this.price = 0;
            this.atk = 0;
            this.pdef = 0;
            this.mdef = 0;
            this.str_plus = 0;
            this.dex_plus = 0;
            this.agi_plus = 0;
            this.int_plus = 0;
            this.element_set = new List<int>();
            this.plus_state_set = new List<int>();
            this.minus_state_set = new List<int>();
        }

    }
}
