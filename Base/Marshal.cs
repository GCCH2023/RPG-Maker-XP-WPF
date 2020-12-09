using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    public class Marshal
    {
        internal static void dump(object obj, File file)
        {
        }

        internal static object load(File file)
        {
            return Load(file);
        }
        /// <summary>
        /// 记录下所有出现的符号名(标识符名)
        /// </summary>
        private static List<string> symbols = new List<string>(32);
        public static object Load(string filename)
        {
            var fileData = System.IO.File.ReadAllBytes(filename);

            symbols.Clear();
            unsafe
            {
                fixed (byte* p = fileData)
                {
                    byte* q = p;
                    return ReadRxdata(ref q);
                }
            }
        }

        public unsafe static object Load(File file)
        {
            return ReadRxdata(ref file.position);
        }

        private unsafe static object ReadRxdata(ref byte* p)
        {
            // 开头两个字节是主版本号和副版本号
            RubyParseException.BaseAddress = (uint)p;

            if (*(UInt16*)p != 0x804)
                throw new RubyParseException((uint)p, "文件格式错误或版本号不是0x0408");

            byte* start = p + 2;
            return ParseValue(ref start);
        }


        static void AddSymbol(string name)
        {
            symbols.Add(name);
        }
        static string GetSymbol(int index)
        {
            return symbols[index];
        }
        /// <summary>
        /// 类命对应类型
        /// </summary>

        static Dictionary<string, Type> nameType = new Dictionary<string, Type>()
        {
            {"RPG::Actor", typeof(RPG.Actor)},
            {"RPG::Animation", typeof(RPG.Animation)},
            {"RPG::Armor", typeof(RPG.Armor)},
            {"RPG::AudioFile", typeof(RPG.AudioFile)},
            {"RPG::Cache", typeof(RPG.Cache)},
            {"RPG::Class", typeof(RPG.Class)},
            {"RPG::CommonEvent", typeof(RPG.CommonEvent)},
            {"RPG::Enemy", typeof(RPG.Enemy)},
            {"RPG::Event", typeof(RPG.Event)},
            {"RPG::EventCommand", typeof(RPG.EventCommand)},
            {"RPG::Item", typeof(RPG.Item)},
            {"RPG::Map", typeof(RPG.Map)},
            {"RPG::MapInfo", typeof(RPG.MapInfo)},
            {"RPG::MoveCommand", typeof(RPG.MoveCommand)},
            {"RPG::MoveRoute", typeof(RPG.MoveRoute)},
            {"RPG::Skill", typeof(RPG.Skill)},
              {"RPG::Sprite", typeof(RPG.Sprite)},
            {"RPG::State", typeof(RPG.State)},
            {"RPG::System", typeof(RPG.System)},
            {"RPG::Tileset", typeof(RPG.Tileset)},
            {"RPG::Troop", typeof(RPG.Troop)},
             {"RPG::Weapon", typeof(RPG.Weapon)},
            {"RPG::Weather", typeof(RPG.Weather)},
           {"RPG::Animation::Frame", typeof(RPG.Animation.Frame)},
          {"RPG::Animation::Timing", typeof(RPG.Animation.Timing)},
            {"RPG::Class::Learning", typeof(RPG.Class.Learning)},
          {"RPG::Enemy::Action", typeof(RPG.Enemy.Action)},
           {"RPG::Event::Page", typeof(RPG.Event.Page)},
          {"RPG::Event::Page::Condition", typeof(RPG.Event.Page.Condition)},
           {"RPG::Event::Page::Graphic", typeof(RPG.Event.Page.Graphic)},
            {"RPG::System::TestBattler", typeof(RPG.System.TestBattler)},
            {"RPG::System::Words", typeof(RPG.System.Words)},
            {"RPG::Troop::Member", typeof(RPG.Troop.Member)},
          {"RPG::Troop::Page", typeof(RPG.Troop.Page)},
          {"RPG::Troop::Page::Condition", typeof(RPG.Troop.Page.Condition)},
          {"Table", typeof(Table)},
        };
        public unsafe static object ParseRubyObject(ref byte* p)
        {
            // "o"
            if (*p++ != 0x6F)
                throw new RubyParseException((uint)p, "文件类名标志错误");

            string className;
            byte kind = *p;     // 当前对象的类型是引用还是定义
            if (kind == 0x3B)      // ';' 符号引用
            {
                p++;

                var index = ParseLongInteger(ref p);        // 获取符号索引
                className = GetSymbol(index);
            }
            else if (*p == 0x3A)         // ':' 定义
            {
                p++;
                // 解析类名
                className = ParseString(ref p);

                AddSymbol(className);
            }
            else
                throw new RubyParseException((uint)p, "期待符号定义");

            if (!nameType.ContainsKey(className))
                throw new RubyParseException((uint)p, className + "未定义");

            var classType = nameType[className];
            // 创建对象
            var obj = System.Activator.CreateInstance(classType);

            // 成员个数
            var memberCount = *p++ - 5;

            // 解析成员
            for (int i = 0; i < memberCount; i++)
            {
                string name;
                if (*p == 0x3A)   // ':'
                {
                    p++;
                    name = ParseString(ref p, true);

                    AddSymbol(name);
                }
                else if(*p == 0x3B)     // ';'
                {
                    p++;
                    var index = ParseLongInteger(ref p);        // 获取符号索引
                    name = GetSymbol(index);
                }
                else
                    throw new RubyParseException((uint)p, "类成员名称前面期望的是 : 或 ; ");

                // 解析成员的值
                var value = ParseValue(ref p);
                var property = classType.GetProperty(name);
                // 设置成员的值
                try
                {
                    property.SetValue(obj, value);
                }
                catch (Exception e)
                {
                    // 可能是int字段的原因
                    if (name == "int")
                    {
                        name = "int1";
                        classType.GetProperty(name).SetValue(obj, value);
                        continue;
                    }

                    // 转换失败，可能是List对象问题，这里还是要判断一下的可能是 空数组[] 的情况，就不知道元素类型啊
                    var list1 = SetList(property, value);
                    if (list1 != null)
                    {
                        property.SetValue(obj, list1);
                        continue;
                    }

                     // 尝试哈希表
                    var dictionary = SetDictionary(property, value);
                    if (dictionary != null)
                    {
                        property.SetValue(obj, dictionary);
                        continue;
                    }

                    throw e;
                }
            }

            //System.Windows.MessageBox.Show("class name = " + className);

            return obj;
        }
        /// <summary>
        /// 将value转换为有具体类型的Dictionary
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object SetDictionary(PropertyInfo property, object value)
        {
            // System.Collections.IDictionary;
            var dictionary = value as Dictionary<object, object>;
            if (dictionary == null)
                return null;

            var type = property.PropertyType;
            var elementTypes = type.GenericTypeArguments;

            var newDictionary = (System.Collections.IDictionary)Activator.CreateInstance(type);

            foreach (var obj in dictionary)
            {
                var k = Utility.Util.ConvertToObject(obj.Key, elementTypes[0]);
                var v = Utility.Util.ConvertToObject(obj.Value, elementTypes[1]);
                newDictionary.Add(k, v);
            }
            return newDictionary;

        }
        /// <summary>
        /// 尝试设置List属性的值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        private static object SetList(PropertyInfo property, object value)
        {
            var list = value as System.Collections.IList;
            if (list == null)
                return null;

            var type = property.PropertyType;
            var elementType = type.GetElementType();
            var newList = (System.Collections.IList)Activator.CreateInstance(type);

            foreach (var obj in list)
            {
                var v = Utility.Util.ConvertToObject(obj, elementType);
                newList.Add(v);
            }
            return newList;
        }
        /// <summary>
        /// 解析类型和值
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static unsafe object ParseValue(ref byte* p)
        {
            switch (*p)
            {
                case 0x30:      // '0' nil
                    p++;
                    return null;
                case 0x54:      // 'T' true
                    p++;
                    return true;
                case 0x46:      // 'F' false
                    p++;
                    return false;
                case 0x6F:      // 'o'
                    return ParseRubyObject(ref p);
                case 0x69:      // 'i'
                    return ParseFixnum(ref p);
                case 0x22:      // '"'
                    p++;
                    return ParseString(ref p);
                case 0x5B:      // '[' 数组
                    p++;
                    return ParseArray(ref p);
                case 0x75:      // 'u' 定义了 _dump函数的类，该函数返回一个字符串用于序列化
                    p++;
                    return ParseUserData(ref p);
                case 0x7B:      // '{' 哈希表
                    p++;
                    return ParseHash(ref p);
                default:
                    throw new RubyParseException((uint)p, "无法识别的类型");
            }
        }
        /// <summary>
        /// 解析哈希表
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static unsafe object ParseHash(ref byte* p)
        {
            // 首先解析键值对个数
            int count = ParseLongInteger(ref p);
            // 还无法知道哈希表的具体类型，因为键值可能为空
            Dictionary<object, object> hash = new Dictionary<object, object>();
            // 解析所有的键值
            for(int i = 0;i<count;i++)
            {
                var key = ParseValue(ref p);
                var value = ParseValue(ref p);
                hash.Add(key, value);
            }
            return hash;
        }
        /// <summary>
        /// 仍然是分析类对象，但是不同的是，该类实现了_dump函数
        /// 可以自己序列化成一个字符串
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static unsafe object ParseUserData(ref byte* p)
        {
            // Table
            // : 后跟，Table长度5+5=0x0A, "Table", Table数据长度
            // 测试了一个例子：Table[6, 100, 1], 数据长度1220
            // Table每个元素占2字节，则共需1200字节，多余的20字节应该记录了每个维度的长度
            // 前20字节如下：0200 0000 0600 0000 6400 0000 0100 0000 5802 0000
            // 对应的int值 2, 6, 100, 1, 600

            // 后面的数据确实与输出数据相同，但是却是按x排列，然后y排列的
            // 也就是第一个字对应table[0, 0], 第二个字对应[1, 0], 依次递推

            // 测试2：Table[6, 100, 1], 需1220字节， 20字后节仍然数据，前20字节
            // 0200 0000 0600 0000 6400 0000 0100 0000 5802 0000
            // 对应的int值 2, 6, 100, 1, 600

            // 测试3：Table[528, 1, 1], 需 1076字节，后20字节仍然数据，前20字节
            // 0100 0000 1002 0000 0100 0000 0100 0000 1002 0000
            // 似乎前20个字节和维度长度没关系啊
            // 大意了，按小端来看，有三个4字就是维度长度的值啊
            // 对应的int值 1, 528, 1, 1, 528

            // 中间三个int应该就是维度长度了，至于头尾的两个int，还不清楚



            // Color
            // Color 共有4个分量，每个分量是用Float存储的，一直以为是int，而Ruby的Float相当于C语言的double
            // 所以理论上是占 8 * 4 = 32个字节，正好与其_dump后的字符串长度 0x25 - 5 = 0x20 = 32相等
            // 下面只要考虑各个分量的次序就行了
            // 经测试，依次对应r, g, b, a

            string name;
            // ":"
            if(*p == 0x3A)
            {
                p++;
                name = ParseString(ref p);
                AddSymbol(name);
            }
            else if(*p == 0x3B)         // ';'
            {
                p++;
                var index = ParseLongInteger(ref p);        // 获取符号索引
                name = GetSymbol(index);
            }else
            {
                throw new RubyParseException((uint)p, "自定义_dump函数类期望 : ");
            }

            int length = ParseLongInteger(ref p);

            object obj = null;
            if(name == "Table")
                obj = Table.Load(p, length);
            else if (name == "Color")
                obj = Utility.ColorUtilities.Load(p, length);
            else
                 throw new RubyParseException((uint)p, "未定义的用户类 " + name);

            p += length;

            return obj;     
        }
        /// <summary>
        /// 解析数组
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static unsafe object ParseArray(ref byte* p)
        {
            // 首先是数组长度
            int length = ParseLongInteger(ref p);
            if (length == 0)
                return new List<object>();

            // 应该计算出List的元素类型
            object obj = null;
            int i = -1;
            for (; i < length && obj == null;i++)
            {
                obj = ParseValue(ref p);
            }

            var elementType = obj.GetType();
            var listType = typeof(List<>).MakeGenericType(new Type[] { elementType });
            var list = (System.Collections.IList)Activator.CreateInstance(listType);

            object defaultValue = DefaultForType(elementType);
            for (int j = 0; j < i;j++)
            {
                list.Add(defaultValue);
            }
            list.Add(obj);
            for (i = i + 1; i < length;i++)
            {
                obj = ParseValue(ref p);
                list.Add(obj);
            }
            return list;

                // 接下来依次解析每个元素
                //List<object> array = new List<object>(length);

                //for (int i = 0; i < length; i++)
                //{
                //    array.Add(ParseValue(ref p));
                //}
                //return array;
        }
        /// <summary>
        /// 获取类型的默认值
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }

        private static unsafe object ParseFixnum(ref byte* p)
        {
            // "i"
            if (*p++ != 0x69)
                throw new RubyParseException((uint)p, "不是Fixnum标志 ");

            return ParseLongInteger(ref p);
        }
        /// <summary>
        /// 解析长整数
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static unsafe int ParseLongInteger(ref byte* p)
        {
            sbyte v = *(sbyte*)p++;
            int value = 0;

            if (v <= -5)        // [-128, -5]       当前字节为直接数据 x+5
            {
                value = v + 5;
            }
            else if (v <= -1)    // [-4, -1]        接下来是有 |x| 个字节的 负整数
            {
                value = *p++;
                for (int i = 1; i < -v; i++)
                    value |= (*p++ << (i * 8));
                value = -value;
            }
            else if (v == 0)     // 0    当前字节为直接数据 0
            {
                value = 0;
            }
            else if (v <= 4)     // [1, 4]       接下来是有 |x| 个字节的 正整数
            {
                value = *p++;
                for (int i = 1; i < v; i++)
                    value |= (*p++ << (i * 8));
            }
            else            // [5, 127]     当前字节为直接数据 x-5
            {
                value = v - 5;
            }
            return value;
        }
        /// <summary>
        /// 检查指定类型中是否存在指定名称成员
        /// </summary>
        /// <param name="name"></param>
        /// <param name="classType"></param>
        private static void CheckMemberName(string name, Type classType)
        {

        }
        /// <summary>
        /// 解析字符串（名称)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public unsafe static string ParseString(ref byte* p, bool isMember = false)
        {
            int length = ParseLongInteger(ref p);      // 名称长度

            // 成员名称
            if (isMember)
            {
                if (*p++ != 0x40)
                    throw new RubyParseException((uint)p, "期望的是 @ ");

                length--;       // 去掉'@'
            }

            string str = BytePtrToString(p, length);
            p += length;
            return str;
        }
        /// <summary>
        /// 将字节指针转为字符串
        /// </summary>
        /// <param name="p">字节指针</param>
        /// <param name="length">字符串长度</param>
        /// <returns></returns>
        public unsafe static string BytePtrToString(byte* p, int length)
        {
            byte[] b = new byte[length];
            for (int i = 0; i < length; i++)
                b[i] = *(p + i);
            return System.Text.Encoding.UTF8.GetString(b);
        }
    }
}
