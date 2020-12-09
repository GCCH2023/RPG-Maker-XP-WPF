using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace XP
{
    //==============================================================================
    // ■ Game_Party
    //------------------------------------------------------------------------------
    // 　处理同伴的类。包含金钱以及物品的信息。本类的实例
    // 请参考 Global.game_party。
    //==============================================================================

    public class Game_Party
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public List<Game_Actor> actors;                   // 角色
        public int gold;                     // 金钱
        public int steps;                   // 步数
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Game_Party()
        {
            // 建立角色序列
            this.actors = new List<Game_Actor>();
            // 初始化金钱与步数
            this.gold = 0;
            this.steps = 0;
            // 生成物品、武器、防具的所持数 hash
            this.items = new Dictionary<int, int>();
            this.weapons = new Dictionary<int, int>();
            this.armors = new Dictionary<int, int>();
        }
        //--------------------------------------------------------------------------
        // ● 设置初期同伴
        //--------------------------------------------------------------------------
        public void setup_starting_members()
        {
            this.actors = new List<Game_Actor>();
            foreach (var i in Global.data_system.party_members)
                this.actors.Add(Global.game_actors[i]);
        }
        //--------------------------------------------------------------------------
        // ● 设置战斗测试用同伴
        //--------------------------------------------------------------------------
        public void setup_battle_test_members()
        {
            this.actors = new List<Game_Actor>();
            foreach (var battler in Global.data_system.test_battlers)
            {
                var actor = Global.game_actors[battler.actor_id];
                actor.level = battler.level;
                gain_weapon(battler.weapon_id, 1);
                gain_armor(battler.armor1_id, 1);
                gain_armor(battler.armor2_id, 1);
                gain_armor(battler.armor3_id, 1);
                gain_armor(battler.armor4_id, 1);
                actor.equip(0, battler.weapon_id);
                actor.equip(1, battler.armor1_id);
                actor.equip(2, battler.armor2_id);
                actor.equip(3, battler.armor3_id);
                actor.equip(4, battler.armor4_id);
                actor.recover_all();
                this.actors.Add(actor);
            }
            this.items = new Dictionary<int, int>();
            for (var i = 1; i <= Global.data_items.Count; i++)
            {
                if (Global.data_items[i].name != "")
                {
                    var occasion = Global.data_items[i].occasion;
                    if (occasion == 0 || occasion == 1)
                        this.items[i] = 99;
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 同伴成员的还原
        //--------------------------------------------------------------------------
        public virtual void refresh()
        {
            // 游戏数据载入后角色对像直接从 Global.game_actors
            // 分离。
            // 回避由于载入造成的角色再设置的问题。
            var new_actors = new List<Game_Actor>();
            for (var i = 0; i < this.actors.Count; i++)
            {
                if (Global.data_actors[this.actors[i].id] != null)
                    new_actors.Add(Global.game_actors[this.actors[i].id]);
            }
            this.actors = new_actors;
        }
        //--------------------------------------------------------------------------
        // ● 获取最大等级
        //--------------------------------------------------------------------------
        public int max_level
        {
            get
            {
                // 同伴人数为 0 的情况下
                if (this.actors.Count == 0)
                    return 0;

                // 初始化本地变量 level
                var level = 0;
                // 求得同伴的最大等级
                foreach (var actor in this.actors)
                    if (level < actor.level)
                        level = actor.level;

                return level;
            }
        }
        //--------------------------------------------------------------------------
        // ● 加入同伴
        //     actor_id : 角色 ID
        //--------------------------------------------------------------------------
        public void add_actor(int actor_id)
        {
            // 获取角色
            var actor = Global.game_actors[actor_id];
            // 同伴人数未满 4 人、本角色不在队伍中的情况下
            if (this.actors.Count < 4 && !this.actors.Contains(actor))
                // 添加角色
                this.actors.Add(actor);
            // 还原主角
            Global.game_player.refresh();
        }
        //--------------------------------------------------------------------------
        // ● 角色离开
        //     actor_id : 角色 ID
        //--------------------------------------------------------------------------
        public void remove_actor(int actor_id)
        {
            // 删除角色
            this.actors.Remove(Global.game_actors[actor_id]);
            // 还原主角
            Global.game_player.refresh();
        }
        //--------------------------------------------------------------------------
        // ● 增加金钱 (减少)
        //     n : 金额
        //--------------------------------------------------------------------------
        public void gain_gold(int n)
        {
            this.gold = Math.Min(Math.Max(this.gold + n, 0), 9999999);
        }
        //--------------------------------------------------------------------------
        // ● 减少金钱
        //     n : 金额
        //--------------------------------------------------------------------------
        public void lose_gold(int n)
        // 调用数值逆转 gain_gold 
        {
            gain_gold(-n);
        }
        //--------------------------------------------------------------------------
        // ● 增加步数
        //--------------------------------------------------------------------------
        public void increase_steps()
        {
            this.steps = Math.Min(this.steps + 1, 9999999);
        }
        //--------------------------------------------------------------------------
        // ● 获取物品的所持数
        //     item_id : 物品 ID
        //--------------------------------------------------------------------------
        public int item_number(int item_id)
        {
            // 如果 hash 个数数值不存在就返回 0

            return this.items.ContainsKey(item_id) ? this.items[item_id] : 0;
        }
        //--------------------------------------------------------------------------
        // ● 获取武器所持数
        //     weapon_id : 武器 ID
        //--------------------------------------------------------------------------
        public int weapon_number(int weapon_id)
        {
            // 如果 hash 个数数值不存在就返回 0

            return this.weapons.ContainsKey(weapon_id) ? this.weapons[weapon_id] : 0;
        }
        //--------------------------------------------------------------------------
        // ● 获取防具所持数
        //     armor_id : 防具 ID
        //--------------------------------------------------------------------------
        public int armor_number(int armor_id)
        {
            // 如果 hash 个数数值不存在就返回 0
            return this.armors.ContainsKey(armor_id) ? this.armors[armor_id] : 0;
        }
        //--------------------------------------------------------------------------
        // ● 增加物品 (减少)
        //     item_id : 物品 ID
        //     n       : 个数
        //--------------------------------------------------------------------------
        public void gain_item(int item_id, int n)
        {
            // 更新 hash 的个数数据
            if (item_id > 0)
                this.items[item_id] = Math.Min(Math.Max(item_number(item_id) + n, 0), 99);
        }
        //--------------------------------------------------------------------------
        // ● 增加武器 (减少)
        //     weapon_id : 武器 ID
        //     n         : 个数
        //--------------------------------------------------------------------------
        public void gain_weapon(int weapon_id, int n)
        {
            // 更新 hash 的个数数据
            if (weapon_id > 0)
                this.weapons[weapon_id] = Math.Min(Math.Max(weapon_number(weapon_id) + n, 0), 99);
        }
        //--------------------------------------------------------------------------
        // ● 增加防具 (减少)
        //     armor_id : 防具 ID
        //     n        : 个数
        //--------------------------------------------------------------------------
        public void gain_armor(int armor_id, int n)
        {
            // 更新 hash 的个数数据
            if (armor_id > 0)
                this.armors[armor_id] = Math.Min(Math.Max(armor_number(armor_id) + n, 0), 99);
        }
        //--------------------------------------------------------------------------
        // ● 减少物品
        //     item_id : 物品 ID
        //     n       : 个数
        //--------------------------------------------------------------------------
        public void lose_item(int item_id, int n)
        {
            // 调用 gain_item 的数值逆转
            gain_item(item_id, -n);
        }
        //--------------------------------------------------------------------------
        // ● 减少武器
        //     weapon_id : 武器 ID
        //     n         : 个数
        //--------------------------------------------------------------------------
        public void lose_weapon(int weapon_id, int n)
        {
            // 调用 gain_weapon 的数值逆转
            gain_weapon(weapon_id, -n);
        }
        //--------------------------------------------------------------------------
        // ● 减少防具
        //     armor_id : 防具 ID
        //     n        : 个数
        //--------------------------------------------------------------------------
        public void lose_armor(int armor_id, int n)
        {
            // 调用 gain_armor 的数值逆转
            gain_armor(armor_id, -n);
        }
        //--------------------------------------------------------------------------
        // ● 判断物品可以使用
        //     item_id : 物品 ID
        //--------------------------------------------------------------------------
        public bool is_item_can_use(int item_id)
        {
            // 物品个数为 0 的情况
            if (item_number(item_id) == 0)
                // 不能使用
                return false;

            // 获取可以使用的时候
            var occasion = Global.data_items[item_id].occasion;
            // 战斗的情况
            if (Global.game_temp.in_battle)
                // 可以使用时为 0 (平时) 或者是 1 (战斗时) 可以使用
                return (occasion == 0 || occasion == 1);

            // 可以使用时为 0 (平时) 或者是 2 (菜单时) 可以使用
            return (occasion == 0 || occasion == 2);
        }
        //--------------------------------------------------------------------------
        // ● 清除全体的行动
        //--------------------------------------------------------------------------
        public void clear_actions()
        {
            // 清除全体同伴的行为
            foreach (var actor in this.actors)
                actor.current_action.clear();
        }
        //--------------------------------------------------------------------------
        // ● 可以输入命令的判定
        //--------------------------------------------------------------------------
        public bool is_inputable
        {
            get
            {
                // 如果一可以输入命令就返回 true
                foreach (var actor in this.actors)
                    if (actor.is_inputable)
                        return true;

                return false;
            }
        }
        //--------------------------------------------------------------------------
        // ● 全灭判定
        //--------------------------------------------------------------------------
        public bool is_all_dead
        {
            get
            {
                // 同伴人数为 0 的情况下
                if (Global.game_party.actors.Count == 0)
                    return false;

                // 同伴中无人 HP 在 0 以上
                foreach (var actor in this.actors)
                    if (actor.hp > 0)
                        return false;

                // 全灭
                return true;
            }
        }
        //--------------------------------------------------------------------------
        // ● 检查连续伤害 (地图用)
        //--------------------------------------------------------------------------
        public void check_map_slip_damage()
        {
            foreach (var actor in this.actors)
            {
                if (actor.hp > 0 && actor.is_slip_damage)
                {
                    actor.hp -= Math.Max(actor.maxhp / 100, 1);
                    if (actor.hp == 0)
                        Global.game_system.se_play(Global.data_system.actor_collapse_se);

                    Global.game_screen.start_flash(Color.FromArgb(128, 255, 0, 0), 4);
                    Global.game_temp.gameover = Global.game_party.is_all_dead;
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 对像角色的随机确定
        //     hp0 : 限制为 HP 0 的角色
        //--------------------------------------------------------------------------
        public Game_Actor random_target_actor(bool hp0 = false)
        {
            // 初始化轮流
            var roulette = new List<Game_Actor>();
            // 循环
            foreach (var actor in this.actors)
            {
                // 符合条件的场合
                if ((!hp0 && actor.is_exist) || (hp0 && actor.is_hp0))
                {
                    // 获取角色职业的位置 [位置]
                    var position = Global.data_classes[actor.class_id].position;
                    // 前卫的话 n = 4、中卫的话 n = 3、后卫的话 n = 2
                    var n = 4 - position;
                    // 添加角色的轮流 n 回
                    for (var i = 0; i < n; i++)
                        roulette.Add(actor);
                }
            }
            // 轮流大小为 0 的情况
            if (roulette.Count == 0)
                return null;

            // 转轮盘赌，决定角色
            return roulette[Global.rand(roulette.Count)];
        }
        //--------------------------------------------------------------------------
        // ● 对像角色的随机确定 (HP 0)
        //--------------------------------------------------------------------------
        public Game_Actor random_target_actor_hp0()
        {
            return random_target_actor(true);
        }
        //--------------------------------------------------------------------------
        // ● 对像角色的顺序确定
        //     actor_index : 角色索引
        //--------------------------------------------------------------------------
        public Game_Actor smooth_target_actor(int actor_index)
        {
            // 取得对像
            var actor = this.actors[actor_index];
            // 对像存在的情况下
            if (actor != null && actor.is_exist)
                return actor;

            // 循环
            foreach (var actor1 in this.actors)
                // 对像存在的情况下
                if (actor1.is_exist)
                    return actor1;

            return null;
        }


        public Dictionary<int, int> items { get; set; }
        public Dictionary<int, int> weapons { get; set; }
        public Dictionary<int, int> armors { get; set; }
    }
}
