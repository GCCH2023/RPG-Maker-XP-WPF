using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
   //==============================================================================
// ■ Interpreter (分割定义 2)
//------------------------------------------------------------------------------
// 　执行时间命令的解释器。本类在 Game_System 类
// 和 Game_Event 类的内部使用。
//==============================================================================

    public partial class Interpreter
    {
        //--------------------------------------------------------------------------
        // ● 执行事件命令
        //--------------------------------------------------------------------------
        public bool execute_command()
        {
            // 到达执行内容列表末尾的情况下
            if (this.index >= this.list.Count - 1)
            {
                // 时间结束
                command_end();
                // 继续
                return true;
            }
            // 事件命令的功能可以参考 this.parameters
            this.parameters = this.list[this.index].parameters;
            // 命令代码分支
            switch (this.list[this.index].code)
            {
                case 101:  // 文章的显示
                    return command_101();
                case 102:  // 显示选择项
                    return command_102();
                case 402:  // [**] 的情况下
                    return command_402();
                case 403:  // 取消的情况下
                    return command_403();
                case 103:  // 处理数值输入
                    return command_103();
                case 104:  // 更改文章选项
                    return command_104();
                case 105:  // 处理按键输入
                    return command_105();
                case 106:  // 等待
                    return command_106();
                case 111:  // 条件分支
                    return command_111();
                case 411:  // 这以外的情况
                    return command_411();
                case 112:  // 循环
                    return command_112();
                case 413:  // 重复上次
                    return command_413();
                case 113:  // 中断循环
                    return command_113();
                case 115:  // 中断时间处理
                    return command_115();
                case 116:  // 暂时删除事件
                    return command_116();
                case 117:  // 公共事件
                    return command_117();
                case 118:  // 标签
                    return command_118();
                case 119:  // 标签跳转
                    return command_119();
                case 121:  // 操作开关
                    return command_121();
                case 122:  // 操作变量
                    return command_122();
                case 123:  // 操作独立开关
                    return command_123();
                case 124:  // 操作计时器
                    return command_124();
                case 125:  // 增减金钱
                    return command_125();
                case 126:  // 增减物品
                    return command_126();
                case 127:  // 增减武器
                    return command_127();
                case 128:  // 增减防具
                    return command_128();
                case 129:  // 替换角色
                    return command_129();
                case 131:  // 更改窗口外关
                    return command_131();
                case 132:  // 更改战斗 RPG.AudioFile
                    return command_132();
                case 133:  // 更改战斗结束 BGS
                    return command_133();
                case 134:  // 更改禁止保存
                    return command_134();
                case 135:  // 更改禁止菜单
                    return command_135();
                case 136:  // 更改禁止遇敌
                    return command_136();
                case 201:  // 场所移动
                    return command_201();
                case 202:  // 设置事件位置
                    return command_202();
                case 203:  // 地图滚动
                    return command_203();
                case 204:  // 更改地图设置
                    return command_204();
                case 205:  // 更改雾的色调
                    return command_205();
                case 206:  // 更改雾的不透明度
                    return command_206();
                case 207:  // 显示动画
                    return command_207();
                case 208:  // 更改透明状态
                    return command_208();
                case 209:  // 设置移动路线
                    return command_209();
                case 210:  // 移动结束后等待
                    return command_210();
                case 221:  // 准备过渡
                    return command_221();
                case 222:  // 执行过渡
                    return command_222();
                case 223:  // 更改画面色调
                    return command_223();
                case 224:  // 画面闪烁
                    return command_224();
                case 225:  // 画面震动
                    return command_225();
                case 231:  // 显示图片
                    return command_231();
                case 232:  // 移动图片
                    return command_232();
                case 233:  // 旋转图片
                    return command_233();
                case 234:  // 更改色调
                    return command_234();
                case 235:  // 删除图片
                    return command_235();
                case 236:  // 设置天候
                    return command_236();
                case 241:  // 演奏 RPG.AudioFile
                    return command_241();
                case 242:  // RPG.AudioFile 的淡入淡出
                    return command_242();
                case 245:  // 演奏 BGS
                    return command_245();
                case 246:  // BGS 的淡入淡出
                    return command_246();
                case 247:  // 记忆 RPG.AudioFile / BGS
                    return command_247();
                case 248:  // 还原 RPG.AudioFile / BGS
                    return command_248();
                case 249:  // 演奏 ME
                    return command_249();
                case 250:  // 演奏 SE
                    return command_250();
                case 251:  // 停止 SE
                    return command_251();
                case 301:  // 战斗处理
                    return command_301();
                case 601:  // 胜利的情况
                    return command_601();
                case 602:  // 逃跑的情况
                    return command_602();
                case 603:  // 失败的情况
                    return command_603();
                case 302:  // 商店的处理
                    return command_302();
                case 303:  // 名称输入的处理
                    return command_303();
                case 311:  // 增减 HP
                    return command_311();
                case 312:  // 增减 SP
                    return command_312();
                case 313:  // 更改状态
                    return command_313();
                case 314:  // 全回复
                    return command_314();
                case 315:  // 增减 EXP
                    return command_315();
                case 316:  // 増減 等级
                    return command_316();
                case 317:  // 増減 能力值
                    return command_317();
                case 318:  // 增减特技
                    return command_318();
                case 319:  // 变更装备
                    return command_319();
                case 320:  // 更改角色名字
                    return command_320();
                case 321:  // 更改角色职业
                    return command_321();
                case 322:  // 更改角色图形
                    return command_322();
                case 331:  // 増減敌人的 HP
                    return command_331();
                case 332:  // 増減敌人的 SP
                    return command_332();
                case 333:  // 更改敌人的状态
                    return command_333();
                case 334:  // 敌人出现
                    return command_334();
                case 335:  // 敌人变身
                    return command_335();
                case 336:  // 敌人全回复
                    return command_336();
                case 337:  // 显示动画
                    return command_337();
                case 338:  // 伤害处理
                    return command_338();
                case 339:  // 强制行动
                    return command_339();
                case 340:  // 战斗中断
                    return command_340();
                case 351:  // 调用菜单画面
                    return command_351();
                case 352:  // 调用存档画面
                    return command_352();
                case 353:  // 游戏结束
                    return command_353();
                case 354:  // 返回标题画面
                    return command_354();
                case 355:  // 脚本
                    return command_355();
                default:      // 其它
                    return true;
            }
        }
        //--------------------------------------------------------------------------
        // ● 事件结束
        //--------------------------------------------------------------------------
        public void command_end()
        {
            // 清除执行内容列表
            this.list = null;
            // 主地图事件与事件 ID 有效的情况下
            if (this.main && this.event_id > 0)
                // 解除事件锁定
                Global.game_map.events[this.event_id].unlock();
        }
        //--------------------------------------------------------------------------
        // ● 指令跳转
        //--------------------------------------------------------------------------
        public bool command_skip()
        {
            // 获取缩进
            var indent = this.list[this.index].indent;
            // 循环
            while (true)
            {
                // 下一个事件命令是同等级的缩进的情况下
                if (this.list[this.index + 1].indent == indent)
                    // 继续
                    return true;

                // 索引的下一个
                this.index += 1;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取角色
        //     parameter : 能力值
        //--------------------------------------------------------------------------
        public Game_Character get_character(int parameter)
        {
            // 能力值分支
            switch (parameter)
            {
                case -1:  // 角色
                    return Global.game_player;
                case 0:  // 本事件
                    {
                        var events = Global.game_map.events;
                        return events == null ? null : events[this.event_id];

                    }
                default:  // 特定的事件
                    {
                        var events = Global.game_map.events;
                        return events == null ? null : events[parameter];
                    }
            }
        }
        //--------------------------------------------------------------------------
        // ● 计算操作的值
        //     operation    : 操作
        //     operand_type : 操作数类型 (0:恒量 1:变量)
        //     operand      : 操作数 (数值是变量 ID)
        //--------------------------------------------------------------------------
        public int operate_value(int operation, int operand_type, int operand)
        {
            var value = 0;
            // 获取操作数
            if (operand_type == 0)
                value = operand;
            else
                value = Global.game_variables[operand];

            // 操作为 [减少] 的情况下反转实际符号
            if (operation == 1)
                value = -value;

            // 返回 value
            return value;
        }
    }
}
