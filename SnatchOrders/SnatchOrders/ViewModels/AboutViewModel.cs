using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace SnatchOrders.ViewModels
{
    public class AboutViewModel  {

        public AboutViewModel()
        {
            //Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}