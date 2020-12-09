using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_Battler (分割定义 2)
    //------------------------------------------------------------------------------
    // 　处理战斗者的类。这个类作为 Game_Actor 类与 Game_Enemy 类的
    // 超级类来使用。
    //==============================================================================

    public partial class Game_Battler
    {
        //--------------------------------------------------------------------------
        // ● 检查状态
        //     state_id : 状态 ID
        //--------------------------------------------------------------------------
        public bool is_state(int state_id)
        {
            // 如果符合被附加的状态的条件就返回 ture
            return this.states.Contains(state_id);
        }
        //--------------------------------------------------------------------------
        // ● 判断状态是否为 full
        //     state_id : 状态 ID
        //--------------------------------------------------------------------------
        public bool is_state_full(int state_id)
        {
            // 如果符合被附加的状态的条件就返回 false
            if (!this.is_state(state_id))
                return false;
            // 秩序回合数 -1 (自动状态) 然后返回 true
            if (this.states_turn.ContainsKey(state_id) && this.states_turn[state_id] == -1)
                return true;
            // 当持续回合数等于自然解除的最低回合数时返回 ture
            return this.states_turn[state_id] == Global.data_states[state_id].hold_turn;
        }
        //--------------------------------------------------------------------------
        // ● 附加状态
        //     state_id : 状态 ID
        //     force    : 强制附加标志 (处理自动状态时使用)
        //--------------------------------------------------------------------------
        public void add_state(int state_id, bool force = false)
        {
            // 无效状态的情况下
            if (Global.data_states[state_id] == null)
                // 过程结束
                return;
            // 无法强制附加的情况下
            if (!force)
            {
                // 已存在的状态循环
                foreach (var i in this.states)
                {
                    // 新的状态和已经存在的状态 (-) 同时包含的情况下、
                    // 本状态不包含变化为新状态的状态变化 (-) 
                    // (ex : 战斗不能与附加中毒同时存在的场合)
                    if (Global.data_states[i].minus_state_set.Contains(state_id) &&
                       !Global.data_states[state_id].minus_state_set.Contains(i))
                        // 过程结束
                        return;
                }
            }
            // 无法附加本状态的情况下
            if (!is_state(state_id))
            {
                // 状态 ID 追加到 this.states 序列中
                this.states.Add(state_id);
                // 选项 [当作 HP 0 的状态] 有效的情况下
                if (Global.data_states[state_id].zero_hp)
                    // HP 更改为 0
                    this.hp = 0;
                // 所有状态的循环
                for (var i = 1; i < Global.data_states.Count; i++)
                {
                    // 状态变化 (+) 处理
                    if (Global.data_states[state_id].plus_state_set.Contains(i))
                        add_state(i);
                    // 状态变化 (-) 处理
                    if (Global.data_states[state_id].minus_state_set.Contains(i))
                        remove_state(i);
                }
                // 按比例大的排序 (值相等的情况下按照强度排序)
                this.states.Sort(
                    (int a, int b) =>
                    {
                        var state_a = Global.data_states[a];
                        var state_b = Global.data_states[b];
                        if (state_a.rating > state_b.rating)
                            return -1;
                        else if (state_a.rating < state_b.rating)
                            return 1;
                        else if (state_a.restriction > state_b.restriction)
                            return -1;
                        else if (state_a.restriction < state_b.restriction)
                            return +1;
                        else
                            return a != b ? 1 : 0;
                    });
            }
            // 强制附加的场合
            if (force)
                // 设置为自然解除的最低回数 -1 (无效)
                this.states_turn[state_id] = -1;
            // 不能强制附加的场合
            if (this.states_turn.ContainsKey(state_id) && this.states_turn[state_id] != -1)
                // 设置为自然解除的最低回数
                this.states_turn[state_id] = Global.data_states[state_id].hold_turn;
            // 无法行动的场合
            if (!is_movable)
                // 清除行动
                this.current_action.clear();
            // 检查 HP 及 SP 的最大值
            this._hp = Math.Min(this.hp, this.maxhp);
            this._sp = Math.Min(this.sp, this.maxsp);
        }
        //--------------------------------------------------------------------------
        // ● 解除状态
        //     state_id : 状态 ID
        //     force    : 强制解除标志 (处理自动状态时使用)
        //--------------------------------------------------------------------------
        public void remove_state(int state_id, bool force = false)
        {
            // 无法附加本状态的情况下
            if (is_state(state_id))
            {
                // 被强制附加的状态、并不是强制解除的情况下
                if (this.states_turn.ContainsKey(state_id) && this.states_turn[state_id] == -1 && !force)
                    // 过程结束
                    return;
                // 现在的 HP 为 0 当作选项 [当作 HP 0 的状态]有效的场合
                if (this._hp == 0 && Global.data_states[state_id].zero_hp)
                {
                    // 判断是否有另外的 [当作 HP 0 的状态]状态
                    var zero_hp = false;
                    foreach (var i in this.states)
                        if (i != state_id && Global.data_states[i].zero_hp)
                            zero_hp = true;
                    // 如果可以解除战斗不能、将 HP 更改为 1
                    if (zero_hp == false)
                        this.hp = 1;

                }
                // 将状态 ID 从 this.states 队列和 this.states_turn hash 中删除 
                this.states.Remove(state_id);
                this.states_turn.Remove(state_id);
            }
            // 检查 HP 及 SP 的最大值
            this._hp = Math.Min(this.hp, this.maxhp);
            this._sp = Math.Min(this.sp, this.maxsp);
        }
        //--------------------------------------------------------------------------
        // ● 获取状态的动画 ID
        //--------------------------------------------------------------------------
        public int state_animation_id
        {
            get
            {
                // 一个状态也没被附加的情况下
                if (this.states.Count == 0)
                    return 0;
                // 返回概率最大的状态动画 ID
                return Global.data_states[this.states[0]].animation_id;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取限制
        //--------------------------------------------------------------------------
        public int restriction
        {
            get
            {
                var restriction_max = 0;
                // 从当前附加的状态中获取最大的 restriction 
                foreach (var i in this.states)
                    if (Global.data_states[i].restriction >= restriction_max)
                        restriction_max = Global.data_states[i].restriction;

                return restriction_max;
            }
        }

        //--------------------------------------------------------------------------
        // ● 判断状态 [无法获得 EXP]
        //--------------------------------------------------------------------------
        public bool is_cant_get_exp
        {
            get
            {
                foreach (var i in this.states)
                    if (Global.data_states[i].cant_get_exp)
                        return true;
                return false;
            }
        }
        //--------------------------------------------------------------------------
        // ● 判断状态 [无法回避攻击]
        //--------------------------------------------------------------------------
        public bool is_cant_evade
        {
            get
            {
                foreach (var i in this.states)
                    if (Global.data_states[i].cant_evade)
                        return true;
                return false;
            }
        }
        //--------------------------------------------------------------------------
        // ● 判断状态 [连续伤害]
        //--------------------------------------------------------------------------
        public bool is_slip_damage
        {
            get
            {
                foreach (var i in this.states)
                    if (Global.data_states[i].slip_damage)
                        return true;
                return false;
            }
        }
        //--------------------------------------------------------------------------
        // ● 解除战斗用状态 (战斗结束时调用)
        //--------------------------------------------------------------------------
        public void remove_states_battle()
        {
            foreach (var i in new List<int>(this.states))
                if (Global.data_states[i].battle_only)
                    remove_state(i);
        }
        //--------------------------------------------------------------------------
        // ● 状态自然解除 (回合改变时调用)
        //--------------------------------------------------------------------------
        public void remove_states_auto()
        {
            foreach (var i in new List<int>(this.states_turn.Keys))
                if (this.states_turn[i] > 0)
                    this.states_turn[i] -= 1;
                else if (Global.rand(100) < Global.data_states[i].auto_release_prob)
                    remove_state(i);
        }
        //--------------------------------------------------------------------------
        // ● 状态攻击解除 (受到物理伤害时调用)
        //--------------------------------------------------------------------------
        public void remove_states_shock()
        {
            foreach (var i in new List<int>(this.states))
                if (Global.rand(100) < Global.data_states[i].shock_release_prob)
                    remove_state(i);
        }
        public virtual bool is_state_guard(int i)
        {
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 状态变化 (+) 的适用
        //     plus_state_set  : 状态变化 (+)
        //--------------------------------------------------------------------------
        public bool states_plus(List<int> plus_state_set)
        {
            // 清除有效标志
            bool effective = false;
            // 循环 (附加状态)
            foreach (var i in plus_state_set)
            {
                // 无法防御本状态的情况下
                if (!this.is_state_guard(i))
                    // 这个状态如果不是 full 的话就设置有效标志
                    effective |= this.is_state_full(i) == false;
                // 状态为 [不能抵抗] 的情况下
                if (Global.data_states[i].nonresistance)
                {
                    // 设置状态变化标志
                    this.state_changed = true;
                    // 附加状态
                    add_state(i);
                }
                // 这个状态不是 full 的情况下
                else if (this.is_state_full(i) == false)
                {
                    // 将状态的有效度变换为概率、与随机数比较
                    //if( Global.rand(100) < [0,100,80,60,40,20,0][this.state_ranks[i]])
                    // 设置状态变化标志
                    this.state_changed = true;
                    // 附加状态
                    add_state(i);
                }
            }
            // 过程结束
            return effective;
        }
        //--------------------------------------------------------------------------
        // ● 状态变化 (-) 的使用
        //     minus_state_set : 状态变化 (-)
        //--------------------------------------------------------------------------
        public bool states_minus(List<int> minus_state_set)
        {
            // 清除有效标志
            var effective = false;
            // 循环 (解除状态)
            foreach (var i in minus_state_set)
            {
                // 如果这个状态被附加则设置有效标志
                effective |= this.is_state(i);
                // 设置状态变化标志
                this.state_changed = true;
                // 解除状态
                remove_state(i);
            }
            // 过程结束
            return effective;
        }

        public bool state_changed { get; set; }
    }

}
