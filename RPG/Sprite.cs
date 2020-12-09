using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using XP.Internal;

namespace XP.RPG
{
    // RPG.Sprite
    // 追加了 RPGXP 使用的各种效果处理的精灵的类。
    //父类Sprite 
    public class Sprite : XP.Internal.Sprite
    {
        //定义module RPG
        public int _whiten_duration { get; set; }
        public int _appear_duration { get; set; }
        public int _escape_duration { get; set; }
        public int _collapse_duration { get; set; }
        public int _damage_duration { get; set; }
        public int _animation_duration { get; set; }
        public bool _blink { get; set; }
        // _static_不知道什么意思
        public static List<Animation> _animations = new List<Animation>();
        public static Dictionary<object, int> _reference_count = new Dictionary<object, int>();
        //方法whiten 
        //使精灵弱白色地闪烁。是战斗者行动时使用的效果。
        public void whiten()
        {
            this.blend_type = 0;
            this.color = Color.FromArgb(128, 255, 255, 255);
            this.opacity = 255;
            this._whiten_duration = 16;
            this._appear_duration = 0;
            this._escape_duration = 0;
            this._collapse_duration = 0;
        }
        //appear 
        //使精灵从透明状态平滑渐变为不透明状态。是复活和敌人出现时使用的效果。
        public void appear()
        {
            this.blend_type = 0;
            this.color = Colors.Transparent;
            this.opacity = 0;
            this._appear_duration = 16;
            this._whiten_duration = 0;
            this._escape_duration = 0;
            this._collapse_duration = 0;
        }


        //escape 
        //使精灵从不透明状态平滑渐变为透明状态。是敌人逃跑时使用的效果。
        public void escape()
        {
            ;
            this.blend_type = 0;
            this.color = Colors.Transparent;
            this.opacity = 255;
            this._escape_duration = 32;
            this._whiten_duration = 0;
            this._appear_duration = 0;
            this._collapse_duration = 0;
        }

        //collapse 
        //使精灵混合上红色，并平滑渐变为透明状态。是 HP 为 0 死亡时使用的效果。
        public void collapse()
        {
            this.blend_type = 1;
            this.color = Colors.Transparent;
            this.opacity = 255;
            this._collapse_duration = 48;
            this._whiten_duration = 0;
            this._appear_duration = 0;
            this._escape_duration = 0;
        }

        //damage(value, critical) 
        //使伤害的数字或“Miss”等字符串弹出在精灵前面。
        //value 指定为正值时为表示普通伤害的白色文字，而指定为负值时为表示回复的绿色文字。且负值时不会显示符号。value 为字符串时会照原样显示白色的文字。
        //critical 指定真的话，会在伤害字符串上追加小的“CRITICAL”的文字。
        //显示伤害的精灵 Z 座标是 3000。
        public void damage(object value, bool critical)
        {
            dispose_damage();
            string damage_string = "";
            if (value is double)
                damage_string = Math.Abs((double)value).ToString();
            else
                damage_string = value.ToString();

            var bitmap = new Bitmap(160, 48);
            bitmap.font.name = "Arial Black";
            bitmap.font.size = 32;
            bitmap.font.color = Colors.Black;
            bitmap.draw_text(-1, 12 - 1, 160, 36, damage_string, 1);
            bitmap.draw_text(+1, 12 - 1, 160, 36, damage_string, 1);
            bitmap.draw_text(-1, 12 + 1, 160, 36, damage_string, 1);
            bitmap.draw_text(+1, 12 + 1, 160, 36, damage_string, 1);
            if ((value is double) && (double)value < 0)
                bitmap.font.color = Color.FromRgb(176, 255, 144);
            else
                bitmap.font.color = Colors.White;
            bitmap.draw_text(0, 12, 160, 36, damage_string, 1);
            if (critical)
            {
                bitmap.font.size = 20;
                bitmap.font.color = Colors.Black;
                bitmap.draw_text(-1, -1, 160, 20, "CRITICAL", 1);
                bitmap.draw_text(+1, -1, 160, 20, "CRITICAL", 1);
                bitmap.draw_text(-1, +1, 160, 20, "CRITICAL", 1);
                bitmap.draw_text(+1, +1, 160, 20, "CRITICAL", 1);
                bitmap.font.color = Colors.White;
                bitmap.draw_text(0, 0, 160, 20, "CRITICAL", 1);
            }
            this._damage_sprite = new XP.Internal.Sprite(this.viewport);
            this._damage_sprite.bitmap = bitmap;
            this._damage_sprite.ox = 80;
            this._damage_sprite.oy = 20;
            this._damage_sprite.x = this.x;
            this._damage_sprite.y = this.y - this.oy / 2;
            this._damage_sprite.z = 3000;
            this._damage_duration = 40;
        }

