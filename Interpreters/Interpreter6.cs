using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Interpreter (分割定义 6)
    //------------------------------------------------------------------------------
    // 　执行事件命令的解释器。本类在 Game_System 类
    // 和 Game_Event 类的内部使用。
    //==============================================================================

    public partial class Interpreter
    {
        //--------------------------------------------------------------------------
        // ● 战斗处理
        //--------------------------------------------------------------------------
        public bool command_301()
        {
            // 如果不是无效的队伍
            if (Global.data_troops[(int)this.parameters[0]] != null)
            {
                // 设置中断战斗标志
                Global.game_temp.battle_abort = true;
                // 设置战斗调用标志
                Global.game_temp.battle_calling = true;
                Global.game_temp.battle_troop_id = (int)this.parameters[0];
                Global.game_temp.battle_can_escape = (bool)this.parameters[1];
                Global.game_temp.battle_can_lose = (bool)this.parameters[2];
                // 设置返回调用
                var current_indent = this.list[this.index].indent;
                return true;

                //Global.game_temp.battle_proc = Proc.new { |n| this.branch[current_indent] = n };
            }
            // 推进索引
            this.index += 1;
            // 结束
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 胜利的情况下
        //--------------------------------------------------------------------------
        public bool command_601()
        {
            // @@
             //战斗结果为胜利的情况下
            //if (this.branch[this.list[this.index].indent] == 0)
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
        // ● 逃跑的情况下
        //--------------------------------------------------------------------------
        public bool command_602()
        {
            // @@
            // 战斗结果为逃跑的情况下
            //if (this.branch[this.list[this.index].indent] == 1)
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
        // ● 失败的情况下
        //--------------------------------------------------------------------------
        public bool command_603()
        {
            // @@
            // 战斗结果为失败的情况下
            //if (this.branch[this.list[this.index].indent] == 2)
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
        // ● 商店的处理
        //--------------------------------------------------------------------------
        public bool command_302()
        {
            // 设置战斗中断标志
            Global.game_temp.battle_abort = true;
            // 设置商店调用标志
            Global.game_temp.shop_calling = true;
            // 设置商品列表的新项目
            Global.game_temp.shop_goods = new List<List<object>>() { this.parameters };
            // 循环
            while (true)
            {
                {
                    // 推进索引
                    this.index += 1;
                    // 下一个事件命令在商店两行以上的情况下
                    if (this.list[this.index].code == 605)
                    {
                        // 在商品列表中添加新项目
                        Global.game_temp.shop_goods.Add(this.list[this.index].parameters);
                    }
                    // 事件命令不在商店两行以上的情况下
                    else
                    {
                        // 技术
                        return false;
                    }
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 名称输入处理
        //--------------------------------------------------------------------------
        public bool command_303()
        {
            // 如果不是无效的角色
            if (Global.data_actors[(int)this.parameters[0]] != null)
            {
                // 设置战斗中断标志
                Global.game_temp.battle_abort = true;
                // 设置名称输入调用标志
                Global.game_temp.name_calling = true;
                Global.game_temp.name_actor_id = (int)this.parameters[0];
                Global.game_temp.name_max_char = (int)this.parameters[1];
            }
            // 推进索引
            this.index += 1;
            // 结束
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 增减 HP
        //--------------------------------------------------------------------------
        public bool command_311()
        {
            // 获取操作值
            var value = operate_value((int)this.parameters[1], (int)this.parameters[2], (int)this.parameters[3]);
            throw new Exception();
            // 处理重复
            //iterate_actor((int)this.parameters[0]) do |actor|
            //  {
            //  // HP 不为 0 的情况下
            //  if(actor.hp > 0){
            //    // 更改 HP (如果不允许战斗不能的状态就设置为 1)
            //    if((int)this.parameters[4] == false && actor.hp + value <= 0){
            //      actor.hp = 1;
            //    }
            //    else{
            //      actor.hp += value;
            //    }
            //  }
            //}
            // 游戏结束判定
            Global.game_temp.gameover = Global.game_party.is_all_dead;
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 增减 SP
        //--------------------------------------------------------------------------
        public bool command_312()
        {
            // 获取操作值
            var value = operate_value((int)this.parameters[1], (int)this.parameters[2], (int)this.parameters[3]);
            throw new Exception();

            // 处理重复
            //iterate_actor((int)this.parameters[0]) do |actor|
            //  {
            //  // 更改角色的 SP
            //  actor.sp += value;
            //}
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 更改状态
        //--------------------------------------------------------------------------
        public bool command_313()
        {
            throw new Exception();

            // 处理重复
            //iterate_actor((int)this.parameters[0]) do |actor|
            //  {  // 更改状态
            //  if((int)this.parameters[1] == 0){
            //    actor.add_state((int)this.parameters[2]);
            //  }
            //  else
            //  {
            //    actor.remove_state((int)this.parameters[2]);
            //  }
            //}
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 全回复
        //--------------------------------------------------------------------------
        public bool command_314()
        {
            throw new Exception();

            // 处理重复
            //iterate_actor((int)this.parameters[0]) do |actor|
            //  {
            //  // 角色全回复
            //  actor.recover_all();
            //}
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 增减 EXP
        //--------------------------------------------------------------------------
        public bool command_315()
        {
            // 获取操作值
            var value = operate_value((int)this.parameters[1], (int)this.parameters[2], (int)this.parameters[3]);
            throw new Exception();

            // 处理重复
            //iterate_actor((int)this.parameters[0]) do |actor|
            //  {  // 更改角色 EXP
            //  actor.exp += value;
            //}
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 增减等级
        //--------------------------------------------------------------------------
        public bool command_316()
        {
            // 获取操作值
            var value = operate_value((int)this.parameters[1], (int)this.parameters[2], (int)this.parameters[3]);
            throw new Exception();

            // 处理重复
            //iterate_actor((int)this.parameters[0]) do |actor|
            //  {   // 更改角色的等级
            //  actor.level += value;
            //}
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 增减能力值
        //--------------------------------------------------------------------------
        public bool command_317()
        {
            // 获取操作值
            var value = operate_value((int)this.parameters[2], (int)this.parameters[3], (int)this.parameters[4]);
            // 获取角色
            var actor = Global.game_actors[(int)this.parameters[0]];
            // 更改能力值
            if (actor != null)
            {
                switch ((int)this.parameters[1])
                {
                    case 0:  // MaxHP
                        actor.maxhp += value; break;
                    case 1:  // MaxSP
                        actor.maxsp += value; break;
                    case 2:  // 力量
                        actor.str += value; break;
                    case 3:  // 灵巧
                        actor.dex += value; break;
                    case 4:  // 速度
                        actor.agi += value; break;
                    case 5:  // 魔力
                        actor.int1 += value; break;
                }
            }
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 增减特技
        //--------------------------------------------------------------------------
        public bool command_318()
        {
            // 获取角色
            var actor = Global.game_actors[(int)this.parameters[0]];
            // 增减特技
            if (actor != null)
            {
                if ((int)this.parameters[1] == 0)
                {
                    actor.learn_skill((int)this.parameters[2]);
                }
                else
                {
                    actor.forget_skill((int)this.parameters[2]);
                }
            }
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 变更装备
        //--------------------------------------------------------------------------
        public bool command_319()
        {
            // 获取角色
            var actor = Global.game_actors[(int)this.parameters[0]];
            // 变更角色
            if (actor != null)
            {
                actor.equip((int)this.parameters[1], (int)this.parameters[2]);
            }
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 更改角色的名字
        //--------------------------------------------------------------------------
        public bool command_320()
        {
            // 获取角色
            var actor = Global.game_actors[(int)this.parameters[0]];
            // 更改名字
            if (actor != null)
            {
                actor.name = (string)this.parameters[1];
            }
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 更改角色的职业
        //--------------------------------------------------------------------------
        public bool command_321()
        {
            // 获取角色
            var actor = Global.game_actors[(int)this.parameters[0]];
            // 更改职业
            if (actor != null)
            {
                actor.class_id = (int)this.parameters[1];
            }
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 更改角色的图形
        //--------------------------------------------------------------------------
        public bool command_322()
        {
            // 获取角色
            var actor = Global.game_actors[(int)this.parameters[0]];
            // 更改图形
            if (actor != null)
            {
                actor.set_graphic((string)this.parameters[1], (int)this.parameters[2],
                  (string)this.parameters[3], (int)this.parameters[4]);
            }
            // 刷新角色
            Global.game_player.refresh();
            // 继续
            return true;
        }
    }

}
