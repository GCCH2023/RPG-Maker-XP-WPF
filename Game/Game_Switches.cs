using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_Switches
    //------------------------------------------------------------------------------
    // 　处理开关的类。编入的是类 Array 的外壳。本类的实例请参考
    // Global.game_switches。
    //==============================================================================

    public class Game_Switches
    {
        public bool[] data = new bool[5001];
        //--------------------------------------------------------------------------
        // ● 获取开关
        //     switch_id : 开关 ID
        //--------------------------------------------------------------------------
        //--------------------------------------------------------------------------
        // ● 设置开关
        //     switch_id : 开关 ID
        //     value     : ON (true) / OFF (false)
        //--------------------------------------------------------------------------

        public bool this[int switch_id]
        {
            get
            {
                if (switch_id <= 5000) // && this.data[switch_id] != null)
                    return this.data[switch_id];
                else
                    return false;
            }
            set
            {
                if (switch_id <= 5000)
                    this.data[switch_id] = value;
            }
        }
    }
}
