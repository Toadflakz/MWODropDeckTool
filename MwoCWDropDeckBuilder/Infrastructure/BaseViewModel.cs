using System;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using MwoCWDropDeckBuilder.Infrastructure.Interfaces;
using MwoCWDropDeckBuilder.Model.Events;
using Prism.Events;

namespace MwoCWDropDeckBuilder.Infrastructure
{
    public class BaseViewModel<TModelType> : BaseViewModel, IBaseViewModel<TModelType>
        where TModelType : class
    {
        public TModelType Model { get; private set; }

        public BaseViewModel(TModelType modelObject)
        {
            Model = modelObject;
        }
    }

    public class BaseViewModel : Prism.Mvvm.BindableBase
    {
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private readonly IInteractionService _interactionService;

        public IUnityContainer Container
        {
            get { return _container; }
        }

        public IEventAggregator EventAggregator
        {
            get { return _eventAggregator; }
        }

        public IInteractionService InteractionService
        {
            get { return _interactionService; }
        }

        public BaseViewModel()
            : this(ServiceLocator.Current.GetInstance<IUnityContainer>(), 
            ServiceLocator.Current.GetInstance<IEventAggregator>(),
            ServiceLocator.Current.GetInstance<IInteractionService>())
        {
        }

        public BaseViewModel(IUnityContainer container, IEventAggregator eventAggregator, IInteractionService interactionService)
        {
            _container = container;
            _eventAggregator = eventAggregator;
            _interactionService = interactionService;
        }

        public void RaiseBusyMessage(bool isBusy, string busyMessage = null, TimeSpan countdownTimeSpan = new TimeSpan())
        {
            EventAggregator.GetEvent<BusyEvent>()
                .Publish(new BusyEventArgs()
                {
                    IsBusy = isBusy, 
                    BusyMessage = busyMessage,
                    Countdown = countdownTimeSpan
                });
        }

        public void NotifyAllProperties()
        {
            OnPropertyChanged(propertyName:string.Empty);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
