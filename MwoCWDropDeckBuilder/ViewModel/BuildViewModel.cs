using MwoCWDropDeckBuilder.Infrastructure;

namespace MwoCWDropDeckBuilder.ViewModel
{
    public class BuildViewModel : BaseViewModel<SmurfyBuild>
    {

        
        public bool IsSelected
        {
            get { return Model.IsSelected; }
            set
            {
                Model.IsSelected = value;
                OnPropertyChanged(() => this.IsSelected);
            }
        }

        public string Url { get { return Model.Url; } }

        public string MechName { get { return Model.MechName; } }

        public string WeaponSummary { get { return Model.WeaponSummary; } }

        public bool IsClan { get { return Model.IsClan; } }

        public string Type { get { return Model.Mech.Type; } }

        public bool IsECM { get { return Model.IsECM; } }

        public int MechTonnage { get { return Model.MechTonnage; } }

        public decimal MaxDps { get { return Model.MaxDps; } }

        public decimal SusDps { get { return Model.SusDps; } }

        public decimal Firepower { get { return Model.Firepower; } }

        public decimal HeatEfficiency { get { return Model.HeatEfficiency; } }

        public decimal EffectiveRange { get { return Model.EffectiveRange; } }

        public decimal TopSpeed { get { return Model.TopSpeed; } }

        public bool HasXL { get { return Model.HasXL; } }

        public BuildViewModel(SmurfyBuild build)
            : base(build)
        {
            IsSelected = true;
        }
         
    }
}