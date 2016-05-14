using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MwoCWDropDeckBuilder.Infrastructure;
using MwoCWDropDeckBuilder.Infrastructure.Interfaces;
using MwoCWDropDeckBuilder.Model;
using MwoCWDropDeckBuilder.Services.Interfaces;


namespace MwoCWDropDeckBuilder.ViewModel
{
    public class BuildListViewModel : ValidatingBaseViewModel, ITabItemViewModel
    {
        private readonly ISmurfyDataLoaderService _smurfyDataLoaderService;
        public string Header { get { return "Build Data"; } }

        public ObservableCollection<BuildViewModel> Builds { get; private set; }

        private LevelType  _selectedLevelType;
        public LevelType SelectedLevelType
        {
            get { return _selectedLevelType; }
            set
            {
                _selectedLevelType = value;
                OnPropertyChanged(() => SelectedLevelType);
                
                UpdateMaps();
            }
        }

        private void UpdateMaps()
        {
            _maps = null;
            OnPropertyChanged(() => Maps);
            SelectedMap = Maps.First();
        }

        private ObservableCollection<LevelType> _mapTypes;
        public ObservableCollection<LevelType> MapTypes
        {
            get
            {
                return _mapTypes ??
                       new ObservableCollection<LevelType>(Enum.GetValues(typeof (LevelType)).Cast<LevelType>());
            }
        }

        private IEnumerable<MwoLevel> _allMaps = MwoLevel.GetMaps();

        private ObservableCollection<MwoLevel> _maps;
        public ObservableCollection<MwoLevel> Maps
        {
            get { return _maps ?? new ObservableCollection<MwoLevel>(_allMaps.Where(x => x.Type == SelectedLevelType)); }
        }

        private MwoLevel _selectedMap;
        public MwoLevel SelectedMap
        {
            get { return _selectedMap; }
            set
            {
                _selectedMap = value;
                OnPropertyChanged(() => this.SelectedMap);

                UpdateBuildsCalculations();
            }
        }

        private bool _useFileAsSource;
        public bool UseFileAsSource
        {
            get { return _useFileAsSource; }
            set
            {
                _useFileAsSource = value;
                OnPropertyChanged(() => this.UseFileAsSource);
            }
        }

        private string _importFilePath;
        [Required(ErrorMessage = "You must provide a path to a text file.")]
        public string ImportFilePath
        {
            get { return _importFilePath; }
            set
            {
                _importFilePath = value;
                OnPropertyChanged(() => this.ImportFilePath);
            }
        }

        private bool _useSmurfyMechBayAsSource;
        public bool UseSmurfyMechBayAsSource
        {
            get { return _useSmurfyMechBayAsSource; }
            set
            {
                _useSmurfyMechBayAsSource = value;
                OnPropertyChanged(() => this.UseSmurfyMechBayAsSource);
            }
        }

        private string _smurfyApiKey;
        [Required(ErrorMessage = "You must provide your Smurfy API Key to load from Smurfy. The API key is found on your account page.")]
        public string SmurfyApiKey
        {
            get { return _smurfyApiKey; }
            set
            {
                _smurfyApiKey = value;
                OnPropertyChanged(() => this.SmurfyApiKey);
            }
        }
        
        public ICommand LoadBuildsCommand { get; private set; }
        public ICommand LoadedCommand { get; private set; }
        public ICommand BrowseImportFileCommand { get; private set; }

        public BuildListViewModel(ISmurfyDataLoaderService smurfyDataLoaderService)
        {
            _smurfyDataLoaderService = smurfyDataLoaderService;
            LoadBuildsCommand = new DelegateCommand(ExecuteLoadBuildsCommand, CanExecuteLoadBuildsCommand);
            BrowseImportFileCommand = new DelegateCommand(ExecuteBrowseImportFileCommand);
            Builds = new ObservableCollection<BuildViewModel>();
            SelectedLevelType = LevelType.QuickPlay;
        }

        private void UpdateBuildsCalculations()
        {
            Builds.ToList().ForEach(x =>
            {
                x.Model.PerformCalculations(SelectedMap);
                x.NotifyAllProperties();
            });
            OnPropertyChanged(() => Builds);
        }

        private bool CanExecuteLoadBuildsCommand()
        {
            bool fileSourceValid = (!UseFileAsSource) || IsValid(() => ImportFilePath);
            
            bool smurfyMechBayValid = (!UseSmurfyMechBayAsSource) || IsValid(() => SmurfyApiKey);
            
            return fileSourceValid && smurfyMechBayValid;
        }

        private void ExecuteLoadBuildsCommand()
        {
            Helper.InvokeForUI(() =>
            {
                RaiseBusyMessage(true, "Loading builds from text file and Smurfy...");
            });

            var tasks = new List<Task>();
            if (UseFileAsSource)
                tasks.Add(_smurfyDataLoaderService.LoadBuildsFromFileSourceAsync(ImportFilePath));
            if (UseSmurfyMechBayAsSource)
                tasks.Add(_smurfyDataLoaderService.LoadBuildsFromSmurfyMechBayAsync(SmurfyApiKey));

            Task.WhenAll(tasks).ContinueWith((taskResult) =>
            {
                var builds = _smurfyDataLoaderService.GetBuilds();

                Helper.InvokeForUI(() =>
                {
                    Builds = new ObservableCollection<BuildViewModel>(builds.Select(x => new BuildViewModel(x)));
                    OnPropertyChanged(() => Builds);

                    RaiseBusyMessage(false);
                });
            });

        }

        private void ExecuteBrowseImportFileCommand()
        {
            var path = InteractionService.GetOpenFileFromUser("Text file(.txt)|*.txt");
            if (!String.IsNullOrEmpty(path))
                ImportFilePath = path;
        }
       
    }
}