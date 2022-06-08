using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Plugin.Geolocator;

namespace ALOE.Helpers
{
    class LocationHelper
    {
        public static async Task<Location> GetLocation()
        {
            try
            {
                if (!IsLocationAvailable) return null;
                return await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Best,
                    Timeout = TimeSpan.FromSeconds(1),
                });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<string> GetCity(Location location)
        {
            try
            {
                var places = await Geocoding.GetPlacemarksAsync(location);
                var curr_place = places?.FirstOrDefault();

                if (curr_place != null)
                {
                    return $"{curr_place.CountryName},{curr_place.Locality}";
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool IsLocationAvailable
        {
            get
            {
                if (!CrossGeolocator.IsSupported)
                {
                    return false;
                }
                return CrossGeolocator.Current.IsGeolocationAvailable;
            }
        }
    }
}
