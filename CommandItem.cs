using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class CommandItem : ContentControl
    {
        static CommandItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandItem),
                new FrameworkPropertyMetadata(typeof(CommandItem)));
        }

        public CommandItem()
        {
        }

        //public bool IsSelected
        //{
        //    get { return (bool)GetValue(IsSelectedProperty); }
        //    set { SetValue(IsSelectedProperty, value); }
        //}

        //public static readonly DependencyProperty IsSelectedProperty =
        //    DependencyProperty.Register("IsSelected", typeof(bool), typeof(CommandItem), new PropertyMetadata(false));


        /// <summary>
        ///     Indicates whether this ListBoxItem is selected.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
                Selector.IsSelectedProperty.AddOwner(typeof(CommandItem),
                        new FrameworkPropertyMetadata(false,
                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal));

        /// <summary>
        ///     Indicates whether this ListBoxItem is selected.
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        /// <summary>
        ///     Event indicating that the IsSelected property is now true.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnSelected(RoutedEventArgs e)
        {
            HandleIsSelectedChanged(true, e);
        }

        /// <summary>
        ///     Event indicating that the IsSelected property is now false.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnUnselected(RoutedEventArgs e)
        {
            HandleIsSelectedChanged(false, e);
        }

        private void HandleIsSelectedChanged(bool newValue, RoutedEventArgs e)
        {
            RaiseEvent(e);
        }
 
    }
}