        //animation(animation, hit) 
        //以精灵作为对象，显示 animation 指定的动画（RPG.Animation）。

        //hit 为真则进行击中的处理，为伪则进行 MISS 的处理。这个是作为「SE 与闪烁的时机」的条件而使用。

        //显示动画的精灵 Z 座标是 2000。

        //动画对象有很多的话，可以对各个精灵调用该方法。那种情况下类内部会自动判断，[画面]中指定位置的动画单元重复显示，一样没有问题。

        //显示的动画图像从 RPG.Cache 模块中取得，动画结束时为了节约内存而被释放。这样在类内部参照计数使用。通常不需要认识这个，但是在外部使用从 RPG.Cache 模块中取得的动画图像时必须注意。

        //animation 指定 null 的话，动画停止。
        public Animation animation(Animation animation, bool hit)
        {
            dispose_animation();
            this._animation = animation;
            if (this._animation == null)
                return this._animation;

            this._animation_hit = hit;
            this._animation_duration = this._animation.frame_max;
            var animation_name = this._animation.animation_name;
            var animation_hue = this._animation.animation_hue;
            bitmap = RPG.Cache.animation(animation_name, animation_hue);
            if (_reference_count.ContainsKey(bitmap))
                _reference_count[bitmap] += 1;
            else
                _reference_count[bitmap] = 1;

            this._animation_sprites = new List<XP.Internal.Sprite>();
            if (this._animation.position != 3 || !_animations.Contains(animation))
            {
                for (var i = 0; i <= 15; i++)
                {
                    var sprite = new XP.Internal.Sprite(this.viewport);
                    sprite.bitmap = bitmap;
                    sprite.visible = false;
                    this._animation_sprites.Add(sprite);
                }
                if (!_animations.Contains(animation))
                    _animations.Add(animation);
            }
            update_animation();
            return null;
        }
        //loop_animation(animation) 
        //以精灵作为对象，循环显示 animation 指定的动画（RPG.Animation）。

        //与普通动画不同，即使到最后显示也不会停止并返回到第一帧重复。而且可以和普通动画同时显示。该方法在显示状态（RPG.State）指定的动画时使用。

        //animation 指定 null 的话，动画停止。
        public Animation loop_animation(RPG.Animation animation)
        {
            if (animation == this._loop_animation)
                return animation;
            dispose_loop_animation();
            this._loop_animation = animation;
            if (this._loop_animation == null)
                return this._loop_animation;
            this._loop_animation_index = 0;
            var animation_name = this._loop_animation.animation_name;
            var animation_hue = this._loop_animation.animation_hue;
            var bitmap = RPG.Cache.animation(animation_name, animation_hue);
            if (_reference_count.ContainsKey(bitmap))
                _reference_count[bitmap] += 1;
            else
                _reference_count[bitmap] = 1;

            this._loop_animation_sprites = new List<XP.Internal.Sprite>();
            for (var i = 0; i <= 15; i++)
            {
                var sprite = new XP.Internal.Sprite(this.viewport);
                sprite.bitmap = bitmap;
                sprite.visible = false;
                this._loop_animation_sprites.Add(sprite);
            }
            update_loop_animation();
            return null;
        }
        //blink_on 
        //闪烁效果为 ON。能周期地反复使精灵亮白的闪烁。是针对指令输入中的角色使用的效果。
        public void blink_on()
        {
            if (!this._blink)
            {
                this._blink = true;
                this._blink_count = 0;
            }
        }

        //blink_off 
        //闪烁效果为 OFF。
        public void blink_off()
        {

            if (this._blink)
            {
                this._blink = false;
                this.color = Colors.Transparent;
            }
        }

        //is_blink 
        //闪烁效果为 ON 时返回真。
        public bool is_blink
        { get { return this._blink; } }

        //is_effect 
        //whiten、appear、escape、collapse、damage、animation 中有任何一个效果正在显示的话返回真。

        //loop_animation，blink 的状态没有影响。
        public bool is_effect
        {
            get
            {
                return this._whiten_duration > 0 ||
                this._appear_duration > 0 ||
                this._escape_duration > 0 ||
                this._collapse_duration > 0 ||
                this._damage_duration > 0 ||
                this._animation_duration > 0;
            }
        }

