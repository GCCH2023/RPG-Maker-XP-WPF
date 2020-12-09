using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace XP.Base
{
    /// <summary>
    /// 所有要显示的游戏对象的基类
    /// 提供基本的游戏对象属性，比如位置和大小
    /// </summary>
    public class GameObject : FrameworkElement
    {
        /// <summary>
        /// 需要显示的视觉对象
        /// </summary>
        private VisualCollection visuals;
        /// <summary>
        /// 平移转换对象
        /// </summary>
        private TranslateTransform translateTransform = new TranslateTransform(0, 0);
        /// <summary>
        /// 缩放对象
        /// </summary>
        private RotateTransform rotateTransform = new RotateTransform(0, 0, 0);
        /// <summary>
        /// 缩放变换
        /// </summary>
        private ScaleTransform scaleTransform = new ScaleTransform(1, 1);

        public GameObject()
        {
            this.visuals = new VisualCollection(this);

            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(this.translateTransform);
            transformGroup.Children.Add(this.rotateTransform);
            transformGroup.Children.Add(scaleTransform);
            this.RenderTransform = transformGroup;
        }
        /// <summary>
        /// 添加一个视觉对象
        /// </summary>
        /// <param name="visual"></param>
        protected void AddVisual(Visual visual)
        {
            visuals.Add(visual);
        }

       /// <summary>
       /// 获取视觉对象的个数
       /// </summary>
        protected override int VisualChildrenCount
        {
            get { return visuals.Count; }
        }

       /// <summary>
       /// 获取视觉对象
       /// </summary>
       /// <param name="index"></param>
       /// <returns></returns>
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= visuals.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return visuals[index];

        }

        /// <summary>
        /// 游戏对象的x坐标
        /// </summary>
        public virtual double x
        {
            get
            {
                return (int)this.translateTransform.X;
            }
            set
            {
                this.translateTransform.X = value;
            }
        }
        /// <summary>
        /// 游戏对象的y坐标
        /// </summary>
        public virtual double y
        {
            get
            {
                return (int)this.translateTransform.Y;
            }
            set
            {
                this.translateTransform.Y = value;
            }
        }
        /// <summary>
        /// 游戏对象的z坐标，大的显示在上面
        /// </summary>
        public virtual int z
        {
            get
            {
                return Canvas.GetZIndex(this);
            }
            set
            {
                Canvas.SetZIndex(this, value);
            }
        }
        /// <summary>
        /// 游戏对象的X轴方向的缩放比例
        /// </summary>
        public double zoom_x
        {
            get
            {
                return (int)this.scaleTransform.ScaleX;
            }
            set
            {
                if (value < 0)
                    value = 0;
                this.scaleTransform.ScaleX = value;
            }
        }
        /// <summary>
        /// 游戏对象的Y轴方向的缩放比例
        /// </summary>
        public double zoom_y
        {
            get
            {
                return this.scaleTransform.ScaleY;
            }
            set
            {
                if (value < 0)
                    value = 0;
                this.scaleTransform.ScaleY = value;
            }
        }
        /// <summary>
        /// 游戏对象的旋转角度
        /// </summary>
        public double angle
        {
            get
            {
                return this.rotateTransform.Angle;
            }
            set
            {
                this.rotateTransform.Angle = value;
            }
        }
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool visible
        {
            get
            {
                return this.Visibility == Visibility.Visible;
            }
            set
            {
                this.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            }
        }
        /// <summary>
        /// 是否左右翻转
        /// 此属性和zoom_x冲突
        /// </summary>
        public bool mirror
        {
            get
            {
                // zoom_x 不能设置为小于0
                return this.scaleTransform.ScaleX == -1;
            }
            set
            {
                this.scaleTransform.ScaleX = value ? -1 : 1;
            }
        }
        /// <summary>
        /// 不透明度
        /// </summary>
        public double opacity
        {
            get
            {
                return (int)(this.Opacity * 255);
            }
            set
            {
                this.Opacity = value / 255.0;
            }
        }

        /// <summary>
        /// 游戏对象的宽度
        /// </summary>
        public virtual int width
        {
            get { return (int)this.Width; }
            set { this.Width = value; }
        }

        /// <summary>
        /// 游戏对象的高度
        /// </summary>
        public virtual int height
        {
            get { return (int)this.Height; }
            set { this.Height = value; }
        }

        /// <summary>
        /// 清除所有对象
        /// </summary>
        public void Clear()
        {
            this.visuals.Clear();
        }
    }
}
