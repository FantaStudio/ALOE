using System;
using System.Collections.Generic;
using ALOE.Database;
using ALOE.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ALOE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
            refreshPageView.Command = new Command(() =>
            {
                PullFuelInfo();
                LoadCards();
                refreshPageView.IsRefreshing = false;
            });
            refreshPageView.IsRefreshing = true;
        }

        private Frame CreateFuelCard(string name,string price)
        {
            Frame frame = new Frame()
            {
                Margin = new Thickness(0, 0, 10, 0),
                HasShadow = true,
                CornerRadius = 10,
                BackgroundColor = Color.FromHex("#796bb5"),
                WidthRequest = 100,
                HeightRequest = 50,
            };

            StackLayout stack = new StackLayout();
            Label labName = new Label() { Text = name, FontSize = 17, Style = (Style)Application.Current.Resources["ParBold"] };
            Label labPrice = new Label() { Text = price + " " + Helpers.Main.Currency, FontSize = 15, Style = (Style)Application.Current.Resources["Par"] };

            stack.Children.Add(labName);
            stack.Children.Add(labPrice);
            frame.Content = stack;

            return frame;
        }

        private void StartLoader()
        {
            Loader.IsVisible = true;
            Loader.IsRunning = true;
        }

        private void StopLoader()
        {
            Loader.IsVisible = false;
            Loader.IsRunning = false;
        }

        private async void PullFuelInfo()
        {
            FuelTypeList.Children.Clear();

            List<FuelType> fuelTypes;
            try
            {
                fuelTypes = await AloeDB.GetFuelTypes().TimeoutAfter(TimeSpan.FromSeconds(15));
            }
            catch (Exception)
            {
                fuelTypes = null;
            }

            if (fuelTypes == null || fuelTypes.Count < 1) return;
            foreach(var ftype in fuelTypes)
            {
                Frame frame = CreateFuelCard(ftype.Name, ftype.Cost.ToString());
                FuelTypeList.Children.Add(frame);
            }
        }

        private async void LoadCards()
        {
            if (Main.CURRENT_USER_LOGIN == null)
            {
                return;
            }

            CardsList.Children.Clear();

            List<ClientCard> clientCards;
            try
            {
                clientCards = await AloeDB.GetClientCards(Main.CURRENT_USER_LOGIN).TimeoutAfter(TimeSpan.FromSeconds(15));
            }
            catch(Exception)
            {
                clientCards = null;
            }


            if (clientCards == null || clientCards.Count < 1)
            {
                CardsList.Children.Add(new Label() { Text = "У вас ещё нет карт :(", Style = (Style)Application.Current.Resources["ParBold"], HorizontalOptions = LayoutOptions.Start});
                return;
            }

            foreach (ClientCard card in clientCards)
            {
                var aloeCard = new UI.AloeCard() { WidthRequest = 50, BalanceReal = card.Bonus, ID = card.ID, NumberReal = card.ID, HeightRequest = 150};
                CardsList.Children.Add(aloeCard);
            }
        }
    }
}