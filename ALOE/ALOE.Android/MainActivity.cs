using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Plugin.CurrentActivity;
using System.Linq;
using Android.Content;
using Java.IO;

namespace ALOE.Droid
{
    [Activity(Label = "ALOE", Icon = "@drawable/logo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int RequestLocationId = 999;

        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage
        };

        protected override void OnStart()
        {
            base.OnStart();

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (LocationPermissions.Any(x => CheckSelfPermission(x) != Permission.Granted))
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(Application);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if(requestCode == RequestLocationId)
            {
                if (grantResults.Length < 1)
                {
                    Finish();
                   //Process.KillProcess(Process.MyPid());
                }
                if (grantResults.Any(x => x != Permission.Granted))
                {
                    AloeSnackbar.Show("Вы отказали в доступе");
                    Finish();
                }
            }
            else
            {
                Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 1000)
            {
                if (resultCode == Result.Ok && DownloadFileHelper.FileBytes != null)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    try
                    {
                        var savedFileUri = data.Data;
                        var fileStream = Application.ContentResolver?.OpenOutputStream(savedFileUri);
                        fileStream.Write(DownloadFileHelper.FileBytes);
                        fileStream.Flush();
                        fileStream.Close();

                        alert.SetTitle("Отлично");
                        alert.SetMessage("Файл успешно сохранён");
                    }
                    catch(Exception)
                    {
                        alert.SetTitle("Ошибка");
                        alert.SetMessage("Неудалось сохранить файл, произошла неизвестная ошибка.");
                    }
                    finally
                    {
                        alert.Create();
                        alert.Show();
                    }
                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}