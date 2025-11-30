using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ContractRegistryApp.Models;
using ContractRegistryApp.Services;

namespace ContractRegistryApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private Contract _selectedContract;

        public ObservableCollection<Contract> Contracts { get; set; }

        public Contract SelectedContract
        {
            get { return _selectedContract; }
            set
            {
                _selectedContract = value;
                OnPropertyChanged();
                // Gombok frissítése
                (OpenFileCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (ModifyCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        // Parancsok
        public ICommand AddContractCommand { get; private set; }
        public ICommand OpenFileCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand ModifyCommand { get; private set; }

        public MainViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;

            _dbService = new DatabaseService();
            Contracts = new ObservableCollection<Contract>();
            LoadData();

            AddContractCommand = new RelayCommand(AddContract);
            OpenFileCommand = new RelayCommand(OpenFile, IsContractSelected);
            DeleteCommand = new RelayCommand(DeleteContract, IsContractSelected);
            ModifyCommand = new RelayCommand(ModifyContract, IsContractSelected);
        }

        private void LoadData()
        {
            Contracts.Clear();
            foreach (var item in _dbService.GetAllContracts()) Contracts.Add(item);
        }

        // Segédfüggvény: Van-e kiválasztva valami?
        private bool IsContractSelected(object obj) => SelectedContract != null;

        // --- HOZZÁADÁS ---
        private void AddContract(object obj)
        {
            var editorWindow = new ContractEditorWindow();
            if (editorWindow.ShowDialog() == true)
            {
                _dbService.AddContract(editorWindow.ResultContract);
                LoadData();
            }
        }

        // --- MÓDOSÍTÁS ---
        private void ModifyContract(object obj)
        {
            if (SelectedContract == null) return;

            // Átadjuk a kiválasztott elemet szerkesztésre
            var editorWindow = new ContractEditorWindow(SelectedContract);
            if (editorWindow.ShowDialog() == true)
            {
                _dbService.UpdateContract(editorWindow.ResultContract);
                LoadData();
            }
        }

        // --- TÖRLÉS ---
        private void DeleteContract(object obj)
        {
            if (SelectedContract == null) return;

            var res = MessageBox.Show($"Biztosan törölni szeretné: {SelectedContract.Name}?",
                                      "Törlés", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (res == MessageBoxResult.Yes)
            {
                _dbService.DeleteContract(SelectedContract.Id);
                LoadData();
                SelectedContract = null; // Törlés után nincs kijelölés
            }
        }

        // --- FÁJL MEGNYITÁSA ---
        private void OpenFile(object obj)
        {
            if (SelectedContract != null && !string.IsNullOrEmpty(SelectedContract.FilePath))
            {
                try { Process.Start(SelectedContract.FilePath); }
                catch (System.Exception ex) { MessageBox.Show("Hiba: " + ex.Message); }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}