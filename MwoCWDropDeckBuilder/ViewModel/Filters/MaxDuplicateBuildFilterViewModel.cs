using System.ComponentModel.DataAnnotations;
using System.Linq;
using MwoCWDropDeckBuilder.Model;

namespace MwoCWDropDeckBuilder.ViewModel.Filters
{
    public class MaxDuplicateBuildFilterViewModel : BaseDropdeckFilterViewModel
    {
        public MaxDuplicateBuildFilterViewModel()
        {
            
        }

        private int _limit;
        [Range(0, 12)]
        public int Limit
        {
            get { return _limit; }
            set
            {
                _limit = value;
                OnPropertyChanged(() => this.Limit);
            }
        }

        public override bool PassFilterConditions(DropDeck item)
        {
            return item.Mechs
                .GroupBy(y => y.MechName)
                .Select(g => new { Chassis = g.Key, Count = g.Count() })
                .Any(d => d.Count > Limit) == false;
        }
    }
}