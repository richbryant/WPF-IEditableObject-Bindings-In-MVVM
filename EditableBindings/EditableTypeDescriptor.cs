using System;
using System.Collections.Generic;
using System.ComponentModel;
using EditableBindings.Metadata;

namespace EditableBindings
{
    public sealed class EditableTypeDescriptor : CustomTypeDescriptor
    {
        private readonly TypeMetaData _context;

        /// <summary>
        /// Initializes a new instance of the DesignTimeTypeDescriptor class.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        public EditableTypeDescriptor(Type objectType)
        {
            var wrapperObjectType = objectType;
            var wrappedObjectType = objectType;
            var baseType = wrapperObjectType.BaseType;
            while (baseType != null && baseType != typeof(object))
            {
                if (baseType.Name == typeof(EditableAdapter<>).Name 
                    && baseType.GetGenericArguments().Length > 0)
                {
                    wrappedObjectType = baseType.GetGenericArguments()[0];
                    break;
                }
                baseType = baseType.BaseType;
            }
            _context = TypeMetaDataRepository.GetFor(wrapperObjectType, wrappedObjectType);
        }

        /// <summary>
        /// Returns the properties for this instance of a component.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the properties for this component instance.
        /// </returns>
        public override PropertyDescriptorCollection GetProperties()
        {
            return new PropertyDescriptorCollection(_context.PropertyDescriptors.ToArray());
        }

        /// <inheritdoc />
        /// <summary>
        /// Returns the properties for this instance of a component using the attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that is used as a filter.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the filtered properties for this component instance.
        /// </returns>
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var descriptors = new List<PropertyDescriptor>();
            foreach (PropertyDescriptor descriptor in GetProperties())
            {
                foreach (var searchAttribute in attributes)
                {
                    if (!descriptor.Attributes.Contains(searchAttribute)) continue;
                    break;
                }
                descriptors.Add(descriptor);
            }
            return new PropertyDescriptorCollection(descriptors.ToArray());
        }
    }
}