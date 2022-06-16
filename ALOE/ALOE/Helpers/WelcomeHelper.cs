using System.Collections.Generic;
using Xamarin.Forms;

namespace ALOE.Helpers
{
    static class WelcomeHelper
    {
       public static readonly List<Onboarding> boardings = new List<Onboarding>
       {
           new Onboarding { Header = "Приветствуем в приложении ALOE", Description = "Добро пожаловать в приложение АЗС \"ALOE\". " +
               "Оно поможет вам экономить и быть в курсе всех новостей"},
           new Onboarding { Header = "Для чего программа лояльности?", Description = "Копите баллы и получайте скидки"},
           new Onboarding { Header = "Бонусы за регистрацию", Description = "Зарегистрируся и получи 50 баллов лояльности!"},
           new Onboarding { Header = "Что ещё?", Description = "Оплачивайте топливо через приложение"},
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
