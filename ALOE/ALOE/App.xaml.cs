using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using ALOE.Database;

namespace ALOE
{
    public partial class App : Application
    {
        public App()
        {
            Helpers.Main.CURRENT_USER_LOGIN = "test";
            AloeDB.CreateTables();

            InitializeComponent();

            Device.SetFlags(new[] { "MediaElement_Experimental", "Brush_Experimental" });

            Connectivity.ConnectivityChanged += (s,e) => OnNoConnection();
            if (Helpers.Main.IsInternetConnectionAvailable)
            {
                MainPage = new WelcomePage();
            }
            else
            {
                OnNoConnection();
            }
        }


        NoConnection _isNotConnectionWindow;
        private void OnNoConnection()
        {
            if (!Helpers.Main.IsInternetConnectionAvailable)
            {
                if (_isNotConnectionWindow == null)
                {
                    _isNotConnectionWindow = new NoConnection();
                }
                MainPage = _isNotConnectionWindow;
            }
            else
            {
                if (_isNotConnectionWindow != null && MainPage is NoConnection)
                {
                    if (Helpers.Main.CURRENT_USER_LOGIN != null)
                    {
                        MainPage = new MainMenu();
                    }
                    else
                    {
                        MainPage = new Oauth(true);
                    }
                }
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
