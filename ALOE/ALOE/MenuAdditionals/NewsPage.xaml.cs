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
    public partial class NewsPage : ContentPage
    {
        public NewsPage()
        {
            InitializeComponent();
            refreshPageView.Command = new Command(() =>
            {
                LoadNews();
                refreshPageView.IsRefreshing = false;
            });
            refreshPageView.IsRefreshing = true;
        }

        private async void LoadNews()
        {
            try
            {
                NewsList.Children.Clear();
                var news = await AloeDB.GetNews();
                if (news == null || news.Count < 1)
                {
                    NewsList.Children.Add(new Label() { Text = "Новости отсутствуют :(", HorizontalOptions = LayoutOptions.Center, TextColor = Color.White });
                    return;
                }
                var newsItems = news.Select(x => new AloeNewsCard() { News = x }).ToList();
                newsItems.ForEach(x => NewsList.Children.Add(x));
            }
            catch (Exception exception)
            {
                await DisplayAlert("Ошибка", exception.Message, "OK");
            }
        }
    }
}