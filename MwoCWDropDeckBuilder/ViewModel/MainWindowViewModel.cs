using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MwoCWDropDeckBuilder.Infrastructure;
using MwoCWDropDeckBuilder.Infrastructure.Interfaces;
using Microsoft.Practices.Unity;
using MwoCWDropDeckBuilder.Model.Events;
using MwoCWDropDeckBuilder.Services.Interfaces;


namespace MwoCWDropDeckBuilder.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly ISmurfyDataLoaderService _smurfyDataLoaderService;
        private DispatcherTimer _countdownTimer;
        private TimeSpan _countdown;

        public ObservableCollection<ITabItemViewModel> Tabs { get; private set; }

        public ICommand LoadedCommand { get; private set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged(() => this.IsLoading);
            }
        }

        private string _busyMessage;
        public string BusyMessage
        {
            get { return _busyMessage; }
            set
            {
                _busyMessage = value;
                OnPropertyChanged(() => this.BusyMessage);
            }
        }

        public MainWindowViewModel(ISmurfyDataLoaderService smurfyDataLoaderService)
        {
            _smurfyDataLoaderService = smurfyDataLoaderService;
            Tabs = new ObservableCollection<ITabItemViewModel>
            {
                Container.Resolve<BuildListViewModel>(),
                Container.Resolve<DropDeckCreatorViewModel>(),
                Container.Resolve<AboutViewModel>()
            };
            EventAggregator.GetEvent<BusyEvent>().Subscribe(OnBusyEvent);
            LoadedCommand = new DelegateCommand(ExecuteLoadedCommand);

        }

        private void ExecuteLoadedCommand()
        {
            RaiseBusyMessage(true, "Loading...");

            var task = Task.Factory.StartNew(() =>
            {

                List<Task> tasks = new List<Task>();
                tasks.Add(_smurfyDataLoaderService.LoadMechsAsync());
                tasks.Add(_smurfyDataLoaderService.LoadWeaponsAsync());

                Task.WaitAll(tasks.ToArray());
            });
            task.ContinueWith(task1 =>
            {
                RaiseBusyMessage(false);
            });
        }

        private void OnBusyEvent(BusyEventArgs args)
        {
            Helper.InvokeForUI(() =>
            {
                IsLoading = args.IsBusy;
                if (!String.IsNullOrEmpty(args.BusyMessage))
                    BusyMessage = args.BusyMessage;

                if (args.Countdown.TotalMilliseconds > 0)
                {
                    _countdown = args.Countdown;
                    _countdownTimer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                    {
                        BusyMessage = String.Format("{0}\n({1} hr(s) {2} min(s) {3} sec(s) left...)", args.BusyMessage, _countdown.Hours, _countdown.Minutes, _countdown.Seconds);
                        if (_countdown == TimeSpan.Zero)
                        {
                            _countdownTimer.Stop();
                            _countdownTimer = null;
                        }

                        _countdown = _countdown.Add(TimeSpan.FromSeconds(-1));
                    }, Application.Current.Dispatcher);

                    _countdownTimer.Start();   
                }
                if (IsLoading == false)
                {
                    _countdownTimer.Stop();
                    _countdown = new TimeSpan();
                }

            });
        }

    }
}