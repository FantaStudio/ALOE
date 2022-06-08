using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace ALOE.Helpers
{
    public static class PrivacyPolicyHelper
    {
        private const string PRIVACTY_POLICY_FILE = "privacyPolicy.doc";
        public static byte[] GetFileBytes()
        {
            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                using (Stream stream = assembly.GetManifestResourceStream($"ALOE.Files.{PRIVACTY_POLICY_FILE}"))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void SaveFile()
        {
            try
            {
                var bytes = GetFileBytes();
                DependencyService.Get<IDownloadFile>().DownloadLocalFile(bytes);
            }
            catch (Exception exception)
            {
                Application.Current.MainPage?.DisplayAlert("Ошибка при попытке сохранения файла", exception.Message,"Окей :(");
            }
        }
    }

    public interface IDownloadFile
    {
        void DownloadLocalFile(byte[] fileInfo);
    }
}
