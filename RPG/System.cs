using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //RPG.System
    //系统的数据类。

    //父类Object 
    //内部类RPG.System.Words 
    //RPG.System.TestBattler 
    //定义module RPG
    public class System
    {
        //属性magic_number 
        //更新检查用的幻数。每次 Maker 保存数据都会写入不同的值。
        public int magic_number { get; set; }
        //party_members 
        //初期同伴。是角色 ID 的数组。
        public List<int> party_members { get; set; }
        //elements 
        //属性名称目录。是以属性 ID 为索引的字符串数组，其 0 号单元为 null。
        public List<string> elements { get; set; }
        //switches 
        //开关名称目录。是以开关 ID 为索引的字符串数组，其 0 号单元为 null。
        public List<string> switches { get; set; }
        //variables 
        //变量名称目录。是以变量 ID 为索引的字符串数组，其 0 号单元为 null。
        public List<string> variables { get; set; }
        //windowskin_name 
        //窗口皮肤图像的文件名。
        public string windowskin_name { get; set; }
        //title_name 
        //标题图像的文件名。
        public string title_name { get; set; }
        //gameover_name 
        //游戏结束图像的文件名。
        public string gameover_name { get; set; }
        //battle_transition 
        //战斗切换时显示的渐变图像的文件名。
        public string battle_transition { get; set; }
        //title_bgm 
        //标题 RPG.AudioFile（RPG.AudioFile）。
        public AudioFile title_bgm { get; set; }
        //battle_bgm 
        //战斗 RPG.AudioFile（RPG.AudioFile）。
        public AudioFile battle_bgm { get; set; }
        //battle_end_me 
        //战斗结束 ME（RPG.AudioFile）。
        public AudioFile battle_end_me { get; set; }
        //gameover_me 
        //游戏结束 ME（RPG.AudioFile）。
        public AudioFile gameover_me { get; set; }
        //cursor_se 
        //光标 SE（RPG.AudioFile）。
        public AudioFile cursor_se { get; set; }
        //decision_se 
        //确定 SE（RPG.AudioFile）。
        public AudioFile decision_se { get; set; }
        //cancel_se 
        //取消 SE（RPG.AudioFile）。
        public AudioFile cancel_se { get; set; }

        //buzzer_se 
        //警告 SE（RPG.AudioFile）。
        public AudioFile buzzer_se { get; set; }

        //equip_se 
        //装备 SE（RPG.AudioFile）。
        public AudioFile equip_se { get; set; }

        //shop_se 
        //商店 SE（RPG.AudioFile）。
        public AudioFile shop_se { get; set; }

        //save_se 
        //存档 SE（RPG.AudioFile）。
        public AudioFile save_se { get; set; }

        //load_se 
        //读档 SE（RPG.AudioFile）。
        public AudioFile load_se { get; set; }

        //battle_start_se 
        //战斗开始 SE（RPG.AudioFile）。
        public AudioFile battle_start_se { get; set; }

        //escape_se 
        //逃跑 SE（RPG.AudioFile）。
        public AudioFile escape_se { get; set; }

        //actor_collapse_se 
        //角色受伤 SE（RPG.AudioFile）。
        public AudioFile actor_collapse_se { get; set; }

        //enemy_collapse_se 
        //敌人受伤 SE（RPG.AudioFile）。
        public AudioFile enemy_collapse_se { get; set; }

        //words 
        //用语（RPG.System.Words）。
        public Words words { get; set; }
        //start_map_id 
        //主角初期位置的地图 ID。
        public int start_map_id { get; set; }
        //start_x 
        //主角初期位置的地图 X 座标。
        public int start_x { get; set; }
        //start_y 
        //主角初期位置的地图 Y 座标。
        public int start_y { get; set; }
        //test_battlers 
        //战斗测试的同伴设定。RPG.System.TestBattler 的数组。
        public List<TestBattler> test_battlers { get; set; }
        //test_troop_id 
        //战斗测试的队伍 ID。
        public int test_troop_id { get; set; }
        //battleback_name 
        //战斗测试和 Maker 内部使用的战斗背景图像的文件名。
        public string battleback_name { get; set; }
        //battler_name 
        //Maker 内部使用的战斗者图像的文件名。
        public string battler_name { get; set; }
        //battler_hue 
        //Maker 内部使用的战斗者图像的色相变化值（0..360）。
        public int battler_hue { get; set; }
        //edit_map_id 
        //Maker 内部使用的现在编辑的地图 ID。
        public int edit_map_id { get; set; }

        // 怎么会有 "@_"这个字段的？
        public int _ { get; set; }

        public System()
        {
            this.magic_number = 0;
            this.party_members = new List<int>();
            this.elements = new List<string>();
            this.switches = new List<string>();
            this.variables = new List<string>();
            this.windowskin_name = "";
            this.title_name = "";
            this.gameover_name = "";
            this.battle_transition = "";
            this.title_bgm = new RPG.AudioFile();
            this.battle_bgm = new RPG.AudioFile();
            this.battle_end_me = new RPG.AudioFile();
            this.gameover_me = new RPG.AudioFile();
            this.cursor_se = new RPG.AudioFile("", 80);
            this.decision_se = new RPG.AudioFile("", 80);
            this.cancel_se = new RPG.AudioFile("", 80);
            this.buzzer_se = new RPG.AudioFile("", 80);
            this.equip_se = new RPG.AudioFile("", 80);
            this.shop_se = new RPG.AudioFile("", 80);
            this.save_se = new RPG.AudioFile("", 80);
            this.load_se = new RPG.AudioFile("", 80);
            this.battle_start_se = new RPG.AudioFile("", 80);
            this.escape_se = new RPG.AudioFile("", 80);
            this.actor_collapse_se = new RPG.AudioFile("", 80);
            this.enemy_collapse_se = new RPG.AudioFile("", 80);
            this.words = new Words();
            this.test_battlers = new List<TestBattler>();
            this.test_troop_id = 1;
            this.start_map_id = 1;
            this.start_x = 0;
            this.start_y = 0;
            this.battleback_name = "";
            this.battler_name = "";
            this.battler_hue = 0;
            this.edit_map_id = 1;
        }

        //RPG.System.Words
        //用语的数据类。

        //父类Object 
        //参照元RPG.System 
        public class Words
        {
            //属性gold 
            //G（货币单位）。
            public string gold { get; set; }
            //hp 
            //HP。
            public string hp { get; set; }
            //sp 
            //SP。
            public string sp { get; set; }
            //str 
            //力量。
            public string str { get; set; }
            //dex 
            //灵巧。
            public string dex { get; set; }
            //agi 
            //速度。
            public string agi { get; set; }
            //int 
            //魔力。
            public string int1 { get; set; }
            //atk 
            //攻击力。
            public string atk { get; set; }
            //pdef 
            //物理防御。
            public string pdef { get; set; }
            //mdef 
            //魔法防御。
            public string mdef { get; set; }
            //weapon 
            //武器。
            public string weapon { get; set; }
            //armor1 
            //盾。
            public string armor1 { get; set; }
            //armor2 
            //头。
            public string armor2 { get; set; }
            //armor3 
            //身体。
            public string armor3 { get; set; }
            //armor4 
            //装饰品。
            public string armor4 { get; set; }
            //attack 
            //攻击。
            public string attack { get; set; }
            //skill 
            //特技。
            public string skill { get; set; }
            //guard 
            //防御。
            public string guard { get; set; }
            //item 
            //物品。
            public string item { get; set; }
            //equip 
            //装备。
            public string equip { get; set; }
            //定义module RPG
            public Words()
            {
                this.gold = "";
                this.hp = "";
                this.sp = "";
                this.str = "";
                this.dex = "";
                this.agi = "";
                this.int1 = "";
                this.atk = "";
                this.pdef = "";
                this.mdef = "";
                this.weapon = "";
                this.armor1 = "";
                this.armor2 = "";
                this.armor3 = "";
                this.armor4 = "";
                this.attack = "";
                this.skill = "";
                this.guard = "";
                this.item = "";
                this.equip = "";
            }
        }

        //RPG.System.TestBattler
        //战斗测试使用的战斗者的数据类。

        //父类Object 
        //参照元RPG.System 
        public class TestBattler
        {
            //属性actor_id 
            //战斗角色 ID。
            public int actor_id { get; set; }
            //level 
            //等级。
            public int level { get; set; }
            //weapon_id 
            //武器 ID。
            public int weapon_id { get; set; }
            //armor1_id 
            //盾 ID。
            public int armor1_id { get; set; }
            //armor2_id 
            //头部防具 ID。
            public int armor2_id { get; set; }
            //armor3_id 
            //身体防具 ID。
            public int armor3_id { get; set; }
            //armor4_id 
            //装饰品 ID。
            public int armor4_id { get; set; }
            //定义module RPG
            public TestBattler()
            {
                this.actor_id = 1;
                this.level = 1;
                this.weapon_id = 0;
                this.armor1_id = 0;
                this.armor2_id = 0;
                this.armor3_id = 0;
                this.armor4_id = 0;
            }
        }


    }
}
