using Prism.Commands;
using Prism.Mvvm;
using System.Diagnostics;
using EditableBindings;

namespace DemoApp.ViewModels
{
    public class ContentPageViewModel : BindableBase
    {
        public ContentPageViewModel()
        {
            Contact = new EditableAdapter<Contact>(new Contact{Name = "Rich", Irrelevant = 49, Sensible = false});
            OkCommand = new DelegateCommand(EndEdit);
            CancelCommand = new DelegateCommand(()=>Contact.CancelEdit());
        }

        public EditableAdapter<Contact> Contact { get; }
        public DelegateCommand OkCommand { get; }

        public DelegateCommand CancelCommand { get; }

        public void EndEdit()
        {
            Debug.WriteLine(Contact.WrappedInstance.Name);
            Debug.WriteLine(Contact.WrappedInstance.Irrelevant);
            Debug.WriteLine(Contact.WrappedInstance.Sensible);
            Contact.EndEdit();
            Debug.WriteLine(Contact.WrappedInstance.Name);
            Debug.WriteLine(Contact.WrappedInstance.Irrelevant);
            Debug.WriteLine(Contact.WrappedInstance.Sensible);
        }
    }
}
