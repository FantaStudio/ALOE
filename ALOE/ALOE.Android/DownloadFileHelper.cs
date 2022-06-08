using ALOE.Droid;
using ALOE.Helpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(DownloadFileHelper))]
namespace ALOE.Droid
{
    public class DownloadFileHelper : IDownloadFile
    {
        public static byte[] FileBytes { get; set; }

        [Obsolete]
        public void DownloadLocalFile(byte[] fileInfo)
        {
            try
            {
                Intent intent = new Intent(Intent.ActionCreateDocument);
                intent.AddCategory(Intent.CategoryOpenable);
                intent.SetType("application/msword");
                intent.PutExtra(Intent.ExtraTitle, "PrivacyPolicy.doc");
                FileBytes = fileInfo;
                Platform.CurrentActivity.StartActivityForResult(intent, 1000);   
            }
            catch (Exception exception)
            {
                Toast.MakeText(Android.App.Application.Context, exception.Message, ToastLength.Short);
            }
        }
    }
}