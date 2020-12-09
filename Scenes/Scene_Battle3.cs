using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_Battle (分割定义 3)
    //------------------------------------------------------------------------------
    // 　处理战斗画面的类。
    //==============================================================================

    public partial class Scene_Battle : Scene
    {
        //--------------------------------------------------------------------------
        // ● 开始角色命令回合
        //--------------------------------------------------------------------------
        public void start_phase3()
        {
            // 转移到回合 3
            this.phase = 3;
            // 设置角色为非选择状态
            this.actor_index = -1;
            this.active_battler = null;
            // 输入下一个角色的命令
            phase3_next_actor();
        }
        //--------------------------------------------------------------------------
        // ● 转到输入下一个角色的命令
        //--------------------------------------------------------------------------
        public void phase3_next_actor()
        {
            // 循环
            do
            {
                // 角色的明灭效果 OFF
                if (this.active_battler != null)
                {
                    this.active_battler.blink = false;
                }
                // 最后的角色的情况
                if (this.actor_index == Global.game_party.actors.Count - 1)
                {
                    // 开始主回合
                    start_phase4();
                    return;
                }
                // 推进角色索引
                this.actor_index += 1;
                this.active_battler = Global.game_party.actors[this.actor_index];
                this.active_battler.blink = true;
                // 如果角色是在无法接受指令的状态就再试
            } while (!this.active_battler.is_inputable);
            // 设置角色的命令窗口
            phase3_setup_command_window();
        }
        //--------------------------------------------------------------------------
        // ● 转向前一个角色的命令输入
        //--------------------------------------------------------------------------
        public void phase3_prior_actor()
        {
            // 循环
            do
            {
                // 角色的明灭效果 OFF
                if (this.active_battler != null)
                {
                    this.active_battler.blink = false;
                }
                // 最初的角色的情况下
                if (this.actor_index == 0)
                {
                    // 开始同伴指令回合
                    start_phase2();
                    return;
                }
                // 返回角色索引
                this.actor_index -= 1;
                this.active_battler = Global.game_party.actors[this.actor_index];
                this.active_battler.blink = true;
                // 如果角色是在无法接受指令的状态就再试
            } while (this.active_battler.is_inputable);
            // 设置角色的命令窗口
            phase3_setup_command_window();
        }
        //--------------------------------------------------------------------------
        // ● 设置角色指令窗口
        //--------------------------------------------------------------------------
        public void phase3_setup_command_window()
        {
            // 同伴指令窗口无效化
            this.party_command_window.active = false;
            this.party_command_window.visible = false;
            // 角色指令窗口无效化
            this.actor_command_window.active = true;
            this.actor_command_window.visible = true;
            // 设置角色指令窗口的位置
            this.actor_command_window.x = this.actor_index * 160;
            // 设置索引为 0
            this.actor_command_window.index = 0;
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (角色命令回合)
        //--------------------------------------------------------------------------
        public void update_phase3()
        {
            // 敌人光标有效的情况下
            if (this.enemy_arrow != null)
            {
                update_phase3_enemy_select();
                // 角色光标有效的情况下
            }
            else if (this.actor_arrow != null)
            {
                update_phase3_actor_select();
                // 特技窗口有效的情况下
            }
            else if (this.skill_window != null)
            {
                update_phase3_skill_select();
                // 物品窗口有效的情况下
            }
            else if (this.item_window != null)
            {
                update_phase3_item_select();
                // 角色指令窗口有效的情况下
            }
            else if (this.actor_command_window.active)
            {
                update_phase3_basic_command();
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (角色命令回合 : 基本命令)
        //--------------------------------------------------------------------------
        public void update_phase3_basic_command()
        {
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 转向前一个角色的指令输入
                phase3_prior_actor();
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 角色指令窗口光标位置分之
                switch (this.actor_command_window.index)
                {
                    case 0:  // 攻击
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 设置行动
                        this.active_battler.current_action.kind = 0;
                        this.active_battler.current_action.basic = 0;
                        // 开始选择敌人
                        start_enemy_select();
                        break;
                    case 1:  // 特技
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 设置行动
                        this.active_battler.current_action.kind = 1;
                        // 开始选择特技
                        start_skill_select();
                        break;
                    case 2:  // 防御
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 设置行动
                        this.active_battler.current_action.kind = 0;
                        this.active_battler.current_action.basic = 1;
                        // 转向下一位角色的指令输入
                        phase3_next_actor();
                        break;
                    case 3:  // 物品
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 设置行动
                        this.active_battler.current_action.kind = 2;
                        // 开始选择物品
                        start_item_select();
                        break;
                }
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (角色命令回合 : 选择特技)
        //--------------------------------------------------------------------------
        public void update_phase3_skill_select()
        {
            // 设置特技窗口为可视状态
            this.skill_window.visible = true;
            // 刷新特技窗口
            this.skill_window.update();
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 结束特技选择
                end_skill_select();
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 获取特技选择窗口现在选择的特技的数据
                this.skill = this.skill_window.skill;
                // 无法使用的情况下
                if (this.skill == null || !this.active_battler.is_skill_can_use(this.skill.id))
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                    return;
                }
                // 演奏确定 SE
                Global.game_system.se_play(Global.data_system.decision_se);
                // 设置行动
                this.active_battler.current_action.skill_id = this.skill.id;
                // 设置特技窗口为不可见状态
                this.skill_window.visible = false;
                // 效果范围是敌单体的情况下
                if (this.skill.scope == 1)
                {
                    // 开始选择敌人
                    start_enemy_select();
                    // 效果范围是我方单体的情况下
                }
                else if (this.skill.scope == 3 || this.skill.scope == 5)
                {
                    // 开始选择角色
                    start_actor_select();
                    // 效果范围不是单体的情况下
                }
                else
                {
                    // 选择特技结束
                    end_skill_select();
                    // 转到下一位角色的指令输入
                    phase3_next_actor();
                }
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (角色命令回合 : 选择物品)
        //--------------------------------------------------------------------------
        public void update_phase3_item_select()
        {
            // 设置物品窗口为可视状态
            this.item_window.visible = true;
            // 刷新物品窗口
            this.item_window.update();
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 选择物品结束
                end_item_select();
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 获取物品窗口现在选择的物品资料
                this.item = (RPG.Item)this.item_window.item;
                // 无法使用的情况下
                if (!Global.game_party.is_item_can_use(this.item.id))
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                    return;
                }
                // 演奏确定 SE
                Global.game_system.se_play(Global.data_system.decision_se);
                // 设置行动
                this.active_battler.current_action.item_id = this.item.id;
                // 设置物品窗口为不可见状态
                this.item_window.visible = false;
                // 效果范围是敌单体的情况下
                if (this.item.scope == 1)
                {
                    // 开始选择敌人
                    start_enemy_select();
                    // 效果范围是我方单体的情况下
                }
                else if (this.item.scope == 3 || this.item.scope == 5)
                {
                    // 开始选择角色
                    start_actor_select();
                    // 效果范围不是单体的情况下
                }
                else
                {
                    // 物品选择结束
                    end_item_select();
                    // 转到下一位角色的指令输入
                    phase3_next_actor();
                }
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面画面 (角色命令回合 : 选择敌人)
        //--------------------------------------------------------------------------
        public void update_phase3_enemy_select()
        {
            // 刷新敌人箭头
            this.enemy_arrow.update();
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 选择敌人结束
                end_enemy_select();
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 演奏确定 SE
                Global.game_system.se_play(Global.data_system.decision_se);
                // 设置行动
                this.active_battler.current_action.target_index = this.enemy_arrow.index;
                // 选择敌人结束
                end_enemy_select();
                // 显示特技窗口中的情况下
                if (this.skill_window != null)
                {
                    // 结束特技选择
                    end_skill_select();
                }
                // 显示物品窗口的情况下
                if (this.item_window != null)
                {
                    // 结束物品选择
                    end_item_select();
                }
                // 转到下一位角色的指令输入
                phase3_next_actor();
            }
        }
        //--------------------------------------------------------------------------
        // ● 画面更新 (角色指令回合 : 选择角色)
        //--------------------------------------------------------------------------
        public void update_phase3_actor_select()
        {
            // 刷新角色箭头
            this.actor_arrow.update();
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 选择角色结束
                end_actor_select();
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 演奏确定 SE
                Global.game_system.se_play(Global.data_system.decision_se);
                // 设置行动
                this.active_battler.current_action.target_index = this.actor_arrow.index;
                // 选择角色结束
                end_actor_select();
                // 显示特技窗口中的情况下
                if (this.skill_window != null)
                {
                    // 结束特技选择
                    end_skill_select();
                }
                // 显示物品窗口的情况下
                if (this.item_window != null)
                {
                    // 结束物品选择
                    end_item_select();
                }
                // 转到下一位角色的指令输入
                phase3_next_actor();
            }
        }
        //--------------------------------------------------------------------------
        // ● 开始选择敌人
        //--------------------------------------------------------------------------
        public void start_enemy_select()
        {
            // 生成敌人箭头
            this.enemy_arrow = new Arrow_Enemy(this.spriteset.viewport1);
            // 关联帮助窗口
            this.enemy_arrow.help_window = this.help_window;
            // 无效化角色指令窗口
            this.actor_command_window.active = false;
            this.actor_command_window.visible = false;
        }
        //--------------------------------------------------------------------------
        // ● 结束选择敌人
        //--------------------------------------------------------------------------
        public void end_enemy_select()
        {
            // 释放敌人箭头
            this.enemy_arrow.dispose();
            this.enemy_arrow = null;
            // 指令为 [战斗] 的情况下
            if (this.actor_command_window.index == 0)
            {
                // 有效化角色指令窗口
                this.actor_command_window.active = true;
                this.actor_command_window.visible = true;
                // 隐藏帮助窗口
                this.help_window.visible = false;
            }
        }
        //--------------------------------------------------------------------------
        // ● 开始选择角色
        //--------------------------------------------------------------------------
        public void start_actor_select(){
    // 生成角色箭头
            this.actor_arrow = new Arrow_Actor(this.spriteset.viewport2);
    this.actor_arrow.index = this.actor_index;
    // 关联帮助窗口
    this.actor_arrow.help_window = this.help_window;
    // 无效化角色指令窗口
    this.actor_command_window.active = false;
    this.actor_command_window.visible = false;
  }
        //--------------------------------------------------------------------------
        // ● 结束选择角色
        //--------------------------------------------------------------------------
        public void end_actor_select()
        {
            // 释放角色箭头
            this.actor_arrow.dispose();
            this.actor_arrow = null;
        }
        //--------------------------------------------------------------------------
        // ● 开始选择特技
        //--------------------------------------------------------------------------
        public void start_skill_select()
        {
            // 生成特技窗口
            this.skill_window = new Window_Skill((Game_Actor)this.active_battler);
            // 关联帮助窗口
            this.skill_window.help_window = this.help_window;
            // 无效化角色指令窗口
            this.actor_command_window.active = false;
            this.actor_command_window.visible = false;
        }
        //--------------------------------------------------------------------------
        // ● 选择特技结束
        //--------------------------------------------------------------------------
        public void end_skill_select()
        {
            // 释放特技窗口
            this.skill_window.dispose();
            this.skill_window = null;
            // 隐藏帮助窗口
            this.help_window.visible = false;
            // 有效化角色指令窗口
            this.actor_command_window.active = true;
            this.actor_command_window.visible = true;
        }
        //--------------------------------------------------------------------------
        // ● 开始选择物品
        //--------------------------------------------------------------------------
        public void start_item_select()
        {
            // 生成物品窗口
            this.item_window = new Window_Item();
            // 关联帮助窗口
            this.item_window.help_window = this.help_window;
            // 无效化角色指令窗口
            this.actor_command_window.active = false;
            this.actor_command_window.visible = false;
        }
        //--------------------------------------------------------------------------
        // ● 结束选择物品
        //--------------------------------------------------------------------------
        public void end_item_select()
        {
            // 释放物品窗口
            this.item_window.dispose();
            this.item_window = null;
            // 隐藏帮助窗口
            this.help_window.visible = false;
            // 有效化角色指令窗口
            this.actor_command_window.active = true;
            this.actor_command_window.visible = true;
        }

        public Arrow_Enemy enemy_arrow { get; set; }

        public Arrow_Actor actor_arrow { get; set; }

        public Window_Skill skill_window { get; set; }

        public Window_Item item_window { get; set; }
    }

}
