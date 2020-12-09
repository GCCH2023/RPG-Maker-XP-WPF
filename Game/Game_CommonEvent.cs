using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
  //==============================================================================
// ■ Game_CommonEvent
//------------------------------------------------------------------------------
// 　处理公共事件的类。包含执行并行事件的功能。
// 本类在 Game_Map 类 (Global.game_map) 的内部使用。
//==============================================================================

    public class Game_CommonEvent
    {
        int common_event_id;
        Interpreter interpreter;
        //--------------------------------------------------------------------------
        // ● 初始对像
        //     common_event_id : 公共事件 ID
        //--------------------------------------------------------------------------
        public Game_CommonEvent(int common_event_id)
        {

            this.common_event_id = common_event_id;
            this.interpreter = null;
            refresh();
        }
        //--------------------------------------------------------------------------
        // ● 获取名称
        //--------------------------------------------------------------------------
        public string name
        {
            get
            {
                return Global.data_common_events[this.common_event_id].name;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取目标
        //--------------------------------------------------------------------------
        public int trigger
        {
            get
            {
                return Global.data_common_events[this.common_event_id].trigger;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取条件开关 ID
        //--------------------------------------------------------------------------
        public int switch_id
        {
            get
            {
                return Global.data_common_events[this.common_event_id].switch_id;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取执行内容
        //--------------------------------------------------------------------------
        public List<RPG.EventCommand> list
        {
            get
            {
                return Global.data_common_events[this.common_event_id].list;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public void refresh()
        {
            // 建立必须的处理并行事件用的解释器
            if (this.trigger == 2 && Global.game_switches[this.switch_id] == true)
            {
                if (this.interpreter == null)
                    this.interpreter = new Interpreter();
            }
            else
                this.interpreter = null;
        }
        //--------------------------------------------------------------------------
        // ● 更新画面
        //--------------------------------------------------------------------------
        public virtual void update()
        {
            // 并行处理有效的情况下
            if (this.interpreter != null)
            {
                // 如果不是在执行中就设置
                if (!this.interpreter.is_running)
                    this.interpreter.setup(this.list, 0);
                // 更新解释器
                this.interpreter.update();
            }
        }
    }
}
