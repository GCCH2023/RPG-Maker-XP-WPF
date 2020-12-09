using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_Player
    //------------------------------------------------------------------------------
    // 　处理主角的类。事件启动的判定、以及地图的滚动等功能。
    // 本类的实例请参考 Global.game_player。
    //==============================================================================

    public class Game_Player : Game_Character
    {
        //--------------------------------------------------------------------------
        // ● 常量
        //--------------------------------------------------------------------------
        public static int CENTER_X = (320 - 16) * 4;  // 画面中央的 X 坐标 * 4
        public static int CENTER_Y = (240 - 16) * 4;  // 画面中央的 Y 坐标 * 4
        //--------------------------------------------------------------------------
        // ● 可以通行判定
        //     x : X 坐标
        //     y : Y 坐标
        //     d : 方向 (0,2,4,6,8)  ※ 0 = 全方向不能通行的情况判定 (跳跃用)
        //--------------------------------------------------------------------------
        public override bool is_passable(int x, int y, int d)
        {
            // 求得新的坐标
            var new_x = x + (d == 6 ? 1 : d == 4 ? -1 : 0);
            var new_y = y + (d == 2 ? 1 : d == 8 ? -1 : 0);
            // 坐标在地图外的情况下
            if (!Global.game_map.is_valid(new_x, new_y))
                // 不能通行
                return false;

            // 调试模式为 ON 并且 按下 CTRL 键的情况下
            if (Global.DEBUG && Input.is_press(Input.CTRL))
                // 可以通行
                return true;

            return base.is_passable(x, y, d);
        }
        //--------------------------------------------------------------------------
        // ● 以画面中央为基准设置地图的显示位置
        //--------------------------------------------------------------------------
        public void center(int x, int y)
        {
            var max_x = (Global.game_map.width - 20) * 128;
            var max_y = (Global.game_map.height - 15) * 128;
            Global.game_map.display_x = Math.Max(0, Math.Min(x * 128 - CENTER_X, max_x));
            Global.game_map.display_y = Math.Max(0, Math.Min(y * 128 - CENTER_Y, max_y));
        }
        //--------------------------------------------------------------------------
        // ● 向指定的位置移动
        //     x : X 坐标
        //     y : Y 坐标
        //--------------------------------------------------------------------------
        public override void moveto(int x, int y)
        {
            base.moveto(x, y);
            // 自连接
            center(x, y);
            // 生成遇敌计数
            make_encounter_count();
        }
        //--------------------------------------------------------------------------
        // ● 增加步数
        //--------------------------------------------------------------------------
        public override void increase_steps()
        {
            base.increase_steps();
            // 不是强制移动路线的场合
            if (!this.move_route_forcing)
            {
                // 增加步数
                Global.game_party.increase_steps();
                // 步数是偶数的情况下
                if (Global.game_party.steps % 2 == 0)
                    // 检查连续伤害
                    Global.game_party.check_map_slip_damage();
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取遇敌计数
        //--------------------------------------------------------------------------
        int _encounter_count;
        public int encounter_count
        {
            get
            {
                return this._encounter_count;
            }
            set { this._encounter_count = value; }
        }
        //--------------------------------------------------------------------------
        // ● 生成遇敌计数
        //--------------------------------------------------------------------------
        public void make_encounter_count()
        {
            // 两种颜色震动的图像
            if (Global.game_map.map_id != 0)
            {
                var n = Global.game_map.encounter_step;
                this.encounter_count = Global.rand(n) + Global.rand(n) + 1;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            // 同伴人数为 0 的情况下
            if (Global.game_party.actors.Count == 0)
            {
                // 清除角色的文件名及对像
                this.character_name = "";
                this.character_hue = 0;
                // 分支结束
                return;
            }
            // 获取带头的角色
            var actor = Global.game_party.actors[0];
            // 设置角色的文件名及对像
            this.character_name = actor.character_name;
            this.character_hue = actor.character_hue;
            // 初始化不透明度和合成方式
            this.opacity = 255;
            this.blend_type = 0;
        }
        //--------------------------------------------------------------------------
        // ● 同位置的事件启动判定
        //--------------------------------------------------------------------------
        public bool check_event_trigger_here(List<int?> triggers)
        {
            var result = false;
            // 事件执行中的情况下
            if (Global.game_system.map_interpreter.is_running)
                return result;

            // 全部事件的循环
            foreach (var event1 in Global.game_map.events.Values)
            {
                // 事件坐标与目标一致的情况下
                if (event1.x == this.x && event1.y == this.y && triggers.Contains(event1.trigger))
                    // 跳跃中以外的情况下、启动判定是同位置的事件
                    if (!event1.is_jumping && event1.is_over_trigger)
                    {
                        event1.start();
                        result = true;
                    }
            }
            return result;
        }
        //--------------------------------------------------------------------------
        // ● 正面事件的启动判定
        //--------------------------------------------------------------------------
        public bool check_event_trigger_there(List<int?> triggers)
        {
            var result = false;
            // 事件执行中的情况下
            if (Global.game_system.map_interpreter.is_running)
                return result;

            // 计算正面坐标
            var new_x = this.x + (this.direction == 6 ? 1 : this.direction == 4 ? -1 : 0);
            var new_y = this.y + (this.direction == 2 ? 1 : this.direction == 8 ? -1 : 0);
            // 全部事件的循环
            foreach (var event1 in Global.game_map.events.Values)
            {
                // 事件坐标与目标一致的情况下
                if (
                    event1.x == new_x && event1.y == new_y &&
                            triggers.Contains(event1.trigger))

                    // 跳跃中以外的情况下、启动判定是正面的事件
                    if (!event1.is_jumping && !event1.is_over_trigger)
                    {
                        event1.start();
                        result = true;
                    }
            }

            // 找不到符合条件的事件的情况下
            if (result == false)
            {
                // 正面的元件是计数器的情况下
                if (Global.game_map.is_counter(new_x, new_y))
                {
                    // 计算 1 元件里侧的坐标
                    new_x += (this.direction == 6 ? 1 : this.direction == 4 ? -1 : 0);
                    new_y += (this.direction == 2 ? 1 : this.direction == 8 ? -1 : 0);
                    // 全事件的循环
                    foreach (var event1 in Global.game_map.events.Values)
                    {
                        // 事件坐标与目标一致的情况下
                        if (
                           event1.x == new_x && event1.y == new_y &&
                                        triggers.Contains(event1.trigger))
                        {

                            // 跳跃中以外的情况下、启动判定是正面的事件
                            if (!event1.is_jumping && !event1.is_over_trigger)
                            {
                                event1.start();
                                result = true;
                            }
                        }
                    }
                }
            }
            return result;
        }
        //--------------------------------------------------------------------------
        // ● 接触事件启动判定
        //--------------------------------------------------------------------------
        public override bool check_event_trigger_touch(int x, int y)
        {
            var result = false;
            // 事件执行中的情况下
            if (Global.game_system.map_interpreter.is_running)
                return result;

            // 全事件的循环
            foreach (var event1 in Global.game_map.events.Values)
            {
                // 事件坐标与目标一致的情况下
                if (event1.x == x && event1.y == y && (event1.trigger == 1 || event1.trigger == 2))
                {
                    // 跳跃中以外的情况下、启动判定是正面的事件
                    if (!event1.is_jumping && !event1.is_over_trigger)
                    {
                        event1.start();
                        result = true;
                    }
                }
            }
            return result;
        }
        //--------------------------------------------------------------------------
        // ● 画面更新
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 本地变量记录移动信息
            var last_moving = is_moving;
            // 移动中、事件执行中、强制移动路线中、
            // 信息窗口一个也不显示的时候
            if (!(is_moving || Global.game_system.map_interpreter.is_running ||
                              this.move_route_forcing || Global.game_temp.message_window_showing))

                // 如果方向键被按下、主角就朝那个方向移动
                switch (Input.dir4())
                {
                    case 2:
                        move_down(); break;
                    case 4:
                        move_left(); break;
                    case 6:
                        move_right(); break;
                    case 8:
                        move_up(); break;
                }

            // 本地变量记忆坐标
            var last_real_x = this.real_x;
            var last_real_y = this.real_y;
            base.update();
            // 角色向下移动、画面上的位置在中央下方的情况下
            if (this.real_y > last_real_y && this.real_y - Global.game_map.display_y > CENTER_Y)
                // 画面向下卷动
                Global.game_map.scroll_down((int)(this.real_y - last_real_y));

            // 角色向左移动、画面上的位置在中央左方的情况下
            if (this.real_x < last_real_x && this.real_x - Global.game_map.display_x < CENTER_X)
                // 画面向左卷动
                Global.game_map.scroll_left((int)(last_real_x - this.real_x));

            // 角色向右移动、画面上的位置在中央右方的情况下
            if (this.real_x > last_real_x && this.real_x - Global.game_map.display_x > CENTER_X)
                // 画面向右卷动
                Global.game_map.scroll_right((int)(this.real_x - last_real_x));

            // 角色向上移动、画面上的位置在中央上方的情况下
            if (this.real_y < last_real_y && this.real_y - Global.game_map.display_y < CENTER_Y)
                // 画面向上卷动
                Global.game_map.scroll_up((int)(last_real_y - this.real_y));

            // 不在移动中的情况下
            if (!is_moving)
            {
                // 上次主角移动中的情况
                if (last_moving)
                {
                    // 与同位置的事件接触就判定为事件启动
                    var result = check_event_trigger_here(new List<int?>() { 1, 2 });
                    // 没有可以启动的事件的情况下
                    if (result == false)
                    {
                        // 调试模式为 ON 并且按下 CTRL 键的情况下除外
                        if (!(Global.DEBUG && Input.is_press(Input.CTRL)))
                        {
                            // 遇敌计数下降
                            if (this.encounter_count > 0)
                                this.encounter_count -= 1;
                        }
                    }
                }
                // 按下 C 键的情况下
                if (Input.is_trigger(Input.C))
                {
                    // 判定为同位置以及正面的事件启动
                    check_event_trigger_here(new List<int?>() { 0 });
                    check_event_trigger_there(new List<int?>() { 0, 1, 2 });
                }
            }
        }
    }
}
