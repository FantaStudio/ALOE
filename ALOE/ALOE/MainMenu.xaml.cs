using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ALOE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : Shell
    {
        public MainMenu()
        {
            InitializeComponent();
            Shell.SetNavBarIsVisible(this, false);
        }
    }
}