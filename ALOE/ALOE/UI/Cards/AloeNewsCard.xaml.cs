using ALOE.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ALOE.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AloeNewsCard : ContentView
    {
        private News _news;
        public News News
        {
            get { return _news; }
            set 
            { 
                _news = value;
                BindingContext = value;
            }
        }

        public AloeNewsCard()
        {
            InitializeComponent();
        }
    }
}