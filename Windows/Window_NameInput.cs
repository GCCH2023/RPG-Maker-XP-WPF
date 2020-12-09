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
    // ■ Window_NameInput
    //------------------------------------------------------------------------------
    // 　输入名称的画面、文字选择窗口。
    //==============================================================================

    public class Window_NameInput : Window_Base
    {
        public static string[] CHARACTER_TABLE =
     {
    "あ","い","う","え","お",
    "か","き","く","け","こ",
    "さ","し","す","せ","そ",
    "た","ち","つ","て","と",
    "な","に","ぬ","ね","の",
    "は","ひ","ふ","へ","ほ",
    "ま","み","む","め","も",
    "や", "" ,"ゆ", "" ,"よ",
    "ら","り","る","れ","ろ",
    "わ", "" ,"を", "" ,"ん",
    "が","ぎ","ぐ","げ","ご",
    "ざ","じ","ず","ぜ","ぞ",
    "だ","ぢ","づ","で","ど",
    "ば","び","ぶ","べ","ぼ",
    "ぱ","ぴ","ぷ","ぺ","ぽ",
    "ゃ","ゅ","ょ","っ","ゎ",
    "ぁ","ぃ","ぅ","ぇ","ぉ",
    "ー","・", "" , "" , "" ,
    "ア","イ","ウ","エ","オ",
    "カ","キ","ク","ケ","コ",
    "サ","シ","ス","セ","ソ",
    "タ","チ","ツ","テ","ト",
    "ナ","ニ","ヌ","ネ","ノ",
    "ハ","ヒ","フ","ヘ","ホ",
    "マ","ミ","ム","メ","モ",
    "ヤ", "" ,"ユ", "" ,"ヨ",
    "ラ","リ","ル","レ","ロ",
    "ワ", "" ,"ヲ", "" ,"ン",
    "ガ","ギ","グ","ゲ","ゴ",
    "ザ","ジ","ズ","ゼ","ゾ",
    "ダ","ヂ","ヅ","デ","ド",
    "バ","ビ","ブ","ベ","ボ",
    "パ","ピ","プ","ペ","ポ",
    "ャ","ュ","ョ","ッ","ヮ",
    "ァ","ィ","ゥ","ェ","ォ",
    "ー","・","ヴ", "" , "" ,
  };
        //--------------------------------------------------------------------------
        // ● 初始化对像
        //--------------------------------------------------------------------------
        public Window_NameInput() :
            base(0, 128, 640, 352)
        {
            this.contents = new Bitmap(width - 32, height - 32);
            this.index = 0;
            refresh();
            update_cursor_rect();
        }
        //--------------------------------------------------------------------------
        // ● 获取文字
        //--------------------------------------------------------------------------
        public string character
        {
            get
            {
                return CHARACTER_TABLE[this.index];
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public override void refresh()
        {
            this.contents.clear();
            for (var i = 0; i <= 179; i++)
            {
                var x = 4 + i / 5 / 9 * 152 + i % 5 * 28;
                var y = i / 5 % 9 * 32;
                this.contents.draw_text(x, y, 28, 32, CHARACTER_TABLE[i], 1);
            }

            this.contents.draw_text(544, 9 * 32, 64, 32, "确定", 1);
        }
        //--------------------------------------------------------------------------
        // ● 刷新光标矩形
        //--------------------------------------------------------------------------
        public override void update_cursor_rect()
        {
            // 光标位置在 [确定] 的情况下
            if (this.index >= 180)
            {
                this.cursor_rect = new Rect(544, 9 * 32, 64, 32);
            }
            // 光标位置在 [确定] 以外的情况下
            else
            {
                var x = 4 + this.index / 5 / 9 * 152 + this.index % 5 * 28;
                var y = this.index / 5 % 9 * 32;
                this.cursor_rect = new Rect(x, y, 28, 32);
            }
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public override void update()
        {
            base.update();
            // 光标位置在 [确定] 的情况下
            if (this.index >= 180)
            {  // 光标下
                if (Input.is_trigger(Input.DOWN))
                {
                    Global.game_system.se_play(Global.data_system.cursor_se);
                    this.index -= 180;
                }

                // 光标上
                if (Input.is_repeat(Input.UP))
                {
                    Global.game_system.se_play(Global.data_system.cursor_se);
                    this.index -= 180 - 40;
                }
            }
            // 光标位置在 [确定] 以外的情况下
            else
            { // 按下方向键右的情况下
                if (Input.is_repeat(Input.RIGHT))
                {  // 按下状态不是重复的情况下、
                    // 光标位置不在右端的情况下
                    if (Input.is_trigger(Input.RIGHT) ||
                                  this.index / 45 < 3 || this.index % 5 < 4)
                    {
                        // 光标向右移动
                        Global.game_system.se_play(Global.data_system.cursor_se);
                        if (this.index % 5 < 4)
                            this.index += 1;
                        else
                            this.index += 45 - 4;

                        if (this.index >= 180)
                            this.index -= 180;

                    }
                }
                // 按下方向键左的情况下
                if (Input.is_repeat(Input.LEFT))
                {
                    // 按下状态不是重复的情况下、
                    // 光标位置不在左端的情况下
                    if (Input.is_trigger(Input.LEFT) ||
                                  this.index / 45 > 0 || this.index % 5 > 0)
                    {
                        // 光标向右移动
                        Global.game_system.se_play(Global.data_system.cursor_se);
                        if (this.index % 5 > 0)
                            this.index -= 1;
                        else
                            this.index -= 45 - 4;

                        if (this.index < 0)
                            this.index += 180;

                    }
                }
                // 按下方向键下的情况下
                if (Input.is_repeat(Input.DOWN))
                {
                    // 光标向下移动
                    Global.game_system.se_play(Global.data_system.cursor_se);
                    if (this.index % 45 < 40)
                        this.index += 5;
                    else
                        this.index += 180 - 40;
                }
                // 按下方向键上的情况下
                if (Input.is_repeat(Input.UP))
                {
                    // 按下状态不是重复的情况下、
                    // 光标位置不在上端的情况下
                    if (Input.is_trigger(Input.UP) || this.index % 45 >= 5)
                    {
                        // 光标向上移动
                        Global.game_system.se_play(Global.data_system.cursor_se);
                        if (this.index % 45 >= 5)
                            this.index -= 5;
                        else
                            this.index += 180;
                    }
                }
                // L 键与 R 键被按下的情况下
                if (Input.is_repeat(Input.L) || Input.is_repeat(Input.R))
                {
                    // 平假名 / 片假名 之间移动
                    Global.game_system.se_play(Global.data_system.cursor_se);
                    if (this.index / 45 < 2)
                        this.index += 90;
                    else
                        this.index -= 90;
                }
            }
            update_cursor_rect();
        }


        public int index { get; set; }
    }
}
