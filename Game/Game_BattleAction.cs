using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_BattleAction
    //------------------------------------------------------------------------------
    // 　处理行动 (战斗中的行动) 的类。这个类在 Game_Battler 类
    // 的内部使用。
    //==============================================================================

    public class Game_BattleAction
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public int speed;      // 速度
        public int kind;   // 种类 (基本 / 特技 / 物品)
        public int basic;    // 基本 (攻击 / 防御 / 逃跑)
        public int skill_id;    // 特技 ID
        public int item_id;      // 物品 ID
        public int target_index;         // 对像索引
        public bool forcing;     // 强制标志
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Game_BattleAction()
        {
            clear();
        }
        //--------------------------------------------------------------------------
        // ● 清除
        //--------------------------------------------------------------------------
        public void clear()
        {
            this.speed = 0;
            this.kind = 0;
            this.basic = 3;
            this.skill_id = 0;
            this.item_id = 0;
            this.target_index = -1;
            this.forcing = false;
        }
        //--------------------------------------------------------------------------
        // ● 有效判定
        //--------------------------------------------------------------------------
        public bool is_valid
        {
            get
            {
                return (!(this.kind == 0 && this.basic == 3));
            }
        }
        //--------------------------------------------------------------------------
        // ● 己方单体使用判定
        //--------------------------------------------------------------------------
        public bool is_for_one_friend
        {
            get
            {
                // 种类为特技、效果范围是我方单体 (包含 HP 0) 的情况
                //[3, 5].Contains
                if (this.kind == 1 &&
                    (Global.data_skills[this.skill_id].scope == 3 || Global.data_skills[this.skill_id].scope == 5))
                    return true;
                // 种类为物品、效果范围是我方单体 (包含 HP 0) 的情况
                if (this.kind == 2 &&
                (Global.data_items[this.item_id].scope == 3 || Global.data_items[this.item_id].scope == 5))
                    return true;

                return false;
            }
        }
        //--------------------------------------------------------------------------
        // ● 己方单体用 (HP 0) 判定
        //--------------------------------------------------------------------------
        public bool is_for_one_friend_hp0
        {
            get
            {
                // 种类为特技、效果范围是我方单体 (HP 0) 的情况
                // [5].Contains
                if (this.kind == 1 && Global.data_skills[this.skill_id].scope == 5)
                    return true;
                // 种类为物品、效果范围是我方单体 (HP 0) 的情况
                if (this.kind == 2 && Global.data_items[this.item_id].scope == 5)
                    return true;
                return false;
            }
        }
        //--------------------------------------------------------------------------
        // ● 随机目标 (角色用)
        //--------------------------------------------------------------------------
        public void decide_random_target_for_actor()
        {
            Game_Battler battler;
            // 效果范围的分支
            if (is_for_one_friend_hp0)
                battler = Global.game_party.random_target_actor_hp0();
            else if (is_for_one_friend)
                battler = Global.game_party.random_target_actor();
            else
                battler = Global.game_troop.random_target_enemy();

            // 对像存在的话取得索引、
            // 对像不存在的场合下清除行动
            if (battler != null)
                this.target_index = (int)battler.index;
            else
                clear();
        }
        //--------------------------------------------------------------------------
        // ● 随机目标 (敌人用)
        //--------------------------------------------------------------------------
        public void decide_random_target_for_enemy()
        {
            Game_Battler battler;
            // 效果范围的分支
            if (is_for_one_friend_hp0)
                battler = Global.game_troop.random_target_enemy_hp0;
            else if (is_for_one_friend)
                battler = Global.game_troop.random_target_enemy();
            else
                battler = Global.game_party.random_target_actor();
            // 对像存在的话取得索引、
            // 对像不存在的场合下清除行动
            if (battler != null)
                this.target_index = (int)battler.index;
            else
                clear();
        }
        //--------------------------------------------------------------------------
        // ● 最后的目标 (角色用)
        //--------------------------------------------------------------------------
        public void decide_last_target_for_actor()
        {
            Game_Battler battler;
            // 效果范围是己方单体以及行动者以外的敌人
            if (this.target_index == -1)
                battler = null;
            else if (is_for_one_friend)
                battler = Global.game_party.actors[this.target_index];
            else
                battler = Global.game_troop.enemies[this.target_index];
            // 对像不存在的场合下清除行动
            if (battler == null || !battler.is_exist)
                clear();
        }
        //--------------------------------------------------------------------------
        // ● 最后的目标 (敌人用)
        //--------------------------------------------------------------------------
        public void decide_last_target_for_enemy()
        {
            Game_Battler battler;
            // 效果范围是己方单体以敌人以外的角色
            if (this.target_index == -1)
                battler = null;
            else if (is_for_one_friend)
                battler = Global.game_troop.enemies[this.target_index];
            else
                battler = Global.game_party.actors[this.target_index];
            // 对像不存在的场合下清除行动
            if (battler == null || !battler.is_exist)
                clear();
        }
    }
}
