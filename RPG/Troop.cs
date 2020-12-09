using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.Troop
    //队伍的数据类。

    //父类Object 

    public class Troop
    {

        //属性id 
        //ID。
        public int id { get; set; }
        //name 
        //名称。
        public string name { get; set; }
        //members 
        //队伍成员。RPG.Troop.Member 的数组。
        public List<Member> members { get; set; }
        //pages 
        //战斗事件。RPG.Troop.Page 的数组。
        public List<Page> pages { get; set; }
        //内部类RPG.Troop.Member 
        //RPG.Troop.Page 
        //定义module RPG
        public Troop()
        {
            this.id = 0;
            this.name = "";
            this.members = new List<Member>();
            //this.pages = new List<Page>() { new RPG.BattleEventPage() };      //???
            this.pages = new List<Page>() { new Page() };
        }


        //RPG.Troop.Member
        //队伍成员的数据类。

        //父类Object 
        //参照元RPG.Troop 
        public class Member
        {
            //属性enemy_id 
            //敌人 ID。
            public int enemy_id { get; set; }
            //x 
            //脚下的 X 座标。
            public int x { get; set; }
            //y 
            //脚下的 Y 座标。
            public int y { get; set; }
            //hidden 
            //选项「中途出现」的真伪值。
            public bool hidden { get; set; }
            //immortal 
            //选项「不死之身」的真伪值。
            public bool immortal { get; set; }
            //定义module RPG
            public Member()
            {
                this.enemy_id = 1;
                this.x = 0;
                this.y = 0;
                this.hidden = false;
                this.immortal = false;
            }
        }


        //RPG.Troop.Page
        //战斗事件（页）的数据类。

        //父类Object 
        //参照元RPG.Troop 
        public class Page
        {
            //属性condition 
            //条件（RPG.Troop.Page.Condition）。
            public Condition condition { get; set; }
            //span 
            //距离（0：战斗，1：回合，2：暂时）。
            public int span { get; set; }
            //list 
            //执行内容。RPG.EventCommand 的数组。
            public List<EventCommand> list { get; set; }
            //内部类RPG.Troop.Page.Condition 
            public Page()
            {
                //定义module RPG
                //  public class Troop
                this.condition = new RPG.Troop.Page.Condition();
                this.span = 0;
                this.list = new List<EventCommand>(){new RPG.EventCommand()};
            }

            //RPG.Troop.Page.Condition
            //战斗事件「条件」的数据类。

            //父类Object 
            //参照元RPG.Troop.Page 
            public class Condition
            {
                //属性turn_valid 
                //表示条件「回合」是否有效的真伪值。
                public bool turn_valid { get; set; }
                //enemy_valid 
                //表示条件「敌人」是否有效的真伪值。
                public bool enemy_valid { get; set; }
                //actor_valid 
                //表示条件「角色」是否有效的真伪值。
                public bool actor_valid { get; set; }
                //switch_valid 
                //表示条件「开关」是否有效的真伪值。
                public bool switch_valid { get; set; }
                //turn_a 
                //turn_b 
                //条件「回合」指定的 a、b 的值。输入为 a + bx 的形式。
                public int turn_a { get; set; }
                public int turn_b { get; set; }
                //enemy_index 
                //条件「敌人」指定的队伍成员的索引（0..7）。
                public int enemy_index { get; set; }
                //enemy_hp 
                //条件「敌人」指定的 HP 比率（%）。
                public int enemy_hp { get; set; }
                //actor_id 
                //条件「角色」指定的角色 ID。
                public int actor_id { get; set; }
                //actor_hp 
                //条件「角色」指定的 HP 比率（%）。
                public int actor_hp { get; set; }
                //switch_id 
                //条件「开关」指定的开关 ID。
                public int switch_id { get; set; }
                //定义module RPG
                public Condition()
                {
                    this.turn_valid = false;
                    this.enemy_valid = false;
                    this.actor_valid = false;
                    this.switch_valid = false;
                    this.turn_a = 0;
                    this.turn_b = 0;
                    this.enemy_index = 0;
                    this.enemy_hp = 50;
                    this.actor_id = 1;
                    this.actor_hp = 50;
                    this.switch_id = 1;
                }
            }
        }
    }
}
