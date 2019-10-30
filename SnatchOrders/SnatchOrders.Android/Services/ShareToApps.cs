using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SnatchOrders.Droid.Services;
using SnatchOrders.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(ShareToApps))]
namespace SnatchOrders.Droid.Services {
    public class ShareToApps : IShare {
        public void ShareMessageToApps(string message) {
            Intent sendIntent = new Intent();
            sendIntent.SetAction(Intent.ActionSend);
            sendIntent.PutExtra(Intent.ExtraText, message);
            sendIntent.SetType("text/plain");

            //Intent shareIntent = Intent.CreateChooser(sendIntent, "");
            Android.App.Application.Context.StartActivity(sendIntent);
        }
    }
}