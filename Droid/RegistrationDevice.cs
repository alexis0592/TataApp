using System;
using Android.Util;
using Gcm.Client;
using TataApp.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(TataApp.Droid.RegistrationDevice))]

namespace TataApp.Droid
{
    public class RegistrationDevice : IRegisterDevice
    {
        public RegistrationDevice()
        {
        }

		#region Methods

		public void RegisterDevice()

		{

			var mainActivity = MainActivity.GetInstance();

			GcmClient.CheckDevice(mainActivity);

			GcmClient.CheckManifest(mainActivity);

			Log.Info("MainActivity", "Registering...");

			GcmClient.Register(mainActivity, Constants.SenderID);

		}

		#endregion
	}
}
