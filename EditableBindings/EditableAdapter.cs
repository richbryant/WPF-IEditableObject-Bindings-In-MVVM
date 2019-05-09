using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using EditableBindings.Metadata;

namespace EditableBindings
{
    [TypeDescriptionProvider(typeof(EditableTypeDescriptionProvider))]
    public class EditableAdapter<TWrappedObject> :
        INotifyPropertyChanged,
        IEditableObject,
        IEditable
    {
        private readonly TWrappedObject _current;
        private readonly Dictionary<PropertyInfo, object> _changedProperties;
        private readonly TypeMetaData _metaData;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditableAdapter&lt;TWrappedObject&gt;"/> class.
        /// </summary>
        /// <param name="current">The object being wrapped.</param>
        public EditableAdapter(TWrappedObject current)
        {
            _changedProperties = new Dictionary<PropertyInfo, object>();
            _current = current;
            _metaData = TypeMetaDataRepository.GetFor(GetType(), typeof(TWrappedObject));
        }

        /// <summary>
        /// Gets the current item.
        /// </summary>
        public TWrappedObject WrappedInstance => _current;

        /// <summary>
        /// Gets the current item.
        /// </summary>
        object IEditable.WrappedInstance => _current;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a value indicating whether this instance has changes.
        /// </summary>
        public bool HasChanges => _changedProperties.Count > 0;

        /// <summary>
        /// Begins an edit on an object.
        /// </summary>
        public void BeginEdit()
        {
        }

        /// <summary>
        /// Discards changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
        /// </summary>
        public void CancelEdit()
        {
            _changedProperties.Clear();
            _metaData?.AllKnownProperties.ForEach(p => OnPropertyChanged(new PropertyChangedEventArgs(p.Name)));
            OnPropertyChanged(new PropertyChangedEventArgs("HasChanges"));
        }

        /// <summary>
        /// Pushes changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> or <see cref="M:System.ComponentModel.IBindingList.AddNew"/> call into the underlying object.
        /// </summary>
        public void EndEdit()
        {
            if (_metaData == null) return;
            foreach (var property in _changedProperties)
            {
                if (property.Key.CanWrite)
                {
                    _metaData.PropertyWriters[property.Key].SetValue(_current, property.Value);
                }
            }
            _changedProperties.Clear();
            _metaData.AllKnownProperties.ForEach(p => OnPropertyChanged(new PropertyChangedEventArgs(p.Name)));
            OnPropertyChanged(new PropertyChangedEventArgs("HasChanges"));
        }

        /// <summary>
        /// Reads the property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        object IEditable.ReadProperty(PropertyInfo property)
        {
            object result = null;
            if (property.CanRead)
            {
                result = _changedProperties.ContainsKey(property) ? _changedProperties[property] : _metaData.PropertyReaders[property].GetValue(_current);
            }
            return result;
        }

        /// <summary>
        /// Writes the property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        void IEditable.WriteProperty(PropertyInfo property, object value)
        {
            if (property.CanWrite)
            {
                if (property.GetValue(_current, null) == null || !property.GetValue(_current, null).Equals(value))
                {
                    if (!_changedProperties.ContainsKey(property))
                    {
                        _changedProperties.Add(property, value);
                    }
                    else
                    {
                        _changedProperties[property] = value;
                    }
                }
                else
                {
                    if (_changedProperties.ContainsKey(property))
                    {
                        _changedProperties.Remove(property);
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs(property.Name));
                OnPropertyChanged(new PropertyChangedEventArgs("HasChanges"));
            }
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
