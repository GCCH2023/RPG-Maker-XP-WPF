using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XP.Internal;

namespace XP
{
    //==============================================================================
    // ■ Window_SaveFile
    //------------------------------------------------------------------------------
    // 　显示存档以及读档画面、保存文件的窗口。
    //==============================================================================

    public class Window_SaveFile : Window_Base
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public string filename;                 // 文件名
        public bool _selected;           // 选择状态
        public DateTime time_stamp;
        public bool file_exist;
        public int name_width { get; set; }
        public int file_index { get; set; }
        public object characters;
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     file_index : 存档文件的索引 (0～3)
        //     filename   : 文件名
        //--------------------------------------------------------------------------
        public Window_SaveFile(int file_index, string filename) :
            base(0, 64 + file_index % 4 * 104, 640, 104)
        {
            this.contents = new Bitmap(width - 32, height - 32);
            this.file_index = file_index;
            this.filename = string.Format("Save{0}.rxdata", this.file_index + 1);

            this.time_stamp = new DateTime(0);   // Time.at(0);
            this.file_exist = System.IO.File.Exists(this.filename);
            if (this.file_exist)
            {
                var file = File.open(this.filename, "r");
                this.time_stamp = file.mtime;
                this.characters = Marshal.load(file);
                //this.frame_count = Marshal.load(file);
                //this.game_system = Marshal.load(file);
                //this.game_switches = Marshal.load(file);
                //this.game_variables = Marshal.load(file);
                //this.total_sec = this.frame_count / Graphics.frame_rate;
                file.close();
            }
            refresh();
            this.selected = false;
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            // 描绘文件编号
            this.contents.font.color = normal_color;
            //var name = string.Format("Save{0}.rxdata", this.file_index + 1);
            var name = string.Format("存档{0}", this.file_index + 1);
            this.contents.draw_text(4, 0, 600, 32, name);
            this.name_width = (int)contents.text_size(name).Width;
            // 存档文件存在的情况下
            if (this.file_exist)
            {
                // 描绘角色
                //for (var i = 0; i < this.characters.Count; i++)
                //{
                //    var bitmap = RPG.Cache.character(this.characters[i][0], this.characters[i][1]);
                //    var cw = bitmap.rect.width / 4;
                //    var ch = bitmap.rect.height / 4;
                //    var src_rect = new Rect(0, 0, cw, ch);
                //    var x = 300 - this.characters.Count * 32 + i * 64 - cw / 2;
                //    this.contents.blt(x, 68 - ch, bitmap, src_rect);
                //}
                //// 描绘游戏时间
                //var hour = this.total_sec / 60 / 60;
                //var min = this.total_sec / 60 % 60;
                //var sec = this.total_sec % 60;
                //var time_string = string.Format("%02d:%02d:%02d", hour, min, sec);
                //this.contents.font.color = normal_color;
                //this.contents.draw_text(4, 8, 600, 32, time_string, 2);
                //// 描绘时间标记
                //this.contents.font.color = normal_color;
                //time_string = this.time_stamp.strftime("%Y/%m/%d %H:%M");
                //this.contents.draw_text(4, 40, 600, 32, time_string, 2);
            }
        }
        //--------------------------------------------------------------------------
        // ● 设置选择状态
        //     selected : 新的选择状态 (true=选择 false=不选择)
        //--------------------------------------------------------------------------
        public bool selected
        {
            set
            {
                this._selected = value;
                update_cursor_rect();
            }
            get { return this._selected; }
        }
        //--------------------------------------------------------------------------
        // ● 刷新光标矩形
        //--------------------------------------------------------------------------
        public override void update_cursor_rect()
        {
            if (this.selected)
            {
                this.cursor_rect = new Rect(0, 0, this.name_width + 8, 32);
                this.active = true;
            }
            else
            {
                this.cursor_rect = new Rect(0, 0, 0, 0);
                this.active = false;
            }
        }

    }
}