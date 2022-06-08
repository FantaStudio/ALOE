using System;
using System.Collections.Generic;
using System.Text;

public interface INotificationManager
{
    event EventHandler NotificationReceived;
    void Initialize();
    void SendNotification(string title, string message, DateTime? notifyTime = null);
}

public static class AloeNotify
{
    public static void Show(string title,string message)
    {
        Xamarin.Forms.DependencyService.Get<INotificationManager>().SendNotification(title,message);
    }
}

