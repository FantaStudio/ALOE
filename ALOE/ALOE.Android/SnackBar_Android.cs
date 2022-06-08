using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Google.Android.Material.Snackbar;
using Plugin.CurrentActivity;
using ALOE.Droid;
[assembly: Xamarin.Forms.Dependency(typeof(SnackBar_Android))]

namespace ALOE.Droid
{
    public class SnackBar_Android : SnackbarInterface
    {
        public void SnackbarShow(string message)
        {
            Activity activity = CrossCurrentActivity.Current.Activity;
            Android.Views.View view = activity.FindViewById(Android.Resource.Id.Content);
            Snackbar.Make(view, message, Snackbar.LengthLong).Show();
        }
    }
}