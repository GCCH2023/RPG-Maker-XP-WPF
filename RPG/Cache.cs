using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using XP.Internal;

namespace XP.RPG
{
    //RPG.Cache
    //读取 RPGXP 的各种图片，建立并存储 Bitmap 对象的模块（高速缓存）。
    //该模块为了读取的高速和节约内存，把建立的 Bitmap 对象保存在内部的哈希表中，当再次使用同一位图时就返回已保存的对象。
    //因为上述动作，与其他精灵传送元指定相同的位图请注意不要用 Bitmap#dispose 释放掉。当使用位图时如果该对象已释放则会自动重新建立。位图会消耗大量内存，所以较少使用的位图还是将其释放为好。
    //当指定的文件名为空字符串时，会建立并返回一个 32×32 的 Bitmap。这与 Maker 的“(None)” 指定相对应。
    public class Cache
    {

        static Dictionary<object, Bitmap> cache = new Dictionary<object, Bitmap>();
        public static Bitmap load_bitmap(string folder_name, string filename, int hue = 0)
        {
            var path = folder_name + filename;

            if (!cache.ContainsKey(path) || cache[path].is_disposed)
            {
                if (filename != "")
                {
                    //var fullPath = Utility.Util.GetFullFileName(path);
                    cache.Add(path, new Bitmap(path));
                }
                else
                    cache.Add(path, new Bitmap(32, 32));
            }
            if (hue == 0)
                return cache[path];
            else
            {
                var key = new { path = path, hue = hue };
                if (!cache.ContainsKey(key) || cache[key].is_disposed)
                {
                    var bitmap = (Bitmap)cache[path].clone();
                    cache.Add(key, bitmap);
                    bitmap.hue_change(hue);
                }
                return cache[key];
            }
        }

        //模块方法RPG.Cache.animation(filename, hue) 
        //取得动画图像。hue 指定色相变化值。
        public static Bitmap animation(string filename, int hue)
        {
            return load_bitmap("Graphics/Animations/", filename, hue);
        }

        //RPG.Cache.autotile(filename) 
        //取得自动地图元件图像。
        public static Bitmap autotile(string filename)
        {
            return load_bitmap("Graphics/Autotiles/", filename);
        }
        //RPG.Cache.battleback(filename) 
        //取得战斗背景图像。
        public static Bitmap battleback(string filename)
        {
            return load_bitmap("Graphics/Battlebacks/", filename);
        }
        //RPG.Cache.battler(filename, hue) 
        //取得战斗者图像。hue 指定色相变化值。
        public static Bitmap battler(string filename, int hue)
        {
            return load_bitmap("Graphics/Battlers/", filename, hue);
        }
        //RPG.Cache.character(filename, hue) 
        //取得角色图像。hue 指定色相变化值。
        public static Bitmap character(string filename, int hue)
        {
            return load_bitmap("Graphics/Characters/", filename, hue);
        }
        //RPG.Cache.fog(filename, hue) 
        //取得雾图像。hue 指定色相变化值。
        public static Bitmap fog(string filename, int hue)
        {
            return load_bitmap("Graphics/Fogs/", filename, hue);
        }
        //RPG.Cache.gameover(filename) 
        //取得游戏结束图像。
        public static Bitmap gameover(string filename)
        {
            return load_bitmap("Graphics/Gameovers/", filename);
        }
        //RPG.Cache.icon(filename) 
        //取得图标图像。
        public static Bitmap icon(string filename)
        {
            return load_bitmap("Graphics/Icons/", filename);
        }
        //RPG.Cache.panorama(filename, hue) 
        //取得远景图像。hue 指定色相变化值。
        public static Bitmap panorama(string filename, int hue)
        {
            return load_bitmap("Graphics/Panoramas/", filename, hue);
        }
        //RPG.Cache.picture(filename) 
        //取得图片图像。
        public static Bitmap picture(string filename)
        {
            return load_bitmap("Graphics/Pictures/", filename);
        }
        //RPG.Cache.tileset(filename) 
        //取得图块图像。
        public static Bitmap tileset(string filename)
        {
            return load_bitmap("Graphics/Tilesets/", filename);
        }
        //RPG.Cache.title(filename) 
        //取得标题图像。
        public static Bitmap title(string filename)
        {
            return load_bitmap("Graphics/Titles/", filename);
        }
        //RPG.Cache.windowskin(filename) 
        //取得窗口皮肤图像。
        public static Bitmap windowskin(string filename)
        {
            return load_bitmap("Graphics/Windowskins/", filename);
        }
        //RPG.Cache.tile(filename, tile_id, hue) 
        //从图块中取得特定的地图元件。tile_id 指定取得文件的 ID，hue 指定色相变化值。
        //在事件的图像（RPG.Event.Page.Graphic）中指定了地图元件时使用。
        public static Bitmap tile(string filename, int tile_id, int hue)
        {
            var key = new
            {
                filename = filename,
                tile_id = tile_id,
                hue = hue
            };
            if (!cache.ContainsKey(key) || cache[key].is_disposed)
            {
                cache.Add(key, new Bitmap(32, 32));
                var x = (tile_id - 384) % 8 * 32;
                var y = (tile_id - 384) / 8 * 32;
                var rect = new Rect(x, y, 32, 32);
                cache[key].blt(0, 0, tileset(filename), rect);
                cache[key].hue_change(hue);
            }
            return cache[key];
        }
        //RPG.Cache.Clear(); 
        //清空高速缓存。
        public static void clear()
        {
            cache.Clear();
        }

    }
}
