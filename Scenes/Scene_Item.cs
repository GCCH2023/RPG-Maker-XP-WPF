using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_Item
    //------------------------------------------------------------------------------
    // 　处理物品画面的类。
    //==============================================================================

    public class Scene_Item : Scene
    {
        public Window_Help help_window { get; set; }
        public Window_Item item_window { get; set; }
        public Window_Target target_window { get; set; }
        public RPG.Item item { get; set; }
        public override void Initialize()
        {
            // 生成帮助窗口、物品窗口
            this.help_window = new Window_Help();
            this.item_window = new Window_Item();
            // 关联帮助窗口
            this.item_window.help_window = this.help_window;
            // 生成目标窗口 (设置为不可见・不活动)
            this.target_window = new Window_Target();
            this.target_window.visible = false;
            this.target_window.active = false;
            // 执行过度
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
                // 如果画面切换就中断循环
            //    if (Global.scene != this)
            //    {
            //        break;
            //    }
            //}
        }
        public override void Uninitialize()
        {
            // 装备过渡
            Graphics.freeze();
            // 释放窗口
            this.help_window.dispose();
            this.item_window.dispose();
            this.target_window.dispose();
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 刷新窗口
            this.help_window.update();
            this.item_window.update();
            this.target_window.update();
            // 物品窗口被激活的情况下: 调用 update_item
            if (this.item_window.active)
            {
                update_item();
                return;
            }
            // 目标窗口被激活的情况下: 调用 update_target
            if (this.target_window.active)
            {
                update_target();
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
                // 切换到菜单画面
                Global.scene = new Scene_Menu(0);
                return;
            }
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 获取物品窗口当前选中的物品数据
                this.item = (RPG.Item)this.item_window.item;
                // 不使用物品的情况下
                if (!(this.item is RPG.Item))
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                    return;
                }
                // 不能使用的情况下
                if (!Global.game_party.is_item_can_use(this.item.id))
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                    return;
                }
                // 演奏确定 SE
                Global.game_system.se_play(Global.data_system.decision_se);
                // 效果范围是我方的情况下
                if (this.item.scope >= 3)
                {
                    // 激活目标窗口
                    this.item_window.active = false;
                    this.target_window.x = (this.item_window.index + 1) % 2 * 304;
                    this.target_window.visible = true;
                    this.target_window.active = true;
                    // 设置效果范围 (单体/全体) 的对应光标位置
                    if (this.item.scope == 4 || this.item.scope == 6)
                        this.target_window.index = -1;
                    else
                        this.target_window.index = 0;
                }
                // 效果在我方以外的情况下
                else
                {
                    // 公共事件 ID 有效的情况下
                    if (this.item.common_event_id > 0)
                    {
                        // 预约调用公共事件
                        Global.game_temp.common_event_id = this.item.common_event_id;
                        // 演奏物品使用时的 SE
                        Global.game_system.se_play(this.item.menu_se);
                        // 消耗品的情况下
                        if (this.item.consumable)
                        {
                            // 使用的物品数减 1
                            Global.game_party.lose_item(this.item.id, 1);
                            // 再描绘物品窗口的项目
                            this.item_window.draw_item(this.item_window.index);


                            // 切换到地图画面
                            Global.scene = new Scene_Map();
                            return;
                        }
                    }
                    return;
                }
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面 (目标窗口被激活的情况下)
        //--------------------------------------------------------------------------
        public void update_target()
        {
            // 按下 B 键的情况下
            if (Input.is_trigger(Input.B))
            {
                // 演奏取消 SE
                Global.game_system.se_play(Global.data_system.cancel_se);
                // 由于物品用完而不能使用的场合
                if (!Global.game_party.is_item_can_use(this.item.id))
                {
                    // 再次生成物品窗口的内容
                    this.item_window.refresh();
                }
                // 删除目标窗口
                this.item_window.active = true;
                this.target_window.visible = false;
                this.target_window.active = false;
                return;
            }
            bool used = false;
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
            {
                // 如果物品用完的情况下
                if (Global.game_party.item_number(this.item.id) == 0)
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                    return;
                }
                // 目标是全体的情况下
                if (this.target_window.index == -1)
                {
                    // 对同伴全体应用物品使用效果
                    foreach (var i in Global.game_party.actors)
                    {
                        used = i.item_effect(this.item) || used;
                    }
                }
                // 目标是单体的情况下
                if (this.target_window.index >= 0)
                {
                    // 对目标角色应用物品的使用效果
                    var target = Global.game_party.actors[this.target_window.index];
                    used = target.item_effect(this.item);
                }
                // 使用物品的情况下
                if (used)
                {
                    // 演奏物品使用时的 SE
                    Global.game_system.se_play(this.item.menu_se);
                    // 消耗品的情况下
                    if (this.item.consumable)
                    {
                        // 使用的物品数减 1
                        Global.game_party.lose_item(this.item.id, 1);
                        // 再描绘物品窗口的项目
                        this.item_window.draw_item(this.item_window.index);
                    }
                    // 再生成目标窗口的内容
                    this.target_window.refresh();
                    // 全灭的情况下
                    if (Global.game_party.is_all_dead)
                    {
                        // 切换到游戏结束画面
                        Global.scene = new Scene_Gameover();
                        return;
                    }
                    // 公共事件 ID 有效的情况下
                    if (this.item.common_event_id > 0)
                    {
                        // 预约调用公共事件
                        Global.game_temp.common_event_id = this.item.common_event_id;
                        // 切换到地图画面
                        Global.scene = new Scene_Map();
                        return;
                    }
                }
                // 无法使用物品的情况下
                if (!used)
                {
                    // 演奏冻结 SE
                    Global.game_system.se_play(Global.data_system.buzzer_se);
                }
                return;
            }
        }
    }
}
