using ALOE.Database;
using ALOE.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ALOE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StocksPage : ContentPage
    {
        public StocksPage()
        {
            InitializeComponent();
            refreshPageView.Command = new Command(() =>
            {
                LoadStocks();
                refreshPageView.IsRefreshing = false;
            });
            refreshPageView.IsRefreshing = true;
        }

        private async void LoadStocks()
        {
            try
            {
                StocksList.Children.Clear();
                var stocks = await AloeDB.GetStocks();
                if (stocks == null || stocks.Count < 1)
                {
                    StocksList.Children.Add(new Label() { Text = "Акции отсутствуют :(", HorizontalOptions = LayoutOptions.Center, TextColor = Color.White });
                    return;
                }
                var stocksItems = stocks.Select(x => new AloeStockCard() { Stock = x }).ToList();
                stocksItems.ForEach(x => StocksList.Children.Add(x));
            }
            catch (Exception exception)
            {
                await DisplayAlert("Ошибка", exception.Message, "OK");
            }
        }
    }
}