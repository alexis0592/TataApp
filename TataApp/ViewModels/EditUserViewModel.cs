using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using TataApp.Helpers;
using TataApp.Models;
using TataApp.Services;
using Xamarin.Forms;

namespace TataApp.ViewModels
{
    public class EditUserViewModel : Employee, INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Attributes
        NavigationService navigationService;
        ApiService apiService;
        DialogService dialogService;
        DataService dataService;
        bool isRunning;
        bool isEnabled;
        MainViewModel mainViewModel;
        ImageSource imageSource;
        MediaFile file;
        #endregion

        #region Properties
        public ObservableCollection<DocumentTypeViewModel> DocumentTypes
        {
            get;
            set;
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

		public ImageSource ImageSource
		{
			set
			{
				if (imageSource != value)
				{
					imageSource = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource"));
				}
			}
			get
			{
				return imageSource;
			}
		}
        #endregion

        #region Constructors
        public EditUserViewModel()
        {
            instance = this;

            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            mainViewModel = MainViewModel.GetInstance();
            DocumentTypes = new ObservableCollection<DocumentTypeViewModel>();
            dataService = new DataService();
            LoadUser();
            LoadDocumentTypes();
            IsEnabled = true;
        }
		#endregion

		#region Singleton
        private static EditUserViewModel instance;

		public static EditUserViewModel GetInstance()
		{
			if (instance == null)
			{
				instance = new EditUserViewModel();
			}

			return instance;
		}
		#endregion

		#region Methods
		void LoadUser()
        {
            var userLogged = MainViewModel.GetInstance().Employee;

            FirstName = userLogged.FirstName;
            LastName = userLogged.LastName;
            EmployeeCode = userLogged.EmployeeCode;
            DocumentTypeId = userLogged.DocumentTypeId;
            Document = userLogged.Document;
            Picture = userLogged.Picture;
            Email = userLogged.Email;
            Phone = userLogged.Phone;
            Address = userLogged.Address;
            ImageSource = userLogged.FullPicture;

        }

        async void LoadDocumentTypes()
        {
            IsRunning = true;
            IsEnabled = false;

            var connection = await apiService.CheckConnectivity();
            if (!connection.IsSuccess)
            {
                IsEnabled = false;
                IsRunning = false;

                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();
            var employee = mainViewModel.Employee;
            var response = await apiService.GetList<DocumentType>(urlAPI,
                                                                  "/api",
                                                                 "/DocumentTypes",
                                                                  employee.TokenType,
                                                                  employee.AccessToken);
            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = false;
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            ReloadDocumentsType((List<DocumentType>)response.Result);
            IsRunning = false;
            isEnabled = true;
        }

        private void ReloadDocumentsType(List<DocumentType> result)
        {
            DocumentTypes.Clear();
            foreach (var documentType in result)
            {
                DocumentTypes.Add(new DocumentTypeViewModel
                {
                    Description = documentType.Description,
                    DocumentTypeId = documentType.DocumentTypeId
                });
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DocumentTypeId"));

        }

		async void PickPicture()
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				await dialogService.ShowMessage(
					"Photo Not Supported",
					":( No available to pick photos from gallery.");
				return;
			}

			file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
			{
				PhotoSize = PhotoSize.Small,
			});

			IsRunning = true;

			if (file != null)
			{
				ImageSource = ImageSource.FromStream(() =>
				{
					var stream = file.GetStream();
					return stream;
				});
			}

			IsRunning = false;
		}

		async void TakePicture()
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable ||
				!CrossMedia.Current.IsTakePhotoSupported)
			{
				await dialogService.ShowMessage(
					"No Camera",
					":( No camera available.");
				return;
			}

			file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				Directory = "Sample",
				Name = "test.jpg",
				PhotoSize = PhotoSize.Small,
			});

			IsRunning = true;

			if (file != null)
			{
				ImageSource = ImageSource.FromStream(() =>
				{
					var stream = file.GetStream();
					return stream;
				});
			}

			IsRunning = false;
		}

		public static bool CheckEmail(string email)
		{
			var expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
			if (Regex.IsMatch(email, expresion))
			{
				if (Regex.Replace(email, expresion, String.Empty).Length == 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
        #endregion

        #region Commands
        public ICommand UpdateCommand
        {
            get { return new RelayCommand(Update); }
        }

        private async void Update()
        {
			if (string.IsNullOrEmpty(FirstName))
			{
				await dialogService.ShowMessage("Error", "You must enter a first name.");
				return;
			}

			if (string.IsNullOrEmpty(LastName))
			{
				await dialogService.ShowMessage("Error", "You must enter a last name.");
				return;
			}

			if (EmployeeCode == 0)
			{
				await dialogService.ShowMessage("Error", "You must enter an employee code.");
				return;
			}

			if (string.IsNullOrEmpty(Document))
			{
				await dialogService.ShowMessage("Error", "You must enter a document.");
				return;
			}

			if (string.IsNullOrEmpty(Email))
			{
				await dialogService.ShowMessage("Error", "You must enter an email.");
				return;
			}

			if (!CheckEmail(Email))
			{
				await dialogService.ShowMessage("Error", "You must enter a valid email.");
				return;
			}

			if (string.IsNullOrEmpty(Phone))
			{
				await dialogService.ShowMessage("Error", "You must enter a phone.");
				return;
			}

			if (string.IsNullOrEmpty(Address))
			{
				await dialogService.ShowMessage("Error", "You must enter an address.");
				return;
			}

            IsRunning = true;
            IsEnabled = false;

            byte[] imageArray = null;
            if (file != null)
            {
                imageArray = FilesHelper.ReadFully(file.GetStream());
                file.Dispose();
            }

            var employee = new Employee
            {
                Address = Address,
                Document = Document,
                DocumentTypeId = DocumentTypeId,
                Email = Email,
                EmployeeCode = EmployeeCode,
                EmployeeId = mainViewModel.Employee.EmployeeId,
                FirstName = FirstName,
                ImageArray = imageArray,
                LastName = LastName,
                LoginTypeId = LoginTypeId,
                Password = Password,
                Phone = Phone,


            };

            var urlApi = Application.Current.Resources["URLAPI"].ToString();
            var response = await apiService.Post(urlApi,
                                               "/api",
                                               "/Employees",
                                                mainViewModel.Employee.TokenType,
                                                mainViewModel.Employee.AccessToken,
                                                employee);
            IsRunning = false;
            IsEnabled = true;

            if(!response.IsSuccess){
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            mainViewModel.Employee = this;
            dataService.DeleteAllAndInsert(this);
            await navigationService.Back();
            
        }

        public ICommand ChooseImageCommand
        {
            get { return new RelayCommand(PickPicture); }
        }

        public ICommand TakePictureCommand
        {
            get { return new RelayCommand(TakePicture); }
        }
        #endregion




    }
}
