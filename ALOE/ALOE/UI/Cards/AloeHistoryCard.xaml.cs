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
    public partial class AloeHistoryCard : ContentView
    {
        private Transaction _transaction;
        public Transaction Transaction
        {
            get { return _transaction; }
            set 
            {
                _transaction = value;
                BindingContext = value;
            }
        }

        public AloeHistoryCard()
        {
            InitializeComponent();
        }
    }
}