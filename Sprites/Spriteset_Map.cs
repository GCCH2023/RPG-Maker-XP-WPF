using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Spriteset_Map
    //------------------------------------------------------------------------------
    // 　处理地图画面活动块和元件的类。本类在
    // Scene_Map 类的内部使用。
    //==============================================================================

    public class Spriteset_Map
    {
        public List<Sprite_Character> character_sprites { get; set; }

        public Viewport viewport1 { get; set; }
        public Viewport viewport2 { get; set; }
        public Viewport viewport3 { get; set; }
        public Tilemap tilemap { get; set; }
        public Plane panorama { get; set; }
        public Plane fog { get; set; }
        public RPG.Weather weather { get; set; }
        public List<Sprite_Picture> picture_sprites { get; set; }
        public Sprite_Timer timer_sprite { get; set; }
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Spriteset_Map()
        {
            // 生成显示端口
            this.viewport1 = new Viewport(0, 0, 640, 480);
            this.viewport2 = new Viewport(0, 0, 640, 480);
            this.viewport3 = new Viewport(0, 0, 640, 480);
            this.viewport2.z = 200;
            this.viewport3.z = 5000;
            // 生成元件地图
            this.tilemap = new Tilemap(this.viewport1);
            this.tilemap.tileset = RPG.Cache.tileset(Global.game_map.tileset_name);
            for (var i = 0; i <= 6; i++)
            {
                var autotile_name = Global.game_map.autotile_names[i];
                this.tilemap.autotiles[i] = RPG.Cache.autotile(autotile_name);
            }
            this.tilemap.priorities = Global.game_map.priorities;
            this.tilemap.map_data = Global.game_map.data;
            // 生成远景平面
            this.panorama = new Plane(this.viewport1);
            this.panorama.z = -1000;
            // 生成雾平面
            this.fog = new Plane(this.viewport1);
            this.fog.z = 3000;
            // 生成角色活动块
            this.character_sprites = new List<Sprite_Character>();
            var eventKeys = new List<int>(Global.game_map.events.Keys);
            eventKeys.Sort();
            foreach (var i in eventKeys)
            {
                var sprite = new Sprite_Character(this.viewport1, Global.game_map.events[i]);
                this.character_sprites.Add(sprite);
            }
            this.character_sprites.Add(new Sprite_Character(this.viewport1, Global.game_player));
            // 生成天气
            this.weather = new RPG.Weather(this.viewport1);
            // 生成图片
            this.picture_sprites = new List<Sprite_Picture>();
            for (int i = 1; i <= 50; i++)
            {
                this.picture_sprites.Add(new Sprite_Picture(this.viewport2,
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
        public virtual void dispose()
        {
            // 释放元件地图
            this.tilemap.tileset.dispose();
            for (var i = 0; i <= 6; i++)
            {
                this.tilemap.autotiles[i].dispose();
            }
            this.tilemap.dispose();
            // 释放远景平面
            this.panorama.dispose();
            // 释放雾平面
            this.fog.dispose();
            // 释放角色活动块
            foreach (var sprite in this.character_sprites)
                sprite.dispose();

            // 释放天候
            this.weather.dispose();
            // 释放图片
            foreach (var sprite in this.picture_sprites)
            {
                sprite.dispose();
            }
            // 释放计时器块
            this.timer_sprite.dispose();
            // 释放显示端口
            this.viewport1.dispose();
            this.viewport2.dispose();
            this.viewport3.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public virtual void update()
        {
            // 远景与现在的情况有差异发生的情况下
            if (this.panorama_name != Global.game_map.panorama_name ||
                     this.panorama_hue != Global.game_map.panorama_hue)
            {
                this.panorama_name = Global.game_map.panorama_name;
                this.panorama_hue = Global.game_map.panorama_hue;
                if (this.panorama.bitmap != null)
                {
                    this.panorama.bitmap.dispose();
                    this.panorama.bitmap = null;
                }
                if (this.panorama_name != "")
                {
                    this.panorama.bitmap = RPG.Cache.panorama(this.panorama_name, this.panorama_hue);
                }
                Graphics.frame_reset();
            }
            // 雾与现在的情况有差异的情况下
            if (this.fog_name != Global.game_map.fog_name || this.fog_hue != Global.game_map.fog_hue)
            {
                this.fog_name = Global.game_map.fog_name;
                this.fog_hue = Global.game_map.fog_hue;
                if (this.fog.bitmap != null)
                {
                    this.fog.bitmap.dispose();
                    this.fog.bitmap = null;
                }
                if (this.fog_name != "")
                {
                    this.fog.bitmap = RPG.Cache.fog(this.fog_name, this.fog_hue);
                }
                Graphics.frame_reset();
            }
            // 刷新元件地图
            this.tilemap.ox = Global.game_map.display_x / 4;
            this.tilemap.oy = Global.game_map.display_y / 4;
            this.tilemap.update();
            // 刷新远景平面
            this.panorama.ox = Global.game_map.display_x / 8;
            this.panorama.oy = Global.game_map.display_y / 8;
            // 刷新雾平面
            this.fog.zoom_x = Global.game_map.fog_zoom / 100.0;
            this.fog.zoom_y = Global.game_map.fog_zoom / 100.0;
            this.fog.opacity = Global.game_map.fog_opacity;
            this.fog.blend_type = Global.game_map.fog_blend_type;
            this.fog.ox = Global.game_map.display_x / 4 + Global.game_map.fog_ox;
            this.fog.oy = Global.game_map.display_y / 4 + Global.game_map.fog_oy;
            this.fog.tone = Global.game_map.fog_tone;
            // 刷新角色活动块
            foreach (var sprite in this.character_sprites)
            {
                sprite.update();
            }
            // 刷新天候图形
            this.weather.type = Global.game_screen.weather_type;
            this.weather.max = Global.game_screen.weather_max;
            this.weather.ox = Global.game_map.display_x / 4;
            this.weather.oy = Global.game_map.display_y / 4;
            this.weather.update();
            // 刷新图片
            foreach (var sprite in this.picture_sprites)
            {
                sprite.update();
            }
            // 刷新计时器块
            this.timer_sprite.update();
            // 设置画面的色调与震动位置
            this.viewport1.tone = Global.game_screen.tone;
            this.viewport1.ox = Global.game_screen.shake;
            // 设置画面的闪烁色
            this.viewport3.color = Global.game_screen.flash_color;
            // 刷新显示端口
            this.viewport1.update();
            this.viewport3.update();
        }

        public string panorama_name { get; set; }

        public int panorama_hue { get; set; }

        public string fog_name { get; set; }

        public int fog_hue { get; set; }
    }

}
