using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Window_Message
    //------------------------------------------------------------------------------
    // 　显示文章的信息窗口。
    //==============================================================================

    public class Window_Message : Window_Selectable
    {
        public Window_InputNumber input_number_window { get; set; }
        public int cursor_width { get; set; }
        public bool contents_showing { get; set; }
        public bool fade_out { get; set; }
        public bool fade_in { get; set; }
        //--------------------------------------------------------------------------
        // ● 初始化状态
        //--------------------------------------------------------------------------
        public Window_Message() :
            base(80, 304, 480, 160)
        {
            this.contents = new Bitmap(width - 32, height - 32);
            this.visible = false;
            this.z = 9998;
            this.fade_in = false;
            this.fade_out = false;
            this.contents_showing = false;
            this.cursor_width = 0;
            this.active = false;
            this.index = -1;
        }
        //--------------------------------------------------------------------------
        // ● 释放
        //--------------------------------------------------------------------------
        public override void dispose()
        {
            terminate_message();
            Global.game_temp.message_window_showing = false;
            if (this.input_number_window != null)
                this.input_number_window.dispose();
            base.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 处理信息结束
        //--------------------------------------------------------------------------
        public void terminate_message()
        {
            this.active = false;
            this.pause = false;
            this.index = -1;
            this.contents.clear();
            // 清除显示中标志
            this.contents_showing = false;
            // 呼叫信息调用
            if (Global.game_temp.message_proc != null)
            {
                throw new Exception();

                //Global.game_temp.message_proc.call;
            }

            // 清除文章、选择项、输入数值的相关变量
            Global.game_temp.message_text = null;
            Global.game_temp.message_proc = null;
            Global.game_temp.choice_start = 99;
            Global.game_temp.choice_max = 0;
            Global.game_temp.choice_cancel_type = 0;
            Global.game_temp.choice_proc = null;
            Global.game_temp.num_input_start = 99;
            Global.game_temp.num_input_variable_id = 0;
            Global.game_temp.num_input_digits_max = 0;
            // 开放金钱窗口
            if (this.gold_window != null)
            {
                this.gold_window.dispose();
                this.gold_window = null;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            this.contents.font.color = normal_color;
            var x = 0;
            var y = 0;
            this.cursor_width = 0;
            // 到选择项的下一行字
            if (Global.game_temp.choice_start == 0)
                x = 8;

            throw new Exception();
            //// 有等待显示的文字的情况下
            //if( Global.game_temp.message_text != null){
            //  var text = Global.game_temp.message_text;
            //    string last_text;
            //  // 限制文字处理
            //  do{
            //    last_text = new string(text);
            //    Regex.
            //    text.gsub!(/\\[Vv]\[([0-9]+)\]/) { Global.game_variables[Global.1.to_i] };
            //  }
            //  while(text == last_text);

            //  text.gsub!(/\\[Nn]\[([0-9]+)\]/) do
            //    Global.game_actors[Global.1.to_i] != null ? Global.game_actors[Global.1.to_i].name : ""
            //  end

            //  // 为了方便、将 "\\\\" 变换为 "\000" 
            //  text.gsub!(/\\\\/) { "\000" }
            //  // "\\C" 变为 "\001" 、"\\G" 变为 "\002"
            //  text.gsub!(/\\[Cc]\[([0-9]+)\]/) { "\001[//{Global.1}]" }
            //  text.gsub!(/\\[Gg]/) { "\002" }
            //  // c 获取 1 个字 (如果不能取得文字就循环)
            //  while ((c = text.slice!(/./m)) != null)
            //    // \\ 的情况下
            //    if c == "\000"
            //      // 还原为本来的文字
            //      c = "\\"
            //    end
            //    // \C[n] 的情况下
            //    if c == "\001"
            //      // 更改文字色
            //      text.sub!(/\[([0-9]+)\]/, "")
            //      color = Global.1.to_i
            //      if color >= 0 && color <= 7
            //        this.contents.font.color = text_color(color)
            //      end
            //      // 下面的文字
            //      continue;
            //    end
            //    // \G 的情况下
            //    if c == "\002"
            //      // 生成金钱窗口
            //      if this.gold_window == null
            //        this.gold_window = Window_Gold.new
            //        this.gold_window.x = 560 - this.gold_window.width
            //        if Global.game_temp.in_battle
            //          this.gold_window.y = 192
            //        else
            //          this.gold_window.y = this.y >= 128 ? 32 : 384
            //        end
            //        this.gold_window.opacity = this.opacity
            //        this.gold_window.back_opacity = this.back_opacity
            //      end
            //      // 下面的文字
            //      continue;
            //    end
            //    // 另起一行文字的情况下
            //    if c == "\n"
            //      // 刷新选择项及光标的高
            //      if y >= Global.game_temp.choice_start
            //        this.cursor_width = [this.cursor_width, x].max
            //      end
            //      // y 加 1
            //      y += 1
            //      x = 0
            //      // 移动到选择项的下一行
            //      if y >= Global.game_temp.choice_start
            //        x = 8
            //      end
            //      // 下面的文字
            //      continue;
            //    end
            //    // 描绘文字
            //    this.contents.draw_text(4 + x, 32 * y, 40, 32, c)
            //    // x 为要描绘文字的加法运算
            //    x += this.contents.text_size(c).width
            //  end
            //end
            //// 选择项的情况
            //if Global.game_temp.choice_max > 0
            //  this.item_max = Global.game_temp.choice_max
            //  this.active = true
            //  this.index = 0
            //end
            //// 输入数值的情况
            //if Global.game_temp.num_input_variable_id > 0
            //  digits_max = Global.game_temp.num_input_digits_max
            //  number = Global.game_variables[Global.game_temp.num_input_variable_id]
            //  this.input_number_window = Window_InputNumber.new(digits_max)
            //  this.input_number_window.number = number
            //  this.input_number_window.x = this.x + 8
            //  this.input_number_window.y = this.y + Global.game_temp.num_input_start * 32
            //end
        }
        //--------------------------------------------------------------------------
        // ● 设置窗口位置与不透明度
        //--------------------------------------------------------------------------
        public void reset_window()
        {
            if (Global.game_temp.in_battle)
                this.y = 16;
            else
            {
                switch (Global.game_system.message_position)
                {
                    case 0:  // 上
                        this.y = 16; break;
                    case 1:  // 中
                        this.y = 160; break;
                    case 2:  // 下
                        this.y = 304; break;
                }
            }
            if (Global.game_system.message_frame == 0)
                this.opacity = 255;
            else
                this.opacity = 0;

            this.back_opacity = 160;
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            base.update();
            // 渐变的情况下
            if (this.fade_in)
            {
                this.contents_opacity += 24;
                if (this.input_number_window != null)
                    this.input_number_window.contents_opacity += 24;

                if (this.contents_opacity == 255)
                    this.fade_in = false;

                return;
            }
            // 输入数值的情况下
            if (this.input_number_window != null)
            {
                this.input_number_window.update();
                // 确定
                if (Input.is_trigger(Input.C))
                {
                    Global.game_system.se_play(Global.data_system.decision_se);
                    Global.game_variables[Global.game_temp.num_input_variable_id] =
                      this.input_number_window.number;
                    Global.game_map.need_refresh = true;
                    // 释放输入数值窗口
                    this.input_number_window.dispose();
                    this.input_number_window = null;
                    terminate_message();
                }
                return;
            }
            // 显示信息中的情况下
            if (this.contents_showing)
            { // 如果不是在显示选择项中就显示暂停标志
                if (Global.game_temp.choice_max == 0)
                    this.pause = true;

                // 取消
                if (Input.is_trigger(Input.B))
                {
                    if (Global.game_temp.choice_max > 0 && Global.game_temp.choice_cancel_type > 0)
                    {
                        Global.game_system.se_play(Global.data_system.cancel_se);
                        throw new Exception();
                        //Global.game_temp.choice_proc.call(Global.game_temp.choice_cancel_type - 1);
                        //terminate_message();
                    }
                }
                // 确定
                if (Input.is_trigger(Input.C))
                {
                    if (Global.game_temp.choice_max > 0)
                    {
                        Global.game_system.se_play(Global.data_system.decision_se);
                        throw new Exception();
                        //Global.game_temp.choice_proc.call(this.index);
                    }
                    terminate_message();
                }
                return;
            }
            // 在渐变以外的状态下有等待显示的信息与选择项的场合
            if (this.fade_out == false && Global.game_temp.message_text != null)
            {
                this.contents_showing = true;
                Global.game_temp.message_window_showing = true;
                reset_window();
                refresh();
                Graphics.frame_reset();
                this.visible = true;
                this.contents_opacity = 0;
                if (this.input_number_window != null)
                    this.input_number_window.contents_opacity = 0;

                this.fade_in = true;
                return;
            }
            // 没有可以显示的信息、但是窗口为可见的情况下
            if (this.visible)
            {
                this.fade_out = true;
                this.opacity -= 48;
                if (this.opacity == 0)
                {
                    this.visible = false;
                    this.fade_out = false;
                    Global.game_temp.message_window_showing = false;
                }
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新光标矩形
        //--------------------------------------------------------------------------
        public override void update_cursor_rect()
        {
            if (this.index >= 0)
            {
                var n = Global.game_temp.choice_start + this.index;
                this.cursor_rect = new Rect(8, n * 32, this.cursor_width, 32);
            }
            else
            { this.cursor_rect = Rect.Empty; }
        }


        public Window_Gold gold_window { get; set; }
    }
}
