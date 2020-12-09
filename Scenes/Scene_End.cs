using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_End
    //------------------------------------------------------------------------------
    // 　处理游戏结束画面的类。
    //==============================================================================

    public class Scene_End : Scene
    {
        public Window_Command command_window { get; set; }

        public override void Initialize()
        {
            // 生成命令窗口
            var s1 = "返回标题画面";
            var s2 = "退出";
            var s3 = "取消";
            this.command_window = new Window_Command(192, new string[] { s1, s2, s3 });
            this.command_window.x = 320 - this.command_window.width / 2;
            this.command_window.y = 240 - this.command_window.height / 2;
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
                // 刷新输入情报
                Input.update();
                // 刷新画面
                update();
                // 如果画面切换的话就中断循环
            //    if (Global.scene != this)
            //    {
            //        break;
            //    }
            //}
        }
        public override void Uninitialize()
        {
            // 准备过渡
            Graphics.freeze();
            // 释放窗口
            this.command_window.dispose();
            // 如果在标题画面切换中的情况下
            if (Global.scene is Scene_Title)
                // 淡入淡出画面
                Graphics.transition();
            Graphics.freeze();
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 刷新命令窗口
            this.command_window.update();
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 切换到菜单画面
                Global.scene = new Scene_Menu(5);
                return;
            }
            // 按下 C 键的场合下
            if (Input.is_trigger(Input.C))
            {
                // 命令窗口光标位置分支
                switch (this.command_window.index)
                {
                    case 0:  // 返回标题画面
                        command_to_title(); break;
                    case 1:  // 退出
                        command_shutdown(); break;
                    case 2:  // 取消
                        command_cancel(); break;
                }
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 选择命令 [返回标题画面] 时的处理
        //--------------------------------------------------------------------------
        public void command_to_title()
        {
            // 演奏确定 SE
            Global.game_system.se_play(Global.data_system.decision_se);
            // 淡入淡出 RPG.AudioFile、BGS、ME
            Audio.bgm_fade(800);
            Audio.bgs_fade(800);
            Audio.me_fade(800);
            // 切换到标题画面
            Global.scene = new Scene_Title();
        }
        //--------------------------------------------------------------------------
        // ● 选择命令 [退出] 时的处理
        //--------------------------------------------------------------------------
        public void command_shutdown()
        {
            // 演奏确定 SE
            Global.game_system.se_play(Global.data_system.decision_se);
            // 淡入淡出 RPG.AudioFile、BGS、ME
            Audio.bgm_fade(800);
            Audio.bgs_fade(800);
            Audio.me_fade(800);
            // 退出
            Global.scene = null;
        }
        //--------------------------------------------------------------------------
        // ● 选择命令 [取消] 时的处理
        //--------------------------------------------------------------------------
        public void command_cancel()
        {
            // 演奏确定 SE
            Global.game_system.se_play(Global.data_system.decision_se);
            // 切换到菜单画面
            Global.scene = new Scene_Menu(5);
        }
    }

}
