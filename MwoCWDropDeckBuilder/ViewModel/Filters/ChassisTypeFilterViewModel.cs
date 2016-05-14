using System.Collections.Generic;
using System.Linq;
using MwoCWDropDeckBuilder.Model;
using Newtonsoft.Json.Serialization;

namespace MwoCWDropDeckBuilder.ViewModel.Filters
{
    public abstract class ChassisTypeFilterViewModel : BaseDropdeckFilterViewModel, IFilterViewModel<SmurfyBuild>
    {
        protected string _chassisType;

        private int _limit;
        public int Limit
        {
            get { return _limit; }
            set
            {
                _limit = value;
                OnPropertyChanged(() => this.Limit);
            }
        }

        public override IEnumerable<Model.DropDeck> ApplyFilter(IEnumerable<Model.DropDeck> dropdecks)
        {
            if (Limit > 0)
            {
                var filterResult = dropdecks.Where(PassFilterConditions);
                return filterResult;
            }
            else
            {
                return dropdecks;
            }
        }

        public IEnumerable<SmurfyBuild> ApplyFilter(IEnumerable<SmurfyBuild> builds)
        {
            if (Limit > 0)
            {
                return builds;
            }
            else
            {
                return builds.Where(x => x.Mech.Type != _chassisType);
            }
        }


        public bool PassFilterConditions(SmurfyBuild item)
        {
            throw new System.NotImplementedException();
        }

        public override bool PassFilterConditions(DropDeck item)
        {
            return item.Mechs
                .Count(x => x.Mech.Type == _chassisType) == Limit;
        }
    }

    public class LightChassisTypeFilterViewModel : ChassisTypeFilterViewModel
    {
        public LightChassisTypeFilterViewModel()
        {
            _chassisType = "Light";
        }
    }

    public class MediumChassisTypeFilterViewModel : ChassisTypeFilterViewModel
    {
        public MediumChassisTypeFilterViewModel()
        {
            _chassisType = "Medium";
        }
    }

    public class HeavyChassisTypeFilterViewModel : ChassisTypeFilterViewModel
    {
        public HeavyChassisTypeFilterViewModel()
        {
            _chassisType = "Heavy";
        }
    }

    public class AssaultChassisTypeFilterViewModel : ChassisTypeFilterViewModel
    {
        public AssaultChassisTypeFilterViewModel()
        {
            _chassisType = "Assault";
        }
    }
}