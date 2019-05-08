using System.Reflection;

namespace EditableBindings
{
    internal interface IEditable
    {
        object WrappedInstance { get; }
        object ReadProperty(PropertyInfo property);
        void WriteProperty(PropertyInfo property, object value);
    }
}