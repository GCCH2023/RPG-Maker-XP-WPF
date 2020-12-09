using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_Equip
    //------------------------------------------------------------------------------
    // 　处理装备画面的类。
    //==============================================================================

    public class Scene_Equip : Scene
    {
        public int actor_index { get; set; }
        public int equip_index { get; set; }
        public Game_Actor actor { get; set; }
        public Window_Help help_window { get; set; }
        public Window_EquipLeft left_window { get; set; }
        public Window_EquipRight right_window { get; set; }
        public Window_EquipItem item_window1 { get; set; }
        public Window_EquipItem item_window2 { get; set; }
        public Window_EquipItem item_window3 { get; set; }
        public Window_EquipItem item_window4 { get; set; }
        public Window_EquipItem item_window5 { get; set; }

        public Window_EquipItem item_window { get; set; }

        //--------------------------------------------------------------------------
        // ● 初始化对像
        //     actor_index : 角色索引
        //     equip_index : 装备索引
        //--------------------------------------------------------------------------
        public Scene_Equip(int actor_index = 0, int equip_index = 0)
        {
            this.actor_index = actor_index;
            this.equip_index = equip_index;
        }
        public override void Initialize()
        {
            // 获取角色
            this.actor = Global.game_party.actors[this.actor_index];
            // 生成窗口
            this.help_window = new Window_Help();
            this.left_window = new Window_EquipLeft(this.actor);
            this.right_window = new Window_EquipRight(this.actor);
            this.item_window1 = new Window_EquipItem(this.actor, 0);
            this.item_window2 = new Window_EquipItem(this.actor, 1);
            this.item_window3 = new Window_EquipItem(this.actor, 2);
            this.item_window4 = new Window_EquipItem(this.actor, 3);
            this.item_window5 = new Window_EquipItem(this.actor, 4);
            // 关联帮助窗口
            this.right_window.help_window = this.help_window;
            this.item_window1.help_window = this.help_window;
            this.item_window2.help_window = this.help_window;
            this.item_window3.help_window = this.help_window;
            this.item_window4.help_window = this.help_window;
            this.item_window5.help_window = this.help_window;
            // 设置光标位置
            this.right_window.index = this.equip_index;
            refresh();
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
                // 如果画面切换的话的就中断循环
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
            this.help_window.dispose();
            this.left_window.dispose();
            this.right_window.dispose();
            this.item_window1.dispose();
            this.item_window2.dispose();
            this.item_window3.dispose();
            this.item_window4.dispose();
            this.item_window5.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public virtual void refresh()
        {
            // 设置物品窗口的可视状态
            this.item_window1.visible = (this.right_window.index == 0);
            this.item_window2.visible = (this.right_window.index == 1);
            this.item_window3.visible = (this.right_window.index == 2);
            this.item_window4.visible = (this.right_window.index == 3);
            this.item_window5.visible = (this.right_window.index == 4);
            // 获取当前装备中的物品
            var item1 = this.right_window.item;
            // 设置当前的物品窗口到 this.item_window
            switch (this.right_window.index)
            {
                case 0:
                    this.item_window = this.item_window1; break;
                case 1:
                    this.item_window = this.item_window2; break;
                case 2:
                    this.item_window = this.item_window3; break;
                case 3:
                    this.item_window = this.item_window4; break;
                case 4:
                    this.item_window = this.item_window5; break;
            }
            // 右窗口被激活的情况下
            if (this.right_window.active)
            {
                // 删除变更装备后的能力
                // @@
                //this.left_window.set_new_parameters(null, null, null);
                this.left_window.set_new_parameters(0, 0, 0);
            }
            // 物品窗口被激活的情况下
            if (this.item_window.active)
            {
                // 获取现在选中的物品
                var item2 = this.item_window.item;
                // 变更装备
                var last_hp = this.actor.hp;
                var last_sp = this.actor.sp;
                this.actor.equip(this.right_window.index, item2 == null ? 0 : item2.id);
                // 获取变更装备后的能力值
                var new_atk = this.actor.atk;
                var new_pdef = this.actor.pdef;
                var new_mdef = this.actor.mdef;
                // 返回到装备
                this.actor.equip(this.right_window.index, item1 == null ? 0 : item1.id);
                this.actor.hp = last_hp;
                this.actor.sp = last_sp;
                // 描画左窗口
                this.left_window.set_new_parameters((int)new_atk, (int)new_pdef, (int)new_mdef);
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 刷新窗口
            this.left_window.update();
            this.right_window.update();
            this.item_window.update();
            refresh();
            // 右侧窗口被激活的情况下: 调用 update_right
            if (this.right_window.active)
            {
                update_right();
                return;
            }
            // 物品窗口被激活的情况下: 调用 update_item
            if (this.item_window.active)
            {
                update_item();
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (右侧窗口被激活的情况下)
        //--------------------------------------------------------------------------
        public void update_right()
        {
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 切换到菜单画面
                Global.scene = new Scene_Menu(2);
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 固定装备的情况下
                if (this.actor.is_equip_fix(this.right_window.index))
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                    return;
                }
                // 演奏确定 SE
                Global.game_system.se_play(Global.data_system.decision_se);
                // 激活物品窗口
                this.right_window.active = false;
                this.item_window.active = true;
                this.item_window.index = 0;
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
                // 切换到别的装备画面
                Global.scene = new Scene_Equip(this.actor_index, this.right_window.index);
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
                // 切换到别的装备画面
                Global.scene = new Scene_Equip(this.actor_index, this.right_window.index);
                return;
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (物品窗口被激活的情况下)
        //--------------------------------------------------------------------------
        public void update_item()
        {
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 激活右侧窗口
                this.right_window.active = true;
                this.item_window.active = false;
                this.item_window.index = -1;
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 演奏装备 SE
                Global.game_system.se_play(Global.data_system.equip_se);
                // 获取物品窗口现在选择的装备数据
                var item = this.item_window.item;
                // 变更装备
                this.actor.equip(this.right_window.index, item == null ? 0 : item.id);
                // 激活右侧窗口
                this.right_window.active = true;
                this.item_window.active = false;
                this.item_window.index = -1;
                // 再生成右侧窗口、物品窗口的内容
                this.right_window.refresh();
                this.item_window.refresh();
                return;
            }
        }
    }

}
