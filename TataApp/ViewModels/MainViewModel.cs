using System;
using System.Collections.ObjectModel;

namespace TataApp.ViewModels
{
    public class MainViewModel
    {
        #region Properties
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }

		public LoginViewModel Login
		{
			get;
			set;
		}
        #endregion

        #region Constructors
        public MainViewModel()
        {
            Menu = new ObservableCollection<MenuItemViewModel>();
            Login = new LoginViewModel();
            LoadMenu();
        }
        #endregion

        #region Methods
        private void LoadMenu()
        {
            Menu.Add(new MenuItemViewModel
            {
                Title = "Register Time",
                Icon = "ic_access_alarms.png",
                PageName = "RegisterTimePage"
            });

            Menu.Add(new MenuItemViewModel
            {
                Title = "Sickleaves",
                Icon = "ic_favorite.png",
                PageName = "SickleavesPage"
            });

            Menu.Add(new MenuItemViewModel
            {
                Title = "Localizate Employees",
                Icon = "ic_my_location.png",
                PageName = "LocalizatePage"
            });

            Menu.Add(new MenuItemViewModel
            {
                Title = "Sign out",
                Icon = "ic_exit_to_app.png",
                PageName = "LoginPage"
            });
        }
        #endregion
    }
}
