using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace EditableBindings.Metadata
{
    internal static class TypeMetaDataRepository
    {
        private static readonly Dictionary<Type, TypeMetaData> Contexts = new Dictionary<Type, TypeMetaData>();
        private static readonly object ContextsLock = new object();

        /// <summary>
        /// Creates the context.
        /// </summary>
        /// <returns></returns>
        public static TypeMetaData GetFor(Type wrapperObjectType, Type wrappedObjectType)
        {
            lock (ContextsLock)
            {
                if (Contexts.ContainsKey(wrapperObjectType)) return Contexts[wrapperObjectType];
                var result = new TypeMetaData();

                var propertyNames = new List<string>();
                foreach (var property in wrapperObjectType.GetProperties())
                {
                    if (property.GetCustomAttributes(true).Count(a => a is BrowsableAttribute attribute && attribute.Browsable == false) !=0) continue;
                    result.PropertyDescriptors.Add(CreateWrapperObjectPropertyWrapper(result, property));
                    result.AllKnownProperties.Add(property);
                    propertyNames.Add(property.Name);
                }

                foreach (var property in wrappedObjectType.GetProperties())
                {
                    if (propertyNames.Contains(property.Name)) continue;
                    result.PropertyDescriptors.Add(CreateWrappedObjectPropertyWrapper(result, property));
                    result.AllKnownProperties.Add(property);
                }
                Contexts.Add(wrapperObjectType, result);
                return Contexts[wrapperObjectType];
            }
        }

        /// <summary>
        /// Creates the wrapped object property wrapper.
        /// </summary>
        private static PropertyDescriptor CreateWrappedObjectPropertyWrapper(TypeMetaData context, PropertyInfo property)
        {
            GetPropertyReader(property, context);
            GetPropertyWriter(property, context);

            var reader = property.CanRead ? new Func<object, object>(o => ((IEditable)o).ReadProperty(property)) : null;
            var writer = property.CanWrite ? new Action<object, object>((o, v) => ((IEditable)o).WriteProperty(property, v)) : null;

            var descriptor = new DelegatePropertyDescriptor(
                property.Name,
                property.DeclaringType,
                property.PropertyType,
                reader,
                writer
                );
            return descriptor;
        }

        /// <summary>
        /// Creates the wrapper object property wrapper.
        /// </summary>
        private static PropertyDescriptor CreateWrapperObjectPropertyWrapper(TypeMetaData context, PropertyInfo property)
        {
            var actualReader = GetPropertyReader(property, context);
            var actualWriter = GetPropertyWriter(property, context);

            var reader = property.CanRead ? new Func<object, object>(o => actualReader.GetValue(o)) : null;
            var writer = property.CanWrite ? new Action<object, object>((o, v) => actualWriter.SetValue(o, v)) : null;

            var descriptor = new DelegatePropertyDescriptor(
                property.Name,
                property.DeclaringType,
                property.PropertyType,
                reader,
                writer
                );
            return descriptor;
        }

        /// <summary>
        /// Gets the property reader.
        /// </summary>
        private static IDelegatePropertyReader GetPropertyReader(PropertyInfo property, TypeMetaData context)
        {
            var actualReader = DelegatePropertyFactory.CreatePropertyReader(property);
            if (actualReader != null)
            {
                context.PropertyReaders.Add(property, actualReader);
            }
            return actualReader;
        }

        /// <summary>
        /// Gets the property writer.
        /// </summary>
        private static IDelegatePropertyWriter GetPropertyWriter(PropertyInfo property, TypeMetaData context)
        {
            var actualWriter = DelegatePropertyFactory.CreatePropertyWriter(property);
            if (actualWriter != null)
            {
                context.PropertyWriters.Add(property, actualWriter);
            }
            return actualWriter;
        }
    }
}
