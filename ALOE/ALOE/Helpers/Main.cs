using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ALOE.Database;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace ALOE.Helpers
{
    static class Main
    {
        public static string CURRENT_USER_LOGIN { get; set; }

        public static string Currency = "₽";

        public static int BonusEqual = 100; // Кол-во бонусов к рублю

        public const int MaxUseBonusPercent = 50; // Максимальный процент, который могут покрыть бонусы

        public static Location PositionToLocation(Position position) => new Location(position.Latitude, position.Longitude);
        public static Position LocationToPosition(Location location) => new Position(location.Latitude, location.Longitude);

        public static bool IsInternetConnectionAvailable
        {
            get
            {
                return Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet ||
                Connectivity.NetworkAccess == NetworkAccess.Internet;
            }
        }
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {

            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {

                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    return await task;
                }
                else
                {
                    throw new TimeoutException("Время ожидания истекло.");
                }
            }
        }
    }
}
