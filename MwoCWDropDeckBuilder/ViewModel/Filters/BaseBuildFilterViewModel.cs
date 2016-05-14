using System.Collections.Generic;
using System.Linq;
using MwoCWDropDeckBuilder.Infrastructure;

namespace MwoCWDropDeckBuilder.ViewModel.Filters
{
    public abstract class BaseBuildFilterViewModel : BaseFilterViewModel, IFilterViewModel<SmurfyBuild>
    {
        public BaseBuildFilterViewModel() 
            : base(true)
        {
            
        }

        public virtual IEnumerable<SmurfyBuild> ApplyFilter(IEnumerable<SmurfyBuild> dropdecks)
        {
            return dropdecks.Where(PassFilterConditions);
        }

        public abstract bool PassFilterConditions(SmurfyBuild item);
    }
}