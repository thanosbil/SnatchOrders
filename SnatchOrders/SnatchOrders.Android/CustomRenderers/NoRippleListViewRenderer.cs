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
using SnatchOrders.Controls;
using SnatchOrders.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NoRippleListView), typeof(NoRippleListViewRenderer))]
namespace SnatchOrders.Droid.CustomRenderers {
    public class NoRippleListViewRenderer : ListViewRenderer {

        public NoRippleListViewRenderer(Context context) : base(context) {

        }

        // CustomRenderer για να κρύψω το ripple effect
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e) {
            base.OnElementChanged(e);

            if (Control != null)
                Control.SetSelector(Resource.Layout.no_selector);
        }
    }
}