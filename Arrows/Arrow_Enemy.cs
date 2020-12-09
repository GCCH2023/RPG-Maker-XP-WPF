using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Arrow_Enemy
    //------------------------------------------------------------------------------
    // 　选择敌人的箭头光标。本类继承 Arrow_Base
    // 类。
    //==============================================================================

    public class Arrow_Enemy : Arrow_Base
    {
        public Arrow_Enemy(Viewport viewport)
            : base(viewport)
        {

        }
        //--------------------------------------------------------------------------
        // ● 获取光标指向的敌人
        //--------------------------------------------------------------------------
        public Game_Enemy enemy
        {
            get
            {
                return Global.game_troop.enemies[this.index];
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            base.update();
            // 如果指向不存在的敌人就离开
            // @@ 不知道这是干嘛的
            //for (int i = 0; i < Global.game_troop.enemies.Count; i++)
            //{
            //    if (this.enemy.is_exist)
            //        break;
            //    this.index += 1;
            //    this.index %= Global.game_troop.enemies.Count;
            //}

            // 当前指向的敌人可能在上个回合死掉了，现在必须先指向还活着的敌人
            while (this.index >= 0 && this.index < Global.game_troop.enemies.Count && !this.enemy.is_exist)
                this.index = (this.index + 1) % Global.game_troop.enemies.Count;

            // 光标右
            if (Input.is_repeat(Input.RIGHT))
            {
                Global.game_system.se_play(Global.data_system.cursor_se);

                //for (int i = 0; i < Global.game_troop.enemies.Count; i++)
                //{
                //    this.index += 1;
                //    this.index %= Global.game_troop.enemies.Count;
                //    if (this.enemy.is_exist)
                //        break;
                //}
                // @@ 替换了
                do
                {
                    this.index = (this.index + 1) % Global.game_troop.enemies.Count;
                } while (!this.enemy.is_exist);
            }
            // 光标左
            if (Input.is_repeat(Input.LEFT))
            {
                Global.game_system.se_play(Global.data_system.cursor_se);
                //for (int i = 0; i < Global.game_troop.enemies.Count; i++)
                //{
                //    this.index += Global.game_troop.enemies.Count - 1;
                //    this.index %= Global.game_troop.enemies.Count;
                //    if (this.enemy.is_exist)
                //        break;
                //}
                // @@ 替换了
                this.index = (this.index - 1 + Global.game_troop.enemies.Count) % Global.game_troop.enemies.Count;
                do
                {
                    this.index = (this.index - 1 + Global.game_troop.enemies.Count) % Global.game_troop.enemies.Count;
                } while (!this.enemy.is_exist);
            }
            // 设置活动块坐标
            if (this.enemy != null)
            {
                this.x = this.enemy.screen_x;
                this.y = this.enemy.screen_y;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新帮助文本
        //--------------------------------------------------------------------------
        public override void update_help()
        {
            // 帮助窗口显示敌人的名字与状态
            this.help_window.set_enemy(this.enemy);
        }
    }
}
