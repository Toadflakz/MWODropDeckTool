using System.ComponentModel.DataAnnotations;
using System.Linq;
using MwoCWDropDeckBuilder.Model;

namespace MwoCWDropDeckBuilder.ViewModel.Filters
{
    public class DuplicateChassisFilterViewModel : BaseDropdeckFilterViewModel
    {
        public DuplicateChassisFilterViewModel()
        {
            ChassisLimit = 1;
        }

        private int _chassisLimit;
        [Range(0,12)]
        public int ChassisLimit
        {
            get { return _chassisLimit; }
            set
            {
                _chassisLimit = value;
                OnPropertyChanged(() => this.ChassisLimit);
            }
        }

        public override bool PassFilterConditions(DropDeck item)
        {
            return item.Mechs
                .GroupBy(y => y.Mech.Chassis)
                .Select(g => new {Chassis = g.Key, Count = g.Count()})
                .Any(d => d.Count > ChassisLimit) == false;
        }
    }
    
}