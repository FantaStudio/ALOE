using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ALOE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LogoImage.ScaleTo(1, 100);
            await LogoImage.ScaleTo(0.5, 500, Easing.Linear);
            await LogoImage.ScaleTo(2, 400, Easing.Linear);
            Application.Current.MainPage = new WelcomePage();
        }
    }
}