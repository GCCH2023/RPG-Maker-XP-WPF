using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Interpreter (分割定义 7)
    //------------------------------------------------------------------------------
    // 　执行事件命令的解释器。本类在 Game_System 类
    // 和 Game_Event 类的内部使用。
    //==============================================================================

    public partial class Interpreter
    {
        //--------------------------------------------------------------------------
        // ● 增减敌人的 HP
        //--------------------------------------------------------------------------
        public bool command_331()
        {
            // 获取操作值
            var value = operate_value((int)this.parameters[1], (int)this.parameters[2], (int)this.parameters[3]);
            throw new Exception();

            // 处理循环
            //iterate_enemy((int)this.parameters[0]) do |enemy|
            //  {
            //  // HP 不为 0 的情况下
            //  if(enemy.hp > 0){
            //    // 更改 HP (如果不允许战斗不能的状态就设置为 1)
            //    if((int)this.parameters[4] == false && enemy.hp + value <= 0){
            //      enemy.hp = 1;
            //    }else{
            //      enemy.hp += value;
            //    }
            //  }
            //}
            // 继续
            return true; ;
        }
        //--------------------------------------------------------------------------
        // ● 增减敌人的 SP
        //--------------------------------------------------------------------------
        public bool command_332()
        {
            // 获取操作值
            var value = operate_value((int)this.parameters[1], (int)this.parameters[2], (int)this.parameters[3]);
            throw new Exception();

            //// 处理循环
            //iterate_enemy((int)this.parameters[0]) do |enemy|
            //  {  // 更改 SP
            //  enemy.sp += value;
            //}
            // 继续
            return true; ;
        }
        //--------------------------------------------------------------------------
        // ● 更改敌人的状态
        //--------------------------------------------------------------------------
        public bool command_333()
        {
            throw new Exception();

            // 处理循环
            //iterate_enemy((int)this.parameters[0]) do |enemy|
            //  {  // 状态选项 [当作 HP 为 0 的状态] 有效的情况下
            //  if(Global.data_states[(int)this.parameters[2]].zero_hp){
            //    // 清除不死身标志
            //    enemy.immortal = false;
            //  }
            //  // 更改状态
            //  if((int)this.parameters[1] == 0){
            //    enemy.add_state((int)this.parameters[2]);
            //  }else{
            //    enemy.remove_state((int)this.parameters[2]);
            //  }
            //}
            // 继续
            return true; ;
        }
        //--------------------------------------------------------------------------
        // ● 敌人的全回复
        //--------------------------------------------------------------------------
        public bool command_334()
        {
            throw new Exception();

            // 处理循环
            //iterate_enemy((int)this.parameters[0]) do |enemy|
            //  {
            //  // 全回复
            //  enemy.recover_all();
            //}
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 敌人出现
        //--------------------------------------------------------------------------
        public bool command_335()
        {
            // 获取敌人
            var enemy = Global.game_troop.enemies[(int)this.parameters[0]];
            // 清除隐藏标志
            if (enemy != null)
            {
                enemy.hidden = false;
            }
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 敌人变身
        //--------------------------------------------------------------------------
        public bool command_336()
        {
            // 获取敌人
            var enemy = Global.game_troop.enemies[(int)this.parameters[0]];
            // 变身处理
            if (enemy != null)
            {
                enemy.transform((int)this.parameters[1]);
            }
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 显示动画
        //--------------------------------------------------------------------------
        public bool command_337()
        {
            throw new Exception();

            // 处理循环
            //iterate_battler((int)this.parameters[0], (int)this.parameters[1]) do |battler|
            //  { // 战斗者存在的情况下
            //  if(battler.is_exist){
            //    // 设置动画 ID
            //    battler.animation_id = (int)this.parameters[2];
            //  }
            //}
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 伤害处理
        //--------------------------------------------------------------------------
        public bool command_338()
        {
            // 获取操作值
            var value = operate_value(0, (int)this.parameters[2], (int)this.parameters[3]);
            throw new Exception();

            // 处理循环
            //iterate_battler((int)this.parameters[0], (int)this.parameters[1]) do |battler|
            //  { // 战斗者存在的情况下
            //  if(battler.is_exist){
            //    // 更改 HP
            //    battler.hp -= value;
            //    // 如果在战斗中
            //    if(Global.game_temp.in_battle){
            //      // 设置伤害
            //      battler.damage = value;
            //      battler.damage_pop = true;
            //    }
            //  }
            //}
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 强制行动
        //--------------------------------------------------------------------------
        public bool command_339()
        {
            throw new Exception();

            //// 忽视是否在战斗中
            //if(! Global.game_temp.in_battle){
            //  return true;
            //}
            //// 忽视回合数为 0
            //if(Global.game_temp.battle_turn == 0){
            //  return true;
            //}
            //// 处理循环 (为了方便、不需要存在复数)
            //iterate_battler((int)this.parameters[0], (int)this.parameters[1]) do |battler|
            //  {  // 战斗者存在的情况下
            //  if(battler.is_exist){
            //    // 设置行动
            //    battler.current_action.kind = (int)this.parameters[2];
            //    if(battler.current_action.kind == 0){
            //      battler.current_action.basic = (int)this.parameters[3];
            //    }else{
            //      battler.current_action.skill_id = (int)this.parameters[3];
            //    }
            //    // 设置行动对像
            //    if((int)this.parameters[4] == -2){
            //      if(battler is Game_Enemy){
            //        battler.current_action.decide_last_target_for_enemy();
            //      }else{
            //        battler.current_action.decide_last_target_for_actor();
            //      }
            //    }else if((int)this.parameters[4] == -1){
            //      if(battler is Game_Enemy){
            //        battler.current_action.decide_random_target_for_enemy();
            //      }else{
            //        battler.current_action.decide_random_target_for_actor();
            //      }
            //    }else if((int)this.parameters[4] >= 0){
            //      battler.current_action.target_index = (int)this.parameters[4];
            //    }
            //    // 设置强制标志
            //    battler.current_action.forcing = true;
            //    // 行动有效并且是 [立即执行] 的情况下
            //    if(battler.current_action.is_valid && this.parameters[5] == 1){
            //      // 设置强制对像的战斗者
            //      Global.game_temp.forcing_battler = battler;
            //      // 推进索引
            //      this.index += 1;
            //      // 结束
            //      return false;
            //    }
            //  }
            //}
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 战斗中断
        //--------------------------------------------------------------------------
        public bool command_340()
        {
            // 设置战斗中断标志
            Global.game_temp.battle_abort = true;
            // 推进索引
            this.index += 1;
            // 结束
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 调用菜单画面
        //--------------------------------------------------------------------------
        public bool command_351()
        {
            // 设置战斗中断标志
            Global.game_temp.battle_abort = true;
            // 设置调用菜单标志
            Global.game_temp.menu_calling = true;
            // 推进索引
            this.index += 1;
            // 结束
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 调用存档画面
        //--------------------------------------------------------------------------
        public bool command_352()
        {
            // 设置战斗中断标志
            Global.game_temp.battle_abort = true;
            // 设置调用存档标志
            Global.game_temp.save_calling = true;
            // 推进索引
            this.index += 1;
            // 结束
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 游戏结束
        //--------------------------------------------------------------------------
        public bool command_353()
        {
            // 设置游戏结束标志
            Global.game_temp.gameover = true;
            // 结束
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 返回标题画面
        //--------------------------------------------------------------------------
        public bool command_354()
        {
            // 设置返回标题画面标志
            Global.game_temp.to_title = true;
            // 结束
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 脚本
        //--------------------------------------------------------------------------
        public bool command_355()
        {
            // script 设置第一行
            var script = this.list[this.index].parameters[0] + "\n";
            // 循环
            while (true)
            {
                {
                    // 下一个事件指令在脚本 2 行以上的情况下
                    if (this.list[this.index + 1].code == 655)
                    {
                        // 添加到 script 2 行以后
                        script += this.list[this.index + 1].parameters[0] + "\n";
                        // 事件指令不在脚本 2 行以上的情况下
                    }
                    else
                    {
                        // 中断循环
                        break;
                    }
                    // 推进索引
                    this.index += 1;
                }
                throw new Exception();

                // 评价
                //var result = eval(script);
                //// 返回值为 false 的情况下
                //if(result == false){
                //  // 结束
                //  return false;
                //}
                // 继续
            }
            return true;

        }

    }
}
