using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    //==============================================================================
    // ■ Game_Map
    //------------------------------------------------------------------------------
    // 　处理地图的类。包含卷动以及可以通行的判断功能。
    // 本类的实例请参考 Global.game_map 。
    //==============================================================================

    public class Game_Map
    {
        //--------------------------------------------------------------------------
        // ● 定义实例变量
        //--------------------------------------------------------------------------
        public string tileset_name;    // 元件 文件名
        private List<string> _autotile_names;
        // 自动元件 文件名

        public List<string> autotile_names
        {
            get { return _autotile_names; }
            set { _autotile_names = value; }
        }
        public string panorama_name;           // 全景 文件名
        public int panorama_hue;         // 全景 色相
        public string fog_name;     // 雾 文件名
        public int fog_hue;  // 雾 色相
        public double fog_opacity;    // 雾 不透明度
        public int fog_blend_type;        // 雾 混合方式
        public double fog_zoom;     // 雾 放大率
        public double fog_sx; // 雾 SX
        public double fog_sy; // 雾 SY
        public string battleback_name;          // 战斗背景 文件名
        public int display_x; // 显示 X 坐标 * 128
        public int display_y;        // 显示 Y 坐标 * 128
        public bool need_refresh;            // 刷新要求标志
        public Table passages;         // 通行表
        public Table priorities;     // 优先表
        public Table terrain_tags;         // 地形标记表
        
        public Dictionary<int, Game_Event> events;      // 事件
        public double fog_ox;       // 雾 原点 X 坐标
        public double fog_oy;       // 雾 原点 Y 坐标
        public Tone fog_tone;        // 雾 色调
        public RPG.Map map;
        public Dictionary<object, Game_CommonEvent> common_events;
        //--------------------------------------------------------------------------
        // ● 初始化条件
        //--------------------------------------------------------------------------
        public Game_Map()
        {
            this.map_id = 0;
            this.display_x = 0;
            this.display_y = 0;
        }
        //--------------------------------------------------------------------------
        // ● 设置
        //     map_id : 地图 ID
        //--------------------------------------------------------------------------
        public void setup(int map_id)
        {
            // 地图 ID 记录到 this.map_id 
            this.map_id = map_id;
            // 地图文件装载后、设置到 this.map 
            // %03d
            this.map = (RPG.Map)Global.load_data(string.Format("Project/Data/Map{0:D3}.rxdata", this.map_id));
            // 定义实例变量设置地图元件信息
            var tileset = Global.data_tilesets[this.map.tileset_id];
            this.tileset_name = tileset.tileset_name;
            this.autotile_names = tileset.autotile_names;
            this.panorama_name = tileset.panorama_name;
            this.panorama_hue = tileset.panorama_hue;
            this.fog_name = tileset.fog_name;
            this.fog_hue = tileset.fog_hue;
            this.fog_opacity = tileset.fog_opacity;
            this.fog_blend_type = tileset.fog_blend_type;
            this.fog_zoom = tileset.fog_zoom;
            this.fog_sx = tileset.fog_sx;
            this.fog_sy = tileset.fog_sy;
            this.battleback_name = tileset.battleback_name;
            this.passages = tileset.passages;
            this.priorities = tileset.priorities;
            this.terrain_tags = tileset.terrain_tags;
            // 初始化显示坐标
            this.display_x = 0;
            this.display_y = 0;
            // 清除刷新要求标志
            this.need_refresh = false;
            // 设置地图事件数据
            this.events = new Dictionary<int, Game_Event>();
            foreach (var i in this.map.events.Keys)
                this.events[i] = new Game_Event(this.map_id, this.map.events[i]);

            // 设置公共事件数据
            this.common_events = new Dictionary<object, Game_CommonEvent>();
            for (var i = 1; i < Global.data_common_events.Count; i++)
                this.common_events[i] = new Game_CommonEvent(i);

            // 初始化雾的各种信息
            this.fog_ox = 0;
            this.fog_oy = 0;
            this.fog_tone = new Tone(0, 0, 0, 0);
            this.fog_tone_target = new Tone(0, 0, 0, 0);
            this.fog_tone_duration = 0;
            this.fog_opacity_duration = 0;
            this.fog_opacity_target = 0;
            // 初始化滚动信息
            this.scroll_direction = 2;
            this.scroll_rest = 0;
            this.scroll_speed = 4;
        }
        //--------------------------------------------------------------------------
        // ● 获取地图 ID
        //--------------------------------------------------------------------------
        int _map_id;


        public int map_id
        {
            get { return _map_id; }
            set { _map_id = value; }
        }
        //--------------------------------------------------------------------------
        // ● 获取宽度
        //--------------------------------------------------------------------------

        public int width
        {
            get { return this.map.width; }
        }
        //--------------------------------------------------------------------------
        // ● 获取高度
        //--------------------------------------------------------------------------
        public int height
        {
            get
            {
                return this.map.height;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取遇敌列表
        //--------------------------------------------------------------------------
        public List<int> encounter_list
        {
            get
            {
                return this.map.encounter_list;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取遇敌步数
        //--------------------------------------------------------------------------
        public int encounter_step
        {
            get
            {
                return this.map.encounter_step;
            }
        }
        //--------------------------------------------------------------------------
        // ● 获取地图数据
        //--------------------------------------------------------------------------
        public Table data
        {
            get
            {
                return this.map.data;
            }
        }
        //--------------------------------------------------------------------------
        // ● RPG.AudioFile / BGS 自动切换
        //--------------------------------------------------------------------------
        public void autoplay()
        {
            if (this.map.autoplay_bgm)
                Global.game_system.bgm_play(this.map.bgm);

            if (this.map.autoplay_bgs)
                Global.game_system.bgs_play(this.map.bgs);
        }
        //--------------------------------------------------------------------------
        // ● 刷新
        //--------------------------------------------------------------------------
        public virtual void refresh()
        {
            // 地图 ID 有效
            if (this.map_id > 0)
            {
                // 刷新全部的地图事件
                foreach (var event1 in this.events.Values)
                    event1.refresh();
            }

            // 刷新全部的公共事件
            foreach (var common_event in this.common_events.Values)
                common_event.refresh();

            // 清除刷新要求标志
            this.need_refresh = false;
        }
        //--------------------------------------------------------------------------
        // ● 向下滚动
        //     distance : 滚动距离
        //--------------------------------------------------------------------------
        public void scroll_down(int distance)
        {
            this.display_y = Math.Min(this.display_y + distance, (this.height - 15) * 128);
        }
        //--------------------------------------------------------------------------
        // ● 向左滚动
        //     distance : 滚动距离
        //--------------------------------------------------------------------------
        public void scroll_left(int distance)
        {
            this.display_x = Math.Max(this.display_x - distance, 0);
        }
        //--------------------------------------------------------------------------
        // ● 向右滚动
        //     distance : 滚动距离
        //--------------------------------------------------------------------------
        public void scroll_right(int distance)
        {
            this.display_x = Math.Min(this.display_x + distance, (this.width - 20) * 128);
        }
        //--------------------------------------------------------------------------
        // ● 向上滚动
        //     distance : 滚动距离
        //--------------------------------------------------------------------------
        public void scroll_up(int distance)
        {
            this.display_y = Math.Max(this.display_y - distance, 0);
        }
        //--------------------------------------------------------------------------
        // ● 有效坐标判定
        //     x          : X 坐标
        //     y          : Y 坐标
        //--------------------------------------------------------------------------
        public bool is_valid(int x, int y)
        {
            return (x >= 0 && x < width && y >= 0 && y < height);
        }
        //--------------------------------------------------------------------------
        // ● 可以通行判定
        //     x          : X 坐标
        //     y          : Y 坐标
        //     d          : 方向 (0,2,4,6,8,10)
        //                  ※ 0,10 = 全方向不能通行的情况的判定 (跳跃等)
        //     self_event : 自己 (判定事件可以通行的情况下)
        //--------------------------------------------------------------------------
        public bool is_passable(int x, int y, int d, object self_event = null)
        {
            // 被给予的坐标地图外的情况下
            if (!is_valid(x, y))
                // 不能通行
                return false;

            // 方向 (0,2,4,6,8,10) 与障碍物接触 (0,1,2,4,8,0) 后变换
            var bit = (1 << (d / 2 - 1)) & 0x0f;
            // 循环全部的事件
            foreach (var event1 in events.Values)
            {
                // 自己以外的元件与坐标相同的情况
                if (event1.tile_id >= 0 && event1 != self_event &&
                                event1.x == x && event1.y == y && !event1.through)
                {

                    // 如果障碍物的接触被设置的情况下
                    if ((this.passages[event1.tile_id] & bit) != 0)
                        // 不能通行
                        return false;
                    // 如果全方向的障碍物的接触被设置的情况下
                    else if ((this.passages[event1.tile_id] & 0x0f) == 0x0f)
                        // 不能通行
                        return false;
                    // 这以外的优先度为 0 的情况下
                    else if (this.priorities[event1.tile_id] == 0)
                        // 可以通行
                        return true;
                }
            }
            // 从层按从上到下的顺序调查循环
            for (var i = 2; i >= 0; i--)
            {
                // 取得元件 ID
                var tile_id = data[x, y, i];
                // 取得元件 ID 失败
                if (tile_id == null)
                    // 不能通行
                    return false;
                // 如果障碍物的接触被设置的情况下
                else if ((this.passages[tile_id] & bit) != 0)
                    // 不能通行
                    return false;
                // 如果全方向的障碍物的接触被设置的情况下
                else if ((this.passages[tile_id] & 0x0f) == 0x0f)
                    // 不能通行
                    return false;
                // 这以外的优先度为 0 的情况下
                else if (this.priorities[tile_id] == 0)
                    // 可以通行
                    return true;
            }
            // 可以通行
            return true;
        }
        //--------------------------------------------------------------------------
        // ● 茂密判定
        //     x          : X 坐标
        //     y          : Y 坐标
        //--------------------------------------------------------------------------
        public bool is_bush(int x, int y)
        {
            if (this.map_id != 0)
            {
                for (var i = 2; i > 0; i--)
                {
                    var tile_id = data[x, y, i];
                    if (tile_id == null)
                        return false;
                    else if ((this.passages[tile_id] & 0x40) == 0x40)
                        return true;
                }
            }
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 柜台判定
        //     x          : X 坐标
        //     y          : Y 坐标
        //--------------------------------------------------------------------------
        public bool is_counter(int x, int y)
        {
            if (this.map_id != 0)
            {
                for (var i = 2; i >= 0; i--)
                {
                    var tile_id = data[x, y, i];
                    if (tile_id == null)
                        return false;
                    else if ((this.passages[tile_id] & 0x80) == 0x80)
                        return true;
                }
            }
            return false;
        }
        //--------------------------------------------------------------------------
        // ● 获取地形标志
        //     x          : X 坐标
        //     y          : Y 坐标
        //--------------------------------------------------------------------------
        public int terrain_tag(int x, int y)
        {
            if (this.map_id != 0)
            {
                for (var i = 2; i >= 0; i--)
                {
                    var tile_id = data[x, y, i];
                    if (tile_id == null)
                        return 0;
                    else if (this.terrain_tags[tile_id] > 0)
                        return this.terrain_tags[tile_id];
                }
            }
            return 0;
        }
        //--------------------------------------------------------------------------
        // ● 获取指定位置的事件 ID
        //     x          : X 坐标
        //     y          : Y 坐标
        //--------------------------------------------------------------------------
        public int check_event(int x, int y)
        {
            foreach (var event1 in Global.game_map.events.Values)
                if (event1.x == x && event1.y == y)
                    return event1.id;

            return 0;
        }
        //--------------------------------------------------------------------------
        // ● 滚动开始
        //     direction : 滚动方向
        //     distance  : 滚动距离
        //     speed     : 滚动速度
        //--------------------------------------------------------------------------
        public void start_scroll(int direction, int distance, int speed)
        {
            this.scroll_direction = direction;
            this.scroll_rest = distance * 128;
            this.scroll_speed = speed;
        }
        //--------------------------------------------------------------------------
        // ● 滚动中判定
        //--------------------------------------------------------------------------
        public bool is_scrolling
        {
            get
            {
                return this.scroll_rest > 0;
            }
        }
        //--------------------------------------------------------------------------
        // ● 开始变更雾的色调
        //     tone     : 色调
        //     duration : 时间
        //--------------------------------------------------------------------------
        public void start_fog_tone_change(Tone tone, int duration)
        {
            this.fog_tone_target = tone.clone();
            this.fog_tone_duration = duration;
            if (this.fog_tone_duration == 0)
                this.fog_tone = this.fog_tone_target.clone();
        }
        //--------------------------------------------------------------------------
        // ● 开始变更雾的不透明度
        //     opacity  : 不透明度
        //     duration : 时间
        //--------------------------------------------------------------------------
        public void start_fog_opacity_change(double opacity, int duration)
        {
            this.fog_opacity_target = opacity * 1.0;
            this.fog_opacity_duration = duration;
            if (this.fog_opacity_duration == 0)
                this.fog_opacity = this.fog_opacity_target;
        }
        //--------------------------------------------------------------------------
        // ● 刷新画面
        //--------------------------------------------------------------------------
        public virtual void update()
        {
            // 还原必要的地图
            if (Global.game_map.need_refresh)
                refresh();

            // 滚动中的情况下
            if (this.scroll_rest > 0)
            {
                // 滚动速度变化为地图坐标系的距离
                var distance = (int)Math.Pow(2, this.scroll_speed);
                // 执行滚动
                switch (this.scroll_direction)
                {
                    case 2:  // 下
                        scroll_down(distance);
                        break;
                    case 4:  // 左
                        scroll_left(distance);
                        break;
                    case 6:  // 右
                        scroll_right(distance); break;
                    case 8:  // 上
                        scroll_up(distance); break;
                }
                // 滚动距离的减法运算
                this.scroll_rest -= distance;
            }
            // 更新地图事件
            foreach (var event1 in this.events.Values)
                event1.update();

            // 更新公共事件
            foreach (var common_event in this.common_events.Values)
                common_event.update();

            // 处理雾的滚动
            this.fog_ox -= this.fog_sx / 8.0;
            this.fog_oy -= this.fog_sy / 8.0;
            // 处理雾的色调变更
            if (this.fog_tone_duration >= 1)
            {
                var d = this.fog_tone_duration;
                var target = this.fog_tone_target;
                this.fog_tone.red = (this.fog_tone.red * (d - 1) + target.red) / d;
                this.fog_tone.green = (this.fog_tone.green * (d - 1) + target.green) / d;
                this.fog_tone.blue = (this.fog_tone.blue * (d - 1) + target.blue) / d;
                this.fog_tone.gray = (this.fog_tone.gray * (d - 1) + target.gray) / d;
                this.fog_tone_duration -= 1;
            }
            // 处理雾的不透明度变更
            if (this.fog_opacity_duration >= 1)
            {
                var d = this.fog_opacity_duration;
                this.fog_opacity = (this.fog_opacity * (d - 1) + this.fog_opacity_target) / d;
                this.fog_opacity_duration -= 1;
            }
        }


        public Tone fog_tone_target { get; set; }
        public int fog_tone_duration { get; set; }
        public int fog_opacity_duration { get; set; }
        public double fog_opacity_target { get; set; }
        public int scroll_direction { get; set; }
        public int scroll_rest { get; set; }
        public int scroll_speed { get; set; }
    }
}
