using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms;
using System.Reflection;

namespace ALOE
{
    public class AloeMap : Map
    {
        public static readonly BindableProperty ShowMeButtonProperty = BindableProperty.Create(nameof(ShowMeButton), typeof(bool), typeof(AloeMap), true);
        public static readonly BindableProperty ZoomButtonsProperty = BindableProperty.Create(nameof(ZoomButtons), typeof(bool), typeof(AloeMap), false);
        public static readonly BindableProperty FuelInfoIsShowedProperty = BindableProperty.Create(nameof(FuelInfoIsShowed), typeof(bool), typeof(AloeMap),false);

        public bool ShowMeButton
        {
            get { return (bool)GetValue(ShowMeButtonProperty); }
            set { SetValue(ShowMeButtonProperty, value); }
        }
        public bool ZoomButtons
        {
            get { return (bool)GetValue(ZoomButtonsProperty); }
            set { SetValue(ZoomButtonsProperty, value); }
        }
        public bool FuelInfoIsShowed
        {
            get { return (bool)GetValue(FuelInfoIsShowedProperty); }
            set { SetValue(FuelInfoIsShowedProperty, value); }
        }

        public int ButtonsBottomPadding { get; set; }

        public void MoveToRussia() => MoveToRegion(MapSpan.FromCenterAndRadius(new Position(60, 100), Distance.FromKilometers(1000)));
        public void MoveToPosition(Position pos, Distance dist = default)
        {
            if (dist == default) dist = Distance.FromKilometers(1);
            MoveToRegion(MapSpan.FromCenterAndRadius(pos, dist));
        }
        public void MoveToLocation(Xamarin.Essentials.Location location, Distance dist = default) => MoveToPosition(new Position(location.Latitude, location.Longitude),dist);
        public AloeMap()
        {
            // RUSSIA
            ButtonsBottomPadding = 310;
        }
    }
}
