﻿using System;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TataApp.Models;
using TataApp.Services;
using Xamarin.Forms;

namespace TataApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        private ApiService apiService;
        private DialogService dialogService;
        private NavigationService navigationService;
        private DataService dataService;
        private string email;
        private string password;
        private bool isRunning;
        private bool isEnabled;
        private bool isRemembered;
        #endregion

        #region Properties
        public string Email
        {
            set
            {
                if (email != value)
                {
                    email = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
                }
            }
            get
            {
                return email;
            }
        }

        public string Password
        {
            set
            {
                if (password != value)
                {
                    password = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));
                }
            }
            get
            {
                return password;
            }
        }

        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get
            {
                return isRunning;
            }
        }

        public bool IsEnabled
        {
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabled"));
                }
            }
            get
            {
                return isEnabled;
            }
        }

        public bool IsRemembered
        {
            set
            {
                if (isRemembered != value)
                {
                    isRemembered = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRemembered"));
                }
            }
            get
            {
                return isRemembered;
            }
        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
			apiService = new ApiService();
			dialogService = new DialogService();
            navigationService = new NavigationService();
            dataService = new DataService();

            IsRemembered = true;
            IsEnabled = true;

            Email = "";
            Password = "";
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
			if (string.IsNullOrEmpty(Email))
			{
				await dialogService.ShowMessage("Error", "You must enter the user email.");
				return;
			}

			if (string.IsNullOrEmpty(Password))
			{
				await dialogService.ShowMessage("Error", "You must enter a password.");
				return;
			}

			IsRunning = true;
			IsEnabled = false;

            var checkConnectivity = await apiService.CheckConnectivity();

            if(!checkConnectivity.IsSuccess){
                IsRunning = false;
				IsEnabled = true;
                await dialogService.ShowMessage("Error", checkConnectivity.Message);
                return;
            }

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

			var token = await apiService.GetToken(
                urlAPI,
				Email,
				Password);

			if (token == null)
			{
				IsRunning = false;
				IsEnabled = true;
				await dialogService.ShowMessage("Error", "The user name or password in incorrect.");
				Password = null;
				return;
			}

			if (string.IsNullOrEmpty(token.AccessToken))
			{
				IsRunning = false;
				IsEnabled = true;
				await dialogService.ShowMessage("Error", token.ErrorDescription);
				Password = null;
				return;
			}

			var response = await apiService.GetEmployeeByEmailOrCode(
                urlAPI,
				"/api",
				"/Employees/GetGetEmployeeByEmailOrCode",
				token.TokenType,
				token.AccessToken,
				token.UserName);

			if (!response.IsSuccess)
			{
				IsRunning = false;
				IsEnabled = true;
				await dialogService.ShowMessage("Error", "Problem ocurred retrieving user information, try latter.");
				return;
			}

			IsRunning = false;
			IsEnabled = true;

			var employee = (Employee)response.Result;

            employee.AccessToken = token.AccessToken;
            employee.IsRemembered = IsRemembered;
            employee.Password = Password;
            employee.TokenExpires = token.Expires;
            employee.TokenType = token.TokenType;

            dataService.DeleteAllAndInsert(employee);

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Employee = employee;
            mainViewModel.RegisterDevice();
            navigationService.SetMainPage("MasterPage");
        }

		public ICommand LoginFacebookCommand
		{
			get { return new RelayCommand(LoginFacebook); }
		}

		void LoginFacebook()
		{
			navigationService.SetMainPage("LoginFacebookPage");
		}

        #endregion
    }
}


