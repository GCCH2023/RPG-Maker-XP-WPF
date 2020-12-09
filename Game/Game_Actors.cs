using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_Actors
    //------------------------------------------------------------------------------
    // 　处理角色排列的类。本类的实例请参考
    //  Global.game_actors。
    //==============================================================================

    public class Game_Actors
    {
        Dictionary<int, Game_Actor> data = new Dictionary<int, Game_Actor>();
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Game_Actors()
        {

        }
        //--------------------------------------------------------------------------
        // ● 获取角色
        //     actor_id : 角色 ID
        //--------------------------------------------------------------------------
        public Game_Actor this[int actor_id]
        {
            get
            {
                if (actor_id > 999 || Global.data_actors[actor_id] == null)
                    return null;
                
                if(!this.data.ContainsKey(actor_id))
                    this.data[actor_id] = new Game_Actor(actor_id);
                return this.data[actor_id];
            }
        }
    }

}
