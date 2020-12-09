using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XP
{
    //==============================================================================
    // ■ Scene_Title
    //------------------------------------------------------------------------------
    // 　处理标题画面的类。
    //==============================================================================

    public class Scene_Title : Scene
    {
        public Scene_Title()
        {
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // 按下 C 键的情况下
            if (e.Key == Key.C || e.Key == Key.Enter)
            {
                // 命令窗口的光标位置的分支
                switch (this.command_window.index)
                {
                    case 0:  // 新游戏
                        command_new_game(); break;
                    case 1:  // 继续
                        command_continue(); break;
                    case 2:  // 退出
                        command_shutdown(); break;
                }
            }
        }
        public override void Initialize()
        {
            Global.PreviewKeyDown += OnKeyDown;

            // 战斗测试的情况下
            if (Global.BTEST)
            {
                battle_test();
                return;
            }

            // 载入数据库
            if (!Global.DataInXml)
            {
                Global.data_actors = (List<RPG.Actor>)Global.load_data("Data/Actors.rxdata");
                Global.data_classes = (List<RPG.Class>)Global.load_data("Data/Classes.rxdata");
                Global.data_skills = (List<RPG.Skill>)Global.load_data("Data/Skills.rxdata");
                Global.data_items = (List<RPG.Item>)Global.load_data("Data/Items.rxdata");
                Global.data_weapons = (List<RPG.Weapon>)Global.load_data("Data/Weapons.rxdata");
                Global.data_armors = (List<RPG.Armor>)Global.load_data("Data/Armors.rxdata");
                Global.data_enemies = (List<RPG.Enemy>)Global.load_data("Data/Enemies.rxdata");
                Global.data_troops = (List<RPG.Troop>)Global.load_data("Data/Troops.rxdata");
                Global.data_states = (List<RPG.State>)Global.load_data("Data/States.rxdata");
                Global.data_animations = (List<RPG.Animation>)Global.load_data("Data/Animations.rxdata");
                Global.data_tilesets = (List<RPG.Tileset>)Global.load_data("Data/Tilesets.rxdata");
                Global.data_common_events = (List<RPG.CommonEvent>)Global.load_data("Data/CommonEvents.rxdata");
                Global.data_system = (RPG.System)Global.load_data("Data/System.rxdata");
            }
            else
            {
                Global.LoadData();
            }

            // 生成系统对像
            Global.game_system = new Game_System();
            // 生成标题图形
            this.sprite = new XP.Internal.Sprite();
            this.sprite.bitmap = RPG.Cache.title(Global.data_system.title_name);
            // 生成命令窗口
            var s1 = "新游戏";
            var s2 = "继续";
            var s3 = "退出";
            this.command_window = new Window_Command(192, new string[] { s1, s2, s3 });
            this.command_window.back_opacity = 160;
            this.command_window.x = 320 - this.command_window.width / 2;
            this.command_window.y = 288;
            // 判定继续的有效性
            // 存档文件一个也不存在的时候也调查
            // 有効为 this.continue_enabled 为 true、无效为 false
            this.continue_enabled = false;
            for (var i = 0; i <= 3; i++)
                if (File.is_exist("Save//{i+1}.rxdata"))
                    this.continue_enabled = true;

            // 继续为有效的情况下、光标停止在继续上
            // 无效的情况下、继续的文字显示为灰色
            if (this.continue_enabled)
                this.command_window.index = 1;
            else
                this.command_window.disable_item(1);

            // 演奏标题 RPG.AudioFile
            Global.game_system.bgm_play(Global.data_system.title_bgm);
            // 停止演奏 ME、BGS
            Audio.me_stop();
            Audio.bgs_stop();
            // 执行过渡
            Graphics.transition();
        }
        //--------------------------------------------------------------------------
        // ● 主处理
        //--------------------------------------------------------------------------
        public override void main()
        {
            //// 主循环
            //while (true)
            //{
            // 刷新游戏画面
            Graphics.update();
            // 刷新输入信息
            Input.update();
            // 刷新画面
            update();
            //    // 如果画面被切换就中断循环
            //    if (Global.scene != this)
            //        break;
            //}
        }
        public override void Uninitialize()
        {
            // 装备过渡
            Graphics.freeze();
            // 释放命令窗口
            this.command_window.dispose();
            // 释放标题图形
            this.sprite.bitmap.dispose();
            this.sprite.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 刷新命令窗口
            this.command_window.update();
            //// 按下 C 键的情况下
            //if (Input.is_trigger(Input.C))
            //{
            //    // 命令窗口的光标位置的分支
            //    switch (this.command_window.index)
            //    {
            //        case 0:  // 新游戏
            //            command_new_game(); break;
            //        case 1:  // 继续
            //            command_continue(); break;
            //        case 2:  // 退出
            //            command_shutdown(); break;
            //    }
            //}
        }
        //--------------------------------------------------------------------------
        // ● 命令 : 新游戏
        //--------------------------------------------------------------------------
        public void command_new_game()
        {
            // 演奏确定 SE
            Global.game_system.se_play(Global.data_system.decision_se);
            // 停止 RPG.AudioFile
            Audio.bgm_stop();
            // 重置测量游戏时间用的画面计数器
            Graphics.frame_count = 0;
            // 生成各种游戏对像
            Global.game_temp = new Game_Temp();
            Global.game_system = new Game_System();
            Global.game_switches = new Game_Switches();
            Global.game_variables = new Game_Variables();
            Global.game_self_switches = new Game_SelfSwitches();
            Global.game_screen = new Game_Screen();
            Global.game_actors = new Game_Actors();
            Global.game_party = new Game_Party();
            Global.game_troop = new Game_Troop();
            Global.game_map = new Game_Map();
            Global.game_player = new Game_Player();
            // 设置初期同伴位置
            Global.game_party.setup_starting_members();
            // 设置初期位置的地图
            Global.game_map.setup(Global.data_system.start_map_id);
            // 主角向初期位置移动
            Global.game_player.moveto(Global.data_system.start_x, Global.data_system.start_y);
            // 刷新主角
            Global.game_player.refresh();
            // 执行地图设置的 RPG.AudioFile 与 BGS 的自动切换
            Global.game_map.autoplay();
            // 刷新地图 (执行并行事件)
            Global.game_map.update();
            // 切换地图画面
            Global.scene = new Scene_Map();
        }
        //--------------------------------------------------------------------------
        // ● 命令 : 继续
        //--------------------------------------------------------------------------
        public void command_continue()
        {
            // 继续无效的情况下
            if (!this.continue_enabled)
            {
                // 演奏无效 SE
                Global.game_system.se_play(Global.data_system.buzzer_se);
                return;
            }
            // 演奏确定 SE
            Global.game_system.se_play(Global.data_system.decision_se);
            // 切换到读档画面
            Global.scene = new Scene_Load();
        }
        //--------------------------------------------------------------------------
        // ● 命令 : 退出
        //--------------------------------------------------------------------------
        public void command_shutdown()
        {
            // 演奏确定 SE
            Global.game_system.se_play(Global.data_system.decision_se);
            // RPG.AudioFile、BGS、ME 的淡入淡出
            Audio.bgm_fade(800);
            Audio.bgs_fade(800);
            Audio.me_fade(800);
            // 退出
            Global.scene = null;
        }
        //--------------------------------------------------------------------------
        // ● 战斗测试
        //--------------------------------------------------------------------------
        public void battle_test()
        {
            // 载入数据库 (战斗测试用)
            Global.data_actors = (List<RPG.Actor>)Global.load_data("Data/BT_Actors.rxdata");
            Global.data_classes = (List<RPG.Class>)Global.load_data("Data/BT_Classes.rxdata");
            Global.data_skills = (List<RPG.Skill>)Global.load_data("Data/BT_Skills.rxdata");
            Global.data_items = (List<RPG.Item>)Global.load_data("Data/BT_Items.rxdata");
            Global.data_weapons = (List<RPG.Weapon>)Global.load_data("Data/BT_Weapons.rxdata");
            Global.data_armors = (List<RPG.Armor>)Global.load_data("Data/BT_Armors.rxdata");
            Global.data_enemies = (List<RPG.Enemy>)Global.load_data("Data/BT_Enemies.rxdata");
            Global.data_troops = (List<RPG.Troop>)Global.load_data("Data/BT_Troops.rxdata");
            Global.data_states = (List<RPG.State>)Global.load_data("Data/BT_States.rxdata");
            Global.data_animations = (List<RPG.Animation>)Global.load_data("Data/BT_Animations.rxdata");
            Global.data_tilesets = (List<RPG.Tileset>)Global.load_data("Data/BT_Tilesets.rxdata");
            Global.data_common_events = (List<RPG.CommonEvent>)Global.load_data("Data/BT_CommonEvents.rxdata");
            Global.data_system = (RPG.System)Global.load_data("Data/BT_System.rxdata");
            // 重置测量游戏时间用的画面计数器
            Graphics.frame_count = 0;
            // 生成各种游戏对像
            Global.game_temp = new Game_Temp();
            Global.game_system = new Game_System();
            Global.game_switches = new Game_Switches();
            Global.game_variables = new Game_Variables();
            Global.game_self_switches = new Game_SelfSwitches();
            Global.game_screen = new Game_Screen();
            Global.game_actors = new Game_Actors();
            Global.game_party = new Game_Party();
            Global.game_troop = new Game_Troop();
            Global.game_map = new Game_Map();
            Global.game_player = new Game_Player();
            // 设置战斗测试用同伴
            Global.game_party.setup_battle_test_members();
            // 设置队伍 ID、可以逃走标志、战斗背景
            Global.game_temp.battle_troop_id = Global.data_system.test_troop_id;
            Global.game_temp.battle_can_escape = true;
            Global.game_map.battleback_name = Global.data_system.battleback_name;
            // 演奏战斗开始 RPG.AudioFile
            Global.game_system.se_play(Global.data_system.battle_start_se);
            // 演奏战斗 RPG.AudioFile
            Global.game_system.bgm_play(Global.game_system.battle_bgm);
            // 切换到战斗画面
            Global.scene = new Scene_Battle();
        }

        public XP.Internal.Sprite sprite { get; set; }

        public Window_Command command_window { get; set; }

        public bool continue_enabled { get; set; }
    }

}
