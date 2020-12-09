using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_Character (分割定义 3)
    //------------------------------------------------------------------------------
    // 　处理角色的类。本类作为 Game_Player 类与 Game_Event
    // 类的超级类使用。
    //==============================================================================

    public partial class Game_Character
    {
        //--------------------------------------------------------------------------
        // ● 向下移动
        //     turn_enabled : 本场地位置更改许可标志
        //--------------------------------------------------------------------------
        public void move_down(bool turn_enabled = true)
        {
            // 面向下
            if (turn_enabled)
                turn_down();

            // 可以通行的场合
            if (is_passable(this.x, this.y, 2))
            {
                // 面向下
                turn_down();
                // 更新坐标
                this.y += 1;
                // 增加步数
                increase_steps();
            }
            // 不能通行的情况下
            else
                // 接触事件的启动判定
                check_event_trigger_touch(this.x, this.y + 1);
        }
        //--------------------------------------------------------------------------
        // ● 向左移动
        //     turn_enabled : 本场地位置更改许可标志
        //--------------------------------------------------------------------------
        public void move_left(bool turn_enabled = true)
        {
            // 面向左
            if (turn_enabled)
                turn_left();

            // 可以通行的情况下
            if (is_passable(this.x, this.y, 4))
            {
                // 面向左
                turn_left();
                // 更新坐标
                this.x -= 1;
                // 增加步数
                increase_steps();
            }
            // 不能通行的情况下
            else
                // 接触事件的启动判定
                check_event_trigger_touch(this.x - 1, this.y);
        }
        //--------------------------------------------------------------------------
        // ● 向右移动
        //     turn_enabled : 本场地位置更改许可标志
        //--------------------------------------------------------------------------
        public void move_right(bool turn_enabled = true)
        {
            // 面向右
            if (turn_enabled)
                turn_right();

            // 可以通行的场合
            if (is_passable(this.x, this.y, 6))
            {
                // 面向右
                turn_right();
                // 更新坐标
                this.x += 1;
                // 增加步数
                increase_steps();
            }
            // 不能通行的情况下
            else
                // 接触事件的启动判定
                check_event_trigger_touch(this.x + 1, this.y);
        }
        //--------------------------------------------------------------------------
        // ● 向上移动
        //     turn_enabled : 本场地位置更改许可标志
        //--------------------------------------------------------------------------
        public void move_up(bool turn_enabled = true)
        {
            // 面向上
            if (turn_enabled)
                turn_up();

            // 可以通行的情况下
            if (is_passable(this.x, this.y, 8))
            {
                // 面向上
                turn_up();
                // 更新坐标
                this.y -= 1;
                // 歩数増加
                increase_steps();
            }
            // 不能通行的情况下
            else
                // 接触事件的启动判定
                check_event_trigger_touch(this.x, this.y - 1);
        }
        //--------------------------------------------------------------------------
        // ● 向左下移动
        //--------------------------------------------------------------------------
        public void move_lower_left()
        {
            // 没有固定面向的场合
            if (!this.direction_fix)
                // 朝向是右的情况下适合的面是左面、朝向是上的情况下适合的面是下面
                this.direction = (this.direction == 6 ? 4 : this.direction == 8 ? 2 : this.direction);

            // 下→左、左→下 的通道可以通行的情况下
            if (
               (is_passable(this.x, this.y, 2) && is_passable(this.x, this.y + 1, 4)) ||
                      (is_passable(this.x, this.y, 4) && is_passable(this.x - 1, this.y, 2)))
            {

                // 更新坐标
                this.x -= 1;
                this.y += 1;
                // 增加步数
                increase_steps();
            }
        }
        //--------------------------------------------------------------------------
        // ● 向右下移动
        //--------------------------------------------------------------------------
        public void move_lower_right()
        {
            // 没有固定面向的场合
            if (!this.direction_fix)
                // 朝向是右的情况下适合的面是左面、朝向是上的情况下适合的面是下面
                this.direction = (this.direction == 4 ? 6 : this.direction == 8 ? 2 : this.direction);

            // 下→右、右→下 的通道可以通行的情况下
            if (
               (is_passable(this.x, this.y, 2) && is_passable(this.x, this.y + 1, 6)) ||
                      (is_passable(this.x, this.y, 6) && is_passable(this.x + 1, this.y, 2)))
            {

                // 更新坐标
                this.x += 1;
                this.y += 1;
                // 增加步数
                increase_steps();
            }
        }
        //--------------------------------------------------------------------------
        // ● 向左上移动
        //--------------------------------------------------------------------------
        public void move_upper_left()
        {
            // 没有固定面向的场合
            if (!this.direction_fix)
                // 朝向是右的情况下适合的面是左面、朝向是上的情况下适合的面是下面
                this.direction = (this.direction == 6 ? 4 : this.direction == 2 ? 8 : this.direction);
            // 上→左、左→上 的通道可以通行的情况下
            if (
               (is_passable(this.x, this.y, 8) && is_passable(this.x, this.y - 1, 4)) ||
                      (is_passable(this.x, this.y, 4) && is_passable(this.x - 1, this.y, 8)))
            {

                // 更新坐标
                this.x -= 1;
                this.y -= 1;
                // 增加步数
                increase_steps();
            }
        }
        //--------------------------------------------------------------------------
        // ● 向右上移动
        //--------------------------------------------------------------------------
        public void move_upper_right()
        {
            // 没有固定面向的场合
            if (!this.direction_fix)
                // 朝向是右的情况下适合的面是左面、朝向是上的情况下适合的面是下面
                this.direction = (this.direction == 4 ? 6 : this.direction == 2 ? 8 : this.direction);

            // 上→右、右→上 的通道可以通行的情况下
            if (
                (is_passable(this.x, this.y, 8) && is_passable(this.x, this.y - 1, 6)) ||
                      (is_passable(this.x, this.y, 6) && is_passable(this.x + 1, this.y, 8)))
            {

                // 更新坐标
                this.x += 1;
                this.y -= 1;
                // 增加步数
                increase_steps();
            }
        }
        //--------------------------------------------------------------------------
        // ● 随机移动
        //--------------------------------------------------------------------------
        public void move_random()
        {
            switch (Global.rand(4))
            {
                case 0:  // 向下移动
                    move_down(false); break;
                case 1:  // 向左移动
                    move_left(false); break;
                case 2:  // 向右移动
                    move_right(false); break;
                case 3:  // 向上移动
                    move_up(false); break;
            }
        }
        //--------------------------------------------------------------------------
        // ● 接近主角
        //--------------------------------------------------------------------------
        public void move_toward_player()
        {
            // 求得与主角的坐标差
            var sx = this.x - Global.game_player.x;
            var sy = this.y - Global.game_player.y;
            // 坐标相等情况下
            if (sx == 0 && sy == 0)
                return;

            // 求得差的绝对值
            var abs_sx = Math.Abs(sx);
            var abs_sy = Math.Abs(sy);
            // 横距离与纵距离相等的情况下
            if (abs_sx == abs_sy)
            {
                // 随机将边数增加 1
                if (Global.rand(2) == 0)
                    abs_sx += 1;
                else
                    abs_sy += 1;
            }

            // 横侧距离长的情况下
            if (abs_sx > abs_sy)
            {
                // 左右方向优先。向主角移动
                if (sx > 0)
                    move_left();
                else
                    move_right();

                if (!is_moving && sy != 0)
                {
                    if (sy > 0)
                        move_up();
                    else
                        move_down();
                }
            }
            // 竖侧距离长的情况下
            else
            {
                // 上下方向优先。向主角移动
                if (sy > 0)
                    move_up();
                else
                    move_down();

                if (!is_moving && sx != 0)
                {
                    if (sx > 0)
                        move_left();
                    else
                        move_right();
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 远离主角
        //--------------------------------------------------------------------------
        public void move_away_from_player()
        {
            // 求得与主角的坐标差
            var sx = this.x - Global.game_player.x;
            var sy = this.y - Global.game_player.y;
            // 坐标相等情况下
            if (sx == 0 && sy == 0)
                return;

            // 求得差的绝对值
            var abs_sx = Math.Abs(sx);
            var abs_sy = Math.Abs(sy);
            // 横距离与纵距离相等的情况下
            if (abs_sx == abs_sy)
                // 随机将边数增加 1
                if (Global.rand(2) == 0)
                    abs_sx += 1;
                else
                    abs_sy += 1;
            // 横侧距离长的情况下
            if (abs_sx > abs_sy)
            {
                // 左右方向优先。远离主角移动
                if (sx > 0)
                    move_right();
                else
                    move_left();
                if (!is_moving && sy != 0)
                {
                    if (sy > 0)
                        move_down();
                    else
                        move_up();
                }
            }
            // 竖侧距离长的情况下
            else
            {
                // 上下方向优先。远离主角移动
                if (sy > 0)
                    move_down();
                else
                    move_up();
                if (!is_moving && sx != 0)
                {
                    if (sx > 0)
                        move_right();
                    else
                        move_left();
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 前进一步
        //--------------------------------------------------------------------------
        public void move_forward()
        {
            switch (this.direction)
            {
                case 2:
                    move_down(false); break;
                case 4:
                    move_left(false); break;
                case 6:
                    move_right(false); break;
                case 8:
                    move_up(false); break;
            }
        }
        //--------------------------------------------------------------------------
        // ● 后退一步
        //--------------------------------------------------------------------------
        public void move_backward()
        {
            // 记忆朝向固定信息
            var last_direction_fix = this.direction_fix;
            // 强制固定朝向
            this.direction_fix = true;
            // 朝向分支
            switch (this.direction)
            {
                case 2:  // 下
                    move_up(false); break;
                case 4:  // 左
                    move_right(false); break;
                case 6:  // 右
                    move_left(false); break;
                case 8:  // 上
                    move_down(false); break;
            }

            // 还原朝向固定信息
            this.direction_fix = last_direction_fix;
        }
        //--------------------------------------------------------------------------
        // ● 跳跃
        //     x_plus : X 坐标增加值
        //     y_plus : Y 坐标增加值
        //--------------------------------------------------------------------------
        public void jump(int x_plus, int y_plus)
        {
            // 增加值不是 (0,0) 的情况下
            if (x_plus != 0 || y_plus != 0)
            {
                // 横侧距离长的情况下
                if (Math.Abs(x_plus) > Math.Abs(y_plus))
                {
                    // 变更左右方向
                    if (x_plus < 0)
                        turn_left();
                    else
                        turn_right();
                }
                // 竖侧距离长的情况下
                else
                {
                    // 变更上下方向
                    if (y_plus < 0)
                        turn_up();
                    else
                        turn_down();
                }
            }
            // 计算新的坐标
            var new_x = this.x + x_plus;
            var new_y = this.y + y_plus;
            // 增加值为 (0,0) 的情况下、跳跃目标可以通行的场合
            if ((x_plus == 0 && y_plus == 0) || is_passable(new_x, new_y, 0))
            {
                // 矫正姿势
                straighten();
                // 更新坐标
                this.x = new_x;
                this.y = new_y;
                // 距计算距离
                var distance = Math.Round(Math.Sqrt(x_plus * x_plus + y_plus * y_plus));
                // 设置跳跃记数
                this.jump_peak = 10 + distance - this.move_speed;
                this.jump_count = this.jump_peak * 2;
                // 清除停止记数信息
                this.stop_count = 0;
            }
        }
        //--------------------------------------------------------------------------
        // ● 面向向下
        //--------------------------------------------------------------------------
        public void turn_down()
        {
            if (!this.direction_fix)
            {
                this.direction = 2;
                this.stop_count = 0;
            }
        }
        //--------------------------------------------------------------------------
        // ● 面向向左
        //--------------------------------------------------------------------------
        public void turn_left()
        {
            if (!this.direction_fix)
            {
                this.direction = 4;
                this.stop_count = 0;
            }
        }
        //--------------------------------------------------------------------------
        // ● 面向向右
        //--------------------------------------------------------------------------
        public void turn_right()
        {
            if (!this.direction_fix)
            {
                this.direction = 6;
                this.stop_count = 0;
            }
        }
        //--------------------------------------------------------------------------
        // ● 面向向上
        //--------------------------------------------------------------------------
        public void turn_up()
        {
            if (!this.direction_fix)
            {
                this.direction = 8;
                this.stop_count = 0;
            }
        }
        //--------------------------------------------------------------------------
        // ● 向右旋转 90 度
        //--------------------------------------------------------------------------
        public void turn_right_90()
        {
            switch (this.direction)
            {
                case 2:
                    turn_left(); break;
                case 4:
                    turn_up(); break;
                case 6:
                    turn_down(); break;
                case 8:
                    turn_right(); break;
            }
        }
        //--------------------------------------------------------------------------
        // ● 向左旋转 90 度
        //--------------------------------------------------------------------------
        public void turn_left_90()
        {
            switch (this.direction)
            {
                case 2:
                    turn_right(); break;
                case 4:
                    turn_down(); break;
                case 6:
                    turn_up(); break;
                case 8:
                    turn_left(); break;
            }
        }
        //--------------------------------------------------------------------------
        // ● 旋转 180 度
        //--------------------------------------------------------------------------
        public void turn_180()
        {
            switch (this.direction)
            {
                case 2:
                    turn_up(); break;
                case 4:
                    turn_right(); break;
                case 6:
                    turn_left(); break;
                case 8:
                    turn_down(); break;
            }
        }
        //--------------------------------------------------------------------------
        // ● 从右向左旋转 90 度
        //--------------------------------------------------------------------------
        public void turn_right_or_left_90()
        {
            if (Global.rand(2) == 0)
                turn_right_90();
            else
                turn_left_90();
        }
        //--------------------------------------------------------------------------
        // ● 随机变换方向
        //--------------------------------------------------------------------------
        public void turn_random()
        {
            switch (Global.rand(4))
            {
                case 0:
                    turn_up(); break;
                case 1:
                    turn_right(); break;
                case 2:
                    turn_left(); break;
                case 3:
                    turn_down(); break;
            }
        }
        //--------------------------------------------------------------------------
        // ● 接近主角的方向
        //--------------------------------------------------------------------------
        public void turn_toward_player()
        {
            // 求得与主角的坐标差
            var sx = this.x - Global.game_player.x;
            var sy = this.y - Global.game_player.y;
            // 坐标相等的场合下
            if (sx == 0 && sy == 0)
                return;

            // 横侧距离长的情况下
            if (Math.Abs(sx) > Math.Abs(sy))
            {
                // 将左右方向变更为朝向主角的方向
                if (sx > 0)
                    turn_left();
                else
                    turn_right();
            }
            // 竖侧距离长的情况下
            else
            {
                // 将上下方向变更为朝向主角的方向
                if (sy > 0)
                    turn_up();
                else
                    turn_down();
            }
        }
        //--------------------------------------------------------------------------
        // ● 背向主角的方向
        //--------------------------------------------------------------------------
        public void turn_away_from_player()
        {
            // 求得与主角的坐标差
            var sx = this.x - Global.game_player.x;
            var sy = this.y - Global.game_player.y;
            // 坐标相等的场合下
            if (sx == 0 && sy == 0)
                return;

            // 横侧距离长的情况下
            if (Math.Abs(sx) > Math.Abs(sy))
            {
                // 将左右方向变更为背离主角的方向
                if (sx > 0)
                    turn_right();
                else
                    turn_left();
            }
            // 竖侧距离长的情况下
            else
            {
                // 将上下方向变更为背离主角的方向
                if (sy > 0)

                    turn_down();
                else
                    turn_up();
            }
        }

        public virtual bool check_event_trigger_touch(int x, int y)
        {
            return false;
        }
    }
}
