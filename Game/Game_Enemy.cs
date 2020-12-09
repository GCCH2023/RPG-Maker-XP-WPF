using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_Enemy
    //------------------------------------------------------------------------------
    // 　处理敌人的类。本类在 Game_Troop 类 (Global.game_troop) 的
    // 内部使用。
    //==============================================================================

    public class Game_Enemy : Game_Battler
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     troop_id     : 循环 ID
        //     member_index : 循环成员的索引
        //--------------------------------------------------------------------------
        public Game_Enemy(int troop_id, int member_index)
        {
            this.troop_id = troop_id;
            this.member_index = member_index;
            var troop = Global.data_troops[this.troop_id];
            this.enemy_id = troop.members[this.member_index].enemy_id;
            var enemy = Global.data_enemies[this.enemy_id];
            this.battler_name = enemy.battler_name;
            this.battler_hue = enemy.battler_hue;
            this.hp = maxhp;
            this.sp = maxsp;
            this.hidden = troop.members[this.member_index].hidden;
            this.immortal = troop.members[this.member_index].immortal;
        }
        //--------------------------------------------------------------------------
        // ● 获取敌人 ID
        //--------------------------------------------------------------------------
        public int id
        {
            get
            {
                return this.enemy_id;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取索引
        //--------------------------------------------------------------------------
        public override int? index
        {
            get
            {
                return this.member_index;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取名称
        //--------------------------------------------------------------------------
        public string name
        {
            get
            {
                return Global.data_enemies[this.enemy_id].name;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本 MaxHP
        //--------------------------------------------------------------------------
        public override double base_maxhp
        {
            get
            {
                return Global.data_enemies[this.enemy_id].maxhp;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本 MaxSP
        //--------------------------------------------------------------------------
        public override double base_maxsp
        {
            get
            {
                return Global.data_enemies[this.enemy_id].maxsp;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本力量
        //--------------------------------------------------------------------------
        public override double base_str
        {
            get
            {
                return Global.data_enemies[this.enemy_id].str;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本灵巧
        //--------------------------------------------------------------------------
        public override double base_dex
        {
            get
            {
                return Global.data_enemies[this.enemy_id].dex;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本速度
        //--------------------------------------------------------------------------
        public override double base_agi
        {
            get
            {
                return Global.data_enemies[this.enemy_id].agi;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本魔力
        //--------------------------------------------------------------------------
        public override double base_int
        {
            get
            {
                return Global.data_enemies[this.enemy_id].int1;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本攻击力
        //--------------------------------------------------------------------------
        public override double base_atk
        {
            get
            {
                return Global.data_enemies[this.enemy_id].atk;
            }
        }

        //--------------------------------------------------------------------------
        // ● 获取基本物理防御
        //--------------------------------------------------------------------------
        public override double base_pdef
        {
            get
            {
                return Global.data_enemies[this.enemy_id].pdef;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本魔法防御
        //--------------------------------------------------------------------------
        public override double base_mdef
        {
            get
            {
                return Global.data_enemies[this.enemy_id].mdef;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本回避修正
        //--------------------------------------------------------------------------
        public override double base_eva
        {
            get
            {
                return Global.data_enemies[this.enemy_id].eva;
            }
        }

        //--------------------------------------------------------------------------
        // ● 普通攻击 获取攻击方动画 ID
        //--------------------------------------------------------------------------
        public override int animation1_id
        {
            get
            {
                return Global.data_enemies[this.enemy_id].animation1_id;
            }
        }

        //--------------------------------------------------------------------------
        // ● 普通攻击 获取对像方动画 ID
        //--------------------------------------------------------------------------
        public override int animation2_id
        {
            get
            {
                return Global.data_enemies[this.enemy_id].animation2_id;
            }
        }

        //--------------------------------------------------------------------------
        // ● 获取属性修正值
        //     element_id : 属性 ID
        //--------------------------------------------------------------------------
        public override int element_rate(int element_id)
        {
            // 获取对应属性有效度的数值
            var table = new int[] { 0, 200, 150, 100, 50, 0, -100 };
            var result = table[Global.data_enemies[this.enemy_id].element_ranks[element_id]];
            // 状态能防御本属性的情况下效果减半
            foreach (var i in this.states)
                if (Global.data_states[i].is_guard_element_set.Contains(element_id))
                    result /= 2;
            // 过程结束
            return result;
        }
        //--------------------------------------------------------------------------
        // ● 获取属性有效度
        //--------------------------------------------------------------------------
        public Table state_ranks
        {
            get
            {
                return Global.data_enemies[this.enemy_id].state_ranks;
            }
        }
        //--------------------------------------------------------------------------
        // ● 属性防御判定
        //     state_id : 状态 ID
        //--------------------------------------------------------------------------
        public override bool is_state_guard(int state_id)
        {
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 获取普通攻击属性
        //--------------------------------------------------------------------------
        public override List<int> element_set
        {
            get
            {
                return new List<int>();
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取普通攻击的状态变化 (+)
        //--------------------------------------------------------------------------
        public override List<int> plus_state_set
        {
            get
            {
                return new List<int>();
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取普通攻击的状态变化 (-)
        //--------------------------------------------------------------------------
        public override List<int> minus_state_set
        {
            get
            {
                return new List<int>();
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取行动
        //--------------------------------------------------------------------------
        public List<RPG.Enemy.Action> actions
        {
            get
            {
                return Global.data_enemies[this.enemy_id].actions;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取 EXP
        //--------------------------------------------------------------------------
        public int exp
        {
            get
            {
                return Global.data_enemies[this.enemy_id].exp;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取金钱
        //--------------------------------------------------------------------------
        public int gold
        {
            get
            {
                return Global.data_enemies[this.enemy_id].gold;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取物品 ID
        //--------------------------------------------------------------------------
        public int item_id
        {
            get
            {
                return Global.data_enemies[this.enemy_id].item_id;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取武器 ID
        //--------------------------------------------------------------------------
        public int weapon_id
        {
            get
            {
                return Global.data_enemies[this.enemy_id].weapon_id;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取防具 ID
        //--------------------------------------------------------------------------
        public int armor_id
        {
            get
            {
                return Global.data_enemies[this.enemy_id].armor_id;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取宝物出现率
        //--------------------------------------------------------------------------
        public int treasure_prob
        {
            get
            {
                return Global.data_enemies[this.enemy_id].treasure_prob;
            }
        }
        //--------------------------------------------------------------------------
        // ● 取得战斗画面 X 坐标
        //--------------------------------------------------------------------------
        public override int screen_x
        {
            get
            {
                return Global.data_troops[this.troop_id].members[this.member_index].x;
            }
        }
        //--------------------------------------------------------------------------
        // ● 取得战斗画面 Y 坐标
        //--------------------------------------------------------------------------
        public override int screen_y
        {
            get
            {
                return Global.data_troops[this.troop_id].members[this.member_index].y;
            }
        }
        //--------------------------------------------------------------------------
        // ● 取得战斗画面 Z 坐标
        //--------------------------------------------------------------------------
        public override int screen_z
        {
            get
            {
                return screen_y;
            }
        }
        //--------------------------------------------------------------------------
        // ● 逃跑
        //--------------------------------------------------------------------------
        public void escape()
        {
            // 设置隐藏标志
            this.hidden = true;
            // 清除当前行动
            this.current_action.clear();
        }
        //--------------------------------------------------------------------------
        // ● 变身
        //     enemy_id : 变身为的敌人 ID
        //--------------------------------------------------------------------------
        public void transform(int enemy_id)
        {
            // 更改敌人 ID
            this.enemy_id = enemy_id;
            // 更改战斗图形
            this.battler_name = Global.data_enemies[this.enemy_id].battler_name;
            this.battler_hue = Global.data_enemies[this.enemy_id].battler_hue;
            // 再生成行动
            make_action();
        }
        //--------------------------------------------------------------------------
        // ● 生成行动
        //--------------------------------------------------------------------------
        public void make_action()
        {
            // 清除当前行动
            this.current_action.clear();
            // 无法行动的情况
            if (!this.is_movable)
                // 过程结束
                return;

            // 抽取现在有效的行动
            var available_actions = new List<RPG.Enemy.Action>();
            var rating_max = 0;
            foreach (var action in this.actions)
            {
                // 确认回合条件
                var n = Global.game_temp.battle_turn;
                var a = action.condition_turn_a;
                var b = action.condition_turn_b;
                if (
                   (b == 0 && n != a) ||
                                (b > 0 && (n < 1 || n < a || n % b != a % b)))

                    continue;

                // 确认 HP 条件
                if (this.hp * 100.0 / this.maxhp > action.condition_hp)
                    continue;

                // 确认等级条件
                if (Global.game_party.max_level < action.condition_level)
                    continue;

                // 确认开关条件
                var switch_id = action.condition_switch_id;
                if (switch_id > 0 && Global.game_switches[switch_id] == false)
                    continue;
                // 符合条件 : 添加本行动
                available_actions.Add(action);
                if (action.rating > rating_max)
                    rating_max = action.rating;
            }
            // 最大概率值作为 3 合计计算(0 除外)
            var ratings_total = 0;
            foreach (var action in available_actions)
                if (action.rating > rating_max - 3)
                    ratings_total += action.rating - (rating_max - 3);

            // 概率合计不为 0 的情况下
            if (ratings_total > 0)
            {
                // 生成随机数
                var value = Global.rand(ratings_total);
                // 设置对应生成随机数的当前行动
                foreach (var action in available_actions)
                {
                    if (action.rating > rating_max - 3)
                    {
                        if (value < action.rating - (rating_max - 3))
                        {
                            this.current_action.kind = action.kind;
                            this.current_action.basic = action.basic;
                            this.current_action.skill_id = action.skill_id;
                            this.current_action.decide_random_target_for_enemy();
                            return;
                        }
                        else
                            value -= action.rating - (rating_max - 3);
                    }
                }
            }
        }

        public int troop_id { get; set; }
        public int member_index { get; set; }
        public int enemy_id { get; set; }
    }

}
