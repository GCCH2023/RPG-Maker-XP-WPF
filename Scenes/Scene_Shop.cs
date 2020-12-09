using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_Shop
    //------------------------------------------------------------------------------
    // 　处理商店画面的类。
    //==============================================================================

    public class Scene_Shop : Scene
    {
        public override void Initialize()
        {
            // 生成帮助窗口
            this.help_window = new Window_Help();
            // 生成指令窗口
            this.command_window = new Window_ShopCommand();
            // 生成金钱窗口
            this.gold_window = new Window_Gold();
            this.gold_window.x = 480;
            this.gold_window.y = 64;
            // 生成时间窗口
            this.dummy_window = new Window_Base(0, 128, 640, 352);
            // 生成购买窗口
            this.buy_window = new Window_ShopBuy(Global.game_temp.shop_goods);
            this.buy_window.active = false;
            this.buy_window.visible = false;
            this.buy_window.help_window = this.help_window;
            // 生成卖出窗口
            this.sell_window = new Window_ShopSell();
            this.sell_window.active = false;
            this.sell_window.visible = false;
            this.sell_window.help_window = this.help_window;
            // 生成数量输入窗口
            this.number_window = new Window_ShopNumber();
            this.number_window.active = false;
            this.number_window.visible = false;
            // 生成状态窗口
            this.status_window = new Window_ShopStatus();
            this.status_window.visible = false;
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
                // 如果画面切换的话就中断循环
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
            this.command_window.dispose();
            this.gold_window.dispose();
            this.dummy_window.dispose();
            this.buy_window.dispose();
            this.sell_window.dispose();
            this.number_window.dispose();
            this.status_window.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 刷新窗口
            this.help_window.update();
            this.command_window.update();
            this.gold_window.update();
            this.dummy_window.update();
            this.buy_window.update();
            this.sell_window.update();
            this.number_window.update();
            this.status_window.update();
            // 指令窗口激活的情况下: 调用 update_command
            if (this.command_window.active)
            {
                update_command();
                return;
            }
            // 购买窗口激活的情况下: 调用 update_buy
            if (this.buy_window.active)
            {
                update_buy();
                return;
            }
            // 卖出窗口激活的情况下: 调用 update_sell
            if (this.sell_window.active)
            {
                update_sell();
                return;
            }
            // 个数输入窗口激活的情况下: 调用 update_number
            if (this.number_window.active)
            {
                update_number();
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (指令窗口激活的情况下)
        //--------------------------------------------------------------------------
        public void update_command()
        {
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 切换到地图画面
                Global.scene = new Scene_Map();
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 命令窗口光标位置分支
                switch (this.command_window.index)
                {
                    case 0:  // 购买
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 窗口状态转向购买模式
                        this.command_window.active = false;
                        this.dummy_window.visible = false;
                        this.buy_window.active = true;
                        this.buy_window.visible = true;
                        this.buy_window.refresh();
                        this.status_window.visible = true;
                        break;
                    case 1:  // 卖出
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 窗口状态转向卖出模式
                        this.command_window.active = false;
                        this.dummy_window.visible = false;
                        this.sell_window.active = true;
                        this.sell_window.visible = true;
                        this.sell_window.refresh();
                        break;
                    case 2:  // 取消
                        // 演奏确定 SE
                        Global.game_system.se_play(Global.data_system.decision_se);
                        // 切换到地图画面
                        Global.scene = new Scene_Map();
                        break;
                }
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (购买窗口激活的情况下)
        //--------------------------------------------------------------------------
        public void update_buy()
        {
            // 设置状态窗口的物品
            this.status_window.item = this.buy_window.item;
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 窗口状态转向初期模式
                this.command_window.active = true;
                this.dummy_window.visible = true;
                this.buy_window.active = false;
                this.buy_window.visible = false;
                this.status_window.visible = false;
                this.status_window.item = null;
                // 删除帮助文本
                this.help_window.set_text("");
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 获取物品
                this.item = this.buy_window.item;
                // 物品无效的情况下、或者价格在所持金以上的情况下
                if (this.item == null || this.item.price > Global.game_party.gold)
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                    return;
                }
                int number = 0;
                // 获取物品所持数
                if (this.item is RPG.Item)
                    number = Global.game_party.item_number(this.item.id);
                else if (this.item is RPG.Weapon)
                    number = Global.game_party.weapon_number(this.item.id);
                else if (this.item is RPG.Armor)
                    number = Global.game_party.armor_number(this.item.id);

                // 如果已经拥有了 99 个情况下
                if (number == 99)
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                    return;
                }
                // 演奏确定 SE
                Global.game_system.se_play(Global.data_system.decision_se);
                // 计算可以最多购买的数量
                var max = this.item.price == 0 ? 99 : Global.game_party.gold / this.item.price;
                max = Math.Min(max, 99 - number);
                // 窗口状态转向数值输入模式
                this.buy_window.active = false;
                this.buy_window.visible = false;
                this.number_window.set(this.item, max, this.item.price);
                this.number_window.active = true;
                this.number_window.visible = true;
            }
        }
        //--------------------------------------------------------------------------
        // ● 画面更新 (卖出窗口激活的情况下)
        //--------------------------------------------------------------------------
        public void update_sell()
        {
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 窗口状态转向初期模式
                this.command_window.active = true;
                this.dummy_window.visible = true;
                this.sell_window.active = false;
                this.sell_window.visible = false;
                this.status_window.item = null;
                // 删除帮助文本
                this.help_window.set_text("");
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 获取物品
                this.item = this.sell_window.item;
                // 设置状态窗口的物品
                this.status_window.item = this.item;
                // 物品无效的情况下、或者价格为 0 (不能卖出) 的情况下
                if (this.item == null || this.item.price == 0)
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                    return;
                }
                // 演奏确定 SE
                Global.game_system.se_play(Global.data_system.decision_se);
                int number = 0;
                // 获取物品所持数
                if (this.item is RPG.Item)
                    number = Global.game_party.item_number(this.item.id);
                else if (this.item is RPG.Weapon)
                    number = Global.game_party.weapon_number(this.item.id);
                else if (this.item is RPG.Armor)
                    number = Global.game_party.armor_number(this.item.id);
                // 最大卖出个数 = 物品的所持数
                var max = number;
                // 窗口状态转向个数输入模式
                this.sell_window.active = false;
                this.sell_window.visible = false;
                this.number_window.set(this.item, max, this.item.price / 2);
                this.number_window.active = true;
                this.number_window.visible = true;
                this.status_window.visible = true;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (个数输入窗口激活的情况下)
        //--------------------------------------------------------------------------
        public void update_number(){
    // 按下 B 键的情况下
    if( Input.is_trigger(Input.B)){
      // 演奏取消 SE
      Global.game_system.se_play(Global.data_system.cancel_se);
      // 设置个数输入窗口为不活动·非可视状态
      this.number_window.active = false;
      this.number_window.visible = false;
      // 命令窗口光标位置分支
      switch( this.command_window.index){
      case 0:  // 购买
        // 窗口状态转向购买模式
        this.buy_window.active = true;
        this.buy_window.visible = true;
              break;
      case 1:  // 卖出
        // 窗口状态转向卖出模式
        this.sell_window.active = true;
        this.sell_window.visible = true;
        this.status_window.visible = false;
              break;
      }
      return;
    }
    // 按下 C 键的情况下
    if( Input.is_trigger(Input.C)){
      // 演奏商店 SE
      Global.game_system.se_play(Global.data_system.shop_se);
      // 设置个数输入窗口为不活动·非可视状态
      this.number_window.active = false;
      this.number_window.visible = false;
      // 命令窗口光标位置分支
      switch( this.command_window.index){
      case 0:  // 购买
        // 购买处理
              Global.game_party.lose_gold(this.number_window.number * this.item.price);
        if(this.item is RPG.Item)
          Global.game_party.gain_item(this.item.id, this.number_window.number);
        else if(this.item is RPG.Weapon)
          Global.game_party.gain_weapon(this.item.id, this.number_window.number);
       else if(this.item is RPG.Armor)
          Global.game_party.gain_armor(this.item.id, this.number_window.number);

        // 刷新各窗口
        this.gold_window.refresh();
        this.buy_window.refresh();
        this.status_window.refresh();
        // 窗口状态转向购买模式
        this.buy_window.active = true;
        this.buy_window.visible = true;
        break;
      case 1:  // 卖出
        // 卖出处理
        Global.game_party.gain_gold(this.number_window.number * (this.item.price / 2));
          if(this.item is RPG.Item)
          Global.game_party.lose_item(this.item.id, this.number_window.number);
        else if(this.item is RPG.Weapon)
          Global.game_party.lose_weapon(this.item.id, this.number_window.number);
       else if(this.item is RPG.Armor)
          Global.game_party.lose_armor(this.item.id, this.number_window.number);
        // 刷新各窗口
        this.gold_window.refresh();
        this.sell_window.refresh();
        this.status_window.refresh();
        // 窗口状态转向卖出模式
        this.sell_window.active = true;
        this.sell_window.visible = true;
        this.status_window.visible = false;
              break;
      }
      return;
    }
  }

        public Window_Help help_window { get; set; }
        public Window_ShopCommand command_window { get; set; }
        public Window_Gold gold_window { get; set; }
        public Window_Base dummy_window { get; set; }
        public Window_ShopBuy buy_window { get; set; }
        public Window_ShopSell sell_window { get; set; }
        public Window_ShopNumber number_window { get; set; }
        public Window_ShopStatus status_window { get; set; }
        public RPG.Goods item { get; set; }
    }
}
