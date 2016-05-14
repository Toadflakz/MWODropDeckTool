using System;
using System.Deployment.Application;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;
using MwoCWDropDeckBuilder.Infrastructure;
using MwoCWDropDeckBuilder.Infrastructure.Interfaces;

namespace MwoCWDropDeckBuilder.ViewModel
{
    public class AboutViewModel : BaseViewModel, ITabItemViewModel
    {

        public string Header
        {
            get { return "About"; }
        }

        private string _version;

        public string Version
        {
            get
            {
                if (String.IsNullOrEmpty(_version))
                {
                    if (ApplicationDeployment.IsNetworkDeployed)
                    {
                        _version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                    }
                    else
                    {
                        _version =
                            FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
                    }

                }
                return _version;
            }
        }

        public ICommand NavigateUrlCommand { get; private set; }

        public AboutViewModel()
        {
            NavigateUrlCommand = new DelegateCommand<string>(ExecuteNavigateUrlCommand);
        }

        public void ExecuteNavigateUrlCommand(string url)
        {
            Process.Start(url);
        }
    }
}