using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ExtendedGrid.ExtendedGridControl;
using Facet.Combinatorics;
using log4net;
using Microsoft.Practices.ObjectBuilder2;
using MwoCWDropDeckBuilder.Infrastructure;
using MwoCWDropDeckBuilder.Infrastructure.Interfaces;
using MwoCWDropDeckBuilder.Model;
using MwoCWDropDeckBuilder.Services.Interfaces;
using Helper = MwoCWDropDeckBuilder.Infrastructure.Helper;

namespace MwoCWDropDeckBuilder.ViewModel
{
    public class DropDeckCreatorViewModel : BaseViewModel, ITabItemViewModel
    {
        private static ILog _log = LogManager.GetLogger(typeof (DropDeckCreatorViewModel));

        public enum DropDeckTypeEnum
        {
            [Description("Community Warfare/4-man Competition")]
            CommunityWarfare,
            [Description("12-man Competition")]
            Competition12Man,
            [Description("8-man Competition")]
            Competition8Man
        }


        private readonly ISmurfyDataLoaderService _smurfyDataLoaderService;
        public string Header { get { return "Dropdeck Combination Creator"; } }





        private DropDeckTypeEnum _dropDeckType;
        public DropDeckTypeEnum DropDeckType
        {
            get { return _dropDeckType; }
            set
            {
                _dropDeckType = value;
                OnPropertyChanged(() => this.DropDeckType);
            }
        }

        public List<DropDeckTypeEnum> DropDeckTypes
        {
            get
            {
                return
                    Enum.GetValues(typeof(DropDeckTypeEnum))
                        .Cast<DropDeckTypeEnum>()
                        //.Except(new List<DropDeckTypeEnum>() { DropDeckTypeEnum.Competition12Man })
                        .ToList();
            }
        }


        public ObservableCollection<IFilterViewModel> Filters { get; private set; }

        public ObservableCollection<DropDeck> DropDecks { get; private set; }

        public ICommand CreateDropDecksFromBuildsCommand { get; private set; }
        public ICommand ExportToExcelCommand { get; private set; }
        public ICommand ExportToCSVCommand { get; private set; }
        public ICommand CopyDetailsCommand { get; private set; }

        public DropDeckCreatorViewModel(ISmurfyDataLoaderService smurfyDataLoaderService)
        {
            _smurfyDataLoaderService = smurfyDataLoaderService;
            CreateDropDecksFromBuildsCommand = new DelegateCommand(ExecuteCreateDropDecksAsync, CanExecuteCreateDropDecksAsync);
            ExportToExcelCommand = new DelegateCommand<ExtendedDataGrid>(ExecuteExportToExcelCommand);
            ExportToCSVCommand = new DelegateCommand<ExtendedDataGrid>(ExecuteExportToCSVCommand);
            Filters = new ObservableCollection<IFilterViewModel>(GetFilterClassInstances());
            CopyDetailsCommand = new DelegateCommand<DropDeck>(ExecuteCopyDetailsCommand);
        }

        private void ExecuteCopyDetailsCommand(DropDeck item)
        {
            StringBuilder sb = new StringBuilder();
            //Main line details
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}{9}", "Tonnage", "Avg Sus. Dps", "Avg Max Dps", "Avg. Firepower", "Avg Heat Eff.", "Avg Eff. Range (excl Lights)", "Avg. Speed", "No. ECM Chassis", "MechSummary", Environment.NewLine);
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}{9}", item.Tonnage.ToString("N0"), item.AverageSusDps.ToString("N2"), item.AverageMaxDps.ToString("N2"), item.AverageFirepower.ToString("N0"), item.AverageHeatEfficiency.ToString("P0"), item.AverageRangeExclLights.ToString("N0"), item.AverageSpeed.ToString("N0"), item.ECMChassis, item.MechSummary, Environment.NewLine);

            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}{5}","Name", "Weapon Summary", "Chassis Type","Tonnage", "Smurfy URL", Environment.NewLine);
            item.Mechs.ForEach(
                x =>
                    sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}{5}", x.MechName, x.WeaponSummary, x.Mech.Type,
                        x.MechTonnage.ToString("N0"), x.Url, Environment.NewLine));


            Clipboard.SetData(DataFormats.Text, sb.ToString());

            Helper.InvokeForUI(() =>
            {
                InteractionService.ShowMessageBox("Copied to Clipboard", "Copied Drop Deck data to Clipboard.",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            });

        }

        private IEnumerable<IFilterViewModel> GetFilterClassInstances()
        {
            List<IFilterViewModel> returnValue = new List<IFilterViewModel>();
            var type = typeof(IFilterViewModel);
            var types = Assembly.GetExecutingAssembly()
                        .GetTypes()
                        .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToList();
            types.ToList().ForEach(filterType => returnValue.Add((IFilterViewModel)Activator.CreateInstance(filterType)));

            return returnValue;

        }

        private void ExecuteExportToCSVCommand(ExtendedDataGrid args)
        {
            RaiseBusyMessage(true, "Exporting to CSV...");

            var path = InteractionService.GetSaveFileFromUser("Comma Separated Value(.csv)|*.csv");
            if (!String.IsNullOrEmpty(path))
                args.ExportToCsv("Dropdecks", path, false);

            RaiseBusyMessage(false);
        }

        private void ExecuteExportToExcelCommand(ExtendedDataGrid args)
        {
            RaiseBusyMessage(true, "Exporting to Excel...");

            var path = InteractionService.GetSaveFileFromUser("Excel Workbook(.xlsx)|*.xlsx");
            if (!String.IsNullOrEmpty(path))
                args.ExportToExcel("Dropdecks", path, false);

            RaiseBusyMessage(false);
        }

        private bool CanExecuteCreateDropDecksAsync()
        {
            return Filters.Where(x => x.IsEnabled).All(x => x.IsValid());
        }

        private void ExecuteCreateDropDecksAsync()
        {
            Helper.InvokeForUI(() =>
            {
                RaiseBusyMessage(true, "Preparing dropdeck combinations...");
            });

            Task.Factory.StartNew(() =>
            {

                var builds = _smurfyDataLoaderService.GetBuilds().Where(x => x.IsSelected).ToList();

                //Apply pre-combination filters (saves on combination creation time
                var preFilters = Filters.OfType<IFilterViewModel<SmurfyBuild>>().Where(x => x.IsEnabled).ToList();
                preFilters.ForEach(x => builds = x.ApplyFilter(builds).ToList());

                int mechsInDropdeck = 0;
                switch (DropDeckType)
                {
                    case DropDeckTypeEnum.CommunityWarfare:
                        mechsInDropdeck = 4;
                        break;
                    case DropDeckTypeEnum.Competition8Man:
                        mechsInDropdeck = 8;
                        break;
                    case DropDeckTypeEnum.Competition12Man:
                        mechsInDropdeck = 12;
                        break;
                }

                var combinator = new Combinations<SmurfyBuild>(builds, mechsInDropdeck, GenerateOption.WithRepetition);

                bool userContinue = true;
                TimeSpan span = TimeSpan.FromMilliseconds(Convert.ToDouble(combinator.Count / 67));
                if (span > new TimeSpan(0, 0, 30))
                {
                    Helper.InvokeForUI(() =>
                    {
                        userContinue = (InteractionService.ShowMessageBox("Do you wish to proceed?",
                            String.Format(
                                "The number of build combinations you are attempting to create can take up to {0} day(s) {1} hr(s) {2} min(s) {3} sec(s) to complete.\n\nDo you wish to continue?",
                                span.Days, span.Hours, span.Minutes, span.Seconds),
                            MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes);
                    });
                }

                IEnumerable<DropDeck> combinations = new List<DropDeck>();


                if (userContinue)
                {
                    Helper.InvokeForUI(() =>
                    {
                        RaiseBusyMessage(true, "Creating dropdeck combinations...", span);
                    });

                    var swSw = PerformanceHelper.Start();

                    combinations = combinator.Select(x => new DropDeck(x)).Distinct().ToList();

                    //Apply the post combination filters
                    var postFilters = Filters.OfType<IFilterViewModel<DropDeck>>().Where(x => x.IsEnabled).ToList();
                    combinations = combinations.Where(x => postFilters.All(filter => filter.PassFilterConditions(x)));
                    //postFilters.ForEach(x => combinations = x.ApplyFilter(combinations).ToList());



                    _log.DebugFormat("{2} Combination : {1} : {0}ms", PerformanceHelper.Stop(swSw), combinations.Count(), mechsInDropdeck);

                }

                Helper.InvokeForUI(() =>
                {
                    DropDecks = new ObservableCollection<DropDeck>(combinations);
                    OnPropertyChanged(() => DropDecks);
                    RaiseBusyMessage(false, string.Empty);
                });
            });
        }


    }
}