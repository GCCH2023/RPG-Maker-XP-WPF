using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace XP
{
    //        进行有关音乐，声音处理的模块。
    public class Audio
    {
        static MediaPlayer player = new MediaPlayer();

        //模块方法Audio.bgm_play(filename[, volume[, pitch]]) 
        //开始 RPG.AudioFile 的播放。其顺序为文件名，音量，指定节拍。
        //会自动搜索 RGSS-RTP 中包含的文件。可以省略文件扩展名。
        public static void bgm_play(string filename, int volume = 20, int pitch = 0)
        {
            player.Volume = volume;
            player.Open(new Uri(filename, UriKind.RelativeOrAbsolute));
            player.MediaEnded += (sender, e) =>
            {//播放结束后 又重新播放
                player.Position = new TimeSpan(0);
            };
            player.Play();
        }


        //Audio.bgm_stop 
        //停止 RPG.AudioFile 的播放。
        public static void bgm_stop()
        {
            if(player != null)
                player.Stop();
        }

        //Audio.bgm_fade(time) 
        //开始 RPG.AudioFile 的淡出。time 指定淡出的时间，单位为毫秒。
        public static void bgm_fade(int time)
        {

        }

        static SoundPlayer bgsPlayer;
        //Audio.bgs_play(filename[, volume[, pitch]]) 
        //开始 BGS 的播放。其顺序为文件名，音量，指定节拍。
        //会自动搜索 RGSS-RTP 中包含的文件。可以省略文件扩展名。
        public static void bgs_play(string filename, int volume = 20, int pitch = 0)
        {
            bgsPlayer = new SoundPlayer(filename);
            bgsPlayer.Play();
        }
        //Audio.bgs_stop 
        //停止 BGS 的播放。
        public static void bgs_stop()
        {
            if(bgsPlayer != null)
                bgsPlayer.Stop();
        }
        //Audio.bgs_fade(time) 
        //开始 BGS 的淡出。time 指定淡出的时间，单位为毫秒。
        public static void bgs_fade(int time)
        {

        }

        static MediaPlayer mePlayer = new MediaPlayer();
        //Audio.me_play(filename[, volume[, pitch]]) 
        //开始 ME 的播放。其顺序为文件名，音量，指定节拍。
        //会自动搜索 RGSS-RTP 中包含的文件。可以省略文件扩展名。
        public static void me_play(string filename, int volume = 20, int pitch = 0)
        {
            mePlayer.Volume = volume;
            mePlayer.Open(new Uri(filename, UriKind.RelativeOrAbsolute));
            mePlayer.Play();
        }
        //Audio.me_stop 
        //停止 ME 的播放。
        public static void me_stop()
        {
            mePlayer.Stop();
        }
        //Audio.me_fade(time) 
        //开始 ME 的淡出。time 指定淡出的时间，单位为毫秒。。
        public static void me_fade(int time)
        {

        }
        static SoundPlayer sePlayer;
        //Audio.se_play(filename[, volume[, pitch]]) 
        //开始 SE 的播放。其顺序为文件名，音量，指定节拍。
        //会自动搜索 RGSS-RTP 中包含的文件。可以省略文件扩展名。
        //如果在极短的时间内连续播放相同 SE 的话，为了防止音裂，会进行自动间隔处理。
        public static void se_play(string filename, int volume = 20, int pitch = 0)
        {
            sePlayer = new SoundPlayer(filename);
            sePlayer.Play();
        }
        //Audio.se_stop 
        //停止全部 SE 的播放。
        public static void se_stop()
        {
            sePlayer.Stop();
        }

    }
}
