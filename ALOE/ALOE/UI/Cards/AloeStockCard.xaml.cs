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
    public partial class AloeStockCard : ContentView
    {
        private Stock _stock;
        public Stock Stock
        {
            get { return _stock; }
            set 
            {
                _stock = value;
                BindingContext = value;
            }
        }

        public AloeStockCard()
        {
            InitializeComponent();
        }
    }
}