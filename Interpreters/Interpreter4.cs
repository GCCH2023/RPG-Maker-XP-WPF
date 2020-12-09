using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Interpreter (分割定义 4)
    //------------------------------------------------------------------------------
    // 　执行事件命令的解释器。本类在 Game_System 类
    // 和 Game_Event 类的内部使用。
    //==============================================================================

    public partial class Interpreter
    {
        //--------------------------------------------------------------------------
        // ● 开关操作
        //--------------------------------------------------------------------------
        public bool command_121()
        {
            // 循环全部操作
            for (var i = (int)this.parameters[0]; i <= (int)this.parameters[1]; i++)
            {
                // 更改开关
                Global.game_switches[i] = ((int)this.parameters[2] == 0);
            }
            // 刷新地图
            Global.game_map.need_refresh = true;
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 变量操作
        //--------------------------------------------------------------------------
        public bool command_122()
        {
            // 初始化值
            var value = 0.0;
            // 操作数的分支
            switch ((int)this.parameters[3])
            {
                case 0:  // 恒量
                    value = (int)this.parameters[4]; break;
                case 1:  // 变量
                    value = Global.game_variables[(int)this.parameters[4]]; break;
                case 2:  // 随机数
                    value = (int)this.parameters[4] + Global.rand((int)this.parameters[5] - (int)this.parameters[4] + 1); break;
                case 3:  // 物品
                    value = Global.game_party.item_number((int)this.parameters[4]); break;
                case 4:  // 角色
                    {
                        var actor = Global.game_actors[(int)this.parameters[4]];
                        if (actor != null)
                        {
                            switch ((int)this.parameters[5])
                            {
                                case 0:  // 等级
                                    value = actor.level; break;
                                case 1:  // EXP
                                    value = actor.exp; break;
                                case 2:  // HP
                                    value = actor.hp; break;
                                case 3:  // SP
                                    value = actor.sp; break;
                                case 4:  // MaxHP
                                    value = actor.maxhp; break;
                                case 5:  // MaxSP
                                    value = actor.maxsp; break;
                                case 6:  // 力量
                                    value = actor.str; break;
                                case 7:  // 灵巧
                                    value = actor.dex; break;
                                case 8:  // 速度
                                    value = actor.agi; break;
                                case 9:  // 魔力
                                    value = actor.int1; break;
                                case 10:  // 攻击力
                                    value = actor.atk; break;
                                case 11:  // 物理防御
                                    value = actor.pdef; break;
                                case 12:  // 魔法防御
                                    value = actor.mdef; break;
                                case 13:  // 回避修正
                                    value = actor.eva; break;
                            }
                        }
                    }
                    break;
                case 5:  // 敌人
                    {
                        var enemy = Global.game_troop.enemies[(int)this.parameters[4]];
                        if (enemy != null)
                        {
                            switch ((int)this.parameters[5])
                            {
                                case 0:  // HP
                                    value = enemy.hp; break;
                                case 1:  // SP
                                    value = enemy.sp; break;
                                case 2:  // MaxHP
                                    value = enemy.maxhp; break;
                                case 3:  // MaxSP
                                    value = enemy.maxsp; break;
                                case 4:  // 力量
                                    value = enemy.str; break;
                                case 5:  // 灵巧
                                    value = enemy.dex; break;
                                case 6:  // 速度
                                    value = enemy.agi; break;
                                case 7:  // 魔力
                                    value = enemy.int1; break;
                                case 8:  // 攻击力
                                    value = enemy.atk; break;
                                case 9:  // 物理防御
                                    value = enemy.pdef; break;
                                case 10:  // 魔法防御
                                    value = enemy.mdef; break;
                                case 11:  // 回避修正
                                    value = enemy.eva; break;
                            }
                        }
                    }
                    break;
                case 6:  // 角色
                    {
                        var character = get_character((int)this.parameters[4]);
                        if (character != null)
                        {
                            switch ((int)this.parameters[5])
                            {
                                case 0:  // X 坐标
                                    value = character.x; break;
                                case 1:  // Y 坐标
                                    value = character.y; break;
                                case 2:  // 朝向
                                    value = character.direction; break;
                                case 3:  // 画面 X 坐标
                                    value = character.screen_x; break;
                                case 4:  // 画面 Y 坐标
                                    value = character.screen_y; break;
                                case 5:  // 地形标记
                                    value = character.terrain_tag; break;
                            }
                        }
                    }
                    break;
                case 7:  // 其它
                    switch ((int)this.parameters[4])
                    {
                        case 0:  // 地图 ID
                            value = Global.game_map.map_id; break;
                        case 1:  // 同伴人数
                            value = Global.game_party.actors.Count; break;
                        case 2:  // 金钱
                            value = Global.game_party.gold; break;
                        case 3:  // 步数
                            value = Global.game_party.steps; break;
                        case 4:  // 游戏时间
                            value = Graphics.frame_count / Graphics.frame_rate; break;
                        case 5:  // 计时器
                            value = Global.game_system.timer / Graphics.frame_rate; break;
                        case 6:  // 存档次数
                            value = Global.game_system.save_count; break;
                    }
                    break;
            }
            // 循环全部操作
            for (var i = (int)this.parameters[0]; i <= (int)this.parameters[1]; i++)
            {
                // 操作分支
                switch ((int)this.parameters[2])
                {
                    case 0:  // 代入
                        Global.game_variables[i] = (int)value; break;
                    case 1:  // 加法
                        Global.game_variables[i] += (int)value; break;
                    case 2:  // 减法
                        Global.game_variables[i] -= (int) value; break;
                    case 3:  // 乘法
                        Global.game_variables[i] *= (int)value; break;
                    case 4:  // 除法
                        if (value != 0)
                        {
                            Global.game_variables[i] /= (int)value;
                        }
                        break;
                    case 5:  // 剩余
                        if (value != 0)
                        {
                            Global.game_variables[i] %= (int)value;
                        }
                        break;
                }
                // 检查上限
                if (Global.game_variables[i] > 99999999)
                {
                    Global.game_variables[i] = 99999999;
                }
                // 检查下限
                if (Global.game_variables[i] < -99999999)
                {
                    Global.game_variables[i] = -99999999;
                }
            }
            // 刷新地图
            Global.game_map.need_refresh = true;
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 独立开关操作
        //--------------------------------------------------------------------------
        public bool command_123(){
    // 事件 ID 有效的情况下
    if(this.event_id > 0){
      // 生成独立开关
      var key = new int[]{Global.game_map.map_id, this.event_id, (int)this.parameters[0]};
      // 更改独立开关
      Global.game_self_switches[key] = ((int)this.parameters[1] == 0);
    }
    // 刷新地图
    Global.game_map.need_refresh = true;
    // 继续
    return true;
  }
        //--------------------------------------------------------------------------
        // ● 计时器操作
        //--------------------------------------------------------------------------
        public bool command_124()
        {
            // 开始的情况
            if ((int)this.parameters[0] == 0)
            {
                Global.game_system.timer = (int)this.parameters[1] * Graphics.frame_rate;
                Global.game_system.timer_working = true;
            }
            // 停止的情况
            if ((int)this.parameters[0] == 1)
            {
                Global.game_system.timer_working = false;
            }
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 增减金钱
        //--------------------------------------------------------------------------
        public bool command_125()
        {
            // 获取要操作的值
            var value = operate_value((int)this.parameters[0], (int)this.parameters[1], (int)this.parameters[2]);
            // 增减金钱
            Global.game_party.gain_gold(value);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 增减物品
        //--------------------------------------------------------------------------
        public bool command_126()
        {
            // 获取要操作的值
            var value = operate_value((int)this.parameters[1], (int)this.parameters[2], (int)this.parameters[3]);
            // 增减物品
            Global.game_party.gain_item((int)this.parameters[0], value);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 增减武器
        //--------------------------------------------------------------------------
        public bool command_127()
        {
            // 获取要操作的值
            var value = operate_value((int)this.parameters[1], (int)this.parameters[2], (int)this.parameters[3]);
            // 增减武器
            Global.game_party.gain_weapon((int)this.parameters[0], value);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 增减防具
        //--------------------------------------------------------------------------
        public bool command_128()
        {
            // 获取要操作的值
            var value = operate_value((int)this.parameters[1], (int)this.parameters[2], (int)this.parameters[3]);
            // 增减防具
            Global.game_party.gain_armor((int)this.parameters[0], value);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 角色的替换
        //--------------------------------------------------------------------------
        public bool command_129()
        {
            // 获取角色
            var actor = Global.game_actors[(int)this.parameters[0]];
            // 角色有效的情况下
            if (actor != null)
            {
                // 操作分支
                if ((int)this.parameters[1] == 0)
                {
                    if ((int)this.parameters[2] == 1)
                    {
                        Global.game_actors[(int)this.parameters[0]].setup((int)this.parameters[0]);
                    }
                    Global.game_party.add_actor((int)this.parameters[0]);
                }
                else
                {
                    Global.game_party.remove_actor((int)this.parameters[0]);
                }
            }
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 更改窗口外观
        //--------------------------------------------------------------------------
        public bool command_131()
        {
            // 设置窗口外观文件名
            Global.game_system.windowskin_name = (string)this.parameters[0];
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 更改战斗 RPG.AudioFile
        //--------------------------------------------------------------------------
        public bool command_132()
        {
            // 设置战斗 RPG.AudioFile
            Global.game_system.battle_bgm = (RPG.AudioFile)this.parameters[0];
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 更改战斗结束的 ME
        //--------------------------------------------------------------------------
        public bool command_133()
        {
            // 设置战斗结束的 ME
            Global.game_system.battle_end_me = (RPG.AudioFile)this.parameters[0];
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 更改禁止存档
        //--------------------------------------------------------------------------
        public bool command_134()
        {
            // 更改禁止存档标志
            Global.game_system.save_disabled = ((int)this.parameters[0] == 0);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 更改禁止菜单
        //--------------------------------------------------------------------------
        public bool command_135()
        {
            // 更改禁止菜单标志
            Global.game_system.menu_disabled = ((int)this.parameters[0] == 0);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 更改禁止遇敌
        //--------------------------------------------------------------------------
        public bool command_136()
        {
            // 更改更改禁止遇敌标志
            Global.game_system.encounter_disabled = ((int)this.parameters[0] == 0);
            // 生成遇敌计数
            Global.game_player.make_encounter_count();
            // 继续
            return true;
        }
    }

}
