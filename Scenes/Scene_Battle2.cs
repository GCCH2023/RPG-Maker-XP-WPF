using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_Battle (分割定义 2)
    //------------------------------------------------------------------------------
    // 　处理战斗画面的类。
    //==============================================================================

    public partial class Scene_Battle : Scene
    {
        //--------------------------------------------------------------------------
        // ● 开始自由战斗回合
        //--------------------------------------------------------------------------
        public void start_phase1()
        {
            // 转移到回合 1
            this.phase = 1;
            // 清除全体同伴的行动
            Global.game_party.clear_actions();
            // 设置战斗事件
            setup_battle_event();
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (自由战斗回合)
        //--------------------------------------------------------------------------
        public void update_phase1()
        {
            // 胜败判定
            if (judge)
            {
                // 胜利或者失败的情况下 : 过程结束
                return;
            }
            // 开始同伴命令回合
            start_phase2();
        }
        //--------------------------------------------------------------------------
        // ● 开始同伴命令回合
        //--------------------------------------------------------------------------
        public void start_phase2()
        {
            // 转移到回合 2
            this.phase = 2;
            // 设置角色为非选择状态
            this.actor_index = -1;
            this.active_battler = null;
            // 有效化同伴指令窗口
            this.party_command_window.active = true;
            this.party_command_window.visible = true;
            // 无效化角色指令窗口
            this.actor_command_window.active = false;
            this.actor_command_window.visible = false;
            // 清除主回合标志
            Global.game_temp.battle_main_phase = false;
            // 清除全体同伴的行动
            Global.game_party.clear_actions();
            // 不能输入命令的情况下
            if (!Global.game_party.is_inputable)
            {
                // 开始主回合
                start_phase4();
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (同伴命令回合)
        //--------------------------------------------------------------------------
        public void update_phase2()
        {
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 同伴指令窗口光标位置分支
                switch (this.party_command_window.index)
                {
                    case 0:  // 战斗
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 开始角色的命令回合
                        start_phase3();
                        break;
                    case 1:  // 逃跑
                        // 不能逃跑的情况下
                        if (Global.game_temp.battle_can_escape == false)
                        {
                            // 演奏冻结 SE
                            Global.game_system.se_play(Global.data_system.buzzer_se);
                            return;
                        }
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 逃走处理
                        update_phase2_escape();
                        break;
                }
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 画面更新 (同伴指令回合 : 逃跑)
        //--------------------------------------------------------------------------
        public void update_phase2_escape()
        {
            // 计算敌人速度的平均值
            var enemies_agi = 0;
            var enemies_number = 0;
            foreach (var enemy in Global.game_troop.enemies)
            {
                if (enemy.is_exist)
                {
                    enemies_agi += (int)enemy.agi;
                    enemies_number += 1;
                }
            }
            if (enemies_number > 0)
            {
                enemies_agi /= enemies_number;
            }
            // 计算角色速度的平均值
            var actors_agi = 0;
            var actors_number = 0;
            foreach (var actor in Global.game_party.actors)
            {
                if (actor.is_exist)
                {
                    actors_agi += (int)actor.agi;
                    actors_number += 1;
                }
            }
            if (actors_number > 0)
            {
                actors_agi /= actors_number;
            }
            // 逃跑成功判定
            var success = Global.rand(100) < 50 * actors_agi / enemies_agi;
            // 成功逃跑的情况下
            if (success)
            {
                // 演奏逃跑 SE
                Global.game_system.se_play(Global.data_system.escape_se);
                // 还原为战斗开始前的 RPG.AudioFile
                Global.game_system.bgm_play(Global.game_temp.map_bgm);
                // 战斗结束
                battle_end(1);
                // 逃跑失败的情况下
            }
            else
            {
                // 清除全体同伴的行动
                Global.game_party.clear_actions();
                // 开始主回合
                start_phase4();
            }
        }
        //--------------------------------------------------------------------------
        // ● 开始结束战斗回合
        //--------------------------------------------------------------------------
        public void start_phase5()
        {
            // 转移到回合 5
            this.phase = 5;
            // 演奏战斗结束 ME
            Global.game_system.me_play(Global.game_system.battle_end_me);
            // 还原为战斗开始前的 RPG.AudioFile
            Global.game_system.bgm_play(Global.game_temp.map_bgm);
            // 初始化 EXP、金钱、宝物
            var exp = 0;
            var gold = 0;
            var treasures = new List<RPG.Goods>();
            // 循环
            foreach (var enemy in Global.game_troop.enemies)
            {
                // 敌人不是隐藏状态的情况下
                if (!enemy.hidden)
                {
                    // 获得 EXP、增加金钱
                    exp += enemy.exp;
                    gold += enemy.gold;
                    // 出现宝物判定
                    if (Global.rand(100) < enemy.treasure_prob)
                    {
                        if (enemy.item_id > 0)
                        {
                            treasures.Add(Global.data_items[enemy.item_id]);
                        }
                        if (enemy.weapon_id > 0)
                        {
                            treasures.Add(Global.data_weapons[enemy.weapon_id]);
                        }
                        if (enemy.armor_id > 0)
                        {
                            treasures.Add(Global.data_armors[enemy.armor_id]);
                        }
                    }
                }
            }
            // 限制宝物数为 6 个
            while(treasures.Count < 6)
            {
                treasures.Add(null);
            }
            treasures = treasures.Take(5).ToList();

            // 获得 EXP
            for (var i = 0; i < Global.game_party.actors.Count; i++)
            {
                var actor = Global.game_party.actors[i];
                if (actor.is_cant_get_exp == false)
                {
                    var last_level = actor.level;
                    actor.exp += exp;
                    if (actor.level > last_level)
                    {
                        this.status_window.level_up(i);
                    }
                }
            }
            // 获得金钱
            Global.game_party.gain_gold(gold);
            // 获得宝物
            foreach (var item_obj in treasures)
            {
                var item = item_obj as RPG.Item;
                if (item != null)
                    Global.game_party.gain_item(item.id, 1);

                var item1 = item_obj as RPG.Weapon;
                if (item != null)
                    Global.game_party.gain_weapon(item1.id, 1);

                var item2 = item_obj as RPG.Armor;
                if (item2 != null)
                    Global.game_party.gain_armor(item2.id, 1);
            }
            // 生成战斗结果窗口
            this.result_window = new Window_BattleResult(exp, gold, treasures);
            // 设置等待计数
            this.phase5_wait_count = 100;
        }
        //--------------------------------------------------------------------------
        // ● 画面更新 (结束战斗回合)
        //--------------------------------------------------------------------------
        public void update_phase5()
        {
            // 等待计数大于 0 的情况下
            if (this.phase5_wait_count > 0)
            {
                // 减少等待计数
                this.phase5_wait_count -= 1;
                // 等待计数为 0 的情况下
                if (this.phase5_wait_count == 0)
                {
                    // 显示结果窗口
                    this.result_window.visible = true;
                    // 清除主回合标志
                    Global.game_temp.battle_main_phase = false;
                    // 刷新状态窗口
                    this.status_window.refresh();
                }
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 战斗结束
                battle_end(0);
            }
        }

        public Window_BattleResult result_window { get; set; }

        public int phase5_wait_count { get; set; }
    }

}
