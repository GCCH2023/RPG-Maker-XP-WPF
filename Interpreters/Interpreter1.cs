using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XP
{
    //==============================================================================
    // ■ Interpreter (分割定义 1)
    //------------------------------------------------------------------------------
    // 　执行事件命令的解释器。本类在 Game_System 类
    // 与 Game_Event 类的内部使用。
    //==============================================================================

    public partial class Interpreter
    {
        //--------------------------------------------------------------------------
        // ● 初始化标志
        //     depth : 事件的深度
        //     main  : 主标志
        //--------------------------------------------------------------------------
        public Interpreter(int depth = 0, bool main = false)
        {
            this.depth = depth;
            this.main = main;
            // 深度超过 100 级
            if (depth > 100)
            {
                MessageBox.Show("调用公用事件超过了限制。");
                Environment.Exit(0);
            }
            // 清除注释器的内部状态
            clear();
        }
        //--------------------------------------------------------------------------
        // ● 清除
        //--------------------------------------------------------------------------
        public void clear()
        {
            this.map_id = 0;                      // 启动时的地图 ID
            this.event_id = 0;           // 事件 ID
            this.message_waiting = false;         // 信息结束后待机中
            this.move_route_waiting = false;     // 移动结束后待机中
            this.button_input_variable_id = 0;    // 输入按钮 变量 ID
            this.wait_count = 0.0;      // 窗口计数
            this.child_interpreter = null;       // 子实例
            this.branch = new Dictionary<object, bool>();                  // 分支数据
        }
        //--------------------------------------------------------------------------
        // ● 设置事件
        //     list     : 执行内容
        //     event_id : 事件 ID
        //--------------------------------------------------------------------------
        public void setup(List<RPG.EventCommand> list, int event_id)
        {
            // 清除注释器的内部状态
            clear();
            // 记忆地图 ID
            this.map_id = Global.game_map.map_id;
            // 记忆事件 ID
            this.event_id = event_id;
            // 记忆执行内容
            this.list = list;
            // 初始化索引
            this.index = 0;
            // 清除分支数据用复述
            this.branch.Clear();
        }
        //--------------------------------------------------------------------------
        // ● 执行中判定
        //--------------------------------------------------------------------------
        public bool is_running
        {
            get
            {
                return this.list != null;
            }
        }
        //--------------------------------------------------------------------------
        // ● 设置启动中事件
        //--------------------------------------------------------------------------
        public void setup_starting_event()
        {
            // 刷新必要的地图
            if (Global.game_map.need_refresh)
                Global.game_map.refresh();

            // 如果调用的公共事件被预约的情况下
            if (Global.game_temp.common_event_id > 0)
            {
                // 设置事件
                setup(Global.data_common_events[Global.game_temp.common_event_id].list, 0);
                // 解除预约
                Global.game_temp.common_event_id = 0;
                return;
            }

            // 循环 (地图事件)
            foreach (var event1 in Global.game_map.events.Values)
            {
                // 如果找到了启动中的事件
                if (event1.starting)
                {
                    // 如果不是自动执行
                    if (event1.trigger < 3)
                    {
                        // 清除启动中标志
                        event1.clear_starting();
                        // 锁定
                        event1.lock1();
                    }
                    // 设置事件
                    setup(event1.list, event1.id);
                    return;
                }
            }
            // 循环(公共事件)
            var compact = Global.data_common_events.Where(x => x != null);
            foreach (var common_event in compact)
            {
                // 目标的自动执行开关为 ON 的情况下
                if (common_event.trigger == 1 &&
                            Global.game_switches[common_event.switch_id] == true)
                {

                    // 设置事件
                    setup(common_event.list, 0);
                    return;
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public virtual void update()
        {
            // 初始化循环计数
            this.loop_count = 0;
            // 循环
            while (true)
            {
                // 循环计数加 1
                this.loop_count += 1;
                // 如果执行了 100 个事件指令
                if (this.loop_count > 100)
                {
                    // 为了防止系统崩溃、调用 Graphics.update
                    Graphics.update();
                    this.loop_count = 0;
                }

                // 如果地图与事件启动有差异
                if (Global.game_map.map_id != this.map_id)
                    // 事件 ID 设置为 0
                    this.event_id = 0;

                // 子注释器存在的情况下
                if (this.child_interpreter != null)
                {
                    // 刷新子注释器
                    this.child_interpreter.update();
                    // 子注释器执行结束的情况下
                    if (!this.child_interpreter.is_running)
                        // 删除字注释器
                        this.child_interpreter = null;
                    // 如果子注释器还存在
                    if (this.child_interpreter != null)
                        return;
                }
                // 信息结束待机的情况下
                if (this.message_waiting)
                    return;

                // 移动结束待机的情况下
                if (this.move_route_waiting)
                {
                    // 强制主角移动路线的情况下
                    if (Global.game_player.move_route_forcing)
                        return;

                    // 循环 (地图事件)
                    foreach (var event1 in Global.game_map.events.Values)
                        // 本事件为强制移动路线的情况下
                        if (event1.move_route_forcing)
                            return;


                    // 清除移动结束待机中的标志
                    this.move_route_waiting = false;
                }

                // 输入按钮待机中的情况下
                if (this.button_input_variable_id > 0)
                {
                    // 执行按钮输入处理
                    input_button();
                    return;
                }

                // 等待中的情况下
                if (this.wait_count > 0)
                {
                    // 减少等待计数
                    this.wait_count -= 1;
                    return;
                }

                // 如果被强制行动的战斗者存在
                if (Global.game_temp.forcing_battler != null)
                    return;

                // 如果各画面的调用标志已经被设置
                if (Global.game_temp.battle_calling ||
                            Global.game_temp.shop_calling ||
                            Global.game_temp.name_calling ||
                            Global.game_temp.menu_calling ||
                            Global.game_temp.save_calling ||
                            Global.game_temp.gameover)
                    return;

                // 执行内容列表为空的情况下
                if (this.list == null)
                {
                    // 主地图事件的情况下
                    if (this.main)
                        // 设置启动中的事件
                        setup_starting_event();

                    // 什么都没有设置的情况下
                    if (this.list == null)
                        return;

                }
                // 尝试执行事件列表、返回值为 false 的情况下
                if (execute_command() == false)
                    return;

                // 推进索引
                this.index += 1;
            }
        }
        //--------------------------------------------------------------------------
        // ● 输入按钮
        //--------------------------------------------------------------------------
        public void input_button()
        {
            // 判定按下的按钮
            var n = 0;
            for (var i = 1; i <= 18; i++)
            {
                if (Input.is_trigger(i))
                    n = i;

                // 按下按钮的情况下
                if (n > 0)
                {
                    // 更改变量值
                    Global.game_variables[this.button_input_variable_id] = n;
                    Global.game_map.need_refresh = true;
                    // 输入按键结束
                    this.button_input_variable_id = 0;
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 设置选择项
        //--------------------------------------------------------------------------
        public void setup_choices(List<object> parameters)
        {
            // choice_max 为设置选择项的项目数
            List<string> texts = (List<string>)parameters[0];
            Global.game_temp.choice_max = texts.Count;
            // message_text 为设置选择项
            foreach (var text in texts)
                Global.game_temp.message_text += text + "\n";

            // 设置取消的情况的处理
            Global.game_temp.choice_cancel_type = (int)parameters[1];
            // 返回调用设置
            var current_indent = this.list[this.index].indent;
            throw new Exception();
            //Global.game_temp.choice_proc = Proc.new { |n| this.branch[current_indent] = n };
        }
        //--------------------------------------------------------------------------
        // ● 角色用 itereta (考虑全体同伴)
        //     parameter : 1 以上为 ID、0 为全体
        //--------------------------------------------------------------------------
        public void iterate_actor(object parameter)
        {
            throw new Exception();
            //// 全体同伴的情况下
            //if( (int)parameter == 0)
            //  {
            //       // 同伴全体循环
            //           foreach(var actor in Global.game_party.actors)
            //             {
            //              // 评价块
            //                   yield actor;
            //              }
            //       }
            //// 单体角色的情况下
            //else
            //  {
            //    // 获取角色
            //        var actor = Global.game_actors[parameter];
            //        // 获取角色
            //    if(actor != null)
            //        yield actor;
            //    }
        }
        //--------------------------------------------------------------------------
        // ● 敌人用 itereta (考虑队伍全体)
        //     parameter : 0 为索引、-1 为全体
        //--------------------------------------------------------------------------
        public void iterate_enemy(object parameter)
        {
            throw new Exception();
            //// 队伍全体的情况下
            //if( (int)parameter == -1)
            //  {
            //// 队伍全体循环
            //  foreach(var enemy in Global.game_troop.enemies)
            //    // 评价块
            //    yield enemy;
            //}

            //// 敌人单体的情况下
            //else
            //  {
            //   // 获取敌人
            //        var enemy = Global.game_troop.enemies[parameter];
            //        // 评价块
            //        if( enemy != null)
            //            yield enemy ;
            //   }
        }
        //--------------------------------------------------------------------------
        // ● 战斗者用 itereta (要考虑全体队伍、全体同伴)
        //     parameter1 : 0 为敌人、1 为角色
        //     parameter2 : 0 以上为索引、-1 为全体
        //--------------------------------------------------------------------------
        public void iterate_battler(object parameter1, object parameter2)
        {
            throw new Exception();

            //  // 敌人的情况下
            //  if( (int)parameter1 == 0)
            //    {
            //          // 调用敌人的 itereta
            //            iterate_enemy(parameter2) do |enemy|
            //              yield enemy;
            //          }

            //  // 角色的情况下
            //  else
            //    {
            //        // 全体同伴的情况下
            //             if( (int)parameter2 == -1)
            //               {
            //          // 同伴全体循环
            //               foreach(var actor in Global.game_party.actors)
            //                 // 评价块
            //                 yield actor;
            //          }

            //             // 角色单体 (N 个人) 的情况下
            //             else
            //               {
            //                // 获取角色
            //                       var actor = Global.game_party.actors[(int)parameter2];
            //                       // 评价块
            //            if (actor != null){
            //                       yield actor;
            //                }
            //        }
            //}
        }

        public int map_id { get; set; }
        public int event_id { get; set; }
        public bool message_waiting { get; set; }
        public int button_input_variable_id { get; set; }
        public double wait_count { get; set; }
        public Interpreter child_interpreter { get; set; }       // 子实例
        public Dictionary<object, bool> branch { get; set; }                  // 分支数据


        public int depth { get; set; }
        public bool main { get; set; }

        public List<RPG.EventCommand> list { get; set; }

        public int loop_count { get; set; }
    }

}
