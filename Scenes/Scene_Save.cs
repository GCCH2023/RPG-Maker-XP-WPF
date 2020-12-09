using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_Save
    //------------------------------------------------------------------------------
    // 　处理存档画面的类。
    //==============================================================================

    public class Scene_Save : Scene_File
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Scene_Save() :
            base("要保存到这个文件吗？")
        {
        }
        //--------------------------------------------------------------------------
        // ● 确定时的处理
        //--------------------------------------------------------------------------
        public override void on_decision(string filename)
        {
            // 演奏存档 SE
            Global.game_system.se_play(Global.data_system.save_se);
            // 写入存档数据
            var file = File.open(filename, "wb");
            write_save_data(file);
            file.close();
            // 如果被事件调用
            if (Global.game_temp.save_calling)
            {
                // 清除存档调用标志
                Global.game_temp.save_calling = false;
                // 切换到地图画面
                Global.scene = new Scene_Map();
                return;
            }
            // 切换到菜单画面
            Global.scene = new Scene_Menu(4);
        }
        //--------------------------------------------------------------------------
        // ● 取消时的处理
        //--------------------------------------------------------------------------
        public override void on_cancel()
        {
            // 演奏取消 SE
            Global.game_system.se_play(Global.data_system.cancel_se);
            // 如果被事件调用
            if (Global.game_temp.save_calling)
            {
                // 清除存档调用标志
                Global.game_temp.save_calling = false;
                // 切换到地图画面
                Global.scene = new Scene_Map();
                return;
            }
            // 切换到菜单画面
            Global.scene = new Scene_Menu(4);
        }
        //--------------------------------------------------------------------------
        // ● 写入存档数据
        //     file : 写入用文件对像 (已经打开)
        //--------------------------------------------------------------------------
        public void write_save_data(File file)
        {
            // 生成描绘存档文件用的角色图形
            var characters = new List<object>();
            for (var i = 0; i < Global.game_party.actors.Count; i++)
            {
                var actor = Global.game_party.actors[i];
                characters.Add(new { actor.character_name, actor.character_hue });
            }
            // 写入描绘存档文件用的角色数据
            Marshal.dump(characters, file);
            // 写入测量游戏时间用画面计数
            Marshal.dump(Graphics.frame_count, file);
            // 增加 1 次存档次数
            Global.game_system.save_count += 1;
            // 保存魔法编号
            // (将编辑器保存的值以随机值替换)
            Global.game_system.magic_number = Global.data_system.magic_number;
            // 写入各种游戏对像
            Marshal.dump(Global.game_system, file);
            Marshal.dump(Global.game_switches, file);
            Marshal.dump(Global.game_variables, file);
            Marshal.dump(Global.game_self_switches, file);
            Marshal.dump(Global.game_screen, file);
            Marshal.dump(Global.game_actors, file);
            Marshal.dump(Global.game_party, file);
            Marshal.dump(Global.game_troop, file);
            Marshal.dump(Global.game_map, file);
            Marshal.dump(Global.game_player, file);
        }
    }

}
