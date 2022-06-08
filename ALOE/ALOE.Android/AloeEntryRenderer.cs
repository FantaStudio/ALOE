using Android.App;
using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ALOE;
using ALOE.Droid;
using Android.Graphics;
using Android.Content.Res;

[assembly: ExportRenderer(typeof(AloeEntry), typeof(AloeEntryRenderer))]
namespace ALOE.Droid
{
    class AloeEntryRenderer : EntryRenderer
    {
        public AloeEntryRenderer(Context context) : base(context) { }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            var aloeEntry = e.NewElement as AloeEntry;
            if (aloeEntry != null)
            {
                Control.SetHighlightColor(aloeEntry.HighlightColor.ToAndroid());
                Control.BackgroundTintList = ColorStateList.ValueOf(aloeEntry.TintColor.ToAndroid());
            }
        }
    }
}