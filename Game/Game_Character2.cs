using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
  //==============================================================================
// ■ Game_Character (分割定义 2)
//------------------------------------------------------------------------------
// 　处理角色的类。本类作为 Game_Player 类与 Game_Event
// 类的超级类使用。
//==============================================================================

    public partial class Game_Character
    {
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public virtual void update()
        {
            // 跳跃中、移动中、停止中的分支
            if (is_jumping)
                update_jump();
            else if (is_moving)
                update_move();
            else
                update_stop();

            // 动画计数超过最大值的情况下
            // ※最大值等于基本值减去移动速度 * 1 的值
            if (this.anime_count > 18 - this.move_speed * 2)
            {
                // 停止动画为 OFF 并且在停止中的情况下
                if (!this.step_anime && this.stop_count > 0)
                    // 还原为原来的图形
                    this.pattern = this.original_pattern;
                // 停止动画为 ON 并且在移动中的情况下
                else
                    // 更新图形
                    this.pattern = (this.pattern + 1) % 4;

                // 清除动画计数
                this.anime_count = 0;
            }
            // 等待中的情况下
            if (this.wait_count > 0)
            {
                // 减少等待计数
                this.wait_count -= 1;
                return;
            }
            // 强制移动路线的场合
            if (this.move_route_forcing)
            {
                // 自定义移动
                move_type_custom();
                return;
            }
            // 事件执行待机中并且为锁定状态的情况下
            if (this.starting || is_lock)
                // 不做规则移动
                return;

            // 如果停止计数超过了一定的值(由移动频度算出)
            if (this.stop_count > (40 - this.move_frequency * 2) * (6 - this.move_frequency))
            {
                // 移动类型分支
                switch (this.move_type)
                {
                    case 1:  // 随机
                        move_type_random(); break;
                    case 2:  // 接近
                        move_type_toward_player(); break;
                    case 3:  // 自定义
                        move_type_custom(); break;
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 更新画面 (跳跃)
        //--------------------------------------------------------------------------
        public void update_jump()
        {
            // 跳跃计数减 1
            this.jump_count -= 1;
            // 计算新坐标
            this.real_x = (this.real_x * this.jump_count + this.x * 128) / (this.jump_count + 1);
            this.real_y = (this.real_y * this.jump_count + this.y * 128) / (this.jump_count + 1);
        }
        //--------------------------------------------------------------------------
        // ● 更新画面 (移动)
        //--------------------------------------------------------------------------
        public void update_move()
        {
            // 移动速度转换为地图坐标系的移动距离
            var distance = (int)Math.Pow(2, this.move_speed);
            // 理论坐标在实际坐标下方的情况下
            if (this.y * 128 > this.real_y)
                // 向下移动
                this.real_y = Math.Min(this.real_y + distance, this.y * 128);

            // 理论坐标在实际坐标左方的情况下
            if (this.x * 128 < this.real_x)
                // 向左移动
                this.real_x = Math.Max(this.real_x - distance, this.x * 128);

            // 理论坐标在实际坐标右方的情况下
            if (this.x * 128 > this.real_x)
                // 向右移动
                this.real_x = Math.Min(this.real_x + distance, this.x * 128);

            // 理论坐标在实际坐标上方的情况下
            if (this.y * 128 < this.real_y)
                // 向上移动
                this.real_y = Math.Max(this.real_y - distance, this.y * 128);

            // 移动时动画为 ON 的情况下
            if (this.walk_anime)
                // 动画计数增加 1.5
                this.anime_count += 1.5;
            // 移动时动画为 OFF、停止时动画为 ON 的情况下
            else if (this.step_anime)
                // 动画计数增加 1
                this.anime_count += 1;
        }
        //--------------------------------------------------------------------------
        // ● 更新画面 (停止)
        //--------------------------------------------------------------------------
        public void update_stop()
        {
            // 停止时动画为 ON 的情况下
            if (this.step_anime)
                // 动画计数增加 1
                this.anime_count += 1;
            // 停止时动画为 OFF 并且、现在的图像与原来的不同的情况下
            else if (this.pattern != this.original_pattern)
                // 动画计数增加 1.5
                this.anime_count += 1.5;

            // 事件执行待机中并且不是锁定状态的情况下
            // ※锁定、处理成立刻停止执行中的事件
            if (!this.starting || is_lock)
                // 停止计数增加 1
                this.stop_count += 1;
        }
        //--------------------------------------------------------------------------
        // ● 移动类型 : 随机
        //--------------------------------------------------------------------------
        public void move_type_random()
        {
            // 随机 0～5 的分支
            switch (Global.rand(6))
            {
                case 0:
                case 1:
                case 2:
                case 3:  // 随机
                    move_random(); break;
                case 4:  // 前进一步
                    move_forward(); break;
                case 5:  // 暂时停止
                    this.stop_count = 0; break;
            }
        }
        //--------------------------------------------------------------------------
        // ● 移动类型 : 接近
        //--------------------------------------------------------------------------
        public void move_type_toward_player()
        {
            // 求得与主角坐标的差
            var sx = this.x - Global.game_player.x;
            var sy = this.y - Global.game_player.y;
            // 求得差的绝对值
            var abs_sx = sx > 0 ? sx : -sx;
            var abs_sy = sy > 0 ? sy : -sy;
            // 如果纵横共计离开 20 个元件
            if (sx + sy >= 20)
            {
                // 随机
                move_random();
                return;
            }

            // 随机 0～5 的分支
            switch (Global.rand(6))
            {
                case 0:
                case 1:
                case 2:
                case 3:  // 接近主角
                    move_toward_player(); break;
                case 4:  // 随机
                    move_random(); break;
                case 5:  // 前进一步
                    move_forward(); break;
            }
        }
        //--------------------------------------------------------------------------
        // ● 移动类型 : 自定义
        //--------------------------------------------------------------------------
        public void move_type_custom()
    {
      // 如果不是停止中就中断
      if( is_jumping || is_moving)
        return;

      // 如果在移动指令列表最后结束还没到达就循环执行
      while (this.move_route_index < this.move_route.list.Count)
      { // 获取移动指令
          var command = this.move_route.list[this.move_route_index];
          // 指令编号 0 号 (列表最后) 的情况下
          if (command.code == 0)
          {
              // 选项 [反复动作] 为 ON 的情况下
              if (this.move_route.repeat)
                  // 还原为移动路线的最初索引
                  this.move_route_index = 0;

              // 选项 [反复动作] 为 OFF 的情况下
              if (!this.move_route.repeat)
              {
                  // 强制移动路线的场合
                  if (this.move_route_forcing && !this.move_route.repeat)
                  {
                      // 强制解除移动路线
                      this.move_route_forcing = false;
                      // 还原为原始的移动路线
                      this.move_route = this.original_move_route;
                      this.move_route_index = this.original_move_route_index;
                      this.original_move_route = null;
                  }
                  // 清除停止计数
                  this.stop_count = 0;
              }
              return;
          }
          // 移动系指令 (向下移动～跳跃) 的情况下
          if (command.code <= 14)
          {  // 命令编号分支
              switch (command.code)
              {
                  case 1:  // 向下移动
                      move_down(); break;
                  case 2:  // 向左移动
                      move_left(); break;
                  case 3:  // 向右移动
                      move_right(); break;
                  case 4:  // 向上移动
                      move_up(); break;
                  case 5:  // 向左下移动
                      move_lower_left(); break;
                  case 6:  // 向右下移动
                      move_lower_right(); break;
                  case 7:  // 向左上移动
                      move_upper_left(); break;
                  case 8:  // 向右上
                      move_upper_right(); break;
                  case 9:  // 随机移动
                      move_random(); break;
                  case 10:  // 接近主角
                      move_toward_player(); break;
                  case 11:  // 远离主角
                      move_away_from_player(); break;
                  case 12:  // 前进一步
                      move_forward(); break;
                  case 13:  // 后退一步
                      move_backward(); break;
                  case 14:  // 跳跃
                      jump((int)command.parameters[0], (int)command.parameters[1]);
                      break;
              }
              // 选项 [无视无法移动的情况] 为 OFF 、移动失败的情况下
              if (!this.move_route.skippable && !is_moving && !is_jumping)
                  return;

              this.move_route_index += 1;
              return;
          }

          // 等待的情况下
          if (command.code == 15)
          {
              // 设置等待计数
              this.wait_count = (int)command.parameters[0] * 2 - 1;
              this.move_route_index += 1;
              return;
          }
          // 朝向变更系指令的情况下
          if (command.code >= 16 && command.code <= 26)
          {
              // 命令编号分支
              switch (command.code)
              {
                  case 16:  // 面向下
                      turn_down(); break;
                  case 17:  // 面向左
                      turn_left(); break;
                  case 18:  // 面向右
                      turn_right(); break;
                  case 19:  // 面向上
                      turn_up(); break;
                  case 20:  // 向右转 90 度
                      turn_right_90(); break;
                  case 21:  // 向左转 90 度
                      turn_left_90(); break;
                  case 22:  // 旋转 180 度
                      turn_180(); break;
                  case 23:  // 从右向左转 90 度
                      turn_right_or_left_90(); break;
                  case 24:  // 随机变换方向
                      turn_random(); break;
                  case 25:  // 面向主角的方向
                      turn_toward_player(); break;
                  case 26:  // 背向主角的方向
                      turn_away_from_player(); break;
              }
              this.move_route_index += 1;
              return;
          }
          // 其它指令的场合
          if (command.code >= 27)
          { // 命令编号分支
              switch (command.code)
              {
                  case 27:  // 开关 ON
                      Global.game_switches[(int)command.parameters[0]] = true;
                      Global.game_map.need_refresh = true;
                      break;
                  case 28:  // 开关 OFF
                      Global.game_switches[(int)command.parameters[0]] = false;
                      Global.game_map.need_refresh = true;
                      break;
                  case 29:  // 更改移动速度
                      this.move_speed = (int)command.parameters[0]; break;
                  case 30:  // 更改移动频度
                      this.move_frequency = (int)command.parameters[0]; break;
                  case 31:  // 移动时动画 ON
                      this.walk_anime = true; break;
                  case 32:  // 移动时动画 OFF
                      this.walk_anime = false; break;
                  case 33:  // 停止时动画 ON
                      this.step_anime = true; break;
                  case 34:  // 停止时动画 OFF
                      this.step_anime = false; break;
                  case 35:  // 朝向固定 ON
                      this.direction_fix = true; break;
                  case 36:  // 朝向固定 OFF
                      this.direction_fix = false; break;
                  case 37:  // 穿透 ON
                      this.through = true; break;
                  case 38:  // 穿透 OFF
                      this.through = false; break;
                  case 39:  // 在最前面显示 ON
                      this.always_on_top = true; break;
                  case 40:  // 在最前面显示 OFF
                      this.always_on_top = false; break;
                  case 41:  // 更改图形
                      this.tile_id = 0;
                      this.character_name = (string)command.parameters[0];
                      this.character_hue = (int)command.parameters[1];
                      if (this.original_direction != (int)command.parameters[2])
                      {
                          this.direction = (int)command.parameters[2];
                          this.original_direction = this.direction;
                          this.prelock_direction = 0;
                      }
                      if (this.original_pattern != (int)command.parameters[3])
                      {
                          this.pattern = (int)command.parameters[3];
                          this.original_pattern = this.pattern;
                      }
                      break;
                  case 42:  // 更改不透明度
                      this.opacity = (int)command.parameters[0]; break;
                  case 43:  // 更改合成方式
                      this.blend_type = (int)command.parameters[0]; break;
                  case 44:  // 演奏 SE
                      Global.game_system.se_play((RPG.AudioFile)command.parameters[0]); break;
                  case 45:  // 脚本
                      //result = eval(command.parameters[0]); break;
                      throw new Exception("未实现脚本动态执行");
              }
              this.move_route_index += 1;
          }
      }
      }
        //--------------------------------------------------------------------------
        // ● 增加步数
        //--------------------------------------------------------------------------
        public virtual void increase_steps()
        {
            // 清除停止步数
            this.stop_count = 0;
        }

        public virtual void refresh()
        {

        }

        public virtual bool starting { get; set; }
    }
}
