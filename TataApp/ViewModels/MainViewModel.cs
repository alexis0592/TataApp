using System;
using System.Collections.ObjectModel;
using TataApp.Models;

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

		public Employee Employee
		{
			get;
			set;
		}
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;

            Menu = new ObservableCollection<MenuItemViewModel>();
            Login = new LoginViewModel();
            LoadMenu();
        }
		#endregion

		#region Singleton
		private static MainViewModel instance;

		public static MainViewModel GetInstance()
		{
			if (instance == null)
			{
				instance = new MainViewModel();
			}

			return instance;
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
