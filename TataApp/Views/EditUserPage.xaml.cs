using System;
using System.Collections.Generic;
using Plugin.Media;
using Plugin.Media.Abstractions;
using TataApp.Services;
using TataApp.ViewModels;
using Xamarin.Forms;

namespace TataApp.Views
{
    public partial class EditUserPage : ContentPage
    {
        DialogService dialogService;
        MediaFile file;

        public EditUserPage()
        {
            dialogService = new DialogService();

            InitializeComponent();
        }

		async void OnActionSheetSimpleClicked(object sender, EventArgs e)
		{
			var action = await DisplayActionSheet("Select Image", "Cancel", null, "Gallery", "Camera");

            if(action == "Gallery"){
                var editUserViewModel = EditUserViewModel.GetInstance();
                PickPicture();
            }
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

			//IsRunning = true;

			//if (file != null)
			//{
			//	ImageSource = ImageSource.FromStream(() =>
			//	{
			//		var stream = file.GetStream();
			//		return stream;
			//	});
			//}

			//IsRunning = false;
		}

    }
}
