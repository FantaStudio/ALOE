using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Android;
using Android.Gms.Maps;
using ALOE;
using ALOE.Droid;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AloeMap), typeof(AloeMapRenderer))]
namespace ALOE.Droid
{
    class AloeMapRenderer : MapRenderer
    {
        private AloeMap _customMap;
        private GoogleMap _nativeMap;

        public AloeMapRenderer(Android.Content.Context cont) : base(cont) { }

        // Gets the CustomMap object
        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null) return;
            _customMap = e.NewElement as AloeMap;
        }

        protected override void OnMapReady(GoogleMap nativeMap, Map map)
        {
            base.OnMapReady(nativeMap, map);
            if (_customMap == null) return;
            map.UiSettings.ZoomControlsEnabled = _customMap.ZoomButtons;
            map.UiSettings.MyLocationButtonEnabled = _customMap.ShowMeButton;
            _nativeMap = NativeMap;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var element = Element as AloeMap;
            if (element != null && e.PropertyName == "FuelInfoIsShowed" && _nativeMap != null)
            {
                if (element.FuelInfoIsShowed)
                {
                    //int _ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels) / 1920;
                    //_nativeMap.SetPadding(0, 0, 0, element.ButtonsBottomPadding * _ScreenHeight);
                }
                else
                    _nativeMap.SetPadding(0, 0, 0, 0);
            }
        }
    }
}