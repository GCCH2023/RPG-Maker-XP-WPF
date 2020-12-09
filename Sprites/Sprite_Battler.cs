using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Sprite_Battler
    //------------------------------------------------------------------------------
    // 　战斗显示用活动块。Game_Battler 类的实例监视、
    // 活动块的状态的监视。
    //==============================================================================

    public class Sprite_Battler : RPG.Sprite
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public Game_Battler battler;                  // 战斗者

        public string battler_name { get; set; }

        public int battler_hue { get; set; }

        public int state_animation_id { get; set; }

        bool _battler_visible;

        public bool battler_visible
        {
            get { return _battler_visible; }
            set { _battler_visible = value; }
        }
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     viewport : 显示端口
        //     battler  : 战斗者 (Game_Battler)
        //--------------------------------------------------------------------------
        public Sprite_Battler(Viewport viewport, Game_Battler battler = null) :
            base(viewport)
        {
            this.battler = battler;
            this.battler_visible = false;
        }
        //--------------------------------------------------------------------------
        // ● 释放
        //--------------------------------------------------------------------------
        public override void dispose()
        {
            if (this.bitmap != null)
                this.bitmap.dispose();
            base.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            base.update();
            // 战斗者为 null 的情况下
            if (this.battler == null)
            {
                this.bitmap = null;
                loop_animation(null);
                return;
            }
            // 文件名和色相与当前情况有差异的情况下
            if (this.battler.battler_name != this.battler_name ||
                      this.battler.battler_hue != this.battler_hue)
            {

                // 获取、设置位图
                this.battler_name = this.battler.battler_name;
                this.battler_hue = this.battler.battler_hue;
                this.bitmap = RPG.Cache.battler(this.battler_name, this.battler_hue);
                this.width = bitmap.width;
                this.height = bitmap.height;
                this.ox = this.width / 2;
                this.oy = this.height;
                // 如果是战斗不能或者是隐藏状态就把透明度设置成 0
                if (this.battler.is_dead || this.battler.hidden)
                    this.opacity = 0;
            }
            // 动画 ID 与当前的情况有差异的情况下
            if (this.battler.damage == null &&
                      this.battler.state_animation_id != this.state_animation_id)
            {

                this.state_animation_id = this.battler.state_animation_id;
                loop_animation(Global.data_animations[this.state_animation_id]);
            }
            // 应该被显示的角色的情况下
            if ((this.battler is Game_Actor) && this.battler_visible)
            {
                // 不是主状态的时候稍稍降低点透明度
                if (Global.game_temp.battle_main_phase)
                {
                    if (this.opacity < 255)
                        this.opacity += 3;
                }
                else
                {
                    if (this.opacity > 207)
                        this.opacity -= 3;
                }
            }
            // 明灭
            if (this.battler.blink)
                blink_on();
            else
                blink_off();

            // 不可见的情况下
            if (!this.battler_visible)
            {
                // 出现
                if (!this.battler.hidden && !this.battler.is_dead &&
                            (this.battler.damage == null || this.battler.damage_pop))
                {
                    appear();
                    this.battler_visible = true;
                }
            }
            // 可见的情况下
            if (this.battler_visible)
            {
                // 逃跑
                if (this.battler.hidden)
                {
                    Global.game_system.se_play(Global.data_system.escape_se);
                    escape();
                    this.battler_visible = false;
                }
                // 白色闪烁
                if (this.battler.white_flash)
                {
                    whiten();
                    this.battler.white_flash = false;
                }
                // 动画
                if (this.battler.animation_id != 0)
                {
                    var animation1 = Global.data_animations[this.battler.animation_id];
                    animation(animation1, this.battler.animation_hit);
                    this.battler.animation_id = 0;
                }
                // 伤害
                if (this.battler.damage_pop)
                {
                    damage(this.battler.damage, this.battler.critical);
                    this.battler.damage = null;
                    this.battler.critical = false;
                    this.battler.damage_pop = false;
                }
                // korapusu
                if (this.battler.damage == null && this.battler.is_dead)
                {
                    if (this.battler is Game_Enemy)
                        Global.game_system.se_play(Global.data_system.enemy_collapse_se);
                    else
                        Global.game_system.se_play(Global.data_system.actor_collapse_se);
                    collapse();
                    this.battler_visible = false;
                }
            }
            // 设置活动块的坐标
            this.x = this.battler.screen_x;
            this.y = this.battler.screen_y;
            this.z = this.battler.screen_z;
        }
    }

}
