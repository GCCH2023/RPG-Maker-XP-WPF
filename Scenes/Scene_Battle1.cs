using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_Battle (分割定义 1)
    //------------------------------------------------------------------------------
    // 　处理战斗画面的类。
    //==============================================================================

    public partial class Scene_Battle : Scene
    {
        public Window_PartyCommand party_command_window { get; set; }
        public Window_Command actor_command_window { get; set; }

        public Window_Help help_window { get; set; }
        public Window_BattleStatus status_window { get; set; }
        public Window_Message message_window { get; set; }
        Spriteset_Battle _spriteset;

        public Spriteset_Battle spriteset
        {
            get { return _spriteset; }
            set { _spriteset = value; }
        }

        public override void Initialize()
        {
            // 初始化战斗用的各种暂时数据
            Global.game_temp.in_battle = true;
            Global.game_temp.battle_turn = 0;
            Global.game_temp.battle_event_flags.Clear();
            Global.game_temp.battle_abort = false;
            Global.game_temp.battle_main_phase = false;
            Global.game_temp.battleback_name = Global.game_map.battleback_name;
            Global.game_temp.forcing_battler = null;
            // 初始化战斗用事件解释器
            Global.game_system.battle_interpreter.setup(null, 0);
            // 准备队伍
            this.troop_id = Global.game_temp.battle_troop_id;
            Global.game_troop.setup(this.troop_id);
            // 生成角色命令窗口
            var s1 = Global.data_system.words.attack;
            var s2 = Global.data_system.words.skill;
            var s3 = Global.data_system.words.guard;
            var s4 = Global.data_system.words.item;
            this.actor_command_window = new Window_Command(160, new string[] { s1, s2, s3, s4 });
            this.actor_command_window.y = 160;
            this.actor_command_window.back_opacity = 160;
            this.actor_command_window.active = false;
            this.actor_command_window.visible = false;
            // 生成其它窗口
            this.party_command_window = new Window_PartyCommand();
            this.help_window = new Window_Help();
            this.help_window.back_opacity = 160;
            this.help_window.visible = false;
            this.status_window = new Window_BattleStatus();
            this.message_window = new Window_Message();
            // 生成活动块
            this.spriteset = new Spriteset_Battle();
            // 初始化等待计数
            this.wait_count = 0;
            // 执行过渡
            if (Global.data_system.battle_transition == "")
            {
                Graphics.transition(20);
            }
            else
            {
                Graphics.transition(40, "Graphics/Transitions/" +
                  Global.data_system.battle_transition);
            }
            // 开始自由战斗回合
            start_phase1();
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
            // 如果画面切换的话就中断循环
            //if (Global.scene != this)
            //{
            //    break;
            //}
            //}
        }
        public override void Uninitialize()
        {
            // 刷新地图
            Global.game_map.refresh();
            // 准备过渡
            Graphics.freeze();
            // 释放窗口
            this.actor_command_window.dispose();
            this.party_command_window.dispose();
            this.help_window.dispose();
            this.status_window.dispose();
            this.message_window.dispose();
            if (this.skill_window != null)
            {
                this.skill_window.dispose();
            }
            if (this.item_window != null)
            {
                this.item_window.dispose();
            }
            if (this.result_window != null)
            {
                this.result_window.dispose();
            }
            // 释放活动块
            this.spriteset.dispose();
            // 标题画面切换中的情况
            if (Global.scene is Scene_Title)
            {
                // 淡入淡出画面
                Graphics.transition();
                Graphics.freeze();
            }
            // 战斗测试或者游戏结束以外的画面切换中的情况
            if (Global.BTEST && !(Global.scene is Scene_Gameover))
            {
                Global.scene = null;
            }
        }
        //--------------------------------------------------------------------------
        // ● 胜负判定
        //--------------------------------------------------------------------------
        public bool judge
        {
            get
            {
                // 全灭判定是真、并且同伴人数为 0 的情况下
                if (Global.game_party.is_all_dead || Global.game_party.actors.Count == 0)
                {
                    // 允许失败的情况下
                    if (Global.game_temp.battle_can_lose)
                    {
                        // 还原为战斗开始前的 RPG.AudioFile
                        Global.game_system.bgm_play(Global.game_temp.map_bgm);
                        // 战斗结束
                        battle_end(2);
                        // 返回 true
                        return true;
                    }
                    // 设置游戏结束标志
                    Global.game_temp.gameover = true;
                    // 返回 true
                    return true;
                }
                // 如果存在任意 1 个敌人就返回 false
                foreach (var enemy in Global.game_troop.enemies)
                {
                    if (enemy.is_exist)
                    {
                        return false;
                    }
                }
                // 开始结束战斗回合 (胜利)
                start_phase5();
                // 返回 true
                return true;
            }
        }
        //--------------------------------------------------------------------------
        // ● 战斗结束
        //     result : 結果 (0:胜利 1:失败 2:逃跑)
        //--------------------------------------------------------------------------
        public void battle_end(int result)
        {
            // 清除战斗中标志
            Global.game_temp.in_battle = false;
            // 清除全体同伴的行动
            Global.game_party.clear_actions();
            // 解除战斗用状态
            foreach (var actor in Global.game_party.actors)
            {
                actor.remove_states_battle();
            }
            // 清除敌人
            Global.game_troop.enemies.Clear();
            // 调用战斗返回
            if (Global.game_temp.battle_proc != null)
            {
                // @@
                //Global.game_temp.battle_proc.call(result);
                Global.game_temp.battle_proc = null;
            }
            // 切换到地图画面
            Global.scene = new Scene_Map();
        }
        //--------------------------------------------------------------------------
        // ● 设置战斗事件
        //--------------------------------------------------------------------------
        public void setup_battle_event()
        {
            // 正在执行战斗事件的情况下
            if (Global.game_system.battle_interpreter.is_running)
            {
                return;
            }
            // 搜索全部页的战斗事件
            for (var index = 0; index < Global.data_troops[this.troop_id].pages.Count; index++)
            {
                // 获取事件页
                var page = Global.data_troops[this.troop_id].pages[index];
                // 事件条件可以参考 c
                var c = page.condition;
                // 没有指定任何条件的情况下转到下一页
                if (!c.turn_valid || c.enemy_valid ||
                       c.actor_valid || c.switch_valid)
                {
                    continue;
                }
                // 执行完毕的情况下转到下一页
                if (Global.game_temp.battle_event_flags[index])
                {
                    continue;
                }
                // 确认回合条件
                if (c.turn_valid)
                {
                    var n = Global.game_temp.battle_turn;
                    var a = c.turn_a;
                    var b = c.turn_b;
                    if ((b == 0 && n != a) ||
                       (b > 0 && (n < 1 || n < a || n % b != a % b)))
                    {
                        continue;
                    }
                }
                // 确认敌人条件
                if (c.enemy_valid)
                {
                    var enemy = Global.game_troop.enemies[c.enemy_index];
                    if (enemy == null || enemy.hp * 100.0 / enemy.maxhp > c.enemy_hp)
                    {
                        continue;
                    }
                }
                // 确认角色条件
                if (c.actor_valid)
                {
                    var actor = Global.game_actors[c.actor_id];
                    if (actor == null || actor.hp * 100.0 / actor.maxhp > c.actor_hp)
                    {
                        continue;
                    }
                }
                // 确认开关条件
                if (c.switch_valid)
                {
                    if (Global.game_switches[c.switch_id] == false)
                    {
                        continue;
                    }
                }
                // 设置事件
                Global.game_system.battle_interpreter.setup(page.list, 0);
                // 本页的范围是 [战斗] 或 [回合] 的情况下
                if (page.span <= 1)
                {
                    // 设置执行结束标志
                    Global.game_temp.battle_event_flags[index] = true;
                }
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 执行战斗事件中的情况下
            if (Global.game_system.battle_interpreter.is_running)
            {
                // 刷新解释器
                Global.game_system.battle_interpreter.update();
                // 强制行动的战斗者不存在的情况下
                if (Global.game_temp.forcing_battler == null)
                {
                    // 执行战斗事件结束的情况下
                    if (!Global.game_system.battle_interpreter.is_running)
                    {
                        // 继续战斗的情况下、再执行战斗事件的设置
                        if (!judge)
                        {
                            setup_battle_event();
                        }
                    }
                    // 如果不是结束战斗回合的情况下
                    if (this.phase != 5)
                    {
                        // 刷新状态窗口
                        this.status_window.refresh();
                    }
                }
            }
            // 系统 (计时器)、刷新画面
            Global.game_system.update();
            Global.game_screen.update();
            // 计时器为 0 的情况下
            if (Global.game_system.timer_working && Global.game_system.timer == 0)
            {
                // 中断战斗
                Global.game_temp.battle_abort = true;
            }
            // 刷新窗口
            this.help_window.update();
            this.party_command_window.update();
            this.actor_command_window.update();
            this.status_window.update();
            this.message_window.update();
            // 刷新活动块
            this.spriteset.update();
            // 处理过渡中的情况下
            if (Global.game_temp.transition_processing)
            {
                // 清除处理过渡中标志
                Global.game_temp.transition_processing = false;
                // 执行过渡
                if (Global.game_temp.transition_name == "")
                {
                    Graphics.transition(20);
                }
                else
                {
                    Graphics.transition(40, "Graphics/Transitions/" +
                      Global.game_temp.transition_name);
                }
            }
            // 显示信息窗口中的情况下
            if (Global.game_temp.message_window_showing)
            {
                return;
            }
            // 显示效果中的情况下
            if (this.spriteset.is_effect)
            {
                return;
            }
            // 游戏结束的情况下
            if (Global.game_temp.gameover)
            {
                // 切换到游戏结束画面
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
            // 中断战斗的情况下
            if (Global.game_temp.battle_abort)
            {
                // 还原为战斗前的 RPG.AudioFile
                Global.game_system.bgm_play(Global.game_temp.map_bgm);
                // 战斗结束
                battle_end(1);
                return;
            }
            // 等待中的情况下
            if (this.wait_count > 0)
            {
                // 减少等待计数
                this.wait_count -= 1;
                return;
            }
            // 强制行动的角色存在、
            // 并且战斗事件正在执行的情况下
            if (Global.game_temp.forcing_battler == null &&
               Global.game_system.battle_interpreter.is_running)
            {
                return;
            }
            // 回合分支
            switch (this.phase)
            {
                case 1:  // 自由战斗回合
                    update_phase1(); break;
                case 2:  // 同伴命令回合
                    update_phase2(); break;
                case 3:  // 角色命令回合
                    update_phase3(); break;
                case 4:  // 主回合
                    update_phase4(); break;
                case 5:  // 战斗结束回合
                    update_phase5(); break;
            }
        }

    }
}
