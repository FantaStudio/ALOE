using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

public interface SnackbarInterface
{
    void SnackbarShow(string message);
}

public static class AloeSnackbar
{
    public static void Show(string message)
    {
        DependencyService.Get<SnackbarInterface>().SnackbarShow(message);
    }
}

