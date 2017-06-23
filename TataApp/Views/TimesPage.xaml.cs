using System;
using TataApp.ViewModels;
using Xamarin.Forms;

namespace TataApp.Views
{
    public partial class TimesPage : ContentPage
    {
        public TimesPage()
        {
            InitializeComponent();

			var timesViewModel = TimesViewModel.GetInstance();
			base.Appearing += (object sender, EventArgs e) =>
			{
				timesViewModel.RefreshCommand.Execute(this);
			};
        }
    }
}
