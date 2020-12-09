using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_Menu
    //------------------------------------------------------------------------------
    // 　处理菜单画面的类。
    //==============================================================================

    public class Scene_Menu : Scene
    {

        public Window_PlayTime playtime_window { get; set; }
        public Window_Command command_window { get; set; }
        public Window_Steps steps_window { get; set; }
        public Window_Gold gold_window { get; set; }
        public Window_MenuStatus status_window { get; set; }

        public int menu_index { get; set; }
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     menu_index : 命令光标的初期位置
        //--------------------------------------------------------------------------
        public Scene_Menu(int menu_index = 0)
        {
            this.menu_index = menu_index;
        }
        public override void Initialize()
        {
            // 生成命令窗口
            var s1 = Global.data_system.words.item;
            var s2 = Global.data_system.words.skill;
            var s3 = Global.data_system.words.equip;
            var s4 = "状态";
            var s5 = "存档";
            var s6 = "结束游戏";
            this.command_window = new Window_Command(160, new string[] { s1, s2, s3, s4, s5, s6 });
            this.command_window.index = this.menu_index;
            // 同伴人数为 0 的情况下
            if (Global.game_party.actors.Count == 0)
            {
                // 物品、特技、装备、状态无效化
                this.command_window.disable_item(0);
                this.command_window.disable_item(1);
                this.command_window.disable_item(2);
                this.command_window.disable_item(3);
            }
            // 禁止存档的情况下
            if (Global.game_system.save_disabled)
            {
                // 存档无效
                this.command_window.disable_item(4);
            }
            // 生成游戏时间窗口
            this.playtime_window = new Window_PlayTime();
            this.playtime_window.x = 0;
            this.playtime_window.y = 224;
            // 生成步数窗口
            this.steps_window = new Window_Steps();
            this.steps_window.x = 0;
            this.steps_window.y = 320;
            // 生成金钱窗口
            this.gold_window = new Window_Gold();
            this.gold_window.x = 0;
            this.gold_window.y = 416;
            // 生成状态窗口
            this.status_window = new Window_MenuStatus();
            this.status_window.x = 160;
            this.status_window.y = 0;
            // 执行过渡
            Graphics.transition();
        }
        //--------------------------------------------------------------------------
        // ● 主处理
        //--------------------------------------------------------------------------
        public override void main()
        {
            // 主循环
            //while (true)
            //{
            // 刷新游戏画面
            Graphics.update();
            // 刷新输入信息
            Input.update();
            // 刷新画面
            update();
            // 如果切换画面就中断循环
            //    if (Global.scene != this)
            //    {
            //        break;
            //    }
            //}
        }
        public override void Uninitialize()
        {
            // 准备过渡
            Graphics.freeze();
            // 释放窗口
            this.command_window.dispose();
            this.playtime_window.dispose();
            this.steps_window.dispose();
            this.gold_window.dispose();
            this.status_window.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 刷新窗口
            this.command_window.update();
            this.playtime_window.update();
            this.steps_window.update();
            this.gold_window.update();
            this.status_window.update();
            // 命令窗口被激活的情况下: 调用 update_command
            if (this.command_window.active)
            {
                update_command();
                return;
            }
            // 状态窗口被激活的情况下: 调用 update_status
            if (this.status_window.active)
            {
                update_status();
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (命令窗口被激活的情况下)
        //--------------------------------------------------------------------------
        public void update_command()
        {
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 切换的地图画面
                Global.scene = new Scene_Map();
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 同伴人数为 0、存档、游戏结束以外的场合
                if (Global.game_party.actors.Count == 0 && this.command_window.index < 4)
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                    return;
                }
                // 命令窗口的光标位置分支
                switch (this.command_window.index)
                {
                    case 0:  // 物品
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 切换到物品画面
                        Global.scene = new Scene_Item();
                        break;
                    case 1:  // 特技
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 激活状态窗口
                        this.command_window.active = false;
                        this.status_window.active = true;
                        this.status_window.index = 0;
                        break;
                    case 2:  // 装备
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 激活状态窗口
                        this.command_window.active = false;
                        this.status_window.active = true;
                        this.status_window.index = 0;
                        break;
                    case 3:  // 状态
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 激活状态窗口
                        this.command_window.active = false;
                        this.status_window.active = true;
                        this.status_window.index = 0;
                        break;
                    case 4:  // 存档
                        // 禁止存档的情况下
                        if (Global.game_system.save_disabled)
                        {
                            // 演奏冻结 SE
                            Global.game_system.se_play(Global.data_system.buzzer_se);
                            return;
                        }
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 切换到存档画面
                        Global.scene = new Scene_Save();
                        break;
                    case 5:  // 游戏结束
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 切换到游戏结束画面
                        Global.scene = new Scene_End();
                        break;
                }
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (状态窗口被激活的情况下)
        //--------------------------------------------------------------------------
        public void update_status()
        {
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 激活命令窗口
                this.command_window.active = true;
                this.status_window.active = false;
                this.status_window.index = -1;
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 命令窗口的光标位置分支
                switch (this.command_window.index)
                {
                    case 1:  // 特技
                        // 本角色的行动限制在 2 以上的情况下
                        if (Global.game_party.actors[this.status_window.index].restriction >= 2)
                        {
                            // 演奏冻结 SE
                            Global.game_system.se_play(Global.data_system.buzzer_se);
                            return;
                        }
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 切换到特技画面
                        Global.scene = new Scene_Skill(this.status_window.index);
                        break;
                    case 2:  // 装备
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 切换的装备画面
                        Global.scene = new Scene_Equip(this.status_window.index);
                        break;
                    case 3:  // 状态
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 切换到状态画面
                        Global.scene = new Scene_Status(this.status_window.index);
                        break;
                }
                return;
            }
        }
    }
}
