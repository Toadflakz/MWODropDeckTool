using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MwoCWDropDeckBuilder.Model;

namespace MwoCWDropDeckBuilder.ViewModel.Filters
{
    public class ClanChassisFilterViewModel : BaseDropdeckFilterViewModel
    {
        public ClanChassisFilterViewModel()
        {
            Limit = 1;
        }

        private int _limit;
        [Range(0,12)]
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
                .Count(y => y.IsClan) <= Limit;
        }
    }
}