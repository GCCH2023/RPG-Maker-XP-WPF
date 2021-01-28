using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace XP.Base
{
    /// <summary>
    /// 可以传入视口构造的对象
    /// </summary>
    public class ViewportObject : GameObject
    {
        public ViewportObject(Viewport viewport = null)
        {
            this.viewport = viewport;
        }

        private void UpdateViewort()
        {
            if (this._viewport == null || this.is_disposed)
                return;

            this.OpacityMask = this._viewport;
            // 可能要更新z值
            this.z = this.z;
        }

        Viewport _viewport = null;
        //viewport 
        //取得生成时指定的视口（Viewport）。
        public Viewport viewport
        {
            get
            {
                return this._viewport;
            }
            set
            {
                if (this._viewport != value)
                {
                    this._viewport = value;
                    this.UpdateViewort();
                }
            }
        }

        bool _is_dispose = false;
        // 方法dispose 
        // 释放精灵。如果已经释放的话则什么也不做。
        public virtual void dispose()
        {
            _is_dispose = true;
        }

        //disposed? 
        //精灵已经释放的话则返回真。
        public bool is_disposed
        {
            get
            {
                return _is_dispose;
            }
        }

        /// <summary>
        /// 游戏对象的z坐标，大的显示在上面
        /// </summary>
        int _z = 0;
        public override int z
        {
            get
            {
                return _z;
            }
            set
            {
                this._z = value;

                if (this.viewport != null && value < this.viewport.z)
                    value = this.viewport.z;

                Canvas.SetZIndex(this, value);
            }
        }
    }
}
