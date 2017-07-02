using System;
using System.Collections.Generic;
using TataApp.ViewModels;
using Xamarin.Forms;

namespace TataApp.Views
{
    public partial class EmployeesPage : ContentPage
    {
        public EmployeesPage()
        {
            InitializeComponent();

			var employeesViewModel = EmployeesViewModel.GetInstance();
			base.Appearing += (object sender, EventArgs e) =>
			{
				employeesViewModel.RefreshCommand.Execute(this);
			};
        }
    }
}
