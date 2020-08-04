using System.ComponentModel;
using System.Runtime.CompilerServices;
using MVVM_Example.Model;

namespace MVVM_Example.ViewModel
{
    public class PhoneViewModel : INotifyPropertyChanged
    {
        private Phone _phone;

        public PhoneViewModel(Phone p)
        {
            _phone = p;
        }

        public string Title
        {
            get { return _phone.Title; }
            set
            {
                _phone.Title = value;
                OnPropertyChanged("Title");
            }
        }

        public string Company
        {
            get { return _phone.Company; }
            set
            {
                _phone.Company = value;
                OnPropertyChanged("Company");
            }
        }

        public int Price
        {
            get { return _phone.Price; }
            set
            {
                _phone.Price = value;
                OnPropertyChanged("Price");
            }
        }
        
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}