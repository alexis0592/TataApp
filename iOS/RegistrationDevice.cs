using System;
using TataApp.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(TataApp.iOS.RegistrationDevice))]

namespace TataApp.iOS
{
    public class RegistrationDevice : IRegisterDevice
    {
        public RegistrationDevice()
        {
        }

        public void RegisterDevice()
        {
            //TODO: Implement Interface
        }
    }
}
