using ALOE.Database;
using ALOE.Helpers;
using ALOE.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ALOE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrivacyPolicyPage : ContentPage
    {
        public PrivacyPolicyPage()
        {
            InitializeComponent();
        }

        private void SavePrivacyPolicyButton_Clicked(object sender, EventArgs e)
        {
            PrivacyPolicyHelper.SaveFile();
        }
    }
}