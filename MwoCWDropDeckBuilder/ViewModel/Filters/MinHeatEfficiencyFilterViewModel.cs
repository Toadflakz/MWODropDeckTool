using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Serialization;

namespace MwoCWDropDeckBuilder.ViewModel.Filters
{
    public class MinHeatEfficiencyFilterViewModel : BaseBuildFilterViewModel
    {
        public MinHeatEfficiencyFilterViewModel()
        {
            
        }

        private int _limit;
        [Range(0,100)]
        public int Limit
        {
            get { return _limit; }
            set
            {
                _limit = value;
                OnPropertyChanged(() => this.Limit);
            }
        }

        public override bool PassFilterConditions(SmurfyBuild item)
        {
            return item.HeatEfficiency >= (Limit/100m);
        }
    }
}