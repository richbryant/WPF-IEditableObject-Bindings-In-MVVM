using Prism.Mvvm;

namespace DemoApp
{
    public class Contact : BindableBase
    {
        private string _name;
        private int _irrelevant;
        private bool _sensible;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int Irrelevant
        {
            get => _irrelevant;
            set => SetProperty(ref _irrelevant, value);
        }

        public bool Sensible
        {
            get => _sensible;
            set => SetProperty(ref _sensible, value);
        }
    }
}
