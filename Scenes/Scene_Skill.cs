using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_Skill
    //------------------------------------------------------------------------------
    // 　处理特技画面的类。
    //==============================================================================

    public class Scene_Skill : Scene
    {
        public int actor_index;
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     actor_index : 角色索引
        //--------------------------------------------------------------------------
        public Scene_Skill(int actor_index = 0, int equip_index = 0)
        {
            this.actor_index = actor_index;
        }
        public override void Initialize()
        {
            // 获取角色
            this.actor = Global.game_party.actors[this.actor_index];
            // 生成帮助窗口、状态窗口、特技窗口
            this.help_window = new Window_Help();
            this.status_window = new Window_SkillStatus(this.actor);
            this.skill_window = new Window_Skill(this.actor);
            // 关联帮助窗口
            this.skill_window.help_window = this.help_window;
            // 生成目标窗口 (设置为不可见・不活动)
            this.target_window = new Window_Target();
            this.target_window.visible = false;
            this.target_window.active = false;
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
                // 如果画面切换的话就中断循环
            //    if (Global.scene != this)
            //        break;
            //}
        }
        public override void Uninitialize()
        {
            // 准备过渡
            Graphics.freeze();
            // 释放窗口
            this.help_window.dispose();
            this.status_window.dispose();
            this.skill_window.dispose();
            this.target_window.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 刷新窗口
            this.help_window.update();
            this.status_window.update();
            this.skill_window.update();
            this.target_window.update();
            // 特技窗口被激活的情况下: 调用 update_skill
            if (this.skill_window.active)
            {
                update_skill();
                return;
            }
            // 目标窗口被激活的情况下: 调用 update_target
            if (this.target_window.active)
            {
                update_target();
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (特技窗口被激活的情况下)
        //--------------------------------------------------------------------------
        public void update_skill()
        {
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 切换到菜单画面
                Global.scene = new Scene_Menu(1);
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 获取特技窗口现在选择的特技的数据
                this.skill = this.skill_window.skill;
                // 不能使用的情况下
                if (this.skill == null || !this.actor.is_skill_can_use(this.skill.id))
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                    return;
                }
                // 演奏确定 SE
                Global.game_system.se_play(Global.data_system.decision_se);
                // 效果范围是我方的情况下
                if (this.skill.scope >= 3)
                {
                    // 激活目标窗口
                    this.skill_window.active = false;
                    this.target_window.x = (this.skill_window.index + 1) % 2 * 304;
                    this.target_window.visible = true;
                    this.target_window.active = true;
                    // 设置效果范围 (单体/全体) 的对应光标位置
                    if (this.skill.scope == 4 || this.skill.scope == 6)
                        this.target_window.index = -1;
                    else if (this.skill.scope == 7)
                        this.target_window.index = this.actor_index - 10;
                    else
                        this.target_window.index = 0;
                }
                // 效果在我方以外的情况下
                else
                {
                    // 公共事件 ID 有效的情况下
                    if (this.skill.common_event_id > 0)
                    {
                        // 预约调用公共事件
                        Global.game_temp.common_event_id = this.skill.common_event_id;
                        // 演奏特技使用时的 SE
                        Global.game_system.se_play(this.skill.menu_se);
                        // 消耗 SP
                        this.actor.sp -= this.skill.sp_cost;
                        // 再生成各窗口的内容
                        this.status_window.refresh();
                        this.skill_window.refresh();
                        this.target_window.refresh();
                        // 切换到地图画面
                        Global.scene = new Scene_Map();
                        return;
                    }
                }
                return;
            }
            // 按下 R 键的情况下
            if (Input.is_trigger(Input.R))
            {
                // 演奏光标 SE
                Global.game_system.se_play(Global.data_system.cursor_se);
                // 移至下一位角色
                this.actor_index += 1;
                this.actor_index %= Global.game_party.actors.Count;
                // 切换到别的特技画面
                Global.scene = new Scene_Skill(this.actor_index);
                return;
            }
            // 按下 L 键的情况下
            if (Input.is_trigger(Input.L))
            {
                // 演奏光标 SE
                Global.game_system.se_play(Global.data_system.cursor_se);
                // 移至上一位角色
                this.actor_index += Global.game_party.actors.Count - 1;
                this.actor_index %= Global.game_party.actors.Count;
                // 切换到别的特技画面
                Global.scene = new Scene_Skill(this.actor_index);
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (目标窗口被激活的情况下)
        //--------------------------------------------------------------------------
        public void update_target()
        {
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 删除目标窗口
                this.skill_window.active = true;
                this.target_window.visible = false;
                this.target_window.active = false;
                return;
            }
            bool used = false;
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 因为 SP 不足而无法使用的情况下
                if (!this.actor.is_skill_can_use(this.skill.id))
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                    return;
                }
                // 目标是全体的情况下
                if (this.target_window.index == -1)
                {
                    // 对同伴全体应用特技使用效果
                    used = false;
                    foreach (var i in Global.game_party.actors)
                    {
                        used |= i.skill_effect(this.actor, this.skill);
                    }
                }
                Game_Actor target = null;
                // 目标是使用者的情况下
                if (this.target_window.index <= -2)
                {
                    // 对目标角色应用特技的使用效果
                    target = Global.game_party.actors[this.target_window.index + 10];
                    used = target.skill_effect(this.actor, this.skill);
                }
                // 目标是单体的情况下
                if (this.target_window.index >= 0)
                {
                    // 对目标角色应用特技的使用效果
                    target = Global.game_party.actors[this.target_window.index];
                    used = target.skill_effect(this.actor, this.skill);
                }
                // 使用特技的情况下
                if (used)
                {
                    // 演奏特技使用时的 SE
                    Global.game_system.se_play(this.skill.menu_se);
                    // 消耗 SP
                    this.actor.sp -= this.skill.sp_cost;
                    // 再生成各窗口内容
                    this.status_window.refresh();
                    this.skill_window.refresh();
                    this.target_window.refresh();
                    // 全灭的情况下
                    if (Global.game_party.is_all_dead)
                    {
                        // 切换到游戏结束画面
                        Global.scene = new Scene_Gameover();
                        return;
                    }
                    // 公共事件 ID 有效的情况下
                    if (this.skill.common_event_id > 0)
                    {
                        // 预约调用公共事件
                        Global.game_temp.common_event_id = this.skill.common_event_id;
                        // 切换到地图画面
                        Global.scene = new Scene_Map();
                        return;
                    }
                }
                // 无法使用特技的情况下
                if (!used)
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                }
                return;
            }
        }

        public Game_Actor actor { get; set; }

        public Window_Help help_window { get; set; }

        public Window_SkillStatus status_window { get; set; }

        public Window_Skill skill_window { get; set; }

        public Window_Target target_window { get; set; }

        public RPG.Skill skill { get; set; }
    }

}
