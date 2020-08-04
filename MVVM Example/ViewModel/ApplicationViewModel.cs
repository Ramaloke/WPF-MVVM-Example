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

        private Phone selectedPhone;

        private int selectedPhoneIndex;

        private readonly IFileService _fileService;
        private readonly IDialogService _dialogService;
        
        public ObservableCollection<Phone> Phones { get; set; }

        public Phone SelectedPhone
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
        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ??
                       (_saveCommand = new RelayCommand(obj =>
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
        private RelayCommand _openCommand;

        public RelayCommand OpenCommand
        {
            get
            {
                return _openCommand ??
                       (_openCommand = new RelayCommand(obj =>
                       {
                           try
                           {
                               if (_dialogService.OpenFileDialog() == true)
                               {
                                   var phones = _fileService.Open(_dialogService.FilePath);
                                   Phones.Clear();
                                   foreach (var phone in phones)
                                   {
                                       Phones.Add(phone);
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
        private RelayCommand _addCommand;

        public RelayCommand AddCommand
        {
            get { return _addCommand ??
                         (_addCommand = new RelayCommand(obj =>
                         {
                            Phone phone = new Phone();
                            Phones.Insert(0, phone);
                            SelectedPhone = phone;
                            
                         })); }
        }
        
        // Command to delete the object
        private RelayCommand _removeCommand;

        public RelayCommand RemoveCommand
        {
            get
            {
                return _removeCommand ??
                       (_removeCommand = new RelayCommand(obj =>
                           {
                               Phone phone = obj as Phone;
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

        //Command to create Phone record copy 
        private RelayCommand _copyCommand;
        public RelayCommand CopyCommand
        {
            get
            {
                return _copyCommand ??
                       (_copyCommand = new RelayCommand(obj =>
                       {
                           Phone phone = obj as Phone;
                           if (phone != null)
                           {
                               Phone phoneCopy = new Phone
                               {
                                   Company = phone.Company,
                                   Price = phone.Price,
                                   Title = phone.Title

                               };
                               Phones.Insert(0, phoneCopy);
                           }
                       }));
            }
        }

        public ApplicationViewModel(IDialogService dialogService, IFileService fileService)
        {
            this._dialogService = dialogService;
            this._fileService = fileService;
            
            //Default data
            Phones = new ObservableCollection<Phone>
            {
                new Phone {Title = "iPhone 101", Company = "Apple", Price = 100000},
                new Phone {Title = "Galaxy S100500 Edge", Company = "Samsung", Price = 110000},
                new Phone {Title = "Mi42S", Company = "Xiaomi", Price = 70000}
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