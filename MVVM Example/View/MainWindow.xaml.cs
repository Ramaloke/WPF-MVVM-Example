using MVVM_Example.ViewModel;

namespace MVVM_Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = new ApplicationViewModel(new DefaultDialogService(), new JsonFileService());
        }
    }
}