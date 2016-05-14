using System.ComponentModel.DataAnnotations;

namespace MwoCWDropDeckBuilder.ViewModel.Filters
{
    public class BuildEffectiveRangeRangeFilterViewModel : BaseBuildFilterViewModel
    {
        public BuildEffectiveRangeRangeFilterViewModel()
        {
            LowerLimit = 0;
            UpperLimit = 260;
        }

        private int _lowerLimit;
        [Range(0,1200)]
        public int LowerLimit
        {
            get { return _lowerLimit; }
            set
            {
                _lowerLimit = value;
                OnPropertyChanged(() => LowerLimit);
            }
        }

        private int _upperLimit;
        [Range(0,1200)]
        public int UpperLimit
        {
            get { return _upperLimit; }
            set
            {
                _upperLimit = value;
                OnPropertyChanged(() => UpperLimit);
            }
        }

        public override bool PassFilterConditions(SmurfyBuild item)
        {
            return item.EffectiveRange >= LowerLimit && item.EffectiveRange <= UpperLimit;
        }

    }
}