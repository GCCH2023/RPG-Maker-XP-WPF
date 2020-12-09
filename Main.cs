using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Main
    //------------------------------------------------------------------------------
    // 　各定义结束后、从这里开始实际处理。
    //==============================================================================
    public class Main
    {
        public static void Begin()
        {
            // 准备过渡
            // 设置系统默认字体
            Font.default_name = "黑体";
            Graphics.freeze();
            // 生成场景对像 (标题画面)
            Global.scene = new Scene_Title();
            // Global.scene 为有效的情况下调用 main 过程
            while (Global.scene != null)
                Global.scene.main();

            // 淡入淡出
            Graphics.transition(20);
            // rescue Errno.ENOENT
            // 补充 Errno.ENOENT 以外错误
            // 无法打开文件的情况下、显示信息后结束
            //filename = Global.!.message.sub("No such file || directory - ", "")
            //print("找不到文件 //{filename}。 ")
        }
    }
}
