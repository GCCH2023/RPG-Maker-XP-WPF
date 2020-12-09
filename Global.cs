using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace XP
{
    public class Global
    {
        // 游戏屏幕大小
        public static readonly double ScreenWidth = 640;
        public static readonly double ScreenHeight = 480;
        /// <summary>
        /// 文件名装文件路径的转换器，由于RPG Maker XP中的图片等文件可以不包含后缀名
        /// 所以需要此类来查找文件（太坑爹了！！！）
        /// </summary>
        public static readonly XP.ImageSourceConverter ImageSourceConverter = new ImageSourceConverter();

        /// <summary>
        /// 在Visual Studio中设计游戏场景时，查找资源用到的文件目录，也就是所有资源的根据
        /// 通常此目录下包含Data文件夹（包含各种rxdata文件），Graphics文件夹（包含图片文件）
        /// Audio文件夹（包含声音文件）
        /// </summary>
        public static string DesignResourceFolder = @"E:\C#项目\RPG Maker\XP\bin\Debug\";

        public static string Title { get; set; }

        static Global()
        {

        }


        public static Game_Temp game_temp;
        public static Game_System game_system = new Game_System();
        public static Game_Party game_party;
        public static Game_Map game_map;
        public static Game_Switches game_switches;
        public static Game_Actors game_actors { get; set; }
        public static Game_Player game_player { get; set; }
        public static Game_Screen game_screen { get; set; }
        public static Game_Troop game_troop { get; set; }
        public static Game_SelfSwitches game_self_switches { get; set; }


        private static Scene _scene;
        public static Scene scene
        {
            get
            {
                return _scene;
            }
            set
            {
                // 设置新的场景时
                if (_scene != value)
                {
                    // 原来有场景的情况下，释放原来的场景的资源
                    if (_scene != null)
                        _scene.Uninitialize();

                    // 如果场景设置为空值，则关闭游戏
                    if (value == null)
                    {
                        App.Current.Shutdown(0);
                        return;
                    }

                    // 将原来的场景从窗口中清除并添加新的场景
                    _scene = value;
                    var gameBoard = ((MainWindow)App.Current.MainWindow).GameBoard.Children;
                    gameBoard.Clear();
                    gameBoard.Add(_scene);
                    ClearAllEventHandlers();

                    // 初始化新场景
                    value.Initialize();
                }
            }
        }
        public static Game_Variables game_variables;
        public static bool BTEST { get; set; }
        // 数据是否保存在XML文件中
        public static bool DataInXml = true;


        public static RPG.System data_system;
        public static List<RPG.State> data_states;
        public static List<RPG.Skill> data_skills;
        public static List<RPG.Actor> data_actors;
        public static List<RPG.Item> data_items;
        public static List<RPG.Class> data_classes;
        public static List<RPG.Armor> data_armors;
        public static List<RPG.Weapon> data_weapons;
        public static List<RPG.CommonEvent> data_common_events;
        public static List<RPG.Troop> data_troops { get; set; }
        public static List<RPG.Enemy> data_enemies { get; set; }
        static List<RPG.Tileset> _data_tilesets;


        public static List<RPG.Tileset> data_tilesets
        {
            get { return _data_tilesets; }
            set { _data_tilesets = value; }
        }
        static Random random = new Random();

        public static int rand(int n)
        {
            return random.Next(n);
        }

        public static object load_data(string filename)
        {
            return Marshal.Load(filename);
        }

        public static void LoadRXData()
        {
            data_actors = (List<RPG.Actor>)Global.load_data("Project/Data/Actors.rxdata");
            data_classes = (List<RPG.Class>)Global.load_data("Project/Data/Classes.rxdata");
            data_skills = (List<RPG.Skill>)Global.load_data("Project/Data/Skills.rxdata");
            data_items = (List<RPG.Item>)Global.load_data("Project/Data/Items.rxdata");
            data_weapons = (List<RPG.Weapon>)Global.load_data("Project/Data/Weapons.rxdata");
            data_armors = (List<RPG.Armor>)Global.load_data("Project/Data/Armors.rxdata");
            data_enemies = (List<RPG.Enemy>)Global.load_data("Project/Data/Enemies.rxdata");
            data_troops = (List<RPG.Troop>)Global.load_data("Project/Data/Troops.rxdata");
            data_states = (List<RPG.State>)Global.load_data("Project/Data/States.rxdata");
            data_animations = (List<RPG.Animation>)Global.load_data("Project/Data/Animations.rxdata");
            data_tilesets = (List<RPG.Tileset>)Global.load_data("Project/Data/Tilesets.rxdata");
            data_common_events = (List<RPG.CommonEvent>)Global.load_data("Project/Data/CommonEvents.rxdata");
            data_system = (RPG.System)Global.load_data("Project/Data/System.rxdata");
        }

        public static void LoadData()
        {
            data_actors = Utility.XML<List<RPG.Actor>>.Read("Xml/Data/Actors.xml");
            data_classes = Utility.XML<List<RPG.Class>>.Read("Xml/Data/Classes.xml");
            data_skills = Utility.XML<List<RPG.Skill>>.Read("Xml/Data/Skills.xml");
            data_items = Utility.XML<List<RPG.Item>>.Read("Xml/Data/Items.xml");
            data_weapons = Utility.XML<List<RPG.Weapon>>.Read("Xml/Data/Weapons.xml");
            data_armors = Utility.XML<List<RPG.Armor>>.Read("Xml/Data/Armors.xml");
            data_enemies = Utility.XML<List<RPG.Enemy>>.Read("Xml/Data/Enemies.xml");
            data_troops = Utility.XML<List<RPG.Troop>>.Read("Xml/Data/Troops.xml");
            data_states = Utility.XML<List<RPG.State>>.Read("Xml/Data/States.xml");
            data_animations = Utility.XML<List<RPG.Animation>>.Read("Xml/Data/Animations.xml");
            data_tilesets = Utility.XML<List<RPG.Tileset>>.Read("Xml/Data/Tilesets.xml");
            data_common_events = Utility.XML<List<RPG.CommonEvent>>.Read("Xml/Data/CommonEvents.xml");
            data_system = Utility.XML<RPG.System>.Read("Xml/Data/System.xml");
        }

        public static void SaveData()
        {
            Utility.XML<List<RPG.Actor>>.Write(data_actors, "Xml\\Data\\Actors.xml");
            Utility.XML<List<RPG.Class>>.Write(data_classes, "Xml\\Data\\Classes.xml");
            Utility.XML<List<RPG.Skill>>.Write(data_skills, "Xml\\Data\\Skills.xml");
            Utility.XML<List<RPG.Item>>.Write(data_items, "Xml\\Data\\Items.xml");
            Utility.XML<List<RPG.Weapon>>.Write(data_weapons, "Xml\\Data\\Weapons.xml");
            Utility.XML<List<RPG.Armor>>.Write(data_armors, "Xml\\Data\\Armors.xml");
            Utility.XML<List<RPG.Enemy>>.Write(data_enemies, "Xml\\Data\\Enemies.xml");
            Utility.XML<List<RPG.Troop>>.Write(data_troops, "Xml\\Data\\Troops.xml");
            Utility.XML<List<RPG.State>>.Write(data_states, "Xml\\Data\\States.xml");
            Utility.XML<List<RPG.Animation>>.Write(data_animations, "Xml\\Data\\Animations.xml");
            Utility.XML<List<RPG.Tileset>>.Write(data_tilesets, "Xml\\Data\\Tilesets.xml");
            Utility.XML<List<RPG.CommonEvent>>.Write(data_common_events, "Xml\\Data\\CommonEvents.xml");
            Utility.XML<RPG.System>.Write(data_system, "Xml\\Data\\System.xml");
        }

        /// <summary>
        /// 将rxdata游戏对象转换到
        /// </summary>
        public static void TransferGameFile()
        {
            LoadRXData();
            SaveData();
            // 地图
            int i = 1;
            bool exist = false;
            while(true)
            {
                string filename = string.Format("Project/Data/Map{0:D3}.rxdata", i);
                exist = System.IO.File.Exists(filename);
  
                if (!exist)
                    return;

                RPG.Map map = (RPG.Map)Global.load_data(filename);
                Utility.XML<RPG.Map>.Write(map, string.Format("Xml\\Data\\Map{0:D3}.xml", i));
                i++;
            }
        }
        public static bool DEBUG { get; set; }

        public static List<RPG.Animation> data_animations { get; set; }


        public static Drawing ViewportDrawing = new GeometryDrawing()
        {
            Brush = Brushes.Black,
            Geometry = new RectangleGeometry() { Rect = new System.Windows.Rect(0, 0, 100, 100) }
        };


        //public static void AddUIElement(UIElement element)
        //{
        //    if(scene != null)
        //        scene.Children.Add(element);
        //}

        //public static void RemoveUIElement(UIElement element)
        //{
        //    if (scene != null)
        //        scene.Children.Remove(element);
        //}

        /// <summary>
        /// 往当前场景中添加游戏对象
        /// </summary>
        /// <param name="element"></param>
        public static void AddUIElement(UIElement element)
        {
            if (scene == null)
                return;

            //((MainWindow)App.Current.MainWindow).GameBoard.Children.Add(element);
            scene.Children.Add(element);
        }
        /// <summary>
        /// 将指定游戏对象从当前场景中移除
        /// </summary>
        /// <param name="element"></param>
        public static void RemoveUIElement(UIElement element)
        {
            if (scene == null)
                return;
            scene.Children.Remove(element);
            //((MainWindow)App.Current.MainWindow).GameBoard.Children.Remove(element);
        }
        private static List<KeyEventHandler> keyUpEventHandlers = new List<KeyEventHandler>();
        /// <summary>
        /// 添加游戏窗口按键弹起事件处理方法
        /// </summary>
        public static event KeyEventHandler PreviewKeyUp
        {
            add 
            {
                App.Current.MainWindow.PreviewKeyUp += value;
                keyUpEventHandlers.Add(value);
            }
            remove 
            {
                App.Current.MainWindow.PreviewKeyUp -= value;
                keyUpEventHandlers.Remove(value);
            }
        }
        private static List<KeyEventHandler> keyDownEventHandlers = new List<KeyEventHandler>();
        /// <summary>
        /// 删除游戏窗口按键弹起事件处理方法
        /// </summary>
        public static event KeyEventHandler PreviewKeyDown
        {
            add
            {
                App.Current.MainWindow.PreviewKeyDown += value;
                keyDownEventHandlers.Add(value);
            }
            remove
            {
                App.Current.MainWindow.PreviewKeyDown -= value;
                keyDownEventHandlers.Remove(value);
            }
        }
        /// <summary>
        /// 清除所有事件处理方法
        /// </summary>
        public static void ClearAllEventHandlers()
        {
            while (keyDownEventHandlers.Count > 0)
                PreviewKeyDown -= keyDownEventHandlers[keyDownEventHandlers.Count - 1];
            while (keyUpEventHandlers.Count > 0)
                PreviewKeyUp -= keyUpEventHandlers[keyUpEventHandlers.Count - 1];
        }
    }
}