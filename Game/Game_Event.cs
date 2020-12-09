using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_Event
    //------------------------------------------------------------------------------
    // 　处理事件的类。条件判断、事件页的切换、并行处理、执行事件功能
    // 在 Game_Map 类的内部使用。
    //==============================================================================

    public class Game_Event : Game_Character
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public int? trigger;               // 目标
        public List<RPG.EventCommand> list;           // 执行内容
        //public bool starting;               // 启动中标志
        public Interpreter interpreter;
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     map_id : 地图 ID
        //     event1  : 事件 (RPG.Event)
        //--------------------------------------------------------------------------
        public Game_Event(int map_id, RPG.Event event1)
        {
            this.map_id = map_id;
            this.event1 = event1;
            this.id = this.event1.id;
            this.erased = false;
            this.starting = false;
            this.through = true;
            // 初期位置的移动
            moveto(this.event1.x, this.event1.y);
            refresh();
        }
        //--------------------------------------------------------------------------
        // ● 清除移动中标志
        //--------------------------------------------------------------------------
        public void clear_starting()
        {
            this.starting = false;
        }
        //--------------------------------------------------------------------------
        // ● 越过目标判定 (不能将相同位置作为启动条件)
        //--------------------------------------------------------------------------
        public bool is_over_trigger
        {
            get
            {
                // 图形是角色、没有开启穿透的情况下
                if (this.character_name != "" && !this.through)
                    // 启动判定是正面
                    return false;

                // 地图上的这个位置不能通行的情况下
                if (!Global.game_map.is_passable(this.x, this.y, 0))
                    // 启动判定是正面
                    return false;

                // 启动判定在同位置
                return true;
            }
        }
        //--------------------------------------------------------------------------
        // ● 启动事件
        //--------------------------------------------------------------------------
        public void start()
        {
            // 执行内容不为空的情况下
            if (this.list.Count > 1)
                this.starting = true;
        }
        //--------------------------------------------------------------------------
        // ● 暂时消失
        //--------------------------------------------------------------------------
        public void erase()
        {
            this.erased = true;
            refresh();
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            // 初始化本地变量 new_page
            RPG.Event.Page new_page = null;
            // 无法暂时消失的情况下
            if (!this.erased)
            {
                // 从编号大的事件页按顺序调查
                for (var i = this.event1.pages.Count - 1; i >= 0; i--)
                {
                    var page = this.event1.pages[i];
                    // 可以参考事件条件 c
                    var c = page.condition;
                    // 确认开关条件 1 
                    if (c.switch1_valid)
                        if (Global.game_switches[c.switch1_id] == false)
                            continue;

                    // 确认开关条件 2 
                    if (c.switch2_valid)
                        if (Global.game_switches[c.switch2_id] == false)
                            continue;

                    // 确认变量条件
                    if (c.variable_valid)
                        if (Global.game_variables[c.variable_id] < c.variable_value)
                            continue;

                    // 确认独立开关条件
                    if (c.self_switch_valid)
                    {
                        var key = new object[] { this.map_id, this.event1.id, c.self_switch_ch };
                        if (Global.game_self_switches[key] != true)
                            continue;
                    }

                    // 设置本地变量 new_page
                    new_page = page;
                    // 跳出循环
                    break;
                }
            }
            // 与上次同一事件页的情况下
            if (new_page == this.page)
                // 过程结束
                return;

            // this.page 设置为现在的事件页
            this.page = new_page;
            // 清除启动中标志
            clear_starting();
            // 没有满足条件的页面的时候
            if (this.page == null)
            {
                // 设置各实例变量
                this.tile_id = 0;
                this.character_name = "";
                this.character_hue = 0;
                this.move_type = 0;
                this.through = true;
                this.trigger = null;
                this.list = null;
                this.interpreter = null;
                // 过程结束
                return;
            }
            // 设置各实例变量
            this.tile_id = this.page.graphic.tile_id;
            this.character_name = this.page.graphic.character_name;
            this.character_hue = this.page.graphic.character_hue;
            if (this.original_direction != this.page.graphic.direction)
            {
                this.direction = this.page.graphic.direction;
                this.original_direction = this.direction;
                this.prelock_direction = 0;
            }

            if (this.original_pattern != this.page.graphic.pattern)
            {
                this.pattern = this.page.graphic.pattern;
                this.original_pattern = this.pattern;
            }

            this.opacity = this.page.graphic.opacity;
            this.blend_type = this.page.graphic.blend_type;
            this.move_type = this.page.move_type;
            this.move_speed = this.page.move_speed;
            this.move_frequency = this.page.move_frequency;
            this.move_route = this.page.move_route;
            this.move_route_index = 0;
            this.move_route_forcing = false;
            this.walk_anime = this.page.walk_anime;
            this.step_anime = this.page.step_anime;
            this.direction_fix = this.page.direction_fix;
            this.through = this.page.through;
            this.always_on_top = this.page.always_on_top;
            this.trigger = this.page.trigger;
            this.list = this.page.list;
            this.interpreter = null;
            // 目标是 [并行处理] 的情况下
            if (this.trigger == 4)
                // 生成并行处理用解释器
                this.interpreter = new Interpreter();

            // 自动事件启动判定
            check_event_trigger_auto();
        }
        //--------------------------------------------------------------------------
        // ● 接触事件启动判定
        //--------------------------------------------------------------------------
        public override bool check_event_trigger_touch(int x, int y)
        {
            // 事件执行中的情况下
            if (Global.game_system.map_interpreter.is_running)
                return false;

            // 目标为 [与事件接触] 以及和主角坐标一致的情况下
            if (this.trigger == 2 && x == Global.game_player.x && y == Global.game_player.y)
                // 除跳跃中以外的情况、启动判定就是正面的事件
                if (!is_jumping && !is_over_trigger)
                    start();

            return base.check_event_trigger_touch(x, y);
        }
        //--------------------------------------------------------------------------
        // ● 自动事件启动判定
        //--------------------------------------------------------------------------
        public void check_event_trigger_auto()
        {
            // 目标为 [与事件接触] 以及和主角坐标一致的情况下
            if (this.trigger == 2 && this.x == Global.game_player.x && this.y == Global.game_player.y)
                // 除跳跃中以外的情况、启动判定就是同位置的事件
                if (!is_jumping && is_over_trigger)
                    start();

            // 目标是 [自动执行] 的情况下
            if (this.trigger == 3)
                start();
        }
        //--------------------------------------------------------------------------
        // ● 更新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            base.update();
            // 自动启动事件判定
            check_event_trigger_auto();
            // 并行处理有效的情况下
            if (this.interpreter != null)
            {
                // 不在执行中的场合的情况下
                if (!this.interpreter.is_running)
                    // 设置事件
                    this.interpreter.setup(this.list, this.event1.id);

                // 更新解释器
                this.interpreter.update();

            }
        }


        public int map_id { get; set; }
        public RPG.Event event1 { get; set; }
        public bool erased { get; set; }
        public RPG.Event.Page page { get; set; }
    }
}
