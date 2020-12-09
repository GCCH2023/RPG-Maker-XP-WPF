using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XP
{
    public class Input
    {
        private static int[] keyboard = new int[256];
        private static int keyCount = Enum.GetNames(typeof(Key)).Length;

        const int KeyRepeat = 4;
        // Input
        // 处理游戏手柄和键盘输入信息的模块。
        // 模块方法Input.update 
        // 更新输入信息。原则上 1 帧调用 1 次。
        public static void update()
        {
            for(int i = 1; i<keyCount; i++)
            {
                var state = Keyboard.GetKeyStates((Key)i);
                var lastState = keyboard[i];
                // 判断键是否重复按下
                keyboard[i] = (int)state;
                if((state & KeyStates.Down) > 0 && (lastState & (int)KeyStates.Down) > 0)
                {
                    keyboard[i] |= KeyRepeat;
                }
            }
        }

        //Input.is_press(num) 
        //判断与编号 num 对应的按钮是否现在被按下。

        //如果按下返回 true，未按下则返回 false。
        //if Input.is_press(Input.C)
        //  do_something
        //end
        public static bool is_press(int num)
        {
            num = ToKey(num);
            return Keyboard.IsKeyDown((Key)num);
        }


        //Input.is_trigger(num) 
        //判断与编号 num 对应的按钮是否重新被按下。
        //只有从未按下状态向按下状态变化的瞬间被认定是「重新被按下」。
        //如果按下返回 true，未按下则返回 false。
        public static bool is_trigger(int num)
        {
            num = ToKey(num);
            return Keyboard.IsKeyDown((Key)num) && Keyboard.IsKeyToggled((Key)num);
        }


        //Input.is_repeat(num) 
        //判断与编号 num 对应的按钮是否重新被按下。
        //和 is_trigger 不同的是，其考虑了连续按下按钮时的重复。
        //如果按下返回 true，未按下则返回 false。
        public static bool is_repeat(int num)
        {
            //num = ToKey(num);
            //return (keyboard[num] & KeyRepeat) > 0;
            num = ToKey(num);
            return Keyboard.IsKeyDown((Key)num) && Keyboard.IsKeyToggled((Key)num);
        }

        //Input.dir4 
        //判断方向按钮的状态，是 4 方向输入的特殊形式，返回与数字键对应的整数（2，4，6，8）。
        //方向按钮未按下（或被看作与其相同）则返回 0。
        public static int dir4()
        {
            if (Keyboard.IsKeyDown(Key.Left))
                return LEFT;

            if (Keyboard.IsKeyDown(Key.Right))
                return RIGHT;

            if (Keyboard.IsKeyDown(Key.Up))
                return UP;

            if (Keyboard.IsKeyDown(Key.Down))
                return DOWN;

            return 0;
        }
        //Input.dir8 
        //判断方向按钮的状态，是 8 方向输入的特殊形式，返回与数字键对应的整数（1，2，3，4，6，7，8，9）。
        //方向按钮未按下（或被看作与其相同）则返回 0。
        public static int dir8()
        {
            return 0;
        }

        private static int ToKey(int num)
        {
            switch(num)
            {
                case 2: return (int)Key.Down;
                case 4: return (int)Key.Left;
                case 6: return (int)Key.Right;
                case 8: return (int)Key.Up;
            }
            return num;
        }
        //常量DOWN LEFT RIGHT UP 
        //与方向按钮的下、左、右、上相对应的编号。
        public static int DOWN = 2;
        public static int LEFT = 4;
        public static int RIGHT = 6;
        public static int UP = 8;
        //A B C X Y Z L R 
        //与各按钮相对应的编号。
        public static int A = (int)Key.A;
        public static int B = (int)Key.B;
        public static int C = (int)Key.C;
        public static int X = (int)Key.X;
        public static int Y = (int)Key.Y;
        public static int Z = (int)Key.Z;
        public static int L = (int)Key.L;
        public static int R = (int)Key.R;
        //SHIFT CTRL ALT 
        //与键盘的 SHIFT、CTRL、ALT 键直接相对应的编号。
        public static int SHIFT = (int)Key.LeftShift;
        public static int CTRL = (int)Key.LeftCtrl;
        public static int ALT = (int)Key.LeftAlt;
        //F5 F6 F7 F8 F9 
        //与键盘的各功能键相对应的编号。除此之外的功能键已被系统保留，不能取得。
        public static int F5 = (int)Key.F5;
        public static int F6 = (int)Key.F6;
        public static int F7 = (int)Key.F7;
        public static int F8 = (int)Key.F8;
        public static int F9 = (int)Key.F9;
    }
}
