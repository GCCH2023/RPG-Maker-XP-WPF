using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.Skill
    //特技的数据类。

    //父类Object 
    public class Skill
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
        //scope 
        //效果范围（0：无，1：敌单体，2：敌全体，3：己方单体，4：己方全体，5：己方单体（HP 0），6：己方全体（HP 0），7：使用者）。
        public int scope { get; set; }
        //occasion 
        //可使用的场合（0：平时，1：战斗中，2：菜单中，3：不能使用）。
        public int occasion { get; set; }
        //animation1_id 
        //使用方的动画 ID。
        public int animation1_id { get; set; }
        //animation2_id 
        //对象方的动画 ID。
        public int animation2_id { get; set; }
        //menu_se 
        //菜单画面使用时的 SE（RPG.AudioFile）。
        public AudioFile menu_se { get; set; }
        //common_event_id 
        //公共事件 ID。
        public int common_event_id { get; set; }
        //sp_cost 
        //消费 SP。
        public int sp_cost { get; set; }
        //power 
        //威力。
        public int power { get; set; }
        //atk_f 
        //攻击力 F。
        public int atk_f { get; set; }
        //eva_f 
        //回避 F。
        public int eva_f { get; set; }
        //str_f 
        //力量 F。
        public int str_f { get; set; }
        //dex_f 
        //灵巧 F。
        public int dex_f { get; set; }
        //agi_f 
        //速度 F。
        public int agi_f { get; set; }
        //int_f 
        //魔力 F。
        public int int_f { get; set; }
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
        public Skill()
        {
            this.id = 0;
            this.name = "";
            this.icon_name = "";
            this.description = "";
            this.scope = 0;
            this.occasion = 1;
            this.animation1_id = 0;
            this.animation2_id = 0;
            this.menu_se = new RPG.AudioFile("", 80);
            this.common_event_id = 0;
            this.sp_cost = 0;
            this.power = 0;
            this.atk_f = 0;
            this.eva_f = 0;
            this.str_f = 0;
            this.dex_f = 0;
            this.agi_f = 0;
            this.int_f = 100;
            this.hit = 100;
            this.pdef_f = 0;
            this.mdef_f = 100;
            this.variance = 15;
            this.element_set = new List<int>();
            this.plus_state_set = new List<int>();
            this.minus_state_set = new List<int>();
        }
    }
}
