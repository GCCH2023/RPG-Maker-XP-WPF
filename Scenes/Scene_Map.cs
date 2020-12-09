using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_Map
    //------------------------------------------------------------------------------
    // 　处理地图画面的类。
    //==============================================================================

    public class Scene_Map : Scene
    {
        public Spriteset_Map spriteset { get; set; }
        public Window_Message message_window { get; set; }

        public Scene_Map()
        {

        }

        public override void Initialize()
        {
            // 生成活动块
            this.spriteset = new Spriteset_Map();
            // 生成信息窗口
            this.message_window = new Window_Message();
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
            // 如果画面切换的话就中断循环
            //if (Global.scene != this)
            //{
            //    break;
            //}
            //}
        }
        public override void Uninitialize()
        {
            // 准备过渡
            Graphics.freeze();
            // 释放活动块
            this.spriteset.dispose();
            // 释放信息窗口
            this.message_window.dispose();
            // 标题画面切换中的情况下
            if (Global.scene is Scene_Title)
            {
                // 淡入淡出画面
                Graphics.transition();
                Graphics.freeze();
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 循环
            while (true)
            {
                // 按照地图、实例、主角的顺序刷新
                // (本更新顺序不会在满足事件的执行条件下成为给予角色瞬间移动
                //  的机会的重要因素)
                Global.game_map.update();
                Global.game_system.map_interpreter.update();
                Global.game_player.update();
                // 系统 (计时器)、画面刷新
                Global.game_system.update();
                Global.game_screen.update();
                // 如果主角在场所移动中就中断循环
                if (!Global.game_temp.player_transferring)
                    break;

                // 执行场所移动
                transfer_player();
                // 处理过渡中的情况下、中断循环
                if (Global.game_temp.transition_processing)
                    break;
            }
            // 刷新活动块
            this.spriteset.update();
            // 刷新信息窗口
            this.message_window.update();
            // 游戏结束的情况下
            if (Global.game_temp.gameover)
            {
                // 切换的游戏结束画面
                Global.scene = new Scene_Gameover();
                return;
            }
            // 返回标题画面的情况下
            if (Global.game_temp.to_title)
            {
                // 切换到标题画面
                Global.scene = new Scene_Title();
                return;
            }
            // 处理过渡中的情况下
            if (Global.game_temp.transition_processing)
            {
                // 清除过渡处理中标志
                Global.game_temp.transition_processing = false;
                // 执行过渡
                if (Global.game_temp.transition_name == "")
                    Graphics.transition(20);
                else
                {
                    Graphics.transition(40, "Graphics/Transitions/" +
                      Global.game_temp.transition_name);
                }
            }
            // 显示信息窗口中的情况下
            if (Global.game_temp.message_window_showing)
                return;

            // 遇敌计数为 0 且、且遇敌列表不为空的情况下
            if (Global.game_player.encounter_count == 0 && Global.game_map.encounter_list.Count != 0)
            {
                // 不是在事件执行中或者禁止遇敌中
                if (!(Global.game_system.map_interpreter.is_running ||
                                    Global.game_system.encounter_disabled))
                {

                    // 确定队伍
                    var n = Global.rand(Global.game_map.encounter_list.Count);
                    var troop_id = Global.game_map.encounter_list[n];
                    // 队伍有效的话
                    if (Global.data_troops[troop_id] != null)
                    {
                        // 设置调用战斗标志
                        Global.game_temp.battle_calling = true;
                        Global.game_temp.battle_troop_id = troop_id;
                        Global.game_temp.battle_can_escape = true;
                        Global.game_temp.battle_can_lose = false;
                        Global.game_temp.battle_proc = null;
                    }
                }
            }
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 不是在事件执行中或菜单禁止中
                if (!(Global.game_system.map_interpreter.is_running ||
                                    Global.game_system.menu_disabled))
                {

                    // 设置菜单调用标志以及 SE 演奏
                    Global.game_temp.menu_calling = true;
                    Global.game_temp.menu_beep = true;
                }
            }
            // 调试模式为 ON 并且按下 F9 键的情况下
            if (Global.DEBUG && Input.is_press(Input.F9))
            {
                // 设置调用调试标志
                Global.game_temp.debug_calling = true;
            }
            // 不在主角移动中的情况下
            if (!Global.game_player.is_moving)
            {
                // 执行各种画面的调用
                if (Global.game_temp.battle_calling)
                    call_battle();
                else if (Global.game_temp.shop_calling)
                    call_shop();
                else if (Global.game_temp.name_calling)
                    call_name();
                else if (Global.game_temp.menu_calling)
                    call_menu();
                else if (Global.game_temp.save_calling)
                    call_save();
                else if (Global.game_temp.debug_calling)
                    call_debug();
            }
        }
        //--------------------------------------------------------------------------
        // ● 调用战斗
        //--------------------------------------------------------------------------
        public void call_battle()
        {
            // 清除战斗调用标志
            Global.game_temp.battle_calling = false;
            // 清除菜单调用标志
            Global.game_temp.menu_calling = false;
            Global.game_temp.menu_beep = false;
            // 生成遇敌计数
            Global.game_player.make_encounter_count();
            // 记忆地图 RPG.AudioFile 、停止 RPG.AudioFile
            Global.game_temp.map_bgm = Global.game_system.playing_bgm;
            Global.game_system.bgm_stop();
            // 演奏战斗开始 SE
            Global.game_system.se_play(Global.data_system.battle_start_se);
            // 演奏战斗 RPG.AudioFile
            Global.game_system.bgm_play(Global.game_system.battle_bgm);
            // 矫正主角姿势
            Global.game_player.straighten();
            // 切换到战斗画面
            Global.scene = new Scene_Battle();
        }
        //--------------------------------------------------------------------------
        // ● 调用商店
        //--------------------------------------------------------------------------
        public void call_shop()
        {
            // 清除商店调用标志
            Global.game_temp.shop_calling = false;
            // 矫正主角姿势
            Global.game_player.straighten();
            // 切换到商店画面
            Global.scene = new Scene_Shop();
        }
        //--------------------------------------------------------------------------
        // ● 调用名称输入
        //--------------------------------------------------------------------------
        public void call_name()
        {
            // 清除调用名称输入标志
            Global.game_temp.name_calling = false;
            // 矫正主角姿势
            Global.game_player.straighten();
            // 切换到名称输入画面
            Global.scene = new Scene_Name();
        }
        //--------------------------------------------------------------------------
        // ● 调用菜单
        //--------------------------------------------------------------------------
        public void call_menu()
        {
            // 清除菜单调用标志
            Global.game_temp.menu_calling = false;
            // 已经设置了菜单 SE 演奏标志的情况下
            if (Global.game_temp.menu_beep)
            {
                // 演奏确定 SE
                Global.game_system.se_play(Global.data_system.decision_se);
                // 清除菜单演奏 SE 标志
                Global.game_temp.menu_beep = false;
            }
            // 矫正主角姿势
            Global.game_player.straighten();
            // 切换到菜单画面
            Global.scene = new Scene_Menu();
        }
        //--------------------------------------------------------------------------
        // ● 调用存档
        //--------------------------------------------------------------------------
        public void call_save()
        {
            // 矫正主角姿势
            Global.game_player.straighten();
            // 切换到存档画面
            Global.scene = new Scene_Save();
        }
        //--------------------------------------------------------------------------
        // ● 调用调试
        //--------------------------------------------------------------------------
        public void call_debug()
        {
            // 清除调用调试标志
            Global.game_temp.debug_calling = false;
            // 演奏确定 SE
            Global.game_system.se_play(Global.data_system.decision_se);
            // 矫正主角姿势
            Global.game_player.straighten();
            // 切换到调试画面
            Global.scene = new Scene_Debug();
        }
        //--------------------------------------------------------------------------
        // ● 主角的场所移动
        //--------------------------------------------------------------------------
        public void transfer_player()
        {
            // 清除主角场所移动调试标志
            Global.game_temp.player_transferring = false;
            // 移动目标与现在的地图有差异的情况下
            if (Global.game_map.map_id != Global.game_temp.player_new_map_id)
                // 设置新地图
                Global.game_map.setup(Global.game_temp.player_new_map_id);

            // 设置主角位置
            Global.game_player.moveto(Global.game_temp.player_new_x, Global.game_temp.player_new_y);
            // 设置主角朝向
            switch (Global.game_temp.player_new_direction)
            {
                case 2:  // 下
                    Global.game_player.turn_down(); break;
                case 4:  // 左
                    Global.game_player.turn_left(); break;
                case 6:  // 右
                    Global.game_player.turn_right(); break;
                case 8:  // 上
                    Global.game_player.turn_up(); break;
            }
            // 矫正主角姿势
            Global.game_player.straighten();
            // 刷新地图 (执行并行事件)
            Global.game_map.update();
            // 在生成活动块
            this.spriteset.dispose();
            this.spriteset = new Spriteset_Map();
            // 处理过渡中的情况下
            if (Global.game_temp.transition_processing)
            {
                // 清除过渡处理中标志
                Global.game_temp.transition_processing = false;
                // 执行过渡
                Graphics.transition(20);
            }
            // 执行地图设置的 RPG.AudioFile、BGS 的自动切换
            Global.game_map.autoplay();
            // 设置画面
            Graphics.frame_reset();
            // 刷新输入信息
            Input.update();
        }
    }

}