        //update 
        //进行各种效果。该方法原则上 1 帧调用 1 次。
        public override void update()
        {
            base.update();
            if (this._whiten_duration > 0)
            {
                this._whiten_duration -= 1;
                this._color.A = (byte)(128 - (16 - this._whiten_duration) * 10);
            }
            if (this._appear_duration > 0)
            {
                this._appear_duration -= 1;
                this.opacity = (16 - this._appear_duration) * 16;
            }
            if (this._escape_duration > 0)
            {
                this._escape_duration -= 1;
                this.opacity = 256 - (32 - this._escape_duration) * 10;
            }

            if (this._collapse_duration > 0)
            {
                this._collapse_duration -= 1;
                this.opacity = 256 - (48 - this._collapse_duration) * 6;
            }

            if (this._damage_duration > 0)
            {
                this._damage_duration -= 1;
                switch (this._damage_duration)
                {
                    case 38:
                    case 39:
                        this._damage_sprite.y -= 4; break;
                    case 36:
                    case 37:
                        this._damage_sprite.y -= 2; break;
                    case 34:
                    case 35:
                        this._damage_sprite.y += 2; break;
                    case 28:
                    case 33:
                        this._damage_sprite.y += 4; break;

                }
                this._damage_sprite.opacity = 256 - (12 - this._damage_duration) * 32;
                if (this._damage_duration == 0)
                    dispose_damage();
            }
            if (this._animation != null && (Graphics.frame_count % 2 == 0))
            {
                this._animation_duration -= 1;
                update_animation();
            }
            if (this._loop_animation != null && (Graphics.frame_count % 2 == 0))
            {
                update_loop_animation();
                this._loop_animation_index += 1;
                this._loop_animation_index %= this._loop_animation.frame_max;
            }
            if (this._blink)
            {
                this._blink_count = (this._blink_count + 1) % 32;
                var alpha = 0;
                if (this._blink_count < 16)
                    alpha = (16 - this._blink_count) * 6;
                else
                    alpha = (this._blink_count - 16) * 6;

                this.color = Color.FromArgb((byte)alpha, 255, 255, 255);
            }
            _animations.Clear();
        }

        public Sprite(Viewport viewport = null)
            : base(viewport)
        {
            this._whiten_duration = 0;
            this._appear_duration = 0;
            this._escape_duration = 0;
            this._collapse_duration = 0;
            this._damage_duration = 0;
            this._animation_duration = 0;
            this._blink = false;
        }

        public override void dispose()
        {
            dispose_damage();
            dispose_animation();
            dispose_loop_animation();
            base.dispose();
        }




        public void dispose_damage()
        {
            if (this._damage_sprite != null)
            {
                this._damage_sprite.bitmap.dispose();
                this._damage_sprite.dispose();
                this._damage_sprite = null;
                this._damage_duration = 0;
            }
        }
        public void dispose_animation()
        {
            if (this._animation_sprites != null)
            {
                var sprite = this._animation_sprites[0];
                if (sprite != null)
                {
                    _reference_count[sprite.bitmap] -= 1;
                    if (_reference_count[sprite.bitmap] == 0)
                        sprite.bitmap.dispose();
                }
                foreach (var sprite1 in this._animation_sprites)
                    sprite1.dispose();
                this._animation_sprites = null;
                this._animation = null;
            }
        }
        public void dispose_loop_animation()
        {
            if (this._loop_animation_sprites != null)
            {
                var sprite = this._loop_animation_sprites[0];
                if (sprite != null)
                {
                    _reference_count[sprite.bitmap] -= 1;
                    if (_reference_count[sprite.bitmap] == 0)
                        sprite.bitmap.dispose();
                }
                foreach (var sprite1 in this._loop_animation_sprites)
                    sprite1.dispose();
                this._loop_animation_sprites = null;
                this._loop_animation = null;
            }
        }

