using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_Battle (分割定义 4)
    //------------------------------------------------------------------------------
    // 　处理战斗画面的类。
    //==============================================================================

    public partial class Scene_Battle : Scene
    {
        //--------------------------------------------------------------------------
        // ● 开始主回合
        //--------------------------------------------------------------------------
        public void start_phase4()
        {
            // 转移到回合 4
            this.phase = 4;
            // 回合数计数
            Global.game_temp.battle_turn += 1;
            // 搜索全页的战斗事件
            for (var index = 0; index < Global.data_troops[this.troop_id].pages.Count; index++)
            {
                // 获取事件页
                var page = Global.data_troops[this.troop_id].pages[index];
                // 本页的范围是 [回合] 的情况下
                if (page.span == 1)
                {
                    // 设置已经执行标志
                    Global.game_temp.battle_event_flags[index] = false;
                }
            }
            // 设置角色为非选择状态
            this.actor_index = -1;
            this.active_battler = null;
            // 有效化同伴指令窗口
            this.party_command_window.active = false;
            this.party_command_window.visible = false;
            // 无效化角色指令窗口
            this.actor_command_window.active = false;
            this.actor_command_window.visible = false;
            // 设置主回合标志
            Global.game_temp.battle_main_phase = true;
            // 生成敌人行动
            foreach (var enemy in Global.game_troop.enemies)
            {
                enemy.make_action();
            }
            // 生成行动顺序
            make_action_orders();
            // 移动到步骤 1
            this.phase4_step = 1;
        }
        //--------------------------------------------------------------------------
        // ● 生成行动循序
        //--------------------------------------------------------------------------
        public void make_action_orders()
        {
            // 初始化序列 this.action_battlers
            this.action_battlers = new List<Game_Battler>();
            // 添加敌人到 this.action_battlers 序列
            foreach (var enemy in Global.game_troop.enemies)
            {
                this.action_battlers.Add(enemy);
            }
            // 添加角色到 this.action_battlers 序列
            foreach (var actor in Global.game_party.actors)
            {
                this.action_battlers.Add(actor);
            }
            // 确定全体的行动速度
            foreach (var battler in this.action_battlers)
            {
                battler.make_action_speed();
            }
            // 按照行动速度从大到小排列
            this.action_battlers.Sort(
                (Game_Battler a, Game_Battler b) =>
                {
                    return b.current_action.speed - a.current_action.speed;
                });
            //this.action_battlers.sort! {|a,b|
            //  b.current_action.speed - a.current_action.speed }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (主回合)
        //--------------------------------------------------------------------------
        public void update_phase4()
        {
            switch (this.phase4_step)
            {
                case 1:
                    update_phase4_step1(); break;
                case 2:
                    update_phase4_step2(); break;
                case 3:
                    update_phase4_step3(); break;
                case 4:
                    update_phase4_step4(); break;
                case 5:
                    update_phase4_step5(); break;
                case 6:
                    update_phase4_step6(); break;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (主回合步骤 1 : 准备行动)
        //--------------------------------------------------------------------------
        public void update_phase4_step1()
        {
            // 隐藏帮助窗口
            this.help_window.visible = false;
            // 判定胜败
            if (judge)
            {
                // 胜利或者失败的情况下 : 过程结束
                return;
            }
            // 强制行动的战斗者不存在的情况下
            if (Global.game_temp.forcing_battler == null)
            {
                // 设置战斗事件
                setup_battle_event();
                // 执行战斗事件中的情况下
                if (Global.game_system.battle_interpreter.is_running)
                {
                    return;
                }
            }
            // 强制行动的战斗者存在的情况下
            if (Global.game_temp.forcing_battler != null)
            {
                // 在头部添加后移动
                this.action_battlers.Remove(Global.game_temp.forcing_battler);
                this.action_battlers.Insert(0, Global.game_temp.forcing_battler);
            }
            // 未行动的战斗者不存在的情况下 (全员已经行动)
            if (this.action_battlers.Count == 0)
            {
                // 开始同伴命令回合
                start_phase2();
                return;
            }
            // 初始化动画 ID 和公共事件 ID
            this.animation1_id = 0;
            this.animation2_id = 0;
            this.common_event_id = 0;
            // 未行动的战斗者移动到序列的头部
            this.active_battler = this.action_battlers[0];
            this.action_battlers.RemoveAt(0);
            // 如果已经在战斗之外的情况下
            if (this.active_battler.index == null)
            {
                return;
            }
            // 连续伤害
            if (this.active_battler.hp > 0 && this.active_battler.is_slip_damage)
            {
                this.active_battler.slip_damage_effect();
                this.active_battler.damage_pop = true;
            }
            // 自然解除状态
            this.active_battler.remove_states_auto();
            // 刷新状态窗口
            this.status_window.refresh();
            // 移至步骤 2
            this.phase4_step = 2;
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (主回合步骤 2 : 开始行动)
        //--------------------------------------------------------------------------
        public void update_phase4_step2()
        {
            // 如果不是强制行动
            if (!this.active_battler.current_action.forcing)
            {
                // 限制为 [敌人为普通攻击] 或 [我方为普通攻击] 的情况下
                if (this.active_battler.restriction == 2 || this.active_battler.restriction == 3)
                {
                    // 设置行动为攻击
                    this.active_battler.current_action.kind = 0;
                    this.active_battler.current_action.basic = 0;
                }
                // 限制为 [不能行动] 的情况下
                if (this.active_battler.restriction == 4)
                {
                    // 清除行动强制对像的战斗者
                    Global.game_temp.forcing_battler = null;
                    // 移至步骤 1
                    this.phase4_step = 1;
                    return;
                }
            }
            // 清除对像战斗者
            this.target_battlers = new List<Game_Battler>();
            // 行动种类分支
            switch (this.active_battler.current_action.kind)
            {
                case 0:  // 基本
                    make_basic_action_result(); break;
                case 1:  // 特技
                    make_skill_action_result(); break;
                case 2:  // 物品
                    make_item_action_result(); break;
            }
            // 移至步骤 3
            if (this.phase4_step == 2)
            {
                this.phase4_step = 3;
            }
        }
        //--------------------------------------------------------------------------
        // ● 生成基本行动结果
        //--------------------------------------------------------------------------
        public void make_basic_action_result()
        {
            Game_Battler target = null;
            int index = 0;
            // 攻击的情况下
            if (this.active_battler.current_action.basic == 0)
            {
                // 设置攻击 ID
                this.animation1_id = this.active_battler.animation1_id;
                this.animation2_id = this.active_battler.animation2_id;
                // 行动方的战斗者是敌人的情况下
                if (this.active_battler is Game_Enemy)
                {
                    if (this.active_battler.restriction == 3)
                    {
                        target = Global.game_troop.random_target_enemy();
                    }
                    else if (this.active_battler.restriction == 2)
                    {
                        target = Global.game_party.random_target_actor();
                    }
                    else
                    {
                        index = this.active_battler.current_action.target_index;
                        target = Global.game_party.smooth_target_actor(index);
                    }
                }
                // 行动方的战斗者是角色的情况下
                if (this.active_battler is Game_Actor)
                {
                    if (this.active_battler.restriction == 3)
                    {
                        target = Global.game_party.random_target_actor();
                    }
                    else if (this.active_battler.restriction == 2)
                    {
                        target = Global.game_troop.random_target_enemy();
                    }
                    else
                    {
                        index = this.active_battler.current_action.target_index;
                        target = Global.game_troop.smooth_target_enemy(index);
                    }
                }
                // 设置对像方的战斗者序列
                this.target_battlers = new List<Game_Battler>() { target };
                // 应用通常攻击效果
                foreach (var target1 in this.target_battlers)
                {
                    target1.attack_effect(this.active_battler);
                }
                return;
            }
            // 防御的情况下
            if (this.active_battler.current_action.basic == 1)
            {
                // 帮助窗口显示"防御"
                this.help_window.set_text(Global.data_system.words.guard, 1);
                return;
            }
            // 逃跑的情况下
            if (this.active_battler is Game_Enemy &&
               this.active_battler.current_action.basic == 2)
            {
                //  帮助窗口显示"逃跑"
                this.help_window.set_text("逃跑", 1);
                // 逃跑
                ((Game_Enemy)this.active_battler).escape();
                return;
            }
            // 什么也不做的情况下
            if (this.active_battler.current_action.basic == 3)
            {
                // 清除强制行动对像的战斗者
                Global.game_temp.forcing_battler = null;
                // 移至步骤 1
                this.phase4_step = 1;
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 设置物品或特技对像方的战斗者
        //     scope : 特技或者是物品的范围
        //--------------------------------------------------------------------------
        public void set_target_battlers(int scope)
        {
            int index = 0;
            // 行动方的战斗者是敌人的情况下
            if (this.active_battler is Game_Enemy)
            {
                // 效果范围分支
                switch (scope)
                {
                    case 1:  // 敌单体
                        index = this.active_battler.current_action.target_index;
                        this.target_battlers.Add(Global.game_party.smooth_target_actor(index));
                        break;
                    case 2:  // 敌全体
                        foreach (var actor in Global.game_party.actors)
                        {
                            if (actor.is_exist)
                            {
                                this.target_battlers.Add(actor);
                            }
                        }
                        break;
                    case 3:  // 我方单体
                        index = this.active_battler.current_action.target_index;
                        this.target_battlers.Add(Global.game_troop.smooth_target_enemy(index));
                        break;
                    case 4:  // 我方全体
                        foreach (var enemy in Global.game_troop.enemies)
                        {
                            if (enemy.is_exist)
                            {
                                this.target_battlers.Add(enemy);
                            }
                        }
                        break;
                    case 5:  // 我方单体 (HP 0) 
                        {
                            index = this.active_battler.current_action.target_index;
                            var enemy = Global.game_troop.enemies[index];
                            if (enemy != null && enemy.is_hp0)
                            {
                                this.target_battlers.Add(enemy);
                            }
                        }
                        break;
                    case 6:  // 我方全体 (HP 0) 
                        foreach (var enemy in Global.game_troop.enemies)
                        {
                            if (enemy != null && enemy.is_hp0)
                            {
                                this.target_battlers.Add(enemy);
                            }
                        }
                        break;
                    case 7:  // 使用者
                        this.target_battlers.Add(this.active_battler);
                        break;
                }
            }
            // 行动方的战斗者是角色的情况下
            if (this.active_battler is Game_Actor)
            {
                // 效果范围分支
                switch (scope)
                {
                    case 1:  // 敌单体
                        index = this.active_battler.current_action.target_index;
                        this.target_battlers.Add(Global.game_troop.smooth_target_enemy(index));
                        break;
                    case 2:  // 敌全体
                        foreach (var enemy in Global.game_troop.enemies)
                        {
                            if (enemy.is_exist)
                            {
                                this.target_battlers.Add(enemy);
                            }
                        }
                        break;
                    case 3:  // 我方单体
                        index = this.active_battler.current_action.target_index;
                        this.target_battlers.Add(Global.game_party.smooth_target_actor(index));
                        break;
                    case 4:  // 我方全体
                        foreach (var actor in Global.game_party.actors)
                        {
                            if (actor.is_exist)
                            {
                                this.target_battlers.Add(actor);
                            }
                        }
                        break;
                    case 5:  // 我方单体 (HP 0) 
                        {
                            index = this.active_battler.current_action.target_index;
                            var actor = Global.game_party.actors[index];
                            if (actor != null && actor.is_hp0)
                            {
                                this.target_battlers.Add(actor);
                            }
                        }
                        break;
                    case 6:  // 我方全体 (HP 0) 
                        foreach (var actor in Global.game_party.actors)
                        {
                            if (actor != null && actor.is_hp0)
                            {
                                this.target_battlers.Add(actor);
                            }
                        }
                        break;
                    case 7:  // 使用者
                        this.target_battlers.Add(this.active_battler);
                        break;
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 生成特技行动结果
        //--------------------------------------------------------------------------
        public void make_skill_action_result()
        {
            // 获取特技
            this.skill = Global.data_skills[this.active_battler.current_action.skill_id];
            // 如果不是强制行动
            if (!this.active_battler.current_action.forcing)
            {
                // 因为 SP 耗尽而无法使用的情况下
                if (!this.active_battler.is_skill_can_use(this.skill.id))
                {
                    // 清除强制行动对像的战斗者
                    Global.game_temp.forcing_battler = null;
                    // 移至步骤 1
                    this.phase4_step = 1;
                    return;
                }
            }
            // 消耗 SP
            this.active_battler.sp -= this.skill.sp_cost;
            // 刷新状态窗口
            this.status_window.refresh();
            // 在帮助窗口显示特技名
            this.help_window.set_text(this.skill.name, 1);
            // 设置动画 ID
            this.animation1_id = this.skill.animation1_id;
            this.animation2_id = this.skill.animation2_id;
            // 设置公共事件 ID
            this.common_event_id = this.skill.common_event_id;
            // 设置对像侧战斗者
            set_target_battlers(this.skill.scope);
            // 应用特技效果
            foreach (var target in this.target_battlers)
            {
                target.skill_effect(this.active_battler, this.skill);
            }
        }
        //--------------------------------------------------------------------------
        // ● 生成物品行动结果
        //--------------------------------------------------------------------------
        public void make_item_action_result()
        {
            // 获取物品
            this.item = Global.data_items[this.active_battler.current_action.item_id];
            // 因为物品耗尽而无法使用的情况下
            if (!Global.game_party.is_item_can_use(this.item.id))
            {
                // 移至步骤 1
                this.phase4_step = 1;
                return;
            }
            // 消耗品的情况下
            if (this.item.consumable)
            {
                // 使用的物品减 1
                Global.game_party.lose_item(this.item.id, 1);
            }
            // 在帮助窗口显示物品名
            this.help_window.set_text(this.item.name, 1);
            // 设置动画 ID
            this.animation1_id = this.item.animation1_id;
            this.animation2_id = this.item.animation2_id;
            // 设置公共事件 ID
            this.common_event_id = this.item.common_event_id;
            // 确定对像
            var index = this.active_battler.current_action.target_index;
            var target = Global.game_party.smooth_target_actor(index);
            // 设置对像侧战斗者
            set_target_battlers(this.item.scope);
            // 应用物品效果
            foreach (var target1 in this.target_battlers)
            {
                target1.item_effect(this.item);
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (主回合步骤 3 : 行动方动画)
        //--------------------------------------------------------------------------
        public void update_phase4_step3()
        {
            // 行动方动画 (ID 为 0 的情况下是白色闪烁)
            if (this.animation1_id == 0)
            {
                this.active_battler.white_flash = true;
            }
            else
            {
                this.active_battler.animation_id = this.animation1_id;
                this.active_battler.animation_hit = true;
            }
            // 移至步骤 4
            this.phase4_step = 4;
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (主回合步骤 4 : 对像方动画)
        //--------------------------------------------------------------------------
        public void update_phase4_step4()
        {
            // 对像方动画
            foreach (var target in this.target_battlers)
            {
                target.animation_id = this.animation2_id;
                target.animation_hit = (target.damage != "Miss");
            }
            // 限制动画长度、最低 8 帧
            this.wait_count = 8;
            // 移至步骤 5
            this.phase4_step = 5;
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (主回合步骤 5 : 显示伤害)
        //--------------------------------------------------------------------------
        public void update_phase4_step5()
        {
            // 隐藏帮助窗口
            this.help_window.visible = false;
            // 刷新状态窗口
            this.status_window.refresh();
            // 显示伤害
            foreach (var target in this.target_battlers)
            {
                if (target.damage != null)
                {
                    target.damage_pop = true;
                }
            }
            // 移至步骤 6
            this.phase4_step = 6;
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (主回合步骤 6 : 刷新)
        //--------------------------------------------------------------------------
        public void update_phase4_step6()
        {
            // 清除强制行动对像的战斗者
            Global.game_temp.forcing_battler = null;
            // 公共事件 ID 有效的情况下
            if (this.common_event_id > 0)
            {
                // 设置事件
                var common_event = Global.data_common_events[this.common_event_id];
                Global.game_system.battle_interpreter.setup(common_event.list, 0);
            }
            // 移至步骤 1
            this.phase4_step = 1;
        }

        public int phase { get; set; }
        public int troop_id { get; set; }
        public int actor_index { get; set; }
        public int phase4_step { get; set; }

        public List<Game_Battler> action_battlers { get; set; }

        public int animation1_id { get; set; }

        public int animation2_id { get; set; }

        public int common_event_id { get; set; }

        public Game_Battler active_battler { get; set; }

        public List<Game_Battler> target_battlers { get; set; }

        public RPG.Skill skill { get; set; }

        public RPG.Item item { get; set; }

        public int wait_count { get; set; }
    }

}
