using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MwoCWDropDeckBuilder.Model;

namespace MwoCWDropDeckBuilder.ViewModel.Filters
{
    public class TechTypeFilterViewModel : BaseBuildFilterViewModel
    {
        public TechTypeFilterViewModel()
        {
            SelectedTechType = TechType.MixedTech;
        }

        private TechType _selectedTechType;
        public TechType SelectedTechType
        {
            get { return _selectedTechType; }
            set
            {
                _selectedTechType = value;
                OnPropertyChanged(() => this.SelectedTechType);
            }
        }

        private ObservableCollection<TechType> _techTypes;
        public ObservableCollection<TechType> TechTypes
        {
            get
            {
                if (_techTypes == null)
                    _techTypes = new ObservableCollection<TechType>(Enum.GetValues(typeof(TechType)).Cast<TechType>());
                return _techTypes;
            }
        }

        public override IEnumerable<SmurfyBuild> ApplyFilter(IEnumerable<SmurfyBuild> dropdecks)
        {
            IEnumerable<SmurfyBuild> returnValue = null;
            switch (SelectedTechType)
            {
                case TechType.Clan:
                    returnValue = dropdecks.Where(x => x.IsClan);
                    break;
                case TechType.IS:
                    returnValue = dropdecks.Where(x => !x.IsClan);
                    break;
                case TechType.MixedTech:
                    returnValue = dropdecks;
                    break;
            }
            return returnValue;
        }

        public override bool PassFilterConditions(SmurfyBuild item)
        {
            return (SelectedTechType == TechType.MixedTech) || ((SelectedTechType == TechType.Clan) ? item.IsClan : !item.IsClan);
        }
    }
}