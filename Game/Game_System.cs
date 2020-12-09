using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_System
    //------------------------------------------------------------------------------
    // 　处理系统附属数据的类。也可执行诸如 RPG.AudioFile 管理之类的功能。本类的实例请参考
    // Global.game_system 。
    //==============================================================================

    public class Game_System : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")  
        {  
            if(PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }  
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public Interpreter map_interpreter = new Interpreter(0, true);// 地图事件用解释程序
        public Interpreter battle_interpreter = new Interpreter(0, false);// 战斗事件用解释程序
        public int timer = 0;// 计时器
        public bool timer_working = false;// 计时器执行中的标志
        public bool save_disabled = false;// 禁止存档
        public bool menu_disabled = false;// 禁止菜单
        public bool encounter_disabled = false;// 禁止遇敌
        public int message_position = 2;// 文章选项 显示位置
        public int message_frame = 0;// 文章选项 窗口外关
        public int save_count = 0;// 存档次数
        public int magic_number = 0;// 魔法编号
        public RPG.AudioFile playing_bgm;
        public RPG.AudioFile memorized_bgm;
        public RPG.AudioFile playing_bgs;
        public RPG.AudioFile memorized_bgs;

        public Game_System()
        {

        }
        //--------------------------------------------------------------------------
        // ● 演奏 RPG.AudioFile
        //     bgm : 演奏的 RPG.AudioFile
        //--------------------------------------------------------------------------
        public void bgm_play(RPG.AudioFile bgm)
        {
            this.playing_bgm = bgm;
            if (bgm != null && bgm.name != "")
                Audio.bgm_play("Audio/BGM/" + bgm.name + ".mid", bgm.volume, bgm.pitch);
            else
                Audio.bgm_stop();

            Graphics.frame_reset();
        }
        //--------------------------------------------------------------------------
        // ● 停止 RPG.AudioFile
        //--------------------------------------------------------------------------
        public void bgm_stop()
        {
            Audio.bgm_stop();
        }
        //--------------------------------------------------------------------------
        // ● RPG.AudioFile 的淡出
        //     time : 淡出时间 (秒)
        //--------------------------------------------------------------------------
        public void bgm_fade(int time)
        {
            this.playing_bgm = null;
            Audio.bgm_fade(time * 1000);
        }
        //--------------------------------------------------------------------------
        // ● 记忆 RPG.AudioFile
        //--------------------------------------------------------------------------
        public void bgm_memorize()
        {
            this.memorized_bgm = this.playing_bgm;
        }
        //--------------------------------------------------------------------------
        // ● 还原 RPG.AudioFile
        //--------------------------------------------------------------------------
        public void bgm_restore()
        {
            bgm_play(this.memorized_bgm);
        }
        //--------------------------------------------------------------------------
        // ● 演奏 BGS
        //     bgs : 演奏的 BGS
        //--------------------------------------------------------------------------
        public void bgs_play(RPG.AudioFile bgs)
        {
            this.playing_bgs = bgs;
            if (bgs != null && bgs.name != "")
                Audio.bgs_play("Audio/BGS/" + bgs.name + ".wav", bgs.volume, bgs.pitch);
            else
                Audio.bgs_stop();

            Graphics.frame_reset();
        }
        //--------------------------------------------------------------------------
        // ● BGS 的淡出
        //     time : 淡出时间 (秒)
        //--------------------------------------------------------------------------
        public void bgs_fade(int time)
        {
            this.playing_bgs = null;
            Audio.bgs_fade(time * 1000);
        }
        //--------------------------------------------------------------------------
        // ● 记忆 BGS
        //--------------------------------------------------------------------------
        public void bgs_memorize()
        {
            this.memorized_bgs = this.playing_bgs;
        }
        //--------------------------------------------------------------------------
        // ● 还原 BGS
        //--------------------------------------------------------------------------
        public void bgs_restore()
        {
            bgs_play(this.memorized_bgs);
        }
        //--------------------------------------------------------------------------
        // ● ME 的演奏
        //     me : 演奏的 ME
        //--------------------------------------------------------------------------
        public void me_play(RPG.AudioFile me)
        {
            if (me != null && me.name != "")
                Audio.me_play("Audio/ME/" + me.name + ".mid", me.volume, me.pitch);
            else
                Audio.me_stop();
            Graphics.frame_reset();
        }
        //--------------------------------------------------------------------------
        // ● SE 的演奏
        //     se : 演奏的 SE
        //--------------------------------------------------------------------------
        public void se_play(RPG.AudioFile se)
        {
            if (se != null && se.name != "")
                Audio.se_play("Audio/SE/" + se.name + ".wav", se.volume, se.pitch);
        }
        //--------------------------------------------------------------------------
        // ● 停止 SE 
        //--------------------------------------------------------------------------
        public void se_stop()
        {
            Audio.se_stop();
        }
        //--------------------------------------------------------------------------
        // ● 获取窗口外观的文件名
        //--------------------------------------------------------------------------
        //--------------------------------------------------------------------------
        // ● 设置窗口外观的文件名
        //     windowskin_name : 新的窗口外观文件名
        //--------------------------------------------------------------------------
        string _windowskin_name;
        public string windowskin_name
        {
            get
            {
                if (this._windowskin_name == null)
                    return Global.data_system.windowskin_name;
                else
                    return this.windowskin_name;
            }
            set
            {
                if (this._windowskin_name != value)
                {
                    this._windowskin_name = value;
                    NotifyPropertyChanged();
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取战斗 RPG.AudioFile
        //--------------------------------------------------------------------------
        //--------------------------------------------------------------------------
        // ● 设置战斗 RPG.AudioFile
        //     battle_bgm : 新的战斗 RPG.AudioFile
        //--------------------------------------------------------------------------
        RPG.AudioFile _battle_bgm;
        public RPG.AudioFile battle_bgm
        {
            get
            {
                if (this._battle_bgm == null)
                    return Global.data_system.battle_bgm;
                else
                    return this._battle_bgm;
            }
            set
            {
                this._battle_bgm = value;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取战斗结束的 RPG.AudioFile
        //--------------------------------------------------------------------------
        //--------------------------------------------------------------------------
        // ● 设置战斗结束的 RPG.AudioFile
        //     battle_end_me : 新的战斗结束 RPG.AudioFile
        //--------------------------------------------------------------------------
        RPG.AudioFile _battle_end_me;
        public RPG.AudioFile battle_end_me
        {
            get
            {
                if (this._battle_end_me == null)
                    return Global.data_system.battle_end_me;
                else
                    return this._battle_end_me;
            }
            set { this._battle_end_me = value; }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public virtual void update()
        {
            // 计时器减 1
            if (this.timer_working && this.timer > 0)
                this.timer -= 1;
        }
    }
}
