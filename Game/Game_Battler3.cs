using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_Battler (分割定义 3)
    //------------------------------------------------------------------------------
    // 　处理战斗者的类。这个类作为 Game_Actor 类与 Game_Enemy 类的
    // 超级类来使用。
    //==============================================================================

    public partial class Game_Battler
    {


        public virtual List<int> minus_state_set { get; set; }
        public virtual List<int> plus_state_set { get; set; }
        //--------------------------------------------------------------------------
        // ● 可以使用特技的判定
        //     skill_id : 特技 ID
        //--------------------------------------------------------------------------
        public virtual bool is_skill_can_use(int skill_id)
        {
            // SP 不足的情况下不能使用
            if (Global.data_skills[skill_id].sp_cost > this.sp)
                return false;
            // 战斗不能的情况下不能使用
            if (is_dead)
                return false;
            // 沉默状态的情况下、物理特技以外的特技不能使用
            if (Global.data_skills[skill_id].atk_f == 0 && this.restriction == 1)
                return false;
            // 获取可以使用的时机
            var occasion = Global.data_skills[skill_id].occasion;
            // 战斗中的情况下
            if (Global.game_temp.in_battle)
                // [平时] 或者是 [战斗中] 可以使用
                return (occasion == 0 || occasion == 1);
            // 不是战斗中的情况下
            else
                // [平时] 或者是 [菜单中] 可以使用
                return (occasion == 0 || occasion == 2);
        }
        //--------------------------------------------------------------------------
        // ● 应用通常攻击效果
        //     attacker : 攻击者 (battler)
        //--------------------------------------------------------------------------
        public bool attack_effect(Game_Battler attacker)
        {
            double damage = 0.0;
            if(this.damage is double)
                damage = Convert.ToDouble(this.damage);
            // 清除会心一击标志
            this.critical = false;
            // 第一命中判定
            var hit_result = (Global.rand(100) < attacker.hit);
            // 命中的情况下
            if (hit_result == true)
            {
                // 计算基本伤害
                var atk = Math.Max(attacker.atk - this.pdef / 2, 0);
                damage = atk * (20 + attacker.str) / 20;
                // 属性修正
                damage *= elements_correct(attacker.element_set);
                damage /= 100;
                // 伤害符号正确的情况下
                if (damage > 0)
                {
                    // 会心一击修正
                    if (Global.rand(100) < 4 * attacker.dex / this.agi)
                    {
                        damage *= 2;
                        this.critical = true;
                    }
                    // 防御修正
                    if (this.is_guarding)
                        damage /= 2;
                }
                // 分散
                if (Math.Abs(damage) > 0)
                {
                    var amp = Math.Max(Math.Abs(damage) * 15 / 100, 1);
                    damage += Global.rand((int)amp + 1) + Global.rand((int)amp + 1) - amp;
                }
                // 第二命中判定
                var eva = 8 * this.agi / attacker.dex + this.eva;
                var hit = damage < 0 ? 100 : 100 - eva;
                hit = this.is_cant_evade ? 100 : hit;
                hit_result = (Global.rand(100) < hit);
            }
            // 命中的情况下
            if (hit_result == true)
            {
                // 状态冲击解除
                remove_states_shock();
                // HP 的伤害计算
                this.hp -= damage;
                // 状态变化
                this.state_changed = false;
                states_plus(attacker.plus_state_set);
                states_minus(attacker.minus_state_set);
            }
            // Miss 的情况下
            else
            {
                // 伤害设置为 "Miss"
                this.damage = "Miss";
                // 清除会心一击标志
                this.critical = false;
            }
            this.damage = damage;
            // 过程结束
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 应用特技效果
        //     user  : 特技的使用者 (battler)
        //     skill : 特技
        //--------------------------------------------------------------------------
        public bool skill_effect(Game_Battler user, RPG.Skill skill)
        {
            // 清除会心一击标志
            this.critical = false;
            // 特技的效果范围是 HP 1 以上的己方、自己的 HP 为 0、
            // 或者特技的效果范围是 HP 0 的己方、自己的 HP 为 1 以上的情况下
            if (((skill.scope == 3 || skill.scope == 4) && this.hp == 0) ||
                      ((skill.scope == 5 || skill.scope == 6) && this.hp >= 1))
                // 过程结束
                return false;

            double damage = 0.0;
            if (this.damage is double)
                damage = Convert.ToDouble(this.damage);

            // 清除有效标志
            var effective = false;
            // 公共事件 ID 是有效的情况下,设置为有效标志
            effective |= skill.common_event_id > 0;
            // 第一命中判定
            var hit = skill.hit;
            if (skill.atk_f > 0)
                hit *= (int)(user.hit / 100);
            var hit_result = (Global.rand(100) < hit);
            // 不确定的特技的情况下设置为有效标志
            effective |= hit < 100;
            // 命中的情况下
            if (hit_result == true)
            {
                // 计算威力
                var power = skill.power + user.atk * skill.atk_f / 100;
                if (power > 0)
                {
                    power -= this.pdef * skill.pdef_f / 200;
                    power -= this.mdef * skill.mdef_f / 200;
                    power = Math.Max(power, 0);
                }
                // 计算倍率
                double rate = 20;
                rate += (user.str * skill.str_f / 100);
                rate += (user.dex * skill.dex_f / 100);
                rate += (user.agi * skill.agi_f / 100);
                rate += (user.int1 * skill.int_f / 100);
                // 计算基本伤害
                damage = power * rate / 20;
                // 属性修正
                damage *= elements_correct(skill.element_set);
                damage /= 100;
                // 伤害符号正确的情况下
                if (damage > 0)
                    // 防御修正
                    if (this.is_guarding)
                        damage /= 2;
                // 分散
                if (skill.variance > 0 && Math.Abs(damage) > 0)
                {
                    var amp = Math.Max(Math.Abs(damage) * skill.variance / 100, 1);
                    damage += Global.rand((int)amp + 1) + Global.rand((int)amp + 1) - amp;
                }
                // 第二命中判定
                var eva = 8 * this.agi / user.dex + this.eva;
                hit = damage < 0 ? 100 : (int)(100 - eva * skill.eva_f / 100);
                hit = this.is_cant_evade ? 100 : hit;
                hit_result = (Global.rand(100) < hit);
                // 不确定的特技的情况下设置为有效标志
                effective |= hit < 100;
            }
            this.damage = damage;
            // 命中的情况下
            if (hit_result == true)
            {
                // 威力 0 以外的物理攻击的情况下
                if (skill.power != 0 && skill.atk_f > 0)
                {
                    // 状态冲击解除
                    remove_states_shock();
                    // 设置有效标志
                    effective = true;
                }
                // HP 的伤害减法运算
                var last_hp = this.hp;
                this.hp -= damage;
                effective |= this.hp != last_hp;
                // 状态变化
                this.state_changed = false;
                effective |= states_plus(skill.plus_state_set);
                effective |= states_minus(skill.minus_state_set);
                // 威力为 0 的场合
                if (skill.power == 0)
                {
                    // 伤害设置为空的字串
                    this.damage = "";
                    // 状态没有变化的情况下
                    if (!this.state_changed)
                        // 伤害设置为 "Miss"
                        this.damage = "Miss";

                }
            }
            // Miss 的情况下
            else
                // 伤害设置为 "Miss"
                this.damage = "Miss";
            // 不在战斗中的情况下
            if (!Global.game_temp.in_battle)
                // 伤害设置为 null
                this.damage = null;
            // 过程结束
            return effective;
        }
        //--------------------------------------------------------------------------
        // ● 应用物品效果
        //     item : 物品
        //--------------------------------------------------------------------------
        public bool item_effect(RPG.Item item)
        {
            // 清除会心一击标志
            this.critical = false;
            // 物品的效果范围是 HP 1 以上的己方、自己的 HP 为 0、
            // 或者物品的效果范围是 HP 0 的己方、自己的 HP 为 1 以上的情况下
            if (((item.scope == 3 || item.scope == 4) && this.hp == 0) ||
                      ((item.scope == 5 || item.scope == 6) && this.hp >= 1))
                // 过程结束
                return false;
            // 清除有效标志
            var effective = false;
            // 公共事件 ID 是有效的情况下,设置为有效标志
            effective |= item.common_event_id > 0;
            // 命中判定
            var hit_result = (Global.rand(100) < item.hit);
            // 不确定的特技的情况下设置为有效标志
            effective |= item.hit < 100;
            // 命中的情况
            if (hit_result == true)
            {
                // 计算回复量
                var recover_hp = maxhp * item.recover_hp_rate / 100 + item.recover_hp;
                var recover_sp = maxsp * item.recover_sp_rate / 100 + item.recover_sp;
                if (recover_hp < 0)
                {
                    recover_hp += this.pdef * item.pdef_f / 20;
                    recover_hp += this.mdef * item.mdef_f / 20;
                    recover_hp = Math.Min(recover_hp, 0);
                }
                // 属性修正
                recover_hp *= elements_correct(item.element_set);
                recover_hp /= 100;
                recover_sp *= elements_correct(item.element_set);
                recover_sp /= 100;
                // 分散
                if (item.variance > 0 && Math.Abs(recover_hp) > 0)
                {
                    var amp = (int)Math.Max(Math.Abs(recover_hp) * item.variance / 100, 1);
                    recover_hp += Global.rand(amp + 1) + Global.rand(amp + 1) - amp;
                }
                if (item.variance > 0 && Math.Abs(recover_sp) > 0)
                {
                    int amp = (int)Math.Max(Math.Abs(recover_sp) * item.variance / 100, 1);
                    recover_sp += Global.rand(amp + 1) + Global.rand(amp + 1) - amp;
                }
                // 回复量符号为负的情况下
                if (recover_hp < 0)
                    // 防御修正
                    if (this.is_guarding)
                        recover_hp /= 2;
                // HP 回复量符号的反转、设置伤害值
                this.damage = -recover_hp;
                // HP 以及 SP 的回复
                var last_hp = this.hp;
                var last_sp = this.sp;
                this.hp += recover_hp;
                this.sp += recover_sp;
                effective |= this.hp != last_hp;
                effective |= this.sp != last_sp;
                // 状态变化
                this.state_changed = false;
                effective |= states_plus(item.plus_state_set);
                effective |= states_minus(item.minus_state_set);
                // 能力上升值有效的情况下
                if (item.parameter_type > 0 && item.parameter_points != 0)
                {
                    // 能力值的分支
                    switch (item.parameter_type)
                    {
                        case 1:  // MaxHP
                            this.maxhp_plus += item.parameter_points; break;
                        case 2:  // MaxSP
                            this.maxsp_plus += item.parameter_points; break;
                        case 3:  // 力量
                            this.str_plus += item.parameter_points; break;
                        case 4:  // 灵巧
                            this.dex_plus += item.parameter_points; break;
                        case 5:  // 速度
                            this.agi_plus += item.parameter_points; break;
                        case 6:  // 魔力
                            this.int_plus += item.parameter_points; break;
                    }
                    // 设置有效标志
                    effective = true;
                }
                // HP 回复率与回复量为 0 的情况下
                if (item.recover_hp_rate == 0 && item.recover_hp == 0)
                {
                    // 设置伤害为空的字符串
                    this.damage = "";
                    // SP 回复率与回复量为 0、能力上升值无效的情况下
                    if (
                        item.recover_sp_rate == 0 && item.recover_sp == 0 &&
                                  (item.parameter_type == 0 || item.parameter_points == 0))

                        // 状态没有变化的情况下
                        if (!this.state_changed)
                            // 伤害设置为 "Miss"
                            this.damage = "Miss";
                }
                // Miss 的情况下
                else
                    // 伤害设置为 "Miss"
                    this.damage = "Miss";
                // 不在战斗中的情况下
                if (!Global.game_temp.in_battle)
                    // 伤害设置为 null
                    this.damage = null;
                // 过程结束
                return effective;
            }
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 应用连续伤害效果
        //--------------------------------------------------------------------------
        public bool slip_damage_effect()
        {
            double damage = 0.0;
            if (this.damage is double)
                damage = Convert.ToDouble(this.damage);

            // 设置伤害
            damage = this.maxhp / 10;
            // 分散
            if (Math.Abs(damage) > 0)
            {
                var amp = Math.Max(Math.Abs(damage) * 15 / 100, 1);
                damage += Global.rand((int)amp + 1) + Global.rand((int)amp + 1) - amp;
            }
            // HP 的伤害减法运算
            this.hp -= damage;
            this.damage = damage;
            // 过程结束
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 属性修正计算
        //     element_set : 属性
        //--------------------------------------------------------------------------
        public int elements_correct(List<int> element_set)
        {
            // 无属性的情况
            if (element_set.Count == 0)
                // 返回 100
                return 100;
            // 在被赋予的属性中返回最弱的
            // ※过程 element_rate 是、本类以及继承的 Game_Actor
            //   和 Game_Enemy 类的定义
            var weakest = -100;
            foreach (var i in element_set)
                weakest = Math.Max(weakest, this.element_rate(i));
            return weakest;
        }

        public virtual List<int> element_set { get; set; }

        public virtual int element_rate(int i)
        {
            return 0;
        }
    }
}
