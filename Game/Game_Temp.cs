using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_Temp
    //------------------------------------------------------------------------------
    // 　在没有存档的情况下，处理临时数据的类。这个类的实例请参考
    // Global.game_temp 。
    //==============================================================================

    public class Game_Temp
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public RPG.AudioFile map_bgm = null;// 地图画面 RPG.AudioFile (战斗时记忆用)
        public string message_text = null;// 信息文章
        public object message_proc = null;// 信息 返回调用 (Proc)
        public int choice_start = 99;// 选择项 开始行
        public int choice_max = 0;// 选择项 项目数
        public int choice_cancel_type = 0;// 选择项 取消的情况
        public object choice_proc = null;// 选择项 返回调用 (Proc)
        public int num_input_start = 99;// 输入数值 开始行
        public int num_input_variable_id = 0;// 输入数值 变量 ID
        public int num_input_digits_max = 0;// 输入数值 位数
        public bool message_window_showing = false;// 显示信息窗口
        public int common_event_id = 0;// 公共事件 ID
        // 战斗中的标志
        public bool in_battle = false;
        public bool battle_calling = false;// 调用战斗的标志
        public int battle_troop_id = 0;// 战斗 队伍 ID
        public bool battle_can_escape = false;// 战斗中 允许逃跑 ID
        public bool battle_can_lose = false;// 战斗中 允许失败 ID
        public object battle_proc = null;// 战斗 返回调用 (Proc)
        public int battle_turn = 0;// 战斗 回合数
        public List<bool> battle_event_flags = new List<bool>();// = {} 战斗 事件执行执行完毕的标志
        public bool battle_abort = false;// 战斗 中断标志
        public bool battle_main_phase = false;// 战斗 状态标志
        public string battleback_name;// 战斗背景 文件名
        public Game_Battler forcing_battler = null;// 强制行动的战斗者
        public bool shop_calling = false;// 调用商店的标志
        public List<List<object>> shop_goods = null;// 商店 商品列表
        public bool name_calling = false;// 输入名称 调用标志
        public int name_actor_id = 0;// 输入名称 角色 ID
        public int name_max_char = 0;// 输入名称 最大字数
        public bool menu_calling = false;// 菜单 调用标志
        public bool menu_beep = false;// 菜单 SE 演奏标志
        public bool save_calling = false;// 存档 调用标志
        public bool debug_calling = false;// 调试 调用标志
        // 主角 场所移动标志
        bool _player_transferring = false;

        public bool player_transferring
        {
            get { return _player_transferring; }
            set { _player_transferring = value; }
        }
        public int player_new_map_id = 0;// 主角 移动目标地图 ID
        public int player_new_x = 0;// 主角 移动目标 X 坐标
        public int player_new_y = 0;// 主角 移动目标 Y 坐标
        public int player_new_direction = 0;// 主角 移动目标 朝向
        public bool transition_processing = false;// 过渡处理中标志
        public string transition_name;// 过渡 文件名
        public bool gameover = false;// 游戏结束标志
        public bool to_title = false;// 返回标题画面标志
        public int last_file_index = 0;// 最后存档的文件编号
        public int debug_top_row = 0;// 调试画面 保存状态用
        public int debug_index = 0;// 调试画面 保存状态用
    }
}
