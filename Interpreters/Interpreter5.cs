using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace XP
{
    //==============================================================================
    // ■ Interpreter (分割定义 5)
    //------------------------------------------------------------------------------
    // 　执行事件命令的注释器。本类在 Game_System 类
    // 和 Game_Event 类的内部使用。
    //==============================================================================

    public partial class Interpreter
    {
        public List<object> parameters { get; set; }
        public int index { get; set; }
        //--------------------------------------------------------------------------
        // ● 场所移动
        //--------------------------------------------------------------------------
        public bool command_201()
        {
            // 战斗中的情况
            if (Global.game_temp.in_battle)
            {
                // 继续
                return true;
            }
            // 场所移动中、信息显示中、过渡处理中的情况下
            if (Global.game_temp.player_transferring ||
               Global.game_temp.message_window_showing ||
               Global.game_temp.transition_processing)
            {
                // 结束
                return false;
            }
            // 设置场所移动标志
            Global.game_temp.player_transferring = true;
            // 指定方法为 [直接指定] 的情况下
            if ((int)this.parameters[0] == 0)
            {
                // 设置主角的移动目标
                Global.game_temp.player_new_map_id = (int)this.parameters[1];
                Global.game_temp.player_new_x = (int)this.parameters[2];
                Global.game_temp.player_new_y = (int)this.parameters[3];
                Global.game_temp.player_new_direction = (int)this.parameters[4];
                // 指定方法为 [使用变量指定] 的情况下
            }
            else
            {
                // 设置主角的移动目标
                Global.game_temp.player_new_map_id = Global.game_variables[(int)this.parameters[1]];
                Global.game_temp.player_new_x = Global.game_variables[(int)this.parameters[2]];
                Global.game_temp.player_new_y = Global.game_variables[(int)this.parameters[3]];
                Global.game_temp.player_new_direction = (int)this.parameters[4];
            }
            // 推进索引
            this.index += 1;
            // 有淡入淡出的情况下
            if ((int)this.parameters[5] == 0)
            {
                // 准备过渡
                Graphics.freeze();
                // 设置过渡处理中标志
                Global.game_temp.transition_processing = true;
                Global.game_temp.transition_name = "";
            }
            // 结束
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 设置事件位置
        //--------------------------------------------------------------------------
        public bool command_202()
        {
            // 战斗中的情况下
            if (Global.game_temp.in_battle)
            {
                // 继续
                return true;
            }
            // 获取角色
            var character = get_character((int)this.parameters[0]);
            // 角色不存在的情况下
            if (character == null)
            {
                // 继续
                return true;
            }
            // 指定方法为 [直接指定] 的情况下
            if ((int)this.parameters[1] == 0)
            {
                // 设置角色的位置
                character.moveto((int)this.parameters[2], (int)this.parameters[3]);
            }
            // 指定方法为 [使用变量指定] 的情况下
            else if ((int)this.parameters[1] == 1)
            {
                // 设置角色的位置
                character.moveto(Global.game_variables[(int)this.parameters[2]],
                  Global.game_variables[(int)this.parameters[3]]);
                // 指定方法为 [与其它事件交换] 的情况下
            }
            else
            {
                var old_x = character.x;
                var old_y = character.y;
                var character2 = get_character((int)this.parameters[2]);
                if (character2 != null)
                {
                    character.moveto(character2.x, character2.y);
                    character2.moveto(old_x, old_y);
                }
            }
            // 设置角色的朝向
            switch ((int)this.parameters[4])
            {
                case 8:  // 上
                    character.turn_up(); break;
                case 6:  // 右
                    character.turn_right(); break;
                case 2:  // 下
                    character.turn_down(); break;
                case 4:  // 左
                    character.turn_left(); break;
            }
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 地图的滚动
        //--------------------------------------------------------------------------
        public bool command_203()
        {
            // 战斗中的情况下
            if (Global.game_temp.in_battle)
            {
                // 继续
                return true;
            }
            // 已经在滚动中的情况下
            if (Global.game_map.is_scrolling)
            {
                // 结束
                return false;
            }
            // 开始滚动
            Global.game_map.start_scroll((int)this.parameters[0], (int)this.parameters[1], (int)this.parameters[2]);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 更改地图设置
        //--------------------------------------------------------------------------
        public bool command_204()
        {
            switch ((int)this.parameters[0])
            {
                case 0:  // 远景
                    Global.game_map.panorama_name = (string)this.parameters[1];
                    Global.game_map.panorama_hue = (int)this.parameters[2];
                    break;
                case 1:  // 雾
                    Global.game_map.fog_name = (string)this.parameters[1];
                    Global.game_map.fog_hue = (int)this.parameters[2];
                    Global.game_map.fog_opacity = (int)this.parameters[3];
                    Global.game_map.fog_blend_type = (int)this.parameters[4];
                    Global.game_map.fog_zoom = (int)this.parameters[5];
                    Global.game_map.fog_sx = (int)this.parameters[6];
                    Global.game_map.fog_sy = (int)this.parameters[7];
                    break;
                case 2:  // 战斗背景
                    Global.game_map.battleback_name = (string)this.parameters[1];
                    Global.game_temp.battleback_name = (string)this.parameters[1];
                    break;
            }
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 更改雾的色调
        //--------------------------------------------------------------------------
        public bool command_205()
        {
            // 开始更改色调
            Global.game_map.start_fog_tone_change((Tone)this.parameters[0], (int)this.parameters[1] * 2);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 更改雾的不透明度
        //--------------------------------------------------------------------------
        public bool command_206()
        {
            // 开始更改不透明度
            Global.game_map.start_fog_opacity_change((int)this.parameters[0], (int)this.parameters[1] * 2);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 显示动画
        //--------------------------------------------------------------------------
        public bool command_207()
        {
            // 获取角色
            var character = get_character((int)this.parameters[0]);
            // 角色不存在的情况下
            if (character == null)
            {
                // 继续
                return true;
            }
            // 设置动画 ID
            character.animation_id = (int)this.parameters[1];
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 更改透明状态
        //--------------------------------------------------------------------------
        public bool command_208()
        {
            // 设置主角的透明状态
            Global.game_player.transparent = ((int)this.parameters[0] == 0);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 设置移动路线
        //--------------------------------------------------------------------------
        public bool command_209()
        {
            // 获取角色
            var character = get_character((int)this.parameters[0]);
            // 角色不存在的情况下
            if (character == null)
            {
                // 继续
                return true;
            }
            // 强制移动路线
            character.force_move_route((RPG.MoveRoute)this.parameters[1]);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 移动结束后等待
        //--------------------------------------------------------------------------
        public bool command_210()
        {
            // 如果不在战斗中
            if (!Global.game_temp.in_battle)
            {
                // 设置移动结束后待机标志
                this.move_route_waiting = true;
            }
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 开始过渡
        //--------------------------------------------------------------------------
        public bool command_221()
        {
            // 显示信息窗口中的情况下
            if (Global.game_temp.message_window_showing)
            {
                // 结束
                return false;
            }
            // 准备过渡
            Graphics.freeze();
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 执行过渡
        //--------------------------------------------------------------------------
        public bool command_222()
        {
            // 已经设置了过渡处理中标志的情况下
            if (Global.game_temp.transition_processing)
            {
                // 结束
                return false;
            }
            // 设置过渡处理中标志
            Global.game_temp.transition_processing = true;
            Global.game_temp.transition_name = (string)this.parameters[0];
            // 推进索引
            this.index += 1;
            // 结束
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 更改画面色调
        //--------------------------------------------------------------------------
        public bool command_223()
        {
            // 开始更改色调
            Global.game_screen.start_tone_change((Tone)this.parameters[0], (int)this.parameters[1] * 2);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 画面闪烁
        //--------------------------------------------------------------------------
        public bool command_224()
        {
            // 开始闪烁
            Global.game_screen.start_flash((Color)this.parameters[0], (int)this.parameters[1] * 2);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 画面震动
        //--------------------------------------------------------------------------
        public bool command_225()
        {
            // 震动开始
            Global.game_screen.start_shake((int)this.parameters[0], (int)this.parameters[1],
              (int)this.parameters[2] * 2);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 显示图片
        //--------------------------------------------------------------------------
        public bool command_231()
        {
            // 获取图片编号
            var number = (int)this.parameters[0] + (Global.game_temp.in_battle ? 50 : 0);
            double x, y;
            // 指定方法为 [直接指定] 的情况下
            if ((int)this.parameters[3] == 0)
            {
                x = (double)this.parameters[4];
                y = (double)this.parameters[5];
                // 指定方法为 [使用变量指定] 的情况下
            }
            else
            {
                x = Global.game_variables[(int)this.parameters[4]];
                y = Global.game_variables[(int)this.parameters[5]];
            }
            // 显示图片
            Global.game_screen.pictures[number].show((string)this.parameters[1], (int)this.parameters[2],
              x, y, (double)this.parameters[6], (double)this.parameters[7], (double)this.parameters[8], (int)this.parameters[9]);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 移动图片
        //--------------------------------------------------------------------------
        public bool command_232()
        {
            // 获取图片编号
            var number = (int)this.parameters[0] + (Global.game_temp.in_battle ? 50 : 0);
            int x, y;
            // 指定方法为 [直接指定] 的情况下
            if ((int)this.parameters[3] == 0)
            {
                x = (int)this.parameters[4];
                y = (int)this.parameters[5];
                // 指定方法为 [使用变量指定] 的情况下
            }
            else
            {
                x = Global.game_variables[(int)this.parameters[4]];
                y = Global.game_variables[(int)this.parameters[5]];
            }
            // 移动图片
            Global.game_screen.pictures[number].move((int)this.parameters[1] * 2, (int)this.parameters[2],
              x, y, (int)this.parameters[6], (int)this.parameters[7], (int)this.parameters[8], (int)this.parameters[9]);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 旋转图片
        //--------------------------------------------------------------------------
        public bool command_233()
        {
            // 获取图片编号
            var number = (int)this.parameters[0] + (Global.game_temp.in_battle ? 50 : 0);
            // 设置旋转速度
            Global.game_screen.pictures[number].rotate((int)this.parameters[1]);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 更改图片色调
        //--------------------------------------------------------------------------
        public bool command_234()
        {
            // 获取图片编号
            var number = (int)this.parameters[0] + (Global.game_temp.in_battle ? 50 : 0);
            // 开始更改色调
            Global.game_screen.pictures[number].start_tone_change((Tone)this.parameters[1],
              (int)this.parameters[2] * 2);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 删除图片
        //--------------------------------------------------------------------------
        public bool command_235()
        {
            // 获取图片编号
            var number = (int)this.parameters[0] + (Global.game_temp.in_battle ? 50 : 0);
            // 删除图片
            Global.game_screen.pictures[number].erase();
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 天候设置
        //--------------------------------------------------------------------------
        public bool command_236()
        {
            // 设置天候
            Global.game_screen.weather((int)this.parameters[0], (int)this.parameters[1], (int)this.parameters[2]);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 演奏 RPG.AudioFile
        //--------------------------------------------------------------------------
        public bool command_241()
        {
            // 演奏 RPG.AudioFile
            Global.game_system.bgm_play((RPG.AudioFile)this.parameters[0]);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● RPG.AudioFile 的淡入淡出
        //--------------------------------------------------------------------------
        public bool command_242()
        {
            // 淡入淡出 RPG.AudioFile
            Global.game_system.bgm_fade((int)this.parameters[0]);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 演奏 BGS
        //--------------------------------------------------------------------------
        public bool command_245()
        {
            // 演奏 BGS
            Global.game_system.bgs_play((RPG.AudioFile)this.parameters[0]);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● BGS 的淡入淡出
        //--------------------------------------------------------------------------
        public bool command_246()
        {
            // 淡入淡出 BGS
            Global.game_system.bgs_fade((int)this.parameters[0]);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 记忆 RPG.AudioFile / BGS
        //--------------------------------------------------------------------------
        public bool command_247()
        {
            // 记忆 RPG.AudioFile / BGS
            Global.game_system.bgm_memorize();
            Global.game_system.bgs_memorize();
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 还原 RPG.AudioFile / BGS
        //--------------------------------------------------------------------------
        public bool command_248()
        {
            // 还原 RPG.AudioFile / BGS
            Global.game_system.bgm_restore();
            Global.game_system.bgs_restore();
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 演奏 ME
        //--------------------------------------------------------------------------
        public bool command_249()
        {
            // 演奏 ME
            Global.game_system.me_play((RPG.AudioFile)this.parameters[0]);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 演奏 SE
        //--------------------------------------------------------------------------
        public bool command_250()
        {
            // 演奏 SE
            Global.game_system.se_play((RPG.AudioFile)this.parameters[0]);
            // 继续
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 停止 SE
        //--------------------------------------------------------------------------
        public bool command_251()
        {
            // 停止 SE
            Audio.se_stop();
            // 继续
            return true;
        }

        public bool move_route_waiting { get; set; }
    }

}
