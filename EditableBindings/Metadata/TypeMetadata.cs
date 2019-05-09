using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace EditableBindings.Metadata
{
    internal sealed class TypeMetaData
    {
        /// <summary>
        /// Initializes a new instance of the EditableWrapperMetaData class.
        /// </summary>
        public TypeMetaData()
        {
            AllKnownProperties = new List<PropertyInfo>();
            PropertyDescriptors = new List<PropertyDescriptor>();
            PropertyReaders = new Dictionary<PropertyInfo, IDelegatePropertyReader>();
            PropertyWriters = new Dictionary<PropertyInfo, IDelegatePropertyWriter>();
        }

        /// <summary>
        /// Gets all known properties.
        /// </summary>
        /// <value>All known properties.</value>
        public List<PropertyInfo> AllKnownProperties { get; }

        /// <summary>
        /// Gets the readers.
        /// </summary>
        /// <value>The readers.</value>
        public Dictionary<PropertyInfo, IDelegatePropertyReader> PropertyReaders { get; }

        /// <summary>
        /// Gets the writers.
        /// </summary>
        /// <value>The writers.</value>
        public Dictionary<PropertyInfo, IDelegatePropertyWriter> PropertyWriters { get; }

        /// <summary>
        /// Gets the property descriptors.
        /// </summary>
        /// <value>The property descriptors.</value>
        public List<PropertyDescriptor> PropertyDescriptors { get; }
    }
}