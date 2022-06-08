using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ALOE.Database;
using ALOE.Helpers;

namespace ALOE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Oauth : ContentPage
    {
        bool isLogin = false;

        public Oauth(bool login = false)
        {
            InitializeComponent();

            isLogin = !login;
            ChangeAction();
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

        private void ActionButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginBox.Text))
            {
                DisplayAlert("Ошибка","Введите логин","OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Text))
            {
                DisplayAlert("Ошибка", "Введите пароль", "OK");
                return;
            }

            if (isLogin)
            {
                TryAuth();
            }
            else
            {
                TryRegister();
            }
        }

        private async void TryAuth()
        {
            try
            {
                StartLoader();
                var result = await AloeDB.TryAuth(LoginBox.Text, PasswordBox.Text).TimeoutAfter(TimeSpan.FromSeconds(15));
                StopLoader();

                if (result == false)
                {
                    throw new Exception("Не удалось авторизироваться");
                }

                // Start app
                Main.CURRENT_USER_LOGIN = LoginBox.Text;
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new MainMenu();
                }
            }
            catch (Exception exception)
            {
                await DisplayAlert("Ошибка", exception.Message, "OK");
                StopLoader();
            }
        }

        private async void TryRegister()
        {
            try
            {
                StartLoader();
                var result = await AloeDB.TryRegister(LoginBox.Text, PasswordBox.Text).TimeoutAfter(TimeSpan.FromSeconds(15));

                //Check result
                if (result == false)
                {
                    throw new Exception("Не удалось зарегистрироваться");
                }

                if (result == null)
                {
                    throw new Exception("Пользователь с таким логином уже существует");
                }

                //Start app
                Main.CURRENT_USER_LOGIN = LoginBox.Text;
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new MainMenu();
                }
                StopLoader();
            }
            catch(Exception exception)
            {
                await DisplayAlert("Ошибка", exception.Message, "OK");
                StopLoader();
            }
        }

        private void ChangeAction()
        {
            if (isLogin)
            {
                OauthTitle.Text = "Регистрация";
                OauthDescription.Text = "Пожалуйста, введите данные для регистрации.";
                QuestionText.Text = "Уже есть аккаунт?";
                ActionButton.Text = "Зарегистрироваться";
                PrivacyPolicyBlock.IsVisible = true;
                QuestionText.Margin = new Thickness(0);
            }
            else
            {
                OauthTitle.Text = "Авторизация";
                OauthDescription.Text = "Пожалуйста, войдите, чтобы продолжить.";
                QuestionText.Text = "Ещё не зарегистрированы?";
                ActionButton.Text = "Войти";
                PrivacyPolicyBlock.IsVisible = false;
                QuestionText.Margin = new Thickness(0, 15, 0, 0);
            }
            isLogin = !isLogin;
        }

        private void QuestionButton_Clicked(object sender, EventArgs e) => ChangeAction();

        private void PrivacyPolicyButton_Clicked(object sender, EventArgs e)
        {
            PrivacyPolicyHelper.SaveFile();
        }
    }
}