using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XP
{
    //==============================================================================
    // ■ Sprite_Character
    //------------------------------------------------------------------------------
    // 　角色显示用脚本。监视 Game_Character 类的实例、
    // 自动变化脚本状态。
    //==============================================================================

    public class Sprite_Character : RPG.Sprite
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public Game_Character character;                // 角色
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     viewport  : 查看端口
        //     character : 角色 (Game_Character)
        //--------------------------------------------------------------------------
        public Sprite_Character(Viewport viewport, Game_Character character = null)
            : base(viewport)
        {
            this.character = character;
            update();
        }
        //--------------------------------------------------------------------------
        // ● 更新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            base.update();
            // 元件 ID、文件名、色相与现在的情况存在差异的情况下
            if (this.tile_id != this.character.tile_id ||
                      this.character_name != this.character.character_name ||
                      this.character_hue != this.character.character_hue)
            {
                // 记忆元件 ID 与文件名、色相
                this.tile_id = this.character.tile_id;
                this.character_name = this.character.character_name;
                this.character_hue = this.character.character_hue;
                // 元件 ID 为有效值的情况下
                if (this.tile_id >= 384)
                {
                    this.bitmap = RPG.Cache.tile(Global.game_map.tileset_name,
                      this.tile_id, this.character.character_hue);
                    this.src_rect = new Rect(0, 0, 32, 32);
                    this.ox = 16;
                    this.oy = 32;
                }
                // 元件 ID 为无效值的情况下
                else
                {
                    this.bitmap = RPG.Cache.character(this.character.character_name,
                      this.character.character_hue);
                    this.cw = bitmap.width / 4;
                    this.ch = bitmap.height / 4;
                    this.ox = this.cw / 2;
                    this.oy = this.ch;
                }
            }
            // 设置可视状态
            this.visible = (!this.character.transparent);
            // 图形是角色的情况下
            if (this.tile_id == 0)
            {
                // 设置传送目标的矩形
                var sx = this.character.pattern * this.cw;
                var sy = (this.character.direction - 2) / 2 * this.ch;
                this.src_rect = new Rect(sx, sy, this.cw, this.ch);
            }
            // 设置脚本的坐标
            this.x = this.character.screen_x;
            this.y = this.character.screen_y;
            this.z = this.character.screen_z(this.ch);
            // 设置不透明度、合成方式、茂密
            this.opacity = this.character.opacity;
            this.blend_type = this.character.blend_type;
            this.bush_depth = this.character.bush_depth;
            // 动画
            if (this.character.animation_id != 0)
            {
                var animation1 = Global.data_animations[this.character.animation_id];
                animation(animation1, true);
                this.character.animation_id = 0;
            }
        }

        public int tile_id { get; set; }

        public string character_name { get; set; }

        public int character_hue { get; set; }

        public int cw { get; set; }

        public int ch { get; set; }
    }

}
