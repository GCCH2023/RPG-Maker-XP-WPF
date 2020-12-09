using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XP.Utility
{
    public class Util
    {
        public static T[] Clone<T>(T[] array)
        {
            //var newArray = new T[array.Count];
            //Array.Copy(array, newArray, array.Count);
            //return newArray;
            return null;
        }
        #region 将一个对象转换为指定类型
        /// <summary>
        /// 将一个对象转换为指定类型
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <param name="type">目标类型</param>
        /// <returns>转换后的对象</returns>
        public static object ConvertToObject(object obj, Type type)
        {
            if (type == null) return obj;

            if (obj.GetType() == type)
                return obj;

            if (obj == null) return type.IsValueType ? Activator.CreateInstance(type) : null;

            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (type.IsAssignableFrom(obj.GetType())) // 如果待转换对象的类型与目标类型兼容，则无需转换
            {
                return obj;
            }
            else if ((underlyingType ?? type).IsEnum) // 如果待转换的对象的基类型为枚举
            {
                if (underlyingType != null && string.IsNullOrEmpty(obj.ToString())) // 如果目标类型为可空枚举，并且待转换对象为null 则直接返回null值
                {
                    return null;
                }
                else
                {
                    return Enum.Parse(underlyingType ?? type, obj.ToString());
                }
            }
            else if (typeof(IConvertible).IsAssignableFrom(underlyingType ?? type)) // 如果目标类型的基类型实现了IConvertible，则直接转换
            {
                try
                {
                    return Convert.ChangeType(obj, underlyingType ?? type, null);
                }
                catch
                {
                    return underlyingType == null ? Activator.CreateInstance(type) : null;
                }
            }
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                if (converter.CanConvertFrom(obj.GetType()))
                {
                    return converter.ConvertFrom(obj);
                }
                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    object o = constructor.Invoke(null);
                    PropertyInfo[] propertys = type.GetProperties();
                    Type oldType = obj.GetType();
                    foreach (PropertyInfo property in propertys)
                    {
                        PropertyInfo p = oldType.GetProperty(property.Name);
                        if (property.CanWrite && p != null && p.CanRead)
                        {
                            property.SetValue(o, ConvertToObject(p.GetValue(obj, null), property.PropertyType), null);
                        }
                    }
                    return o;
                }
            }
            return obj;
        }
        #endregion

        /// <summary>
        /// 获取完整的文件名，也就是说输入的参数是不包括后缀名的文件名
        /// 如果找不到则返回null
        /// 如果找到多个，比如a.jpg, a.png，则返回第一个，至于第一个是什么类型，则是不确定的
        /// </summary>
        /// <param name="filename"></param>
        public static string GetFullFileName(string filename)
        {
            var path = System.IO.Path.GetDirectoryName(filename);
            var name = System.IO.Path.GetFileName(filename) + ".*";

            var files = System.IO.Directory.GetFiles(path, name);

            if (files.Length == 0)
                return null;

            return files[0];
        }

        /// <summary>
        /// 清除一个对象的某个事件所挂钩的delegate
        /// </summary>
        /// <param name="ctrl">控件对象</param>
        /// <param name="eventName">事件名称，默认的</param>
        public static void ClearEvents(object ctrl, string eventName = "_EventAll")
        {
            if (ctrl == null) return;
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Static;
            EventInfo[] events = ctrl.GetType().GetEvents(bindingFlags);
            if (events == null || events.Length < 1) return;

            for (int i = 0; i < events.Length; i++)
            {
                try
                {
                    EventInfo ei = events[i];
                    //只删除指定的方法，默认是_EventAll，前面加_是为了和系统的区分，防以后雷同
                    if (eventName != "_EventAll" && ei.Name != eventName) continue;

                    /********************************************************
                     * class的每个event都对应了一个同名(变了，前面加了Event前缀)的private的delegate类
                     * 型成员变量（这点可以用Reflector证实）。因为private成
                     * 员变量无法在基类中进行修改，所以为了能够拿到base 
                     * class中声明的事件，要从EventInfo的DeclaringType来获取
                     * event对应的成员变量的FieldInfo并进行修改
                     ********************************************************/
                    FieldInfo fi = ei.DeclaringType.GetField("Event_" + ei.Name, bindingFlags);
                    if (fi != null)
                    {
                        // 将event对应的字段设置成null即可清除所有挂钩在该event上的delegate
                        fi.SetValue(ctrl, null);
                    }
                }
                catch { }
            }
        }
    }
}
