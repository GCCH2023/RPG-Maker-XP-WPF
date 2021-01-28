using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace XP
{
    /// <summary>
    /// 将所给区域划分为指定列
    /// </summary>
    public class GridPanel : Panel
    {


        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(int), typeof(GridPanel),
                new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(ColumnsChanged),
                    new CoerceValueCallback(CoerceColumns)));

        private static object CoerceColumns(DependencyObject d, object baseValue)
        {
            if ((int)baseValue < 0)
                return 1;
            return baseValue;
        }

        private static void ColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
        {
            if (this.InternalChildren.Count == 0)
                return new Size(0, 0);

            foreach (UIElement child in this.InternalChildren)
            {
                child.Measure(availableSize);
            }

           UIElement child1 = this.InternalChildren[0];
            // 每个元素的大小一样
            double width = double.IsInfinity(availableSize.Width) ? child1.DesiredSize.Width * this.Columns : availableSize.Width;
            double height = child1.DesiredSize.Height * this.InternalChildren.Count / this.Columns;

            return new Size(width, height);
        }

        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize)
        {
            if (this.InternalChildren.Count == 0)
                return new Size(0, 0);

            UIElement child1 = this.InternalChildren[0];
            int row = (int)Math.Ceiling(this.InternalChildren.Count / (double)this.Columns);
            // 尽可能给多少用多少
            double totalWidth = double.IsInfinity(finalSize.Width) ? child1.DesiredSize.Width * this.Columns : finalSize.Width;
            double totalHeight = double.IsInfinity(finalSize.Height) ?
                child1.DesiredSize.Height *  row : finalSize.Height;

            // 每个元素的大小一样
            double width = totalWidth / this.Columns;
            double height = totalHeight / row;

            int i = 0, j = 0;
            foreach (UIElement child in this.InternalChildren)
            {
                child.Arrange(new Rect(i * width, j * height, width, height));

                i++;
                if(i == this.Columns)
                {
                    i = 0;
                    j++;
                }
            }

            return new Size(totalWidth, totalHeight);
        }
    }
}
