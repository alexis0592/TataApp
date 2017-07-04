using System;
using System.Threading.Tasks;

namespace TataApp.Services
{
    public class DialogService
    {
        public async Task ShowMessage(string title, string message)
		{
			await App.Current.MainPage.DisplayAlert(title, message, "Accept");
		}

		public async Task<bool> ShowConfirm(string title, string message)
		{
			return await App.Current.MainPage.DisplayAlert(title, message, "Yes", "No");
		}

        public async Task<string> ShowActionSheet(String title, String cancel, 
                                                  String destruction, 
                                                  params String[] buttons){
            return await App.Current.MainPage.DisplayActionSheet(title, cancel, 
                                                                 destruction, buttons);
        }
    }
}
