using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MVVM_Example.Model;
using MVVM_Example.ViewModel.Commands;

namespace MVVM_Example.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {

        private PhoneViewModel selectedPhone;

        private int selectedPhoneIndex;

        private IFileService _fileService;
        private IDialogService _dialogService;
        
        public ObservableCollection<PhoneViewModel> Phones { get; set; }

        public PhoneViewModel SelectedPhone
        {
            get { return selectedPhone; }
            set
            {
                selectedPhone = value;
                OnPropertyChanged("SelectedPhone");
            }
        }

        public int SelectedPhoneIndex
        {
            get { return selectedPhoneIndex; }
            set
            {
                selectedPhoneIndex = value; 
            }
        }
        
        // Command to save file
        private RelayCommand saveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                       (saveCommand = new RelayCommand(obj =>
                       {
                           try
                           {
                               if (_dialogService.SaveFileDialog() == true)
                               {
                                   _fileService.Save(_dialogService.FilePath,
                                       Phones.Select(phone => new Phone
                                               {Company = phone.Company, Price = phone.Price, Title = phone.Title})
                                           .ToList());
                                   _dialogService.ShowMessage("File saved");
                               }
                           }
                           catch (Exception ex)
                           {
                               _dialogService.ShowMessage(ex.Message);
                           }
                       }));
            }
        }
        
        //Command to open file
        private RelayCommand openCommand;

        public RelayCommand OpenCommand
        {
            get
            {
                return openCommand ??
                       (openCommand = new RelayCommand(obj =>
                       {
                           try
                           {
                               if (_dialogService.OpenFileDialog() == true)
                               {
                                   var phones = _fileService.Open(_dialogService.FilePath);
                                   Phones.Clear();
                                   foreach (var phone in phones)
                                   {
                                       Phones.Add(new PhoneViewModel(phone));
                                   }

                                   _dialogService.ShowMessage("File opened");
                               }
                           }
                           catch (Exception ex)
                           {
                               _dialogService.ShowMessage(ex.Message);
                           }
                       }));
            }
        }

        // Command to add new object
        private RelayCommand addCommand;

        public RelayCommand AddCommand
        {
            get { return addCommand ??
                         (addCommand = new RelayCommand(obj =>
                         {
                            PhoneViewModel phone = new PhoneViewModel(new Phone());
                            Phones.Insert(0, phone);
                            SelectedPhone = phone;
                            
                         })); }
        }
        
        // Command to delete the object
        private RelayCommand removeCommand;

        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                       (removeCommand = new RelayCommand(obj =>
                           {
                               PhoneViewModel phone = obj as PhoneViewModel;
                               var selectedIndex = selectedPhoneIndex;
                               if (phone != null)
                               {
                                   Phones.Remove(phone);
                               }

                               UpdatePhonesListSelection(Phones.Count, selectedIndex);
                           },
                           (obj) => Phones.Count > 0)); //Button will be disabled in "canExecute = false"
            }
        }

        private void UpdatePhonesListSelection(int phonesCount, int selectedIndex)
        {
            if (phonesCount == 0)
            {
                SelectedPhone = null;
            }
            else if (phonesCount <= selectedIndex)
            {
                SelectedPhone = Phones[phonesCount - 1];
            }
            else
            {
                SelectedPhone = Phones[selectedIndex];
            }
        }

        private RelayCommand doubleCommand;
        public RelayCommand DoubleCommand
        {
            get
            {
                return doubleCommand ??
                       (doubleCommand = new RelayCommand(obj =>
                       {
                           PhoneViewModel phone = obj as PhoneViewModel;
                           if (phone != null)
                           {
                               Phone phoneCopy = new Phone
                               {
                                   Company = phone.Company,
                                   Price = phone.Price,
                                   Title = phone.Title

                               };
                               Phones.Insert(0, new PhoneViewModel(phoneCopy));
                           }
                       }));
            }
        }

        public ApplicationViewModel(IDialogService dialogService, IFileService fileService)
        {
            this._dialogService = dialogService;
            this._fileService = fileService;
            
            //Default data
            Phones = new ObservableCollection<PhoneViewModel>
            {
                new PhoneViewModel(new Phone {Title = "iPhone 10", Company = "Apple", Price = 100000}),
                new PhoneViewModel(new Phone {Title = "Galaxy S100500 Edge", Company = "Samsung", Price = 110000}),
                new PhoneViewModel(new Phone {Title = "Mi42S", Company = "Xiaomi", Price = 70000})
            };
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}