using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XP
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(CommandItem))]
    public class WindowCommand : Selector
    {
        static WindowCommand()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCommand),
                new FrameworkPropertyMetadata(typeof(WindowCommand)));
        }

        public WindowCommand()
        {
            this.Loaded += WindowCommandLoaded;
            this.PreviewKeyDown += WindowCommandKeyDown;
        }

        private void WindowCommandKeyDown(object sender, KeyEventArgs e)
        {
            var count = this.ItemContainerGenerator.Items.Count;
            var index = this.SelectedIndex;
            switch(e.Key)
            {
                case Key.Down:
                    this.SelectedIndex = (this.SelectedIndex + this.Columns) % count;
                    break;
                case Key.Up:
                    this.SelectedIndex = (this.SelectedIndex + count - this.Columns) % count;
                    break;
                case Key.Left:
                    this.SelectedIndex = (this.SelectedIndex + count - 1) % count;
                    break;
                case Key.Right:
                    this.SelectedIndex = (this.SelectedIndex + 1) % count;
                    break;
                case Key.Return:
                     var args = new RoutedEventArgs(WindowCommand.SelectEvent, this.SelectedIndex);
                     RaiseEvent(args);
                     return;
            }
            var args1 = new RoutedPropertyChangedEventArgs<int>(index, this.SelectedIndex);
            args1.RoutedEvent = WindowCommand.SelectIndexChangedEvent;
            RaiseEvent(args1);
        }

        // 当前项改变时触发，也就是鼠标移动到项上或按下方向键，初始化时不触发
        public static readonly RoutedEvent SelectIndexChangedEvent =
             EventManager.RegisterRoutedEvent(
            "SelectIndexChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(WindowCommand));

        public event RoutedPropertyChangedEventHandler<int> SelectIndexChanged
        {
            add { AddHandler(SelectIndexChangedEvent, value); }
            remove { RemoveHandler(SelectIndexChangedEvent, value); }
        }

        // 选择某项时触发的事件，也就是按下回车键或者鼠标单击触发
        public static readonly RoutedEvent SelectEvent = EventManager.RegisterRoutedEvent(
            "Select", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WindowCommand));

        public event RoutedEventHandler Select
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }

        private void WindowCommandLoaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            if(newValue != null && newValue.GetEnumerator().MoveNext())
            {
                // 数据源有元素的话，选中第一个
                this.SelectedIndex = 0;
            }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is CommandItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var item = new CommandItem();
            item.MouseEnter += CommandItem_MouseEnter;
            item.MouseLeave += CommandItem_MouseLeave;
            item.MouseLeftButtonUp += CommandItem_Click;
            return item;
        }

        private void CommandItem_Click(object sender, MouseButtonEventArgs e)
        {
            var args = new RoutedEventArgs(WindowCommand.SelectEvent, this.SelectedIndex);
            RaiseEvent(args);
        }

        private void CommandItem_MouseLeave(object sender, MouseEventArgs e)
        {
            var item = sender as CommandItem;
            var index = this.ItemContainerGenerator.IndexFromContainer(item);
            this.SelectedIndex = index;
        }

        private void CommandItem_MouseEnter(object sender, MouseEventArgs e)
        {
            var item = sender as CommandItem;
            var index0 = this.SelectedIndex;
            var index = this.ItemContainerGenerator.IndexFromContainer(item);
            this.SelectedIndex = index;

            var args1 = new RoutedPropertyChangedEventArgs<int>(index0, index);
            args1.RoutedEvent = WindowCommand.SelectIndexChangedEvent;
            RaiseEvent(args1);
        }

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public static readonly DependencyProperty ColumnsProperty =
            GridPanel.ColumnsProperty.AddOwner(typeof(WindowCommand));
    }
}
