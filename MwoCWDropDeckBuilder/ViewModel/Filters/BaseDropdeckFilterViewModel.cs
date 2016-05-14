using System.Collections.Generic;
using System.Linq;
using MwoCWDropDeckBuilder.Model;

namespace MwoCWDropDeckBuilder.ViewModel.Filters
{
    public abstract class BaseDropdeckFilterViewModel : BaseFilterViewModel, IFilterViewModel<DropDeck>
    {
        public BaseDropdeckFilterViewModel() 
            : base(false)
        {
            
        }

        public virtual IEnumerable<DropDeck> ApplyFilter(IEnumerable<DropDeck> dropdecks)
        {
            return dropdecks.Where(PassFilterConditions);
        }

        public abstract bool PassFilterConditions(DropDeck item);
    }
}