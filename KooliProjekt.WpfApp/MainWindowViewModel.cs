using System.Collections.ObjectModel;
using System.Windows.Input;
using KooliProjekt.WpfApp.Api;

namespace KooliProjekt.WpfApp
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        public ObservableCollection<Client> Lists { get; private set; }
        public ICommand NewCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public Predicate<Client> ConfirmDelete { get; set; }

        private readonly IApiClient _apiClient;

        public MainWindowViewModel() : this(new ApiClient())
        {
        }

        public MainWindowViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient;

            Lists = new ObservableCollection<Client>();

            NewCommand = new RelayCommand<Client>(
                // Execute
                list =>
                {
                    SelectedItem = new Client();
                }
            );

            SaveCommand = new RelayCommand<Client>(
            async list =>
            {
              ErrorMessage = null;
              var result = await _apiClient.Save(SelectedItem);
              if (result.HasError)
             {
              ErrorMessage = "Viga salvestamisel: " + result.Error;
              return;
             }
             await Load();
             },
             list => SelectedItem != null
             );


            DeleteCommand = new RelayCommand<Client>(
    async list =>
    {
        ErrorMessage = null;

        if (ConfirmDelete != null && !ConfirmDelete(SelectedItem))
            return;

        var result = await _apiClient.Delete(SelectedItem.Id);
        if (result.HasError)
        {
            ErrorMessage = "Viga kustutamisel: " + result.Error;
            return;
        }

        Lists.Remove(SelectedItem);
        SelectedItem = null;
    },
    list => SelectedItem != null
);


        public async Task Load()
        {
            Lists.Clear();
            ErrorMessage = null;

            var result = await _apiClient.List();
            if (result.HasError)
            {
                ErrorMessage = "Viga laadimisel: " + result.Error;
                return;
            }

            foreach (var list in result.Value)
            {
                Lists.Add(list);
            }
        }


        private Client _selectedItem;
        public Client SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                NotifyPropertyChanged();
            }
        }

    }

}
