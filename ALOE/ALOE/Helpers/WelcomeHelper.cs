using System.Collections.Generic;
using Xamarin.Forms;

namespace ALOE.Helpers
{
    static class WelcomeHelper
    {
       public static readonly List<Onboarding> boardings = new List<Onboarding>
       {
           new Onboarding { Header = "Приветствуем в программе лояльности", Description = "Добро пожаловать в программу лояльности АЗС \"ALOE\""},
           new Onboarding { Header = "Для чего программа лояльности?", Description = "Копите баллы и получайте скидки"},
           new Onboarding { Header = "Что ещё?", Description = "Оплачивайте топливо через приложение"}
       };

        public static bool IsFirstOpen() => Application.Current.Properties.ContainsKey("FirstUse");
        public static void SetFirstUse() => Application.Current.Properties["FirstUse"] = false;
    }

    public class Onboarding
    {
        public string Header { get; set; }
        public string Description { get; set; }
    }
}
