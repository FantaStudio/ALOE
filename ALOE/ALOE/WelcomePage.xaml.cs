using System;
using Xamarin.Forms;
using System.Timers;
using ALOE.Helpers;

namespace ALOE
{
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            AnimateCarousel();
        }

        Timer cTimer;
        private void AnimateCarousel()
        {
            cTimer = new Timer(5000) { AutoReset = true, Enabled = true };
            cTimer.Elapsed += (s, e) => 
            {
                Device.BeginInvokeOnMainThread
                (() =>
                {
                    if (cvOnboarding.Position == WelcomeHelper.boardings.Count - 1)
                    {
                        cvOnboarding.Position = 0;
                        return;
                    }
                    cvOnboarding.Position += 1;
                });
            };
        }

        private void EnterButton_Clicked(object sender, EventArgs e)
        {
            if (Application.Current != null)
            {
                Application.Current.MainPage = new Oauth(true);
            }
        }

        private void RegisterButton_Clicked(object sender, EventArgs e)
        {
            if (Application.Current != null)
            {
                Application.Current.MainPage = new Oauth();
            }
        }
    }
}
