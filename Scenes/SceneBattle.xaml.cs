using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XP
{
    /// <summary>
    /// SceneBattle.xaml 的交互逻辑
    /// </summary>
    public partial class SceneBattle : UserControl
    {
        // 队伍id
        private int troop_id;
        public SceneBattle()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 加载战斗场景
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SceneBattle_Loaded(object sender, RoutedEventArgs e)
        {
            //// 初始化战斗用的各种暂时数据
            //Global.game_temp.in_battle = true;
            //Global.game_temp.battle_turn = 0;
            //Global.game_temp.battle_event_flags.Clear();
            //Global.game_temp.battle_abort = false;
            //Global.game_temp.battle_main_phase = false;
            //Global.game_temp.battleback_name = Global.game_map.battleback_name;
            //Global.game_temp.forcing_battler = null;
            //// 初始化战斗用事件解释器
            //Global.game_system.battle_interpreter.setup(null, 0);
            //// 准备队伍
            //this.troop_id = Global.game_temp.battle_troop_id;
            //Global.game_troop.setup(this.troop_id);
            //// 生成角色命令窗口
            //this.ActorCommand.ItemsSource = new List<Command>()
            //{
            //    new Command(Global.data_system.words.attack),
            //    new Command(Global.data_system.words.skill),
            //    new Command(Global.data_system.words.guard),
            //    new Command(Global.data_system.words.item)
            //};
            this.ActorCommand.ItemsSource = new List<Command>()
            {
                new Command("攻击"),
                new Command("特技"),
                new Command("防御"),
                new Command("物品"),
            };


            // 生成其它窗口
            this.PartyCommand.ItemsSource = new List<Command>()
            {
                new Command("战斗"),
                new Command("逃跑"),
            };
           
            //this.status_window = new Window_BattleStatus();
            this.ActorStatusWindow.DataContext = new List<object>()
            {
                new{name="阿尔西斯", hp=999, sp=999, status="[正常]"},
                new{name="阿尔西斯", hp=999, sp=999, status="[正常]"},
                new{name="阿尔西斯", hp=999, sp=999, status="[正常]"},
                new{name="阿尔西斯", hp=999, sp=999, status="[正常]"},
            };

            //this.message_window = new Window_Message();
            //// 生成活动块
            //this.spriteset = new Spriteset_Battle();
            //// 初始化等待计数
            //this.wait_count = 0;
            //// 执行过渡
            //if (Global.data_system.battle_transition == "")
            //{
            //    Graphics.transition(20);
            //}
            //else
            //{
            //    Graphics.transition(40, "Graphics/Transitions/" +
            //      Global.data_system.battle_transition);
            //}
            //// 开始自由战斗回合
            //start_phase1();
        }

        private void ActorAction_Select(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(ActorCommand.SelectedItem.ToString());
        }

        private void ActorAction_SelectIndexChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {

        }

        private void PartyCommand_Select(object sender, RoutedEventArgs e)
        {

        }

        private void PartyCommand_SelectIndexChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {

        }

    }
}
