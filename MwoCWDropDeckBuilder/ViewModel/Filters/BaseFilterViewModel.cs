using MwoCWDropDeckBuilder.Infrastructure;

namespace MwoCWDropDeckBuilder.ViewModel.Filters
{
    public abstract class BaseFilterViewModel : ValidatingBaseViewModel, IFilterViewModel
    {
        public bool IsPreFilter { get; private set; }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged(() => this.IsEnabled);
            }
        }

        protected BaseFilterViewModel(bool isPrefilter)
        {
            IsPreFilter = isPrefilter;
        }
    }
}