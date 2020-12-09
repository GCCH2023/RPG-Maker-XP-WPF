using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Scene_Debug
    //------------------------------------------------------------------------------
    // 　处理调试画面的类。
    //==============================================================================

    class Scene_Debug : Scene
    {
        public override void Initialize()
        {
            // 生成窗口
            this.left_window = new Window_DebugLeft();
            this.right_window = new Window_DebugRight();
            this.help_window = new Window_Base(192, 352, 448, 128);
            this.help_window.contents = new Bitmap(406, 96);
            // 还原为上次选择的项目
            this.left_window.top_row = Global.game_temp.debug_top_row;
            this.left_window.index = Global.game_temp.debug_index;
            this.right_window.mode = this.left_window.mode;
            this.right_window.top_id = this.left_window.top_id;
            // 执行过渡
            Graphics.transition();
        }
        //--------------------------------------------------------------------------
        // ● 主处理
        //--------------------------------------------------------------------------
        public override void main()
        {
          
            // 主循环
            //while (true)
            //{
                // 刷新游戏画面
                Graphics.update();
                // 刷新输入情报
                Input.update();
                // 刷新画面
                update();
                // 如果画面被切换的话就中断循环
            //    if (Global.scene != this)
            //        break;
            //}
        }

        public override void Uninitialize()
        {
            // 刷新地图
            Global.game_map.refresh();
            // 装备过渡
            Graphics.freeze();
            // 释放窗口
            this.left_window.dispose();
            this.right_window.dispose();
            this.help_window.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 刷新窗口
            this.right_window.mode = this.left_window.mode;
            this.right_window.top_id = this.left_window.top_id;
            this.left_window.update();
            this.right_window.update();
            // 记忆选择中的项目
            Global.game_temp.debug_top_row = this.left_window.top_row;
            Global.game_temp.debug_index = this.left_window.index;
            // 左侧窗口被激活的情况下: 调用 update_left
            if (this.left_window.active)
            {
                update_left();
                return;
            }

            // 右侧窗口被激活的情况下: 调用 update_right
            if (this.right_window.active)
            {
                update_right();
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (左侧窗口被激活的情况下)
        //--------------------------------------------------------------------------
        public void update_left()
        {
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 切换到地图画面
                Global.scene = new Scene_Map();
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 演奏确定 SE
                Global.game_system.se_play(Global.data_system.decision_se);
                // 显示帮助
                if (this.left_window.mode == 0)
                {
                    var text1 = "C (Enter) : ON / OFF";
                    this.help_window.contents.draw_text(4, 0, 406, 32, text1);
                }
                else
                {
                    var text1 = "← : -1   → : +1";
                    var text2 = "L (Pageup) : -10";
                    var text3 = "R (Pagedown) : +10";
                    this.help_window.contents.draw_text(4, 0, 406, 32, text1);
                    this.help_window.contents.draw_text(4, 32, 406, 32, text2);
                    this.help_window.contents.draw_text(4, 64, 406, 32, text3);
                }
                // 激活右侧窗口
                this.left_window.active = false;
                this.right_window.active = true;
                this.right_window.index = 0;
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (右侧窗口被激活的情况下)
        //--------------------------------------------------------------------------
        public void update_right()
        {
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 激活左侧窗口
                this.left_window.active = true;
                this.right_window.active = false;
                this.right_window.index = -1;
                // 删除帮助
                this.help_window.contents.clear();
                return;
            }

            // 获取被选择的开关 / 变量的 ID
            var current_id = this.right_window.top_id + this.right_window.index;
            // 开关的情况下
            if (this.right_window.mode == 0)
            {
                // 按下 C 键的情况下
                if (Input.is_trigger(Input.C))
                {
                    // 演奏确定 SE
                    Global.game_system.se_play(Global.data_system.decision_se);
                    // 逆转 ON / OFF 状态
                    Global.game_switches[current_id] = (!Global.game_switches[current_id]);
                    this.right_window.refresh();
                    return;
                }
            }
            // 变量的情况下
            if (this.right_window.mode == 1)
            {
                // 按下右键的情况下
                if (Input.is_repeat(Input.RIGHT))
                {
                    // 演奏光标 SE
                    Global.game_system.se_play(Global.data_system.cursor_se);
                    // 变量加 1
                    Global.game_variables[current_id] += 1;
                    // 检查上限
                    if (Global.game_variables[current_id] > 99999999)
                        Global.game_variables[current_id] = 99999999;

                    this.right_window.refresh();
                    return;
                }
            }

            // 按下左键的情况下
            if (Input.is_repeat(Input.LEFT))
            {
                // 演奏光标 SE
                Global.game_system.se_play(Global.data_system.cursor_se);
                // 变量减 1
                Global.game_variables[current_id] -= 1;
                // 检查下限
                if (Global.game_variables[current_id] < -99999999)
                    Global.game_variables[current_id] = -99999999;

                this.right_window.refresh();
                return;
            }
            // 按下 R 键的情况下
            if (Input.is_repeat(Input.R))
            {
                // 演奏光标 SE
                Global.game_system.se_play(Global.data_system.cursor_se);
                // 变量加 10
                Global.game_variables[current_id] += 10;
                // 检查上限
                if (Global.game_variables[current_id] > 99999999)
                    Global.game_variables[current_id] = 99999999;

                this.right_window.refresh();
                return;
            }
            // 按下 L 键的情况下
            if (Input.is_repeat(Input.L))
            {
                // 演奏光标 SE
                Global.game_system.se_play(Global.data_system.cursor_se);
                // 变量减 10
                Global.game_variables[current_id] -= 10;
                // 检查下限
                if (Global.game_variables[current_id] < -99999999)
                    Global.game_variables[current_id] = -99999999;

                this.right_window.refresh();
                return;
            }
        }

        public Window_DebugLeft left_window { get; set; }
        public Window_DebugRight right_window { get; set; }
        public Window_Base help_window { get; set; }
    }
}
