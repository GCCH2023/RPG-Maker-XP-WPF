using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.Class
    //职业的数据类。

    //父类Object 
    public class Class
    {

        //属性id 
        //ID。
        public int id { get; set; }
        //name 
        //名称。
        public string name { get; set; }
        //position 
        //位置（0：前卫，1：中卫，2：后卫）。
        public int position { get; set; }
        //weapon_set 
        //包含可装备武器 ID 的数组。
        public List<int> weapon_set { get; set; }
        //armor_set 
        //包含可装备防具 ID 的数组。
        public List<int> armor_set { get; set; }
        //element_ranks 
        //属性有效度。是以属性 ID 为索引的一维数组（Table），其值分 6 级（0：A，1：B，2：C，3：D，4：E，5：F）。
        public Table element_ranks { get; set; }
        //state_ranks 
        //状态有效度。是以状态 ID 为索引的一维数组（Table），其值分 6 级（0：A，1：B，2：C，3：D，4：E，5：F）。
        public Table state_ranks { get; set; }
        //learnings 
        //学会的特技。RPG.Class.Learning 的数组。
        public List<Learning> learnings { get; set; }
        //内部类RPG.Class.Learning 
        //定义module RPG
        public Class()
        {
            this.id = 0;
            this.name = "";
            this.position = 0;
            this.weapon_set = new List<int>();
            this.armor_set = new List<int>();
            this.element_ranks = new Table(1);
            this.state_ranks = new Table(1);
            this.learnings = new List<Learning>();
        }
        //        RPG.Class.Learning
        //职业「学会的特技」的数据类。

        //父类Object 
        public class Learning
        {
            //参照元RPG.Class 
            //属性level 
            //等级。
            public int level { get; set; }
            //skill_id 
            //学会的特技 ID。
            public int skill_id { get; set; }
            //定义module RPG
            public Learning()
            {
                this.level = 1;
                this.skill_id = 1;
            }
        }
    }
}
