﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
   //==============================================================================
// ■ Game_SelfSwitches
//------------------------------------------------------------------------------
// 　处理独立开关的类。编入的是类 Hash 的外壳。本类的实例请参考
// Global.game_self_switches。
//==============================================================================

    public class Game_SelfSwitches
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Dictionary<object, bool> data = new Dictionary<object, bool>();
        //--------------------------------------------------------------------------
        // ● 获取独立开关
        //     key : 键
        //--------------------------------------------------------------------------
        //--------------------------------------------------------------------------
        // ● 设置独立开关
        //     key   : 键
        //     value : ON (true) / OFF (false)
        //--------------------------------------------------------------------------

        public bool this[object key]
        {
            get
            {
                return this.data[key] == true ? true : false;
            }
            set
            {
                this.data[key] = value;
            }
        }
    }
}
