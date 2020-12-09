using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.Actor
    //角色的数据类。
    //父类Object 

    public class Actor
    {
        //属性id 
        //ID。
        public int id { get; set; }


        //name 
        //名称。
        public string name { get; set; }

        //class_id 
        //职业 ID。
        public int class_id { get; set; }
        //initial_level 
        //初期等级。
        public int initial_level { get; set; }

        //final_level 
        //最终等级。
        public int final_level { get; set; }

        //exp_basis 
        //EXP 曲线的基本值（10..50）。
        public int exp_basis { get; set; }
        //exp_inflation 
        //EXP 曲线的增加度（10..50）。
        public int exp_inflation { get; set; }
        //character_name 
        //人物图像的文件名。
        public string character_name { get; set; }

        //character_hue 
        //人物图像的色相变化值（0..360）。
        public int character_hue { get; set; }

        //battler_name 
        public string battler_name { get; set; }



        //battler_hue 
        //战斗图像的色相变化值（0..360）。
        public int battler_hue { get; set; }

        //parameters 
        //包含了各等级基本参数的二维数组（Table）。

        //具体来说应该是 parameters[kind, level] 的形式。

        //kind 是参数的种类（0：MaxHP，1：MaxSP，2：力量，3：灵巧，4：速度，5：魔力）。
        public Table parameters { get; set; }

        //weapon_id 
        //初期装备的武器的 ID。
        public int weapon_id { get; set; }

        //armor1_id 
        //初期装备的盾的 ID。
        public int armor1_id { get; set; }

        //armor2_id 
        //初期装备的头部防具的 ID。
        public int armor2_id { get; set; }
        //armor3_id 
        //初期装备的身体防具的 ID。
        public int armor3_id { get; set; }

        //armor4_id 
        //初期装备的装饰品的 ID。
        public int armor4_id { get; set; }

        //weapon_fix 
        //武器的装备固定标记。
        public bool weapon_fix { get; set; }


        //armor1_fix 
        //盾的装备固定标记。
        public bool armor1_fix { get; set; }
        //armor2_fix 
        //头部防具的装备固定标记。
        public bool armor2_fix { get; set; }

        //armor3_fix 
        //身体防具的装备固定标记。
        public bool armor3_fix { get; set; }

        //armor4_fix 
        //装饰品的装备固定标记。
        public bool armor4_fix { get; set; }

        //定义module RPG
        public Actor()
        {
            this.id = 0;
            this.name = "";
            this.class_id = 1;
            this.initial_level = 1;
            this.final_level = 99;
            this.exp_basis = 30;
            this.exp_inflation = 30;
            this.character_name = "";
            this.character_hue = 0;
            this.battler_name = "";
            this.battler_hue = 0;
            this.parameters = new Table(6, 100);
            for (var i = 1; i <= 99; i++)
            {
                this.parameters[0, i] = 500 + i * 50;
                this.parameters[1, i] = 500 + i * 50;
                this.parameters[2, i] = 50 + i * 5;
                this.parameters[3, i] = 50 + i * 5;
                this.parameters[4, i] = 50 + i * 5;
                this.parameters[5, i] = 50 + i * 5;
            }
            this.weapon_id = 0;
            this.armor1_id = 0;
            this.armor2_id = 0;
            this.armor3_id = 0;
            this.armor4_id = 0;
            this.weapon_fix = false;
            this.armor1_fix = false;
            this.armor2_fix = false;
            this.armor3_fix = false;
            this.armor4_fix = false;
        }
    }
}
