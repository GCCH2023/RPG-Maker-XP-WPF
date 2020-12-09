using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Scene_Gameover
    //------------------------------------------------------------------------------
    // 　处理游戏结束画面的类。
    //==============================================================================

    public class Scene_Gameover : Scene
    {
        XP.Internal.Sprite sprite;
        public override void Initialize()
        {
            // 生成游戏结束图形
            this.sprite = new XP.Internal.Sprite();
            this.sprite.bitmap = RPG.Cache.gameover(Global.data_system.gameover_name);
            // 停止 RPG.AudioFile、BGS
            Global.game_system.bgm_play(null);
            Global.game_system.bgs_play(null);
            // 演奏游戏结束 ME
            Global.game_system.me_play(Global.data_system.gameover_me);
            // 执行过渡
            Graphics.transition(120);
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
                // 刷新画面情报
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
            // 释放游戏结束图形
            this.sprite.bitmap.dispose();
            this.sprite.dispose();
            // 执行过度
            Graphics.transition(40);
            // 准备过渡
            Graphics.freeze();
            // 战斗测试的情况下
            if (Global.BTEST)
                Global.scene = null;
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            // 按下 C 键的情况下
            if (Input.is_trigger(Input.C))
                // 切换到标题画面
                Global.scene = new Scene_Title();
        }
    }
}
