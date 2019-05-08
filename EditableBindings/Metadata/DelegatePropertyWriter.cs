using System;

namespace EditableBindings.Metadata
{
    internal interface IDelegatePropertyWriter
    {
        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        void SetValue(object instance, object value);
    }

    internal sealed class DelegatePropertyWriter<TInstance, TProperty> : 
        IDelegatePropertyWriter
    {
        private Action<TInstance, TProperty> _setValueDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegatePropertyWriter"/> class.
        /// </summary>
        /// <param name="setValueDelegate">The set value delegate.</param>
        public DelegatePropertyWriter(Action<TInstance, TProperty> setValueDelegate)
        {
            _setValueDelegate = setValueDelegate;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        public void SetValue(object instance, object value)
        {
            _setValueDelegate((TInstance)instance, (TProperty)value);
        }
    }
}
