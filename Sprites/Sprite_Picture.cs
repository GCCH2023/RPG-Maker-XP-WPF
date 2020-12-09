using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Sprite_Picture
    //------------------------------------------------------------------------------
    // 　显示图片用的活动块。Game_Picture 类的实例监视、
    // 活动块状态的自动变化。
    //==============================================================================

    public class Sprite_Picture : XP.Internal.Sprite
    {
        public Game_Picture picture { get; set; }
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     viewport : 显示端口
        //     picture  : 图片 (Game_Picture)
        //--------------------------------------------------------------------------
        public Sprite_Picture(Viewport viewport, Game_Picture picture) :
            base(viewport)
        {
            this.picture = picture;
            update();
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
        // ● 更新画面
        //--------------------------------------------------------------------------
        public override void update(){
    base.update();
    // 图片的文件名与当前的情况有差异的情况下
    if( this.picture_name != this.picture.name){
      // 将文件名记忆到实例变量
      this.picture_name = this.picture.name;
      // 文件名不为空的情况下
      if( this.picture_name != ""){
        // 获取图片图形
        this.bitmap = RPG.Cache.picture(this.picture_name);
      }
    }
    // 文件名是空的情况下
    if( this.picture_name == ""){
      // 将活动块设置为不可见
      this.visible = false;
      return;
    }
    // 将活动块设置为可见
    this.visible = true;
    // 设置传送原点
    if( this.picture.origin == 0){
      this.ox = 0;
      this.oy = 0;
    }
    else
    {
      this.ox = this.bitmap.width / 2;
      this.oy = this.bitmap.height / 2;
    }
    // 设置活动块的坐标
    this.x = (int)this.picture.x;
    this.y = (int)this.picture.y;
    this.z = (int)this.picture.number;
    // 设置放大率、不透明度、合成方式
    this.zoom_x = this.picture.zoom_x / 100.0;
    this.zoom_y = this.picture.zoom_y / 100.0;
    this.opacity = this.picture.opacity;
    this.blend_type = this.picture.blend_type;
    // 设置旋转角度、色调
    this.angle = this.picture.angle;
    this.tone = this.picture.tone;
  }

        public string picture_name { get; set; }
    }

}
