using System;

namespace EditableBindings.Metadata
{

    internal interface IDelegatePropertyReader
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        object GetValue(object instance);
    }

    internal class DelegatePropertyReader<TInstance, TProperty> : 
        IDelegatePropertyReader
    {
        private Func<TInstance, TProperty> _getValueDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegatePropertyReader"/> class.
        /// </summary>
        /// <param name="getValueDelegate"></param>
        public DelegatePropertyReader(Func<TInstance, TProperty> getValueDelegate)
        {
            _getValueDelegate = getValueDelegate;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public object GetValue(object instance)
        {
            return _getValueDelegate((TInstance)instance);
        }
    }
}
