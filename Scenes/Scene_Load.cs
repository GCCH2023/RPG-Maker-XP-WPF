using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_Load
    //------------------------------------------------------------------------------
    // 　处理读档画面的类。
    //==============================================================================

    public class Scene_Load : Scene_File
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Scene_Load()
            : base("要载入哪个文件？")
        {
            // 再生成临时对像
            Global.game_temp = new Game_Temp();
            // 选择存档时间最新的文件
            Global.game_temp.last_file_index = 0;
            DateTime latest_time = new DateTime(0); // Time.at(0);
            for (var i = 0; i <= 3; i++)
            {
                var filename = make_filename(i);
                if (File.is_exist(filename))
                {
                    var file = File.open(filename, "r");
                    if (file.mtime > latest_time)
                    {
                        latest_time = file.mtime;
                        Global.game_temp.last_file_index = i;
                    }
                    file.close();
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 确定时的处理
        //--------------------------------------------------------------------------
        public override void on_decision(string filename)
        {
            // 文件不存在的情况下
            if (!File.is_exist(filename))
            {
                // 演奏冻结 SE
                Global.game_system.se_play(Global.data_system.buzzer_se);
                return;
            }
            // 演奏读档 SE
            Global.game_system.se_play(Global.data_system.load_se);
            // 写入存档数据
            var file = File.open(filename, "rb");
            read_save_data(file);
            file.close();
            // 还原 RPG.AudioFile、BGS
            Global.game_system.bgm_play(Global.game_system.playing_bgm);
            Global.game_system.bgs_play(Global.game_system.playing_bgs);
            // 刷新地图 (执行并行事件)
            Global.game_map.update();
            // 切换到地图画面
            Global.scene = new Scene_Map();
        }
        //--------------------------------------------------------------------------
        // ● 取消时的处理
        //--------------------------------------------------------------------------
        public override void on_cancel()
        {
            // 演奏取消 SE
            Global.game_system.se_play(Global.data_system.cancel_se);
            // 切换到标题画面
            Global.scene = new Scene_Title();
        }
        //--------------------------------------------------------------------------
        // ● 读取存档数据
        //     file : 读取用文件对像 (已经打开)
        //--------------------------------------------------------------------------
        public void read_save_data(File file)
        {
            // 读取描绘存档文件用的角色数据
            var characters = Marshal.load(file);
            // 读取测量游戏时间用画面计数
            Graphics.frame_count = (int)Marshal.load(file);
            // 读取各种游戏对像
            Global.game_system = (Game_System)Marshal.load(file);
            Global.game_switches = (Game_Switches)Marshal.load(file);
            Global.game_variables = (Game_Variables)Marshal.load(file);
            Global.game_self_switches = (Game_SelfSwitches)Marshal.load(file);
            Global.game_screen = (Game_Screen)Marshal.load(file);
            Global.game_actors = (Game_Actors)Marshal.load(file);
            Global.game_party = (Game_Party)Marshal.load(file);
            Global.game_troop = (Game_Troop)Marshal.load(file);
            Global.game_map = (Game_Map)Marshal.load(file);
            Global.game_player = (Game_Player)Marshal.load(file);
            // 魔法编号与保存时有差异的情况下
            // (加入编辑器的编辑过的数据)
            if (Global.game_system.magic_number != Global.data_system.magic_number)
            {
                // 重新装载地图
                Global.game_map.setup(Global.game_map.map_id);
                Global.game_player.center(Global.game_player.x, Global.game_player.y);
            }
            // 刷新同伴成员
            Global.game_party.refresh();
        }
    }

}
