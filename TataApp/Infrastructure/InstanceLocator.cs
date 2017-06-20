using System;
using TataApp.ViewModels;

namespace TataApp.Infrastructure
{
    public class InstanceLocator
    {
		public MainViewModel Main
		{
			get;
			set;
		}

		public InstanceLocator()
		{
			Main = new MainViewModel();
		}
    }
}