        public void update_animation()
        {
            if (this._animation_duration > 0)
            {
                var frame_index = this._animation.frame_max - this._animation_duration;
                var cell_data = this._animation.frames[frame_index].cell_data;
                var position = this._animation.position;
                animation_set_sprites(this._animation_sprites, cell_data, position);
                foreach (var timing in this._animation.timings)
                    if (timing.frame == frame_index)
                        animation_process_timing(timing, this._animation_hit);
            }
            else
                dispose_animation();
        }
        public void update_loop_animation()
        {
            var frame_index = this._loop_animation_index;
            var cell_data = this._loop_animation.frames[frame_index].cell_data;
            var position = this._loop_animation.position;
            animation_set_sprites(this._loop_animation_sprites, cell_data, position);
            foreach (var timing in this._loop_animation.timings)
                if (timing.frame == frame_index)
                    animation_process_timing(timing, true);
        }
        public void animation_set_sprites(List<XP.Internal.Sprite> sprites, Table cell_data, int position)
        {
            for (var i = 0; i <= 15; i++)
            {
                var sprite = sprites[i];
                var pattern = cell_data[i, 0];
                if (sprite == null || pattern == null || pattern == -1)
                {
                    if (sprite != null)
                    {
                        sprite.visible = false;
                        continue;
                    }
                }
                sprite.visible = true;
                sprite.src_rect = new Rect(pattern % 5 * 192, pattern / 5 * 192, 192, 192);
                if (position == 3)
                {
                    if (this.viewport != null)
                    {
                        sprite.x = (int)(this.viewport.rect.Width / 2);
                        sprite.y = (int)(this.viewport.rect.Height - 160);
                    }
                    else
                    {
                        sprite.x = 320;
                        sprite.y = 240;
                    }
                }
                else
                {
                    sprite.x = this.x - this.ox + (int)this.src_rect.Width / 2;
                    sprite.y = this.y - this.oy + (int)this.src_rect.Height / 2;
                    if (position == 0)
                        sprite.y -= (int)this.src_rect.Height / 4;
                    if (position == 2)
                        sprite.y += (int)this.src_rect.Height / 4;
                }
                sprite.x += cell_data[i, 1];
                sprite.y += cell_data[i, 2];
                sprite.z = 2000;
                sprite.ox = 96;
                sprite.oy = 96;
                sprite.zoom_x = cell_data[i, 3] / 100.0;
                sprite.zoom_y = cell_data[i, 3] / 100.0;
                sprite.angle = cell_data[i, 4];
                sprite.mirror = (cell_data[i, 5] == 1);
                sprite.opacity = cell_data[i, 6] * this.opacity / 255.0;
                sprite.blend_type = cell_data[i, 7];
            }
        }

        public void animation_process_timing(RPG.Animation.Timing timing, bool hit)
        {
            if (
               (timing.condition == 0) ||
                        (timing.condition == 1 && hit == true) ||
                        (timing.condition == 2 && hit == false))
            {

                if (timing.se.name != "")
                {
                    var se = timing.se;
                    Audio.se_play("Audio/SE/" + se.name + ".wav", se.volume, se.pitch);
                }
                switch (timing.flash_scope)
                {
                    case 1:
                        this.flash(timing.flash_color, timing.flash_duration * 2 / 30.0); break;
                    case 2:
                        if (this.viewport != null)
                            this.viewport.flash(timing.flash_color, timing.flash_duration * 2);
                        break;
                    case 3:
                        this.flash(null, timing.flash_duration * 2 / 30.0);
                        break;
                }
            }
        }
        /// ????
        public override double x
        {
            get { return base.x; }
            set
            {
                var sx = value - this.x;
                if (sx != 0)
                {
                    if (this._animation_sprites != null)
                        for (var i = 0; i <= 15; i++)
                            this._animation_sprites[i].x += sx;

                    if (this._loop_animation_sprites != null)
                        for (var i = 0; i <= 15; i++)
                            this._loop_animation_sprites[i].x += sx;
                }


                base.x = value;
            }
        }
        public override double y
        {
            get { return base.y; }
            set
            {
                var sy = value - this.y;
                if (sy != 0)
                {
                    if (this._animation_sprites != null)
                        for (var i = 0; i <= 15; i++)
                            this._animation_sprites[i].y += sy;

                    if (this._loop_animation_sprites != null)
                        for (var i = 0; i <= 15; i++)
                            this._loop_animation_sprites[i].y += sy;
                }

                base.y = value;
            }
        }



        public XP.Internal.Sprite _damage_sprite { get; set; }
        public List<XP.Internal.Sprite> _animation_sprites { get; set; }

        public Animation _animation { get; set; }

        public bool _animation_hit { get; set; }

        public Animation _loop_animation { get; set; }

        public int _loop_animation_index { get; set; }

        public List<XP.Internal.Sprite> _loop_animation_sprites { get; set; }

        public int _blink_count { get; set; }
    }
}
