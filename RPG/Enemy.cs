using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.Enemy
    //敌人的数据类。

    //父类Object 
    public class Enemy
    {
        //属性id 
        //ID。
        public int id { get; set; }
        //name 
        //名称。
        string _name;

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        //battler_name 
        //战斗者图像的文件名。
        public string battler_name { get; set; }
        //battler_hue 
        //战斗者图像的色相变化值（0..360）。
        public int battler_hue { get; set; }
        //maxhp 
        //MaxHP。
        public int maxhp { get; set; }
        //maxsp 
        //MaxSP。
        public int maxsp { get; set; }
        //str 
        //力量。
        public int str { get; set; }
        //dex 
        //灵巧。
        public int dex { get; set; }
        //agi 
        //速度。
        public int agi { get; set; }
        //int 
        //魔力。
        public int int1 { get; set; }
        //atk 
        //攻击力。
        public int atk { get; set; }
        //pdef 
        //物理防御。
        public int pdef { get; set; }
        //mdef 
        //魔法防御。
        public int mdef { get; set; }
        //eva 
        //回避修正。
        public int eva { get; set; }
        //animation1_id 
        //攻击方的动画 ID。
        public int animation1_id { get; set; }
        //animation2_id 
        //对象方的动画 ID。
        public int animation2_id { get; set; }
        //element_ranks 
        //属性有效度。是以属性 ID 为索引的一维数组（Table），其值分 6 级（0：A，1：B，2：C，3：D，4：E，5：F）。
        public Table element_ranks { get; set; }
        //state_ranks 
        //状态有效度。是以状态 ID 为索引的一维数组（Table），其值分 6 级（0：A，1：B，2：C，3：D，4：E，5：F）。
        public Table state_ranks { get; set; }
        //actions 
        //行为。RPG.Enemy.Action 的数组。
        public List<Action> actions { get; set; }
        //exp 
        //EXP。
        public int exp { get; set; }
        //gold 
        //金钱。
        public int gold { get; set; }
        //item_id 
        //宝物的物品 ID。
        public int item_id { get; set; }
        //weapon_id 
        //宝物的武器 ID。
        public int weapon_id { get; set; }
        //armor_id 
        //宝物的防具 ID。
        public int armor_id { get; set; }
        //treasure_prob 
        //宝物出现率。
        public int treasure_prob { get; set; }
        //内部类RPG.Enemy.Action 
        //定义module RPG
        public Enemy()
        {
            this.id = 0;
            this.name = "";
            this.battler_name = "";
            this.battler_hue = 0;
            this.maxhp = 500;
            this.maxsp = 500;
            this.str = 50;
            this.dex = 50;
            this.agi = 50;
            this.int1 = 50;
            this.atk = 100;
            this.pdef = 100;
            this.mdef = 100;
            this.eva = 0;
            this.animation1_id = 0;
            this.animation2_id = 0;
            this.element_ranks = new Table(1);
            this.state_ranks = new Table(1);
            this.actions = new List<Action>() { new RPG.Enemy.Action() };
            this.exp = 0;
            this.gold = 0;
            this.item_id = 0;
            this.weapon_id = 0;
            this.armor_id = 0;
            this.treasure_prob = 100;
        }

        //RPG.Enemy.Action
        //敌人「行为」的数据类。

        //父类Object 
        //参照元RPG.Enemy 
        public class Action
        {

            //属性kind 
            //种类（0：基本，1：特技）。
            public int kind { get; set; }
            //basic 
            //种类为「基本」时，其内容（0：攻击，1：防御，2：逃跑，3：什么也不做）。
            public int basic { get; set; }
            //skill_id 
            //种类为「特技」时，其 ID。
            public int skill_id { get; set; }
            //condition_turn_a 
            //condition_turn_b 
            //条件「回合」指定的 a、b 的值。输入为 a + bx 的形式。

            //以回合为条件但未指定值时默认为 a = 0、b = 1。
            public int condition_turn_a { get; set; }
            public int condition_turn_b { get; set; }
            //condition_hp 
            //条件「HP」指定的比率（%）。

            //以 HP 为条件但未指定值时默认为 100。
            public int condition_hp { get; set; }
            //condition_level 
            //条件「等级」指定的标准值。

            //以等级为条件但未指定值时默认为 1。
            public int condition_level { get; set; }
            //condition_switch_id 
            //条件「开关」指定的开关 ID。

            //以开关为条件但未指定值时默认为 0（所以需要检查是否为 0）。
            public int condition_switch_id { get; set; }
            //rating 
            //额定值（1..10）。
            public int rating { get; set; }
            //定义module RPG
            public Action()
            {
                this.kind = 0;
                this.basic = 0;
                this.skill_id = 1;
                this.condition_turn_a = 0;
                this.condition_turn_b = 1;
                this.condition_hp = 100;
                this.condition_level = 1;
                this.condition_switch_id = 0;
                this.rating = 5;
            }
        }

    }
}
