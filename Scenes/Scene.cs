using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace XP
{
    public abstract class Scene : Canvas
    {
        /// <summary>
        /// 初始化游戏内容，不要把要显示到场景上的内容（比如精灵）
        /// 在构造函数内初始化，因为那样会把游戏内容添加到前一个场景中
        /// 然后在设置此场景时，那些内容将会被移除
        /// 不要在构造方法里注册事件处理
        /// </summary>
        public virtual void Initialize()
        {

        }
        /// <summary>
        /// 清理，如果注册了事件处理方法的话，应该清理掉
        /// </summary>
        public virtual void Uninitialize()
        {

        }
        public abstract void main();
        public abstract void update();
    }
}
