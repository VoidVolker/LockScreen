using System;
using System.Windows;

namespace LockScreen.DataTypes.Events
{
    public class DependencyPropertyChangedEventArgs<T>(DependencyPropertyChangedEventArgs e) : EventArgs
    {
        public T NewValue { get; private set; } = (T)e.NewValue;
        public T OldValue { get; private set; } = (T)e.OldValue;
        public DependencyProperty Property { get; private set; } = e.Property;
    }
}
