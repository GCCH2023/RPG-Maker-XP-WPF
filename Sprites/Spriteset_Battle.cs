using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XP
{
    //==============================================================================
    // ■ Spriteset_Battle
    //------------------------------------------------------------------------------
    // 　处理战斗画面的活动块的类。本类在 Scene_Battle 类
    // 的内部使用。
    //==============================================================================

    public class Spriteset_Battle
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public Viewport viewport1;               // 敌人方的显示端口
        public Viewport viewport2;               // 角色方的显示端口

        public Viewport viewport3 { get; set; }
        public Viewport viewport4 { get; set; }

        public XP.Internal.Sprite battleback_sprite { get; set; }
        public List<Sprite_Battler> enemy_sprites { get; set; }
        public List<Sprite_Battler> actor_sprites { get; set; }
        public RPG.Weather weather { get; set; }
        public List<Sprite_Picture> picture_sprites { get; set; }
        //--------------------------------------------------------------------------
        // ● 初始化变量
        //--------------------------------------------------------------------------
        public Spriteset_Battle()
        {
            // 生成显示端口
            this.viewport1 = new Viewport(0, 0, 640, 320);
            this.viewport2 = new Viewport(0, 0, 640, 480);
            this.viewport3 = new Viewport(0, 0, 640, 480);
            this.viewport4 = new Viewport(0, 0, 640, 480);
            this.viewport2.z = 101;
            this.viewport3.z = 200;
            this.viewport4.z = 5000;
            // 生成战斗背景活动块
            this.battleback_sprite = new XP.Internal.Sprite(this.viewport1);
            // 生成敌人活动块
            this.enemy_sprites = new List<Sprite_Battler>();
            for (var i = Global.game_troop.enemies.Count - 1; i >= 0; i--)
            {
                var enemy = Global.game_troop.enemies[i];
                this.enemy_sprites.Add(new Sprite_Battler(this.viewport1, enemy));
            }
            // 生成敌人活动块
            this.actor_sprites = new List<Sprite_Battler>();
            this.actor_sprites.Add(new Sprite_Battler(this.viewport2));
            this.actor_sprites.Add(new Sprite_Battler(this.viewport2));
            this.actor_sprites.Add(new Sprite_Battler(this.viewport2));
            this.actor_sprites.Add(new Sprite_Battler(this.viewport2));
            // 生成天候
            this.weather = new RPG.Weather(this.viewport1);
            // 生成图片活动块
            this.picture_sprites = new List<Sprite_Picture>();
            for (var i = 51; i <= 100; i++)
            {
                this.picture_sprites.Add(new Sprite_Picture(this.viewport3,
                  Global.game_screen.pictures[i]));
            }
            // 生成计时器块
            this.timer_sprite = new Sprite_Timer();
            // 刷新画面
            update();
        }
        //--------------------------------------------------------------------------
        // ● 释放
        //--------------------------------------------------------------------------
        public void dispose()
        {
            // 如果战斗背景位图存在的情况下就释放
            if (this.battleback_sprite.bitmap != null)
            {
                this.battleback_sprite.bitmap.dispose();
            }
            // 释放战斗背景活动块
            this.battleback_sprite.dispose();
            // 释放敌人活动块、角色活动块
            foreach (var sprite in this.enemy_sprites)
            {
                sprite.dispose();
            }
            foreach (var sprite in this.actor_sprites)
            {
                sprite.dispose();
            }
            // 释放天候
            this.weather.dispose();
            // 释放图片活动块
            foreach (var sprite in this.picture_sprites)
            {
                sprite.dispose();
            }
            // 释放计时器活动块
            this.timer_sprite.dispose();
            // 释放显示端口
            this.viewport1.dispose();
            this.viewport2.dispose();
            this.viewport3.dispose();
            this.viewport4.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 显示效果中判定
        //--------------------------------------------------------------------------
        public bool is_effect
        {
            get
            {
                // 如果是在显示效果中的话就返回 true
                foreach (var sprite in this.enemy_sprites)
                {
                    if (sprite.is_effect)
                        return true;
                }
                foreach (var sprite in this.actor_sprites)
                {
                    if (sprite.is_effect)
                        return true;
                }
                return false;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public virtual void update()
        {
            // 刷新角色的活动块 (对应角色的替换)
            this.actor_sprites[0].battler = Global.game_party.actors[0];
            this.actor_sprites[1].battler = Global.game_party.actors[1];
            this.actor_sprites[2].battler = Global.game_party.actors[2];
            this.actor_sprites[3].battler = Global.game_party.actors[3];
            // 战斗背景的文件名与现在情况有差异的情况下
            if (this.battleback_name != Global.game_temp.battleback_name)
            {
                this.battleback_name = Global.game_temp.battleback_name;
                if (this.battleback_sprite.bitmap != null)
                {
                    this.battleback_sprite.bitmap.dispose();
                }
                this.battleback_sprite.bitmap = RPG.Cache.battleback(this.battleback_name);
                this.battleback_sprite.src_rect = new Rect(0, 0, 640, 320);
            }
            // 刷新战斗者的活动块
            foreach (var sprite in this.enemy_sprites)
            {
                sprite.update();
            }
            foreach (var sprite in this.actor_sprites)
            {
                sprite.update();
            }
            // 刷新天气图形
            this.weather.type = Global.game_screen.weather_type;
            this.weather.max = Global.game_screen.weather_max;
            this.weather.update();
            // 刷新图片活动块
            foreach (var sprite in this.picture_sprites)
            {
                sprite.update();
            }
            // 刷新计时器活动块
            this.timer_sprite.update();
            // 设置画面的色调与震动位置
            this.viewport1.tone = Global.game_screen.tone;
            this.viewport1.ox = Global.game_screen.shake;
            // 设置画面的闪烁色
            this.viewport4.color = Global.game_screen.flash_color;
            // 刷新显示端口
            this.viewport1.update();
            this.viewport2.update();
            this.viewport4.update();
        }

        public Sprite_Timer timer_sprite { get; set; }

        public string battleback_name { get; set; }
    }

}
