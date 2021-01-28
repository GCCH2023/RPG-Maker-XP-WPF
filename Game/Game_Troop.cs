using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_Troop
    //------------------------------------------------------------------------------
    // 　处理队伍的类。本类的实例请参考 Global.game_troop
    // 
    //==============================================================================

    public class Game_Troop
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Game_Troop()
        {
            // 建立敌人序列
            this.enemies = new List<Game_Enemy>();
        }
        //--------------------------------------------------------------------------
        // ● 获取敌人
        //--------------------------------------------------------------------------
        List<Game_Enemy> _enemies;
        public List<Game_Enemy> enemies
        {
            get { return this._enemies; }
            set { this._enemies = value; }
        }
        //--------------------------------------------------------------------------
        // ● 设置
        //     troop_id : 敌人 ID
        //--------------------------------------------------------------------------
        public void setup(int troop_id)
        {
            // 由敌人序列的设置来确定队伍的设置
            this.enemies = new List<Game_Enemy>();
            var troop = Global.data_troops[troop_id];
            for (var i = 0; i < troop.members.Count; i++)
            {
                var enemy = Global.data_enemies[troop.members[i].enemy_id];
                if (enemy != null)
                    this.enemies.Add(new Game_Enemy(troop_id, i));
            }
        }
        //--------------------------------------------------------------------------
        // ● 对像敌人的随机确定
        //     hp0 : 限制 HP 0 的敌人
        //--------------------------------------------------------------------------
        public Game_Enemy random_target_enemy(bool hp0 = false)
        {
            // 初始化轮流
            var roulette = new List<Game_Enemy>();
            // 循环
            foreach (var enemy in this.enemies)
            {
                // 条件符合的情况下
                if ((!hp0 && enemy.is_exist) || (hp0 && enemy.is_hp0))
                    // 添加敌人到轮流
                    roulette.Add(enemy);
            }

            // 轮流尺寸为 0 的情况下
            if (roulette.Count == 0)
                return null;

            // 转轮盘赌，决定敌人
            return roulette[Global.rand(roulette.Count)];
        }
        //--------------------------------------------------------------------------
        // ● 对像敌人的随机确定 (HP 0)
        //--------------------------------------------------------------------------
        public Game_Enemy random_target_enemy_hp0
        {
            get
            {
                return random_target_enemy(true);
            }
        }
        //--------------------------------------------------------------------------
        // ● 对像角色的顺序确定
        //     enemy_index : 敌人索引
        //--------------------------------------------------------------------------
        public Game_Enemy smooth_target_enemy(int enemy_index)
        {
            // 获取敌人
            var enemy = this.enemies[enemy_index];
            // 敌人存在的场合
            if (enemy != null && enemy.is_exist)
                return enemy;

            // 循环
            foreach (var enemy1 in this.enemies)
                // 敌人存在的场合
                if (enemy1.is_exist)
                    return enemy1;

            return null;
        }
    }

}
