﻿using System;
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

        private void Navigate()
        {
            if(PageName == "LoginPage"){

                navigationService.SetMainPage("LoginPage");
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
