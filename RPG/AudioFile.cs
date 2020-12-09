using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.RPG
{
    //        RPG.AudioFile
    //声音文件的数据类。兼容全部类型（RPG.AudioFile、BGS、ME、SE）。

    //父类Object 
    //参照元RPG.Map 
    //RPG.Skill 
    //RPG.Item 
    //RPG.Animation.Timing 
    //RPG.System 
    public class AudioFile
    {

        //属性name 
        //文件名。
        public string name { get; set; }
        //volume 
        //音量（0..100）。RPG.AudioFile、ME 的标准值为 100，BGS、SE 的标准值为 80。
        public int volume { get; set; }
        //pitch 
        //节拍（50..150）。标准值为 100。
        public int pitch { get; set; }
        //定义module RPG
        public AudioFile(string name = "", int volume = 100, int pitch = 100)
        {
            this.name = name;
            this.volume = volume;
            this.pitch = pitch;
        }
        public AudioFile()
        {

        }
    }
}
