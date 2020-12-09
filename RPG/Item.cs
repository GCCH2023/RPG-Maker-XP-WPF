using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.Item
    //物品的数据类。

    //父类Object 
    public class Item : Goods
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
        //scope 
        //效果范围（0：无，1：敌单体，2：敌全体，3：己方单体，4：己方全体，5：己方单体（HP 0），6：己方全体（HP 0），7：使用者）。
        public int scope { get; set; }
        //occasion 
        //可能使用时（0：平时，1：战斗中，2：菜单中，3：不能使用）。
        public int occasion { get; set; }
        //animation1_id 
        //使用方的动画 ID。
        public int animation1_id { get; set; }
        //animation2_id 
        //对象方的动画 ID。
        public int animation2_id { get; set; }
        //menu_se 
        //菜单画面使用时的 SE（RPG.AudioFile）。
        public RPG.AudioFile menu_se { get; set; }
        //common_event_id 
        //公共事件 ID。
        public int common_event_id { get; set; }
        //price 
        //价格。
        //public int price { get; set; }
        //consumable 
        //是否消耗的真伪值。
        public bool consumable { get; set; }
        //parameter_type 
        //参数（0：无，1：MaxHP，2：MaxSP，3：力量，4：灵巧，5：速度，6：魔力）。
        public int parameter_type { get; set; }
        //parameter_points 
        //参数的上升量。
        public int parameter_points { get; set; }
        //recover_hp_rate 
        //HP 回复率。
        public int recover_hp_rate { get; set; }
        //recover_hp 
        //HP 回复量。
        public int recover_hp { get; set; }
        //recover_sp_rate 
        //SP 回复率。
        public int recover_sp_rate { get; set; }
        //recover_sp 
        //SP 回复量。
        public int recover_sp { get; set; }
        //hit 
        //命中率。
        public int hit { get; set; }
        //pdef_f 
        //物理防御 F。
        public int pdef_f { get; set; }
        //mdef_f 
        //魔法防御 F。
        public int mdef_f { get; set; }
        //variance 
        //分散度。
        public int variance { get; set; }
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
        public Item()
        {
            this.id = 0;
            this.name = "";
            this.icon_name = "";
            this.description = "";
            this.scope = 0;
            this.occasion = 0;
            this.animation1_id = 0;
            this.animation2_id = 0;
            this.menu_se = new RPG.AudioFile("", 80);
            this.common_event_id = 0;
            this.price = 0;
            this.consumable = true;
            this.parameter_type = 0;
            this.parameter_points = 0;
            this.recover_hp_rate = 0;
            this.recover_hp = 0;
            this.recover_sp_rate = 0;
            this.recover_sp = 0;
            this.hit = 100;
            this.pdef_f = 0;
            this.mdef_f = 0;
            this.variance = 0;
            this.element_set = new List<int>();
            this.plus_state_set = new List<int>();
            this.minus_state_set = new List<int>();
        }
    }
}
