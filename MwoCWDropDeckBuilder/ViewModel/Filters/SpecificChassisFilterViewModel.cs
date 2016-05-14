using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using MwoCWDropDeckBuilder.Model;
using MwoCWDropDeckBuilder.Services.Interfaces;

namespace MwoCWDropDeckBuilder.ViewModel.Filters
{
    public class SpecificChassisFilterViewModel : BaseDropdeckFilterViewModel
    {
        public SpecificChassisFilterViewModel()
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

        private string _selectedChassis;
        [Required]
        public string SelectedChassis
        {
            get { return _selectedChassis; }
            set
            {
                _selectedChassis = value;
                OnPropertyChanged(() => this.SelectedChassis);
            }
        }

        private ObservableCollection<string> _allChassis;
        public ObservableCollection<string> AllChassis
        {
            get
            {
                if (_allChassis == null)
                {
                    _allChassis = new ObservableCollection<string>(ServiceLocator.Current.GetInstance<ISmurfyDataLoaderService>().GetChassisNames());
                }
                return _allChassis;
            }
        }


        public override bool PassFilterConditions(DropDeck item)
        {
            return item.Mechs.Count(y => y.Mech.Chassis == SelectedChassis) == Limit;
        }
    }
}