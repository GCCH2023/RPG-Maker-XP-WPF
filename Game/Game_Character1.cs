using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_Character (分割定义 1)
    //------------------------------------------------------------------------------
    // 　处理角色的类。本类作为 Game_Player 类与 Game_Event
    // 类的超级类使用。
    //==============================================================================

    public partial class Game_Character
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public int id;         // ID
        public int x;         // 地图 X 坐标 (理论坐标)
        public int y;         // 地图 Y 坐标 (理论坐标)
        public double real_x;            // 地图 X 坐标 (实际坐标 * 128)
        public double real_y;            // 地图 Y 坐标 (实际坐标 * 128)
        public int tile_id;           // 元件 ID  (0 为无效)
        public string character_name;         // 角色 文件名
        public int character_hue;      // 角色 色相
        public int opacity;// 不透明度
        public int blend_type;    // 合成方式
        public int direction;        // 朝向
        // 图案
        private int _pattern;

        public int pattern
        {
            get { return _pattern; }
            set { _pattern = value; }
        }
        public bool move_route_forcing;       // 移动路线强制标志
        public bool through;           // 穿透
        public int animation_id;           // 动画 ID
        public bool transparent;         // 透明状态
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Game_Character()
        {
            ;
            this.id = 0;
            this.x = 0;
            this.y = 0;
            this.real_x = 0;
            this.real_y = 0;
            this.tile_id = 0;
            this.character_name = "";
            this.character_hue = 0;
            this.opacity = 255;
            this.blend_type = 0;
            this.direction = 2;
            this.pattern = 0;
            this.move_route_forcing = false;
            this.through = false;
            this.animation_id = 0;
            this.transparent = false;
            this.original_direction = 2;
            this.original_pattern = 0;
            this.move_type = 0;
            this.move_speed = 4;
            this.move_frequency = 6;
            this.move_route = null;
            this.move_route_index = 0;
            this.original_move_route = null;
            this.original_move_route_index = 0;
            this.walk_anime = true;
            this.step_anime = false;
            this.direction_fix = false;
            this.always_on_top = false;
            this.anime_count = 0;
            this.stop_count = 0;
            this.jump_count = 0;
            this.jump_peak = 0;
            this.wait_count = 0;
            this.locked = false;
            this.prelock_direction = 0;
        }
        //--------------------------------------------------------------------------
        // ● 移动中判定
        //--------------------------------------------------------------------------
        public bool is_moving
        {
            get
            {
                // 如果在移动中理论坐标与实际坐标不同
                return (this.real_x != this.x * 128 || this.real_y != this.y * 128);
            }
        }
        //--------------------------------------------------------------------------
        // ● 跳跃中判定
        //--------------------------------------------------------------------------
        public bool is_jumping
        {
            get
            {
                // 如果跳跃中跳跃点数比 0 大
                return this.jump_count > 0;
            }
        }
        //--------------------------------------------------------------------------
        // ● 矫正姿势
        //--------------------------------------------------------------------------
        public void straighten()
        {
            // 移动时动画以及停止动画为 ON 的情况下
            if (this.walk_anime || this.step_anime)
                // 设置图形为 0
                this.pattern = 0;

            // 清除动画计数
            this.anime_count = 0;
            // 清除被锁定的向前朝向
            this.prelock_direction = 0;
        }
        //--------------------------------------------------------------------------
        // ● 强制移动路线
        //     move_route : 新的移动路线
        //--------------------------------------------------------------------------
        public void force_move_route(RPG.MoveRoute move_route)
        {
            // 保存原来的移动路线
            if (this.original_move_route == null)
            {
                this.original_move_route = this.move_route;
                this.original_move_route_index = this.move_route_index;
            }

            // 更改移动路线
            this.move_route = move_route;
            this.move_route_index = 0;
            // 设置强制移动路线标志
            this.move_route_forcing = true;
            // 清除被锁定的向前朝向
            this.prelock_direction = 0;
            // 清除等待计数
            this.wait_count = 0;
            // 自定义移动
            move_type_custom();
        }
        //--------------------------------------------------------------------------
        // ● 可以通行判定
        //     x : X 坐标
        //     y : Y 坐标
        //     d : 方向 (0,2,4,6,8)  ※ 0 = 全方向不能通行的情况判定 (跳跃用)
        //--------------------------------------------------------------------------
        public virtual bool is_passable(int x, int y, int d)
        {
            // 求得新的坐标
            var new_x = x + (d == 6 ? 1 : d == 4 ? -1 : 0);
            var new_y = y + (d == 2 ? 1 : d == 8 ? -1 : 0);
            // 坐标在地图以外的情况
            if (!Global.game_map.is_valid(new_x, new_y))
                // 不能通行
                return false;

            // 穿透是 ON 的情况下
            if (this.through)
                // 可以通行
                return true;

            // 移动者的元件无法来到指定方向的情况下
            if (!Global.game_map.is_passable(x, y, d, this))
                // 通行不可
                return false;

            // 从指定方向不能进入到移动处的元件的情况下
            if (!Global.game_map.is_passable(new_x, new_y, 10 - d))
                // 不能通行
                return false;

            // 循环全部事件
            foreach (var event1 in Global.game_map.events.Values)
            {
                // 事件坐标于移动目标坐标一致的情况下
                if (event1.x == new_x && event1.y == new_y)
                {
                    // 穿透为 ON
                    if (!event1.through)
                    {
                        // 自己就是事件的情况下
                        if (this != Global.game_player)
                            // 不能通行
                            return false;
                        // 自己是主角、对方的图形是角色的情况下
                        if (event1.character_name != "")
                            // 不能通行
                            return false;
                    }
                }
            }
            // 主角的坐标与移动目标坐标一致的情况下
            if (Global.game_player.x == new_x && Global.game_player.y == new_y)
                // 穿透为 ON
                if (!Global.game_player.through)
                    // 自己的图形是角色的情况下
                    if (this.character_name != "")
                        // 不能通行
                        return false;
            // 可以通行
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 锁定
        //--------------------------------------------------------------------------
        public void lock1()
        {
            // 如果已经被锁定的情况下
            if (this.locked)
                // 过程结束
                return;

            // 保存锁定前的朝向
            this.prelock_direction = this.direction;
            // 保存主角的朝向
            turn_toward_player();
            // 设置锁定中标志
            this.locked = true;

        }
        //--------------------------------------------------------------------------
        // ● 锁定中判定
        //--------------------------------------------------------------------------
        public bool is_lock
        {
            get
            {
                return this.locked;
            }
        }
        //--------------------------------------------------------------------------
        // ● 解除锁定
        //--------------------------------------------------------------------------
        public void unlock()
        {
            // 没有锁定的情况下
            if (!this.locked)
                // 过程结束
                return;

            // 清除锁定中标志
            this.locked = false;
            // 没有固定朝向的情况下
            if (!this.direction_fix)
                // 如果保存了锁定前的方向
                if (this.prelock_direction != 0)
                    // 还原为锁定前的方向
                    this.direction = this.prelock_direction;

        }
        //--------------------------------------------------------------------------
        // ● 移动到指定位置
        //     x : X 坐标
        //     y : Y 坐标
        //--------------------------------------------------------------------------
        public virtual void moveto(int x, int y)
        {
            this.x = x % Global.game_map.width;
            this.y = y % Global.game_map.height;
            this.real_x = this.x * 128;
            this.real_y = this.y * 128;
            this.prelock_direction = 0;
        }

        //--------------------------------------------------------------------------
        // ● 获取画面 X 坐标
        //--------------------------------------------------------------------------
        public int screen_x
        {
            get
            {
                // 通过实际坐标和地图的显示位置来求得画面坐标
                return (int)((this.real_x - Global.game_map.display_x + 3) / 4 + 16);
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取画面 Y 坐标
        //--------------------------------------------------------------------------
        public int screen_y
        {
            get
            {
                // 通过实际坐标和地图的显示位置来求得画面坐标
                var y = (this.real_y - Global.game_map.display_y + 3) / 4 + 32;
                double n;
                // 取跳跃计数小的 Y 坐标
                if (this.jump_count >= this.jump_peak)
                    n = this.jump_count - this.jump_peak;
                else
                    n = this.jump_peak - this.jump_count;
                return (int)(y - (this.jump_peak * this.jump_peak - n * n) / 2);
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取画面 Z 坐标
        //     height : 角色的高度
        //--------------------------------------------------------------------------
        public int screen_z(int height = 0)
        {
            // 在最前显示的标志为 ON 的情况下
            if (this.always_on_top)
                // 无条件设置为 999
                return 999;

            // 通过实际坐标和地图的显示位置来求得画面坐标
            var z = (this.real_y - Global.game_map.display_y + 3) / 4 + 32;
            // 元件的情况下
            if (this.tile_id > 0)
                // 元件的优先不足 * 32 
                return (int)(z + Global.game_map.priorities[this.tile_id] * 32);
            // 角色的场合
            else
                // 如果高度超过 32 就判定为满足 31
                return (int)(z + ((height > 32) ? 31 : 0));
        }
        //--------------------------------------------------------------------------
        // ● 取得繁茂
        //--------------------------------------------------------------------------
        public int bush_depth
        {
            get
            {
                // 是元件、并且在最前显示为 ON 的情况下
                if (this.tile_id > 0 || this.always_on_top)
                    return 0;

                // 在跳跃以外的状态时繁茂处元件的属性为 12，除此之外为 0
                if (this.jump_count == 0 && Global.game_map.is_bush(this.x, this.y))
                    return 12;
                else
                    return 0;
            }
        }
        //--------------------------------------------------------------------------
        // ● 取得地形标记
        //--------------------------------------------------------------------------
        public int terrain_tag
        {
            get
            {
                return Global.game_map.terrain_tag(this.x, this.y);
            }
        }


        public int original_direction { get; set; }
        public int original_pattern { get; set; }
        public int move_type { get; set; }
        public int move_speed { get; set; }
        public int move_frequency { get; set; }
        public int move_route_index { get; set; }
        public double anime_count { get; set; }
        public int prelock_direction { get; set; }
        public bool walk_anime { get; set; }
        public bool step_anime { get; set; }
        public int wait_count { get; set; }
        public RPG.MoveRoute move_route { get; set; }
        public RPG.MoveRoute original_move_route { get; set; }
        public int original_move_route_index { get; set; }
        public double jump_count { get; set; }
        public bool direction_fix { get; set; }
        public bool always_on_top { get; set; }
        public int stop_count { get; set; }
        public double jump_peak { get; set; }
        public bool locked { get; set; }
    }
}
