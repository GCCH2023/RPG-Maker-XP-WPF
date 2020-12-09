using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Interpreter (分割定义 3)
    //------------------------------------------------------------------------------
    // 　执行事件指令的解释器。本类在 Game_System 类
    // 和 Game_Event 类的内部使用。
    //==============================================================================

    public partial class Interpreter
    {
        //--------------------------------------------------------------------------
        // ● 显示文章
        //--------------------------------------------------------------------------
        public bool command_101()
  {
    // 另外的文章已经设置过 message_text 的情况下
    if( Global.game_temp.message_text != null)
      // 结束
      return false;

    // 设置信息结束后待机和返回调用标志
    this.message_waiting = true;
            // @@
    //Global.game_temp.message_proc = Proc.new { this.message_waiting = false };
    // message_text 设置为 1 行
    Global.game_temp.message_text = this.list[this.index].parameters[0] + "\n";
    var line_count = 1;
    // 循环
    while(true){
    {
      // 下一个事件指令为文章两行以上的情况
      if( this.list[this.index+1].code == 401)
        {
           // message_text 添加到第 2 行以下
                Global.game_temp.message_text += this.list[this.index+1].parameters[0] + "\n";
                line_count += 1;
           }
      // 事件指令不在文章两行以下的情况
      else
        // 下一个事件指令为显示选择项的情况下
        if( this.list[this.index+1].code == 102)
          {
              throw new Exception();
                 // 如果选择项能收纳在画面里
                         //if( this.list[this.index+1].parameters[0].Count <= 4 - line_count)
                         //  {
                         //     // 推进索引
                         //            this.index += 1;
                         //            // 设置选择项
                         //            Global.game_temp.choice_start = line_count;
                         //            setup_choices(this.list[this.index].parameters);
                         //     }
                 }
        // 下一个事件指令为处理输入数值的情况下
        else if( this.list[this.index+1].code == 103)
          {
          // 如果数值输入窗口能收纳在画面里
                  if( line_count < 4)
                    {
                      // 推进索引
                              this.index += 1;
                              // 设置输入数值
                              Global.game_temp.num_input_start = line_count;
                              Global.game_temp.num_input_variable_id = (int)this.list[this.index].parameters[0];
                              Global.game_temp.num_input_digits_max = (int)this.list[this.index].parameters[1];
                      }
                // 继续
                return true;
          }
      // 推进索引
      this.index += 1;
    }
  }
        }
        //--------------------------------------------------------------------------
        // ● 显示选择项
        //--------------------------------------------------------------------------
        public bool command_102()
  {
    // 文章已经设置过 message_text 的情况下
    if( Global.game_temp.message_text != null)
      // 结束
      return false;

    // 设置信息结束后待机和返回调用标志
    this.message_waiting = true;
            // @@
    //Global.game_temp.message_proc = Proc.new { this.message_waiting = false };
    // 设置选择项
    Global.game_temp.message_text = "";
    Global.game_temp.choice_start = 0;
    setup_choices(this.parameters);
    // 继续
    return true;
  }
        //--------------------------------------------------------------------------
        // ● [**] 的情况下
        //--------------------------------------------------------------------------
        public bool command_402()
        {
            // 如果符合的选择项被选择
            if (this.branch[this.list[this.index].indent] == (bool)this.parameters[0])
            {
                // 删除分支数据
                this.branch.Remove(this.list[this.index].indent);
                // 继续
                return true;
            }
            // 不符合条件的情况下 : 指令跳转
            return command_skip();
        }
        //--------------------------------------------------------------------------
        // ● 取消的情况下
        //--------------------------------------------------------------------------
        public bool command_403()
        {
            // @@
            //// 如果选择了选择项取消
            //if (this.branch[this.list[this.index].indent] == 4)
            //{
            //    // 删除分支数据
            //    this.branch.Remove(this.list[this.index].indent);
            //    // 继续
            //    return true;
            //}
            // 不符合条件的情况下 : 指令跳转
            return command_skip();
        }
        //--------------------------------------------------------------------------
        // ● 处理数值输入
        //--------------------------------------------------------------------------
        public bool command_103(){
    // 文章已经设置过 message_text 的情况下
    if(Global.game_temp.message_text != null){
      // 结束
      return false;
    }
    // 设置信息结束后待机和返回调用标志
    this.message_waiting = true;
            // @@
    //Global.game_temp.message_proc = Proc.new { this.message_waiting = false };
    // 设置数值输入
    Global.game_temp.message_text = "";
    Global.game_temp.num_input_start = 0;
    Global.game_temp.num_input_variable_id = (int)this.parameters[0];
    Global.game_temp.num_input_digits_max = (int)this.parameters[1];
    // 继续
    return true;
  }
        //--------------------------------------------------------------------------
        // ● 更改文章选项
        //--------------------------------------------------------------------------
        public bool command_104()
        {
            // 正在显示信息的情况下
            if (Global.game_temp.message_window_showing)
            {
                // 结束
                return false;
            }
            // 更改各个选项
            Global.game_system.message_position = (int)this.parameters[0];
            Global.game_system.message_frame = (int)this.parameters[1];
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 处理按键输入
        //--------------------------------------------------------------------------
        public bool command_105()
        {
            // 设置按键输入用变量 ID
            this.button_input_variable_id = (int)this.parameters[0];
            // 推进索引
            this.index += 1;
            // 结束
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 等待
        //--------------------------------------------------------------------------
        public bool command_106()
        {
            // 设置等待计数
            this.wait_count = (int)this.parameters[0] * 2;
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 条件分支
        //--------------------------------------------------------------------------
        public bool command_111(){
    // 初始化本地变量 result
    var result = false;
    // 条件判定
    switch((int)this.parameters[0])
    {
        case 0:  // 开关
      result = (Global.game_switches[(int)this.parameters[1]] == ((int)this.parameters[2] == 0));break;
    case 1:  // 变量
            {
      var value1 = Global.game_variables[(int)this.parameters[1]];
                int value2;
            if((int)this.parameters[2] == 0){
              value2 = (int)this.parameters[3];
            }
            else
            {
              value2 = Global.game_variables[(int)this.parameters[3]];
            }
            switch((int)this.parameters[4]){
                case 0:  // 等于
              result = (value1 == value2);break;
            case 1:  // 以上
              result = (value1 >= value2);break;
            case 2:  // 以下
              result = (value1 <= value2);break;
            case 3:  // 超过
              result = (value1 > value2);break;
            case 4:  // 未满
              result = (value1 < value2);break;
            case 5:  // 以外
              result = (value1 != value2);break;
            }
            }
      break;
    case 2:  // 独立开关
      {
       if(this.event_id > 0){
              var key = new List<int>{Global.game_map.map_id, this.event_id, (int)this.parameters[1]};
              if((int)this.parameters[2] == 0){
                result = (Global.game_self_switches[key] == true);
              }
              else
              {
                result = (Global.game_self_switches[key] != true);
              }
            }
       }
            break;
    case 3:  // 计时器
      if(Global.game_system.timer_working){
        var sec = Global.game_system.timer / Graphics.frame_rate;
        if((int)this.parameters[2] == 0){
          result = (sec >= (int)this.parameters[1]);
        }
        else{
          result = (sec <= (int)this.parameters[1]);
        }
      }
            break;
    case 4:  // 角色
            {
      var actor = Global.game_actors[(int)this.parameters[1]];
            if(actor != null){
              switch((int)this.parameters[2]){
                  case 0:  // 同伴
                result = (Global.game_party.actors.Contains(actor));break;
              case 1:  // 名称
                result = (actor.name == (string)this.parameters[3]);break;
              case 2:  // 特技
                result = (actor.is_skill_learn((int)this.parameters[3]));break;
              case 3:  // 武器
                result = (actor.weapon_id == (int)this.parameters[3]);break;
              case 4:  // 防具
                      result = (actor.armor1_id == (int)this.parameters[3] ||
                          actor.armor2_id == (int)this.parameters[3] ||
                          actor.armor3_id == (int)this.parameters[3] ||
                          actor.armor4_id == (int)this.parameters[3]);break;
              case 5:  // 状态
                result = (actor.is_state((int)this.parameters[3]));break;
              }
            }
            }
            break;
    case 5:  // 敌人
      {
          var enemy = Global.game_troop.enemies[(int)this.parameters[1]];
            if(enemy != null){
              switch((int)this.parameters[2]){
                  case 0:  // 出现
                result = (enemy.is_exist);break;
              case 1:  // 状态
                result = (enemy.is_state((int)this.parameters[3]));break;
              }
            }
       }
            break;
    case 6:  // 角色
      {
      var character = get_character((int)this.parameters[1]);
      if(character != null){
        result = (character.direction == (int)this.parameters[2]);
      }
      }
            break;
    case 7:  // 金钱
      {
      if((int)this.parameters[2] == 0){
        result = (Global.game_party.gold >= (int)this.parameters[1]);
      }
      else{
        result = (Global.game_party.gold <= (int)this.parameters[1]);
      }
      }
          break;
    case 8:  // 物品
      result = (Global.game_party.item_number((int)this.parameters[1]) > 0);break;
    case 9:  // 武器
      result = (Global.game_party.weapon_number((int)this.parameters[1]) > 0);break;
    case 10:  // 防具
      result = (Global.game_party.armor_number((int)this.parameters[1]) > 0);break;
    case 11:  // 按钮
      result = (Input.is_press((int)this.parameters[1]));break;
            // @@
    //case 12:  // 活动块
    //  result = eval((int)this.parameters[1]);break;
    }
    // 判断结果保存在 hash 中
    this.branch[this.list[this.index].indent] = result;
    // 判断结果为真的情况下
    if(this.branch[this.list[this.index].indent] == true){
      // 删除分支数据
      this.branch.Remove(this.list[this.index].indent);
      // 继续
      return true;
    }
    // 不符合条件的情况下 : 指令跳转
    return command_skip();
  }
        //--------------------------------------------------------------------------
        // ● 这以外的情况
        //--------------------------------------------------------------------------
        public bool command_411()
        {
            // 判断结果为假的情况下
            if (this.branch[this.list[this.index].indent] == false)
            {
                // 删除分支数据
                this.branch.Remove(this.list[this.index].indent);
                // 继续
                return true;
            }
            // 不符合条件的情况下 : 指令跳转
            return command_skip();
        }
        //--------------------------------------------------------------------------
        // ● 循环
        //--------------------------------------------------------------------------
        public bool command_112()
        {
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 循环上次
        //--------------------------------------------------------------------------
        public bool command_413()
        {
            // 获取缩进
            var indent = this.list[this.index].indent;
            // 循环
            while (true)
            {
                // 推进索引
                this.index -= 1;
                // 本事件指令是同等级的缩进的情况下
                if (this.list[this.index].indent == indent)
                {
                    // 继续
                    return true;
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 中断循环
        //--------------------------------------------------------------------------
        public bool command_113()
        {
            // 获取缩进
            var indent = this.list[this.index].indent;
            // 将缩进复制到临时变量中
            var temp_index = this.index;
            // 循环
            while (true)
            {
                // 推进索引
                temp_index += 1;
                // 没找到符合的循环的情况下
                if (temp_index >= this.list.Count - 1)
                {
                    // 继续
                    return true;
                }
                // 本事件命令为 [重复上次] 的情况下
                if (this.list[temp_index].code == 413 && this.list[temp_index].indent < indent)
                {
                    // 刷新索引
                    this.index = temp_index;
                    // 继续
                    return true;
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 中断事件处理
        //--------------------------------------------------------------------------
        public bool command_115()
        {
            // 结束事件
            command_end();
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 事件暂时删除
        //--------------------------------------------------------------------------
        public bool command_116()
        {
            // 事件 ID 有效的情况下
            if (this.event_id > 0)
            {
                // 删除事件
                Global.game_map.events[this.event_id].erase();
            }
            // 推进索引
            this.index += 1;
            // 继续
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 公共事件
        //--------------------------------------------------------------------------
        public bool command_117()
        {
            // 获取公共事件
            var common_event = Global.data_common_events[(int)this.parameters[0]];
            // 公共事件有效的情况下
            if (common_event != null)
            {
                // 生成子解释器
                this.child_interpreter = new Interpreter(this.depth + 1);
                this.child_interpreter.setup(common_event.list, this.event_id);
            }
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 标签
        //--------------------------------------------------------------------------
        public bool command_118()
        {
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 标签跳转
        //--------------------------------------------------------------------------
        public bool command_119()
        {
            // 获取标签名
            var label_name = (string)this.parameters[0];
            // 初始化临时变量
            var temp_index = 0;
            // 循环
            while (true)
            {
                // 没找到符合的标签的情况下
                if (temp_index >= this.list.Count - 1)
                {
                    // 继续
                    return true;
                }
                // 本事件指令为指定的标签的名称的情况下
                if (this.list[temp_index].code == 118 &&
                   (string)this.list[temp_index].parameters[0] == label_name)
                {
                    // 刷新索引
                    this.index = temp_index;
                    // 继续
                    return true;
                }
                // 推进索引
                temp_index += 1;
            }
            return false;
        }
    }

}
