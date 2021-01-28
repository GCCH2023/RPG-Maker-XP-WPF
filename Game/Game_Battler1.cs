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
    // ■ Game_Battler (分割定义 1)
    //------------------------------------------------------------------------------
    // 　处理战斗者的类。这个类作为 Game_Actor 类与 Game_Enemy 类的
    // 超级类来使用。
    //==============================================================================

    public partial class Game_Battler : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")  
        {  
            if(PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //--------------------------------------------------------------------------
        // ● 更改 HP
        //     hp : 新的 HP
        //--------------------------------------------------------------------------
        public double hp
        {
            get
            {
                return this._hp;
            }
            set
            {
                this._hp = Math.Max(Math.Min(value, maxhp), 0);
                // 解除附加的战斗不能状态
                for (var i = 1; i < Global.data_states.Count; i++)
                {
                    if (Global.data_states[i].zero_hp)
                    {
                        if (this.is_dead)
                            add_state(i);
                        else
                            remove_state(i);
                    }
                }
                // 取整
                this._hp = Math.Floor(this._hp);
                NotifyPropertyChanged();
            }
        }
        //--------------------------------------------------------------------------
        // ● 更改 SP
        //     sp : 新的 SP
        //--------------------------------------------------------------------------
        public double sp
        {
            get { return this._sp; }
            set
            {
                this._sp = Math.Max(Math.Min(value, maxsp), 0);
                // 取整
                this._sp = Math.Floor(this._sp);
                NotifyPropertyChanged();
            }
        }
        // 闪烁标志
        bool _blink = false;
        public bool blink
        {
            get { return this._blink; }
            set
            {
                if (_blink != value)
                {
                    _blink = value;
                    NotifyPropertyChanged();
                }
            }
        }
        // 战斗者 文件名
        private string _battler_name; 
        public string battler_name
        {
            get { return _battler_name; }
            set 
            {
                if (_battler_name != value)
                {
                    _battler_name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public int battler_hue = 0;         // 战斗者 色相
        public double _hp = 0;   // HP
        public double _sp = 0;   // SP
        public List<int> states = new List<int>();     // 状态
        public Dictionary<int, int> states_turn = new Dictionary<int,int>();
        public double maxhp_plus = 0;
        public double maxsp_plus = 0;
        public double str_plus = 0;
        public double dex_plus = 0;
        public double agi_plus = 0;
        public double int_plus = 0;

        public bool hidden = false;      // 隐藏标志
        public bool immortal = false;       // 不死身标志
        // 显示伤害标志
        public bool damage_pop = false;
        // 伤害结果，数值或者"Miss"
        object _damage = 0.0;

        public object damage
        {
            get { return _damage; }
            set
            {
                if (value is double)
                    value = Math.Floor((double)value);
                _damage = value;
            }
        }

        public bool critical = false;    // 会心一击标志
        public int animation_id = 0;          // 动画 ID
        public bool animation_hit = false;          // 动画 击中标志
        public bool white_flash = false;         // 白色屏幕闪烁标志

        public Game_BattleAction _current_action = new Game_BattleAction();

        public virtual int animation1_id { get; set; }
        public virtual int animation2_id { get; set; }
        public virtual int screen_x { get; set; }
        public virtual int screen_y { get; set; }
        public virtual int screen_z { get; set; }

        //--------------------------------------------------------------------------
        // ● 获取 MaxHP
        //--------------------------------------------------------------------------
        // ● 设置 MaxHP
        //     maxhp : 新的 MaxHP
        //--------------------------------------------------------------------------
        public virtual double maxhp
        {

            get
            {
                double n = Math.Min(Math.Max(base_maxhp + this.maxhp_plus, 1), 999999);
                foreach (var i in this.states)
                    n *= Global.data_states[i].maxhp_rate / 100.0;

                n = Math.Min(Math.Max((int)n, 1), 999999);
                return n;
            }
            set
            {
                this.maxhp_plus += value - this.maxhp;
                this.maxhp_plus = Math.Min(Math.Max(this.maxhp_plus, -9999), 9999);
                this.hp = Math.Min(this._hp, this.maxhp);
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取 MaxSP
        //--------------------------------------------------------------------------
        // ● 设置 MaxSP
        //     maxsp : 新的 MaxSP
        //--------------------------------------------------------------------------
        public double maxsp
        {
            get
            {
                double n = Math.Min(Math.Max(base_maxsp + this.maxsp_plus, 0), 9999);
                foreach (var i in this.states)
                    n *= Global.data_states[i].maxsp_rate / 100.0;

                n = Math.Min(Math.Max((int)n, 0), 9999);
                return n;
            }
            set
            {
                this.maxsp_plus += value - this.maxsp;
                this.maxsp_plus = Math.Min(Math.Max(this.maxsp_plus, -9999), 9999);
                this.sp = Math.Min(this._sp, this.maxsp);
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取力量
        //--------------------------------------------------------------------------
        // ● 设置力量
        //     str : 新的力量
        //--------------------------------------------------------------------------
        public double str
        {
            get
            {
                double n = Math.Min(Math.Max(base_str + this.str_plus, 1), 999);
                foreach (var i in this.states)
                    n *= Global.data_states[i].str_rate / 100.0;

                n = Math.Min(Math.Max((int)n, 1), 999);
                return n;
            }
            set
            {
                this.str_plus += value - this.str;
                this.str_plus = Math.Min(Math.Max(this.str_plus, -999), 999);
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取灵巧
        //--------------------------------------------------------------------------
        // ● 设置灵巧
        //     dex : 新的灵巧
        //--------------------------------------------------------------------------
        public double dex
        {
            get
            {
                double n = Math.Min(Math.Max(base_dex + this.dex_plus, 1), 999);
                foreach (var i in this.states)
                    n *= Global.data_states[i].dex_rate / 100.0;

                n = Math.Min(Math.Max((int)n, 1), 999);
                return n;
            }
            set
            {
                this.dex_plus += value - this.dex;
                this.dex_plus = Math.Min(Math.Max(this.dex_plus, -999), 999);

            }
        }
        //--------------------------------------------------------------------------
        // ● 获取/设置速度
        //--------------------------------------------------------------------------
        public double agi
        {
            get
            {
                double n = Math.Min(Math.Max(base_agi + this.agi_plus, 1), 999);
                foreach (var i in this.states)
                    n *= Global.data_states[i].agi_rate / 100.0;

                n = Math.Min(Math.Max((int)n, 1), 999);
                return n;
            }
            set
            {
                this.agi_plus += value - this.agi;
                this.agi_plus = Math.Min(Math.Min(this.agi_plus, -999), 999);

            }
        }
        //--------------------------------------------------------------------------
        // ● 获取/设置魔力
        //--------------------------------------------------------------------------
        public double int1
        {
            get
            {
                double n = Math.Min(Math.Max(base_int + this.int_plus, 1), 999);
                foreach (var i in this.states)
                    n *= Global.data_states[i].int_rate / 100.0;

                n = Math.Min(Math.Max((int)n, 1), 999);
                return n;
            }
            set
            {
                this.int_plus += value - this.int1;
                this.int_plus = Math.Min(Math.Max(this.int_plus, -999), 999);
            }
        }

        //--------------------------------------------------------------------------
        // ● 获取命中率
        //--------------------------------------------------------------------------
        public double hit
        {
            get
            {
                double n = 100;
                foreach (var i in this.states)
                    n *= Global.data_states[i].hit_rate / 100.0;
                return (int)n;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取攻击力
        //--------------------------------------------------------------------------
        public double atk
        {
            get
            {
                double n = base_atk;
                foreach (var i in this.states)
                    n *= Global.data_states[i].atk_rate / 100.0;
                return (int)n;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取物理防御
        //--------------------------------------------------------------------------
        public double pdef
        {
            get
            {
                double n = base_pdef;
                foreach (var i in this.states)
                    n *= Global.data_states[i].pdef_rate / 100.0;
                return (int)n;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取魔法防御
        //--------------------------------------------------------------------------
        public double mdef
        {
            get
            {
                double n = base_mdef;
                foreach (var i in this.states)
                    n *= Global.data_states[i].mdef_rate / 100.0;
                return (int)n;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取回避修正
        //--------------------------------------------------------------------------
        public double eva
        {
            get
            {
                double n = base_eva;
                foreach (var i in this.states)
                    n *= Global.data_states[i].eva;
                return n;
            }
        }
        //--------------------------------------------------------------------------
        // ● 全回复
        //--------------------------------------------------------------------------
        public void recover_all()
        {
            this.hp = this.maxhp;
            this.sp = maxsp;
            foreach (var i in new List<int>(this.states))
                remove_state(i);
        }
        //--------------------------------------------------------------------------
        // ● 获取当前的动作
        //--------------------------------------------------------------------------
        public Game_BattleAction current_action
        {
            get
            {
                return this._current_action;
            }
        }
        //--------------------------------------------------------------------------
        // ● 确定动作速度
        //--------------------------------------------------------------------------
        public int make_action_speed()
        {
            this.current_action.speed = (int)(agi + Global.rand(10 + (int)agi / 4));
            return this.current_action.speed;
        }
        //--------------------------------------------------------------------------
        // ● 战斗不能判定
        //--------------------------------------------------------------------------
        public bool is_dead
        {
            get
            {
                return (this._hp == 0 && !this.immortal);
            }
        }
        //--------------------------------------------------------------------------
        // ● 存在判定
        //--------------------------------------------------------------------------
        public bool is_exist
        {
            get
            {
                return (!this.hidden && (this._hp > 0 || this.immortal));
            }
        }
        //--------------------------------------------------------------------------
        // ● HP 0 判定
        //--------------------------------------------------------------------------
        public bool is_hp0
        {
            get
            {
                return (!this.hidden && this._hp == 0);
            }
        }
        //--------------------------------------------------------------------------
        // ● 可以输入命令判定
        //--------------------------------------------------------------------------
        public bool is_inputable
        {
            get
            {
                return (!this.hidden && restriction <= 1);
            }
        }
        //--------------------------------------------------------------------------
        // ● 可以行动判定
        //--------------------------------------------------------------------------
        public bool is_movable
        {
            get
            {
                return (!this.hidden && restriction < 4);
            }
        }
        //--------------------------------------------------------------------------
        // ● 防御中判定
        //--------------------------------------------------------------------------
        public bool is_guarding
        {
            get
            {
                return (this.current_action.kind == 0 && this.current_action.basic == 1);
            }
        }
        //--------------------------------------------------------------------------
        // ● 休止中判定
        //--------------------------------------------------------------------------
        public bool is_resting
        {
            get
            {
                return (this.current_action.kind == 0 && this.current_action.basic == 3);
            }
        }

        public virtual int? index
        {
            get
            {
                return -1;
            }
        }

        public virtual double base_maxhp { get; set; }
        public virtual double base_maxsp { get; set; }

        public virtual double base_str { get; set; }
        public virtual double base_dex { get; set; }
        public virtual double base_agi { get; set; }
        public virtual double base_int { get; set; }
        public virtual double base_atk { get; set; }
        public virtual double base_pdef { get; set; }
        public virtual double base_mdef { get; set; }
        public virtual double base_eva { get; set; }
    }
}
