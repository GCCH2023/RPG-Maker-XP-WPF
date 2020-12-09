using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_Name
    //------------------------------------------------------------------------------
    // 　处理名称输入画面的类。
    //==============================================================================

    public class Scene_Name : Scene
    {
        public override void Initialize()
        {
            // 获取角色
            this.actor = Global.game_actors[Global.game_temp.name_actor_id];
            // 生成窗口
            this.edit_window = new Window_NameEdit(this.actor, Global.game_temp.name_max_char);
            this.input_window = new Window_NameInput();
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
                // 刷新信息
                update();
                // 如果画面切换就中断循环
            //    if (Global.scene != this)
            //        break;
            //}
        }
        public override void Uninitialize()
        {
            // 准备过渡
            Graphics.freeze();
            // 释放窗口
            this.edit_window.dispose();
            this.input_window.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 刷新窗口
            this.edit_window.update();
            this.input_window.update();
            // 按下 B 键的情况下
            if (Input.is_repeat(Input.B))
            {
                // 光标位置为 0 的情况下
                if (this.edit_window.index == 0)
                {
                    return;
                }
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 删除文字
                this.edit_window.back();
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 光标位置为 [确定] 的情况下
                if (this.input_window.character == null)
                {
                    // 名称为空的情况下
                    if (this.edit_window.name == "")
                    {
                        // 还原为默认名称
                        this.edit_window.restore_default();
                        // 名称为空的情况下
                        if (this.edit_window.name == "")
                        {
                            // 演奏冻结 SE
                            Global.game_system.se_play(Global.data_system.buzzer_se);
                            return;
                        }
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        return;
                    }
                    // 更改角色名称
                    this.actor.name = this.edit_window.name;
                    // 演奏确定 SE
                    Global.game_system.se_play(Global.data_system.decision_se);
                    // 切换到地图画面
                    Global.scene = new Scene_Map();
                    return;
                }
                // 光标位置为最大的情况下
                if (this.edit_window.index == Global.game_temp.name_max_char)
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                    return;
                }
                // 文字为空的情况下
                if (this.input_window.character == "")
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                    return;
                }
                // 演奏确定 SE
                Global.game_system.se_play(Global.data_system.decision_se);
                // 添加文字
                this.edit_window.add(this.input_window.character);
                return;
            }
        }

        public Game_Actor actor { get; set; }

        public Window_NameEdit edit_window { get; set; }

        public Window_NameInput input_window { get; set; }
    }

}
