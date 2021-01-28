using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_Actor
    //------------------------------------------------------------------------------
    // 　处理角色的类。本类在 Game_Actors 类 (Global.game_actors)
    // 的内部使用、Game_Party 类请参考 (Global.game_party) 。
    //==============================================================================

    public class Game_Actor : Game_Battler
    {
        //--------------------------------------------------------------------------
        // ● 更改名称
        //     name : 新的名称
        //--------------------------------------------------------------------------
        string _name;
        public string name
        {
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                    NotifyPropertyChanged();
                }
            }
            get { return this._name; }
        }
        //--------------------------------------------------------------------------
        // ● 更改状态字符串
        //     status : 新的名称
        //--------------------------------------------------------------------------
        string _status;
        public string status
        {
            set
            {
                if (this._status != value)
                {
                    this._status = value;
                    NotifyPropertyChanged();
                }
            }
            get { return this._status; }
        }
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public string character_name;        // 角色 文件名
        public int character_hue;            // 角色 色相

        public int weapon_id;             // 武器 ID
        public int armor1_id;            // 盾 ID
        public int armor2_id;            // 头防具 ID
        public int armor3_id;            // 身体体防具 ID
        public int armor4_id;            // 装饰品 ID
        public List<int> skills;      // 特技
        public int actor_id;
        public int[] exp_list;

        /// <summary>
        /// 仅用于测试或序列化
        /// </summary>
        public Game_Actor()
        {

        }
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     actor_id : 角色 ID
        //--------------------------------------------------------------------------
        public Game_Actor(int actor_id)
        {
            setup(actor_id);
        }
        //--------------------------------------------------------------------------
        // ● 设置
        //     actor_id : 角色 ID
        //--------------------------------------------------------------------------
        public void setup(int actor_id)
        {
            var actor = Global.data_actors[actor_id];
            this.actor_id = actor_id;
            this.name = actor.name;
            this.character_name = actor.character_name;
            this.character_hue = actor.character_hue;
            this.battler_name = actor.battler_name;
            this.battler_hue = actor.battler_hue;
            this.class_id = actor.class_id;
            this.weapon_id = actor.weapon_id;
            this.armor1_id = actor.armor1_id;
            this.armor2_id = actor.armor2_id;
            this.armor3_id = actor.armor3_id;
            this.armor4_id = actor.armor4_id;
            this.exp_list = new int[101];
            this._level = actor.initial_level;
            this.states_turn = new Dictionary<int, int>();
            make_exp_list();
            this.states = new List<int>();
            this._exp = this.exp_list[this.level];
            this.skills = new List<int>();
            this._hp = maxhp;
            this._sp = maxsp;
            this.maxhp_plus = 0;
            this.maxsp_plus = 0;
            this.str_plus = 0;
            this.dex_plus = 0;
            this.agi_plus = 0;
            this.int_plus = 0;
            // 学会特技
            for (var i = 1; i <= this.level; i++)
            {
                foreach (var j in Global.data_classes[this.class_id].learnings)
                    if (j.level == i)
                        learn_skill(j.skill_id);
            }
            // 刷新自动状态
            update_auto_state(null, Global.data_armors[this.armor1_id]);
            update_auto_state(null, Global.data_armors[this.armor2_id]);
            update_auto_state(null, Global.data_armors[this.armor3_id]);
            update_auto_state(null, Global.data_armors[this.armor4_id]);
        }
        //--------------------------------------------------------------------------
        // ● 获取角色 ID 
        //--------------------------------------------------------------------------
        public int id
        {
            get { return this.actor_id; }
        }
        //--------------------------------------------------------------------------
        // ● 获取索引
        //--------------------------------------------------------------------------
        public override int? index
        {
            get
            {
                return Global.game_party.actors.IndexOf(this);
            }
        }
        //--------------------------------------------------------------------------
        // ● 计算 EXP
        //--------------------------------------------------------------------------
        public void make_exp_list()
        {
            var actor = Global.data_actors[this.actor_id];
            this.exp_list[1] = 0;
            var pow_i = 2.4 + actor.exp_inflation / 100.0;
            for (var i = 2; i <= 100; i++)
            {
                if (i > actor.final_level)
                    this.exp_list[i] = 0;
                else
                {
                    var n = actor.exp_basis * (Math.Pow((i + 3), pow_i)) / (Math.Pow(5, pow_i));
                    this.exp_list[i] = this.exp_list[i - 1] + (int)n;
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 取得属性修正值
        //     element_id : 属性 ID
        //--------------------------------------------------------------------------
        public override int element_rate(int element_id)
        {
            // 获取对应属性有效度的数值
            var table = new int[] { 0, 200, 150, 100, 50, 0, -100 };
            var result = table[(int)Global.data_classes[this.class_id].element_ranks[element_id]];
            // 防具能防御本属性的情况下效果减半
            foreach (var i in new int[] { this.armor1_id, this.armor2_id, this.armor3_id, this.armor4_id })
            {
                var armor = Global.data_armors[i];
                if (armor != null && armor.is_guard_element_set.Contains(element_id))
                    result /= 2;
            }
            // 状态能防御本属性的情况下效果减半
            foreach (var i in this.states)
                if (Global.data_states[i].is_guard_element_set.Contains(element_id))
                    result /= 2;

            return result;
        }
        //--------------------------------------------------------------------------
        // ● 获取属性有效度
        //--------------------------------------------------------------------------
        public Table state_ranks
        {
            get
            {
                return Global.data_classes[this.class_id].state_ranks;
            }
        }
        //--------------------------------------------------------------------------
        // ● 判定防御属性
        //     state_id : 属性 ID
        //--------------------------------------------------------------------------
        public override bool is_state_guard(int state_id)
        {
            foreach (var i in new int[] { this.armor1_id, this.armor2_id, this.armor3_id, this.armor4_id })
            {
                var armor = Global.data_armors[i];
                if (armor != null)
                    if (armor.guard_state_set.Contains(state_id))
                        return true;
            }
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 获取普通攻击属性
        //--------------------------------------------------------------------------
        public override List<int> element_set
        {
            get
            {
                var weapon = Global.data_weapons[this.weapon_id];
                return weapon != null ? weapon.element_set : new List<int>() { 0 };
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取普通攻击状态变化 (+)
        //--------------------------------------------------------------------------
        public override List<int> plus_state_set
        {
            get
            {
                var weapon = Global.data_weapons[this.weapon_id];
                return weapon != null ? weapon.plus_state_set : new List<int>() { 0 };
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取普通攻击状态变化 (-)
        //--------------------------------------------------------------------------
        public override List<int> minus_state_set
        {
            get
            {
                var weapon = Global.data_weapons[this.weapon_id];
                return weapon != null ? weapon.minus_state_set : new List<int>() { 0 };
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取 MaxHP
        //--------------------------------------------------------------------------
        double _maxhp;
        public override double maxhp
        {
            get
            {
                var n = Math.Min(Math.Max(base_maxhp + this.maxhp_plus, 1), 9999);
                foreach (var i in this.states)
                    n *= Global.data_states[i].maxhp_rate / 100.0;
                n = Math.Min(Math.Max((int)n, 1), 9999);
                return n;
            }
            set { this._maxhp = value; }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本 MaxHP
        //--------------------------------------------------------------------------
        public override double base_maxhp
        {
            get
            {
                return (int)Global.data_actors[this.actor_id].parameters[0, this.level];
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本 MaxSP
        //--------------------------------------------------------------------------
        public override double base_maxsp
        {
            get
            {
                return (int)Global.data_actors[this.actor_id].parameters[1, this.level];
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本力量
        //--------------------------------------------------------------------------
        public override double base_str
        {
            get
            {
                var n = (int)Global.data_actors[this.actor_id].parameters[2, this.level];
                var weapon = Global.data_weapons[this.weapon_id];
                var armor1 = Global.data_armors[this.armor1_id];
                var armor2 = Global.data_armors[this.armor2_id];
                var armor3 = Global.data_armors[this.armor3_id];
                var armor4 = Global.data_armors[this.armor4_id];
                n += weapon != null ? weapon.str_plus : 0;
                n += armor1 != null ? armor1.str_plus : 0;
                n += armor2 != null ? armor2.str_plus : 0;
                n += armor3 != null ? armor3.str_plus : 0;
                n += armor4 != null ? armor4.str_plus : 0;
                return Math.Min(Math.Max(n, 1), 999);
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本灵巧
        //--------------------------------------------------------------------------
        public override double base_dex
        {
            get
            {
                var n = (int)Global.data_actors[this.actor_id].parameters[3, this.level];
                var weapon = Global.data_weapons[this.weapon_id];
                var armor1 = Global.data_armors[this.armor1_id];
                var armor2 = Global.data_armors[this.armor2_id];
                var armor3 = Global.data_armors[this.armor3_id];
                var armor4 = Global.data_armors[this.armor4_id];
                n += weapon != null ? weapon.dex_plus : 0;
                n += armor1 != null ? armor1.dex_plus : 0;
                n += armor2 != null ? armor2.dex_plus : 0;
                n += armor3 != null ? armor3.dex_plus : 0;
                n += armor4 != null ? armor4.dex_plus : 0;
                return Math.Min(Math.Max(n, 1), 999);
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本速度
        //--------------------------------------------------------------------------
        public override double base_agi
        {
            get
            {
                var n = (int)Global.data_actors[this.actor_id].parameters[4, this.level];
                var weapon = Global.data_weapons[this.weapon_id];
                var armor1 = Global.data_armors[this.armor1_id];
                var armor2 = Global.data_armors[this.armor2_id];
                var armor3 = Global.data_armors[this.armor3_id];
                var armor4 = Global.data_armors[this.armor4_id];
                n += weapon != null ? weapon.agi_plus : 0;
                n += armor1 != null ? armor1.agi_plus : 0;
                n += armor2 != null ? armor2.agi_plus : 0;
                n += armor3 != null ? armor3.agi_plus : 0;
                n += armor4 != null ? armor4.agi_plus : 0;
                return Math.Min(Math.Max(n, 1), 999);
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本魔力
        //--------------------------------------------------------------------------
        public override double base_int
        {
            get
            {
                var n = (int)Global.data_actors[this.actor_id].parameters[5, this.level];
                var weapon = Global.data_weapons[this.weapon_id];
                var armor1 = Global.data_armors[this.armor1_id];
                var armor2 = Global.data_armors[this.armor2_id];
                var armor3 = Global.data_armors[this.armor3_id];
                var armor4 = Global.data_armors[this.armor4_id];
                n += weapon != null ? weapon.int_plus : 0;
                n += armor1 != null ? armor1.int_plus : 0;
                n += armor2 != null ? armor2.int_plus : 0;
                n += armor3 != null ? armor3.int_plus : 0;
                n += armor4 != null ? armor4.int_plus : 0;
                return Math.Min(Math.Max(n, 1), 999);
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本攻击力
        //--------------------------------------------------------------------------
        public override double base_atk
        {
            get
            {
                var weapon = Global.data_weapons[this.weapon_id];
                return weapon != null ? weapon.atk : 0;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本物理防御
        //--------------------------------------------------------------------------
        public override double base_pdef
        {
            get
            {
                var weapon = Global.data_weapons[this.weapon_id];
                var armor1 = Global.data_armors[this.armor1_id];
                var armor2 = Global.data_armors[this.armor2_id];
                var armor3 = Global.data_armors[this.armor3_id];
                var armor4 = Global.data_armors[this.armor4_id];
                var pdef1 = weapon != null ? weapon.pdef : 0;
                var pdef2 = armor1 != null ? armor1.pdef : 0;
                var pdef3 = armor2 != null ? armor2.pdef : 0;
                var pdef4 = armor3 != null ? armor3.pdef : 0;
                var pdef5 = armor4 != null ? armor4.pdef : 0;
                return pdef1 + pdef2 + pdef3 + pdef4 + pdef5;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本魔法防御
        //--------------------------------------------------------------------------
        public override double base_mdef
        {
            get
            {
                var weapon = Global.data_weapons[this.weapon_id];
                var armor1 = Global.data_armors[this.armor1_id];
                var armor2 = Global.data_armors[this.armor2_id];
                var armor3 = Global.data_armors[this.armor3_id];
                var armor4 = Global.data_armors[this.armor4_id];
                var mdef1 = weapon != null ? weapon.mdef : 0;
                var mdef2 = armor1 != null ? armor1.mdef : 0;
                var mdef3 = armor2 != null ? armor2.mdef : 0;
                var mdef4 = armor3 != null ? armor3.mdef : 0;
                var mdef5 = armor4 != null ? armor4.mdef : 0;
                return mdef1 + mdef2 + mdef3 + mdef4 + mdef5;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取基本回避修正
        //--------------------------------------------------------------------------
        public override double base_eva
        {
            get
            {
                var armor1 = Global.data_armors[this.armor1_id];
                var armor2 = Global.data_armors[this.armor2_id];
                var armor3 = Global.data_armors[this.armor3_id];
                var armor4 = Global.data_armors[this.armor4_id];
                var eva1 = armor1 != null ? armor1.eva : 0;
                var eva2 = armor2 != null ? armor2.eva : 0;
                var eva3 = armor3 != null ? armor3.eva : 0;
                var eva4 = armor4 != null ? armor4.eva : 0;
                return eva1 + eva2 + eva3 + eva4;
            }
        }
        //--------------------------------------------------------------------------
        // ● 普通攻击 获取攻击方动画 ID
        //--------------------------------------------------------------------------
        public override int animation1_id
        {
            get
            {
                var weapon = Global.data_weapons[this.weapon_id];
                return weapon != null ? weapon.animation1_id : 0;
            }
        }
        //--------------------------------------------------------------------------
        // ● 普通攻击 获取对像方动画 ID
        //--------------------------------------------------------------------------
        public override int animation2_id
        {
            get
            {
                var weapon = Global.data_weapons[this.weapon_id];
                return weapon != null ? weapon.animation2_id : 0;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取类名
        //--------------------------------------------------------------------------
        public string class_name
        {
            get
            {
                return Global.data_classes[this.class_id].name;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取 EXP 字符串
        //--------------------------------------------------------------------------
        public string exp_s
        {
            get
            {
                return this.exp_list[this.level + 1] > 0 ? this.exp.ToString() : "-------";
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取下一等级的 EXP 字符串
        //--------------------------------------------------------------------------
        public string next_exp_s
        {
            get
            {
                return this.exp_list[this.level + 1] > 0 ? this.exp_list[this.level + 1].ToString() : "-------";
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取离下一等级还需的 EXP 字符串
        //--------------------------------------------------------------------------
        public string next_rest_exp_s
        {
            get
            {
                return this.exp_list[this.level + 1] > 0 ?
                  (this.exp_list[this.level + 1] - this.exp).ToString() : "-------";
            }
        }
        //--------------------------------------------------------------------------
        // ● 更新自动状态
        //     old_armor : 卸下防具
        //     new_armor : 装备防具
        //--------------------------------------------------------------------------
        public void update_auto_state(RPG.Armor old_armor, RPG.Armor new_armor)
        {
            // 强制解除卸下防具的自动状态
            if (old_armor != null && old_armor.auto_state_id != 0)
                remove_state(old_armor.auto_state_id, true);
            // 强制附加装备防具的自动状态
            if (new_armor != null && new_armor.auto_state_id != 0)
                add_state(new_armor.auto_state_id, true);
        }
        //--------------------------------------------------------------------------
        // ● 装备固定判定
        //     equip_type : 装备类型
        //--------------------------------------------------------------------------
        public bool is_equip_fix(int equip_type)
        {
            switch (equip_type)
            {
                case 0: // 武器
                    return Global.data_actors[this.actor_id].weapon_fix; 
                case 1:  // 盾
                    return Global.data_actors[this.actor_id].armor1_fix; 
                case 2: // 头
                    return Global.data_actors[this.actor_id].armor2_fix; 
                case 3: // 身体
                    return Global.data_actors[this.actor_id].armor3_fix; 
                case 4:  // 装饰品
                    return Global.data_actors[this.actor_id].armor4_fix; 
            }
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 变更装备
        //     equip_type : 装备类型
        //     id    : 武器 || 防具 ID  (0 为解除装备)
        //--------------------------------------------------------------------------
        public void equip(int equip_type, int id)
        {
            switch (equip_type)
            {
                case 0:  // 武器
                    if (id == 0 || Global.game_party.weapon_number(id) > 0)
                    {
                        Global.game_party.gain_weapon(this.weapon_id, 1);
                        this.weapon_id = id;
                        Global.game_party.lose_weapon(id, 1);
                    }
                    break;
                case 1:    // 盾
                    if (id == 0 || Global.game_party.armor_number(id) > 0)
                    {
                        update_auto_state(Global.data_armors[this.armor1_id], Global.data_armors[id]);
                        Global.game_party.gain_armor(this.armor1_id, 1);
                        this.armor1_id = id;
                        Global.game_party.lose_armor(id, 1);
                    }
                    break;
                case 2:    // 头
                    if (id == 0 || Global.game_party.armor_number(id) > 0)
                    {
                        update_auto_state(Global.data_armors[this.armor2_id], Global.data_armors[id]);
                        Global.game_party.gain_armor(this.armor2_id, 1);
                        this.armor2_id = id;
                        Global.game_party.lose_armor(id, 1);
                    }
                    break;
                case 3:    // 身体
                    if (id == 0 || Global.game_party.armor_number(id) > 0)
                    {
                        update_auto_state(Global.data_armors[this.armor3_id], Global.data_armors[id]);
                        Global.game_party.gain_armor(this.armor3_id, 1);
                        this.armor3_id = id;
                        Global.game_party.lose_armor(id, 1);
                    }
                    break;
                case 4:   // 装饰品
                    if (id == 0 || Global.game_party.armor_number(id) > 0)
                    {
                        update_auto_state(Global.data_armors[this.armor4_id], Global.data_armors[id]);
                        Global.game_party.gain_armor(this.armor4_id, 1);
                        this.armor4_id = id;
                        Global.game_party.lose_armor(id, 1);
                    }
                    break;
            }
        }
        //--------------------------------------------------------------------------
        // ● 可以装备判定
        //     item : 物品
        //--------------------------------------------------------------------------
        public bool is_equippable(object item)
        {
            // 武器的情况
            if (item is RPG.Weapon)
                // 包含当前的职业可以装备武器的场合
                if (Global.data_classes[this.class_id].weapon_set.Contains(((RPG.Weapon)item).id))
                    return true;
            // 防具的情况
            if (item is RPG.Armor)
                // 不包含当前的职业可以装备武器的场合
                if (Global.data_classes[this.class_id].armor_set.Contains(((RPG.Armor)item).id))
                    return true;

            return false;
        }
        //--------------------------------------------------------------------------
        // ● 更改 EXP
        //     exp : 新的 EXP
        //--------------------------------------------------------------------------
        int _exp;
        public int exp
        {
            set
            {
                this._exp = Math.Max(Math.Min(value, 9999999), 0);
                // 升级
                while (this._exp >= this.exp_list[this.level + 1] && this.exp_list[this.level + 1] > 0)
                {
                    this.level += 1;
                    // 学会特技
                    foreach (var j in Global.data_classes[this.class_id].learnings)
                        if (j.level == this.level)
                            learn_skill(j.skill_id);
                }

                // 降级
                while (this.exp < this.exp_list[this.level])
                    this.level -= 1;

                // 修正当前的 HP 与 SP 超过最大值
                this.hp = Math.Min(this.hp, this.maxhp);
                this.sp = Math.Min(this.sp, this.maxsp);
            }
            get { return this._exp; }
        }
        //--------------------------------------------------------------------------
        // ● 更改等级
        //     level : 新的等级
        //--------------------------------------------------------------------------
        int _level;
        public int level
        {
            set
            {
                // 检查上下限
                var level = Math.Max(Math.Min(value, Global.data_actors[this.actor_id].final_level), 1);
                // 更改 EXP
                this._exp = this.exp_list[level];
                this._level = level;
            }
            get { return this._level; }
        }
        //--------------------------------------------------------------------------
        // ● 觉悟特技
        //     skill_id : 特技 ID
        //--------------------------------------------------------------------------
        public void learn_skill(int skill_id)
        {
            if (skill_id > 0 && !is_skill_learn(skill_id))
            {
                this.skills.Add(skill_id);
                this.skills.Sort();
            }
        }
        //--------------------------------------------------------------------------
        // ● 遗忘特技
        //     skill_id : 特技 ID
        //--------------------------------------------------------------------------
        public void forget_skill(int skill_id)
        {
            this.skills.Remove(skill_id);
        }
        //--------------------------------------------------------------------------
        // ● 已经学会的特技判定
        //     skill_id : 特技 ID
        //--------------------------------------------------------------------------
        public bool is_skill_learn(int skill_id)
        {
            return this.skills.Contains(skill_id);
        }
        //--------------------------------------------------------------------------
        // ● 可以使用特技判定
        //     skill_id : 特技 ID
        //--------------------------------------------------------------------------
        public override bool is_skill_can_use(int skill_id)
        {
            if (!is_skill_learn(skill_id))
                return false;

            return base.is_skill_can_use(skill_id);
        }
      
        //--------------------------------------------------------------------------
        // ● 更改职业 ID
        //     class_id : 新的职业 ID
        //--------------------------------------------------------------------------
        int _class_id;
        public int class_id
        {
            set
            {
                if (Global.data_classes[value] != null)
                    this._class_id = value;
                // 避开无法装备的物品
                if (!is_equippable(Global.data_weapons[this.weapon_id]))
                    equip(0, 0);

                if (!is_equippable(Global.data_armors[this.armor1_id]))
                    equip(1, 0);

                if (!is_equippable(Global.data_armors[this.armor2_id]))
                    equip(2, 0);

                if (!is_equippable(Global.data_armors[this.armor3_id]))
                    equip(3, 0);

                if (!is_equippable(Global.data_armors[this.armor4_id]))
                    equip(4, 0);
            }
            get { return this._class_id; }
        }
        //--------------------------------------------------------------------------
        // ● 更改图形
        //     character_name : 新的角色 文件名
        //     character_hue  : 新的角色 色相
        //     battler_name   : 新的战斗者 文件名
        //     battler_hue    : 新的战斗者 色相
        //--------------------------------------------------------------------------
        public void set_graphic(string character_name, int character_hue, string battler_name, int battler_hue)
        {
            this.character_name = character_name;
            this.character_hue = character_hue;
            this.battler_name = battler_name;
            this.battler_hue = battler_hue;
        }
        //--------------------------------------------------------------------------
        // ● 取得战斗画面的 X 坐标
        //--------------------------------------------------------------------------
        public override int screen_x
        {
            get
            {
                // 返回计算后的队伍 X 坐标的排列顺序
                if (this.index != null)
                    return (int)this.index * 160 + 80;
                else
                    return 0;
            }
        }
        //--------------------------------------------------------------------------
        // ● 取得战斗画面的 Y 坐标
        //--------------------------------------------------------------------------
        public override int screen_y
        {
            get
            {
                return 464;
            }
        }
        //--------------------------------------------------------------------------
        // ● 取得战斗画面的 Z 坐标
        //--------------------------------------------------------------------------
        public override int screen_z
        {
            get
            {
                // 返回计算后的队伍 Z 坐标的排列顺序
                //if (this.index != null)
                //    return 4 - (int)this.index;
                //else
                //    return 0;
                return 101;
            }
        }
    }

}
