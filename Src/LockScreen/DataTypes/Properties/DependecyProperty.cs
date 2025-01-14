using System;
using System.Linq.Expressions;
using System.Windows;

using LockScreen.DataTypes.Events;
using LockScreen.Extension.Properties;

namespace LockScreen.DataTypes.Properties
{
    //https://habr.com/ru/articles/149835/
    // I renamed it to DP and R to make less visual noise

    public delegate void PropertyChangedCallback<TProperty>(DependencyPropertyChangedEventArgs<TProperty> e);

    /// <summary>
    /// Generic Dependency Property
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class DP<T> where T : DependencyObject
    {
        /// <summary>
        /// Register property
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public static DependencyProperty R<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            return R(propertyExpression, default, null);
        }

        /// <summary>
        /// Register property
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DependencyProperty R<TProperty>(Expression<Func<T, TProperty>> propertyExpression, TProperty defaultValue)
        {
            return R(propertyExpression, defaultValue, null);
        }

        /// <summary>
        /// Register property
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="propertyChangedCallbackFunc"></param>
        /// <returns></returns>
        public static DependencyProperty R<TProperty>(Expression<Func<T, TProperty>> propertyExpression, Func<T, PropertyChangedCallback<TProperty>> propertyChangedCallbackFunc)
        {
            return R(propertyExpression, default, propertyChangedCallbackFunc);
        }

        /// <summary>
        /// Register property
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="defaultValue"></param>
        /// <param name="propertyChangedCallbackFunc"></param>
        /// <returns></returns>
        public static DependencyProperty R<TProperty>(Expression<Func<T, TProperty>> propertyExpression, TProperty defaultValue, Func<T, PropertyChangedCallback<TProperty>> propertyChangedCallbackFunc)
        {
            string propertyName = propertyExpression.RetrieveMemberName();
            PropertyChangedCallback callback = ConvertCallback(propertyChangedCallbackFunc);

            return DependencyProperty.Register(
                propertyName,
                typeof(TProperty),
                typeof(T),
                new PropertyMetadata(defaultValue, callback));
        }

        private static PropertyChangedCallback ConvertCallback<TProperty>(Func<T, PropertyChangedCallback<TProperty>> propertyChangedCallbackFunc)
        {
            return propertyChangedCallbackFunc == null
                ? null
                : new PropertyChangedCallback((d, e) =>
                    {
                        PropertyChangedCallback<TProperty> callback = propertyChangedCallbackFunc((T)d);
                        callback?.Invoke(new DependencyPropertyChangedEventArgs<TProperty>(e));
                    });
        }
    }
}
