using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.State
    //状态的数据类。

    //父类Object 
    public class State
    {

        //属性id 
        //ID。
        public int id { get; set; }
        //name 
        //名称。
        public string name { get; set; }
        //animation_id 
        //动画 ID。
        public int animation_id { get; set; }
        //restriction 
        //限制（0：无，1：不能使用魔法，2：普通攻击敌人，3：普通攻击同伴，4：不行动）。
        public int restriction { get; set; }
        //nonresistance 
        //选项「不能抵抗」的真伪值。
        public bool nonresistance { get; set; }
        //zero_hp 
        //选项「当作 HP 为 0 的状态」的真伪值。
        public bool zero_hp { get; set; }
        //cant_get_exp 
        //选项「无法获得 EXP」的真伪值。
        public bool cant_get_exp { get; set; }
        //cant_evade 
        //选项「不能回避攻击」的真伪值。
        public bool cant_evade { get; set; }
        //slip_damage 
        //选项「连续伤害」的真伪值。
        public bool slip_damage { get; set; }
        //rating 
        //额定值（0..10）。
        public int rating { get; set; }
        //hit_rate 
        //命中率 %。
        public int hit_rate { get; set; }
        //maxhp_rate 
        //MaxHP %。
        public int maxhp_rate { get; set; }
        //maxsp_rate 
        //MaxSP %。
        public int maxsp_rate { get; set; }
        //str_rate 
        //力量 %。
        public int str_rate { get; set; }
        //dex_rate 
        //灵巧 %。
        public int dex_rate { get; set; }
        //agi_rate 
        //速度 %。
        public int agi_rate { get; set; }
        //int_rate 
        //魔力 %。
        public int int_rate { get; set; }
        //atk_rate 
        //攻击力 %。
        public int atk_rate { get; set; }
        //pdef_rate 
        //物理防御 %。
        public int pdef_rate { get; set; }
        //mdef_rate 
        //魔法防御 %。
        public int mdef_rate { get; set; }
        //eva 
        //回避修正。
        public int eva { get; set; }
        //battle_only 
        //是否战斗结束时解除的真伪值。
        public bool battle_only { get; set; }
        //hold_turn 
        //auto_release_prob 
        //hold_turn 回合经过后 auto_release_prob % 的概率解除。
        public int hold_turn { get; set; }
        public int auto_release_prob { get; set; }
        //shock_release_prob 
        //受到物理攻击后 shock_release_prob % 的概率解除。
        public int shock_release_prob { get; set; }
        //guard_element_set 
        //属性防御。为属性 ID 的数组。
        public List<int> guard_element_set { get; set; }
        //plus_state_set 
        //附加状态。为状态 ID 的数组。
        public List<int> plus_state_set { get; set; }
        //minus_state_set 
        //解除状态。为状态 ID 的数组。
        public List<int> minus_state_set { get; set; }
        //定义module RPG
        public State()
        {
            this.id = 0;
            this.name = "";
            this.animation_id = 0;
            this.restriction = 0;
            this.nonresistance = false;
            this.zero_hp = false;
            this.cant_get_exp = false;
            this.cant_evade = false;
            this.slip_damage = false;
            this.rating = 5;
            this.hit_rate = 100;
            this.maxhp_rate = 100;
            this.maxsp_rate = 100;
            this.str_rate = 100;
            this.dex_rate = 100;
            this.agi_rate = 100;
            this.int_rate = 100;
            this.atk_rate = 100;
            this.pdef_rate = 100;
            this.mdef_rate = 100;
            this.eva = 0;
            this.battle_only = true;
            this.hold_turn = 0;
            this.auto_release_prob = 0;
            this.shock_release_prob = 0;
            this.guard_element_set = new List<int>();
            this.plus_state_set = new List<int>();
            this.minus_state_set = new List<int>();
        }

        // 附加
        public List<int> is_guard_element_set { get; set; }

    }
}
