using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ALOE.Database;
using System.Text.RegularExpressions;

namespace ALOE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        static readonly BindableProperty IsEditProperty = BindableProperty.Create(nameof(IsEdit), typeof(bool), typeof(Profile), false);
        public bool IsEdit 
        {
            get { return (bool)GetValue(IsEditProperty); }
            set { SetValue(IsEditProperty, value); }
        }

        public Profile()
        {
            IsEdit = false;
            InitializeComponent();
            BindingContext = this;
            Appearing += Profile_Appearing;
        }

        private void Profile_Appearing(object sender, EventArgs e)
        {
            StartLoader();
            LoadProfile();
            StopLoader();
        }

        private void StartLoader()
        {
            Loader.IsRunning = true;
            Loader.IsVisible = true;
        }
        private void StopLoader()
        {
            Loader.IsRunning = false;
            Loader.IsVisible = false;
        }

        private async void LoadProfile()
        {
            if (Helpers.Main.CURRENT_USER_LOGIN == null) return;
            Client client = await AloeDB.GetClient(Helpers.Main.CURRENT_USER_LOGIN);
            if (client == null) return;
            NameBox.Text = client.Name;
            SurnameBox.Text = client.Surname;
            MiddlenameBox.Text = client.Middlename;
            EmailBox.Text = client.Email;
            PhoneBox.Text = client.Phone;
            LoginBox.Text = client.Login;
        }

        private async void SaveProfile()
        {
            try
            {
                var client = await AloeDB.GetClient(Helpers.Main.CURRENT_USER_LOGIN);
                if (client == null)
                    throw new Exception("Призошла неизвестная ошибка, возможно закончился ваш сеанс");

                if (!string.IsNullOrWhiteSpace(NameBox.Text))
                {
                    if (IsStringLikeName(NameBox.Text))
                        client.Name = NameBox.Text;
                    //newData.Add(new KeyValuePair<string, string>("name", NameBox.Text));
                    else
                        throw new Exception("Неверное имя");
                }
                if (!string.IsNullOrWhiteSpace(SurnameBox.Text))
                {
                    if(IsStringLikeName(SurnameBox.Text))
                        client.Surname = SurnameBox.Text;
                    //newData.Add(new KeyValuePair<string, string>("surname", SurnameBox.Text));
                    else
                        throw new Exception("Неверная фамилия");
                }
                if (!string.IsNullOrWhiteSpace(MiddlenameBox.Text))
                {
                    if (IsStringLikeName(MiddlenameBox.Text))
                        client.Middlename = MiddlenameBox.Text;
                    //newData.Add(new KeyValuePair<string, string>("middlename", MiddlenameBox.Text));
                    else
                        throw new Exception("Неверное отчество");
                }
                if (!string.IsNullOrWhiteSpace(EmailBox.Text))
                {
                    if(IsStringLikeMail(EmailBox.Text))
                        client.Email = EmailBox.Text;
                        //newData.Add(new KeyValuePair<string, string>("phone", PhoneBox.Text));
                    else
                        throw new Exception("Неверный почтовый адрес");
                }
                if (!string.IsNullOrWhiteSpace(PhoneBox.Text))
                {
                    if (IsStringLikePhone(PhoneBox.Text))
                        client.Phone = PhoneBox.Text;
                    //newData.Add(new KeyValuePair<string, string>("mail", EmailBox.Text));
                    else
                        throw new Exception("Неверный номер телефона");
                }
                var aloeresp = await AloeDB.SaveUser(client);
                if (aloeresp == false) throw new Exception("Не удалось сохранить данные");
                await DisplayAlert("Здорово!", "Данные успешно изменены", "ОК");
                LoadProfile();
            }
            catch (Exception exception)
            {
                await DisplayAlert("Ошибка",exception.Message,"ОК");
                LoadProfile();
            }
        }

        bool IsStringLikePhone(string text) => Regex.IsMatch(text, @"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$");

        bool IsStringLikeName(string text) => Regex.IsMatch(text, @"^[a-zA-Zа-яА-Я]+$");

        bool IsStringLikeMail(string text) => Regex.IsMatch(text, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

        private void ActionButton_Clicked(object sender, EventArgs e)
        {
            if (IsEdit)
            {
                StartLoader();
                SaveProfile();
                StopLoader();
                ActionButton.Text = "Редактировать";
            }
            else
                ActionButton.Text = "Сохранить";
            IsEdit = !IsEdit;
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            IsEdit = false;
            ActionButton.Text = "Редактировать";
            LoadProfile();
        }
    }
}