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
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();
            refreshPageView.Command = new Command(() =>
            {
                LoadHistory();
                refreshPageView.IsRefreshing = false;
            });
            refreshPageView.IsRefreshing = true;
        }

        private async void LoadHistory()
        {
            try
            {
                HistoryList.Children.Clear();
                var history = await AloeDB.GetClientTransactions(Helpers.Main.CURRENT_USER_LOGIN);
                //history.Add(new Transaction() { Type = "Покупка топлива", FuelObjectID = 1, DispenserNumber = 2, FuelType = "95", LitresCount = 5, Cost = 260, Date = DateTime.Now, SubBonusCount = 0 });
                if (history == null || history.Count < 1)
                {
                    HistoryList.Children.Add(new Label() { Text = "История пуста :(", HorizontalOptions = LayoutOptions.Center, TextColor = Color.White });
                    return;
                }
                var transactionsItems = history.Select(x => new AloeHistoryCard() { Transaction = x }).ToList();
                transactionsItems.ForEach(x => HistoryList.Children.Add(x));
            }
            catch (Exception exception)
            {
                await DisplayAlert("Ошибка", exception.Message, "OK");
            }
        }
    }
}