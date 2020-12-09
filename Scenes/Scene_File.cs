using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_File
    //------------------------------------------------------------------------------
    // 　存档画面及读档画面的超级类。
    //==============================================================================

    public class Scene_File : Scene
    {
        public List<Window_SaveFile> savefile_windows { get; set; }
        public int file_index { get; set; }
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     help_text : 帮助窗口显示的字符串
        //--------------------------------------------------------------------------
        public Scene_File(string help_text)
        {
            this.help_text = help_text;
        }
        public override void Initialize()
        {
            // 生成帮助窗口
            this.help_window = new Window_Help();
            this.help_window.set_text(this.help_text);
            // 生成存档文件窗口
            this.savefile_windows = new List<Window_SaveFile>(4);
            for (var i = 0; i <= 3; i++)
            {
                this.savefile_windows.Add(new Window_SaveFile(i, make_filename(i)));
            }
            // 选择最后操作的文件
            this.file_index = Global.game_temp.last_file_index;
            this.savefile_windows[this.file_index].selected = true;
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
            this.help_window.dispose();
            foreach (var i in this.savefile_windows)
            {
                i.dispose();
            }
        }
        public virtual void on_decision(string filename)
        {

        }
        public virtual void on_cancel()
        {

        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 刷新窗口
            this.help_window.update();
            foreach (var i in this.savefile_windows)
                i.update();

            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 调用过程 on_decision (定义继承目标)
                on_decision(make_filename(this.file_index));
                Global.game_temp.last_file_index = this.file_index;
                return;
            }
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 调用过程 on_cancel (定义继承目标)
                on_cancel();
                return;
            }
            // 按下方向键下的情况下
            if (Input.is_repeat(Input.DOWN))
            {
                // 方向键下的按下状态不是重复的情况下、
                // 并且光标的位置在 3 以前的情况下
                if (Input.is_trigger(Input.DOWN) || this.file_index < 3)
                {
                    // 演奏光标 SE
                    Global.game_system.se_play(Global.data_system.cursor_se);
                    // 光标向下移动
                    this.savefile_windows[this.file_index].selected = false;
                    this.file_index = (this.file_index + 1) % 4;
                    this.savefile_windows[this.file_index].selected = true;
                    return;
                }
            }
            // 按下方向键上的情况下
            if (Input.is_repeat(Input.UP))
            {
                // 方向键上的按下状态不是重复的情况下、
                // 并且光标的位置在 0 以后的情况下
                if (Input.is_trigger(Input.UP) || this.file_index > 0)
                {
                    // 演奏光标 SE
                    Global.game_system.se_play(Global.data_system.cursor_se);
                    // 光标向上移动
                    this.savefile_windows[this.file_index].selected = false;
                    this.file_index = (this.file_index + 3) % 4;
                    this.savefile_windows[this.file_index].selected = true;
                    return;
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 生成文件名
        //     file_index : 文件名的索引 (0～3)
        //--------------------------------------------------------------------------
        public string make_filename(int file_index)
        {
            return string.Format("Save{0}.rxdata", file_index + 1);
        }

        public string help_text { get; set; }
        public Window_Help help_window { get; set; }
    }
}
