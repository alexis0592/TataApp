using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TataApp.Services;

namespace TataApp.ViewModels
{
    public class MenuItemViewModel
    {
        #region Attributes
        private NavigationService navigationService;
        #endregion

        #region Properties
        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }
        #endregion

        #region Commands
        public ICommand NavigateCommand
        {
            get { return new RelayCommand(Navigate); }
        }

        private async void Navigate()
        {
            if(PageName == "LoginPage"){

                navigationService.SetMainPage(PageName);
            }else{

                var mainViewModel = MainViewModel.GetInstance();

                switch(PageName){
                    case "TimesPage":
                        mainViewModel.Times = new TimesViewModel();
                        break;
					case "LocationsPage":
                        mainViewModel.Locations = new LocationsViewModel();
						break;
                    case "EditUserPage":
                        mainViewModel.EditUser = new EditUserViewModel();
                        break;
                    default:
                        break;
                }

                await navigationService.Navigate(PageName);
            }

        }
        #endregion


        #region Constructors
        public MenuItemViewModel(){
            navigationService = new NavigationService();
        }
        #endregion
    }
}
