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
    public partial class AloeCard : ContentView
    {
        public static readonly BindableProperty BalanceProperty = BindableProperty.Create(nameof(Balance), typeof(string), typeof(AloeCard), "");
        public static readonly BindableProperty NumberProperty = BindableProperty.Create(nameof(Number), typeof(string), typeof(AloeCard), "");

        public string Balance
        {
            get { return (string)GetValue(BalanceProperty); }
            set { SetValue(BalanceProperty, value); }
        }
        public string Number
        {
            get { return (string)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        int _balance;
        public int BalanceReal
        {
            get { return _balance; }
            set
            {
                _balance = value;
                Balance = _balance.ToString("#,#") + " Бонусов";
            }
        }

        int _number;
        public int NumberReal
        {
            get { return _number; }
            set
            {
                _number = value;
                Number = _number.ToString("#,#");
            }
        }

        public int ID { get; set; }

        public AloeCard()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}