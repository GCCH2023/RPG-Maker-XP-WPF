using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_Status
    //------------------------------------------------------------------------------
    // 　处理状态画面的类。
    //==============================================================================

    public class Scene_Status : Scene
    {
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     actor_index : 角色索引
        //--------------------------------------------------------------------------
        public Scene_Status(int actor_index = 0, int equip_index = 0)
        {
            this.actor_index = actor_index;
        }
        public override void Initialize()
        {
            // 获取角色
            this.actor = Global.game_party.actors[this.actor_index];
            // 生成状态窗口
            this.status_window = new Window_Status(this.actor);
            // 执行过渡
            Graphics.transition();
        }
        //--------------------------------------------------------------------------
        // ● 主处理
        //--------------------------------------------------------------------------
        public override void main()
        {
            // 主循环
            //while (true)
            //{
                // 刷新游戏画面
                Graphics.update();
                // 刷新输入信息
                Input.update();
                // 刷新画面
                update();
                // 如果画面被切换的话就中断循环
            //    if (Global.scene != this)
            //        break;
            //}
        }
        public override void Uninitialize()
        {
            // 准备过渡
            Graphics.freeze();
            // 释放窗口
            this.status_window.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 切换到菜单画面
                Global.scene = new Scene_Menu(3);
                return;
            }
            // 按下 R 键的情况下
            if (Input.is_trigger(Input.R))
            {
                // 演奏光标 SE
                Global.game_system.se_play(Global.data_system.cursor_se);
                // 移至下一位角色
                this.actor_index += 1;
                this.actor_index %= Global.game_party.actors.Count;
                // 切换到别的状态画面
                Global.scene = new Scene_Status(this.actor_index);
                return;
            }
            // 按下 L 键的情况下
            if (Input.is_trigger(Input.L))
            {
                // 演奏光标 SE
                Global.game_system.se_play(Global.data_system.cursor_se);
                // 移至上一位角色
                this.actor_index += Global.game_party.actors.Count - 1;
                this.actor_index %= Global.game_party.actors.Count;
                // 切换到别的状态画面
                Global.scene = new Scene_Status(this.actor_index);
                return;
            }
        }

        public int actor_index { get; set; }

        public Game_Actor actor { get; set; }

        public Window_Status status_window { get; set; }
    }

}
