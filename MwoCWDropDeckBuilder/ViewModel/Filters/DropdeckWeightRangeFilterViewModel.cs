using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MwoCWDropDeckBuilder.ViewModel.Filters
{
    public class DropdeckWeightRangeFilterViewModel : BaseDropdeckFilterViewModel
    {
        public DropdeckWeightRangeFilterViewModel()
        {
            LowerLimit = 250;
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
                OnPropertyChanged(() => this.LowerLimit);
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
                OnPropertyChanged(() => this.UpperLimit);
            }
        }

        public override bool PassFilterConditions(Model.DropDeck item)
        {
            return item.Tonnage >= LowerLimit && item.Tonnage <= UpperLimit;
        }
    }
}