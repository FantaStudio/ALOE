using ALOE.Helpers;
using ALOE.OSRM.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;

namespace ALOE.OSRM
{
    class OSRM_API
    {
        private static readonly string API_URL = "https://router.project-osrm.org/";
        public static HttpClient GetOSRMClient() => ApiHelper.CreateClient(API_URL);

        static string GetUrl(string service, List<Tuple<string, string>> urlParams)
        {
            var uriBuilder = new UriBuilder(API_URL);
            uriBuilder.Path += service;
            var url = uriBuilder.Uri.ToString();

            var encodedParams = urlParams
                .Select(x => string.Format("{0}={1}", HttpUtility.UrlEncode(x.Item1), HttpUtility.UrlEncode(x.Item2)))
                .ToList();

            var result = url + "?" + string.Join("&", encodedParams);
            return result;
        }

        static string GetValidURL(string service, string version, string profile, List<Location> coordinates, List<Tuple<string, string>> urlParams = null)
        {
            var uriBuilder = new UriBuilder(API_URL);
            uriBuilder.Path += service + "/" + version + "/" + profile + "/" + CreateCoordinatesUrl(coordinates);
            var url = uriBuilder.Uri.ToString();

            string result = url;
            if (urlParams != null && urlParams.Count > 0)
            {
                var encodedParams = urlParams
                    .Select(x => string.Format("{0}={1}", HttpUtility.UrlEncode(x.Item1), HttpUtility.UrlEncode(x.Item2)))
                    .ToList();

                result += "?" + string.Join("&", encodedParams);
            }
            return result;
        }

        static string CreateCoordinatesUrl(List<Location> locations, bool combineToOneAsPolyline = false)
        {
            if (locations == null)
            {
                return string.Empty;
            }
            if (combineToOneAsPolyline)
            {
                var encodedLocs = OsrmPolylineConverter.Encode(locations, 1E5);
                return "polyline(" + encodedLocs + ")";
            }
            else
            {
                return string.Join(";", locations.Select(x => x.Longitude.ToString("", CultureInfo.InvariantCulture)
                        + "," + x.Latitude.ToString("", CultureInfo.InvariantCulture)));
            }
        }

        public static async Task<string> CallRequest(string service, string version, string profile, List<Location> coordinates, List<Tuple<string, string>> urlParams = null)
        {
            try
            {
                string responseStr = null;
                var client = GetOSRMClient();
                using (var response = await client.GetAsync(GetValidURL(service, version, profile, coordinates, urlParams)))
                {
                    responseStr = await response.Content.ReadAsStringAsync();
                };
                return responseStr;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }

    public class Waypoint
    {

        [JsonProperty("hint")]
        public string Hint { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("location")]
        protected double[] LocationArr { get; set; }

        public Location Location
        {
            get
            {
                if (LocationArr == null)
                    return null;

                return new Location(LocationArr[0], LocationArr[1]);
            }
        }
    }

    public class RouteLeg
    {
        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("steps")]
        public RouteStep[] Steps { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }
    }

    public class RouteStep
    {
        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("geometry")]
        public string GeometryStr { get; set; }

        public Location[] Geometry
        {
            get
            {
                if (string.IsNullOrEmpty(GeometryStr))
                {
                    return new Location[0];
                }

                return OsrmPolylineConverter.Decode(GeometryStr, 1E5)
                    .ToArray();
            }
        }

        [JsonProperty("maneuver")]
        public StepManeuver Maneuver { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class RouteResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("waypoints")]
        public Waypoint[] Waypoints { get; set; }

        [JsonProperty("routes")]
        public Route[] Routes { get; set; }
    }

    public class Route
    {
        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("legs")]
        public RouteLeg[] Legs { get; set; }

        [JsonProperty("geometry")]
        public string GeometryStr { get; set; }

        public Location[] Geometry
        {
            get
            {
                if (string.IsNullOrEmpty(GeometryStr))
                {
                    return new Location[0];
                }

                return OsrmPolylineConverter.Decode(GeometryStr, 1E5)
                    .ToArray();
            }
        }

        public static async Task<Route> GetRoute(Location start, Location end)
        {
            string response = await OSRM_API.CallRequest("route", "v1", "foot", new List<Location> { start, end });
            if (response == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<RouteResponse>(response).Routes[0];
        }
    }

    public class StepManeuver
    {
        /// <summary>
        /// The clockwise angle from true north to the direction of travel immediately after the maneuver.
        /// </summary>
        [JsonProperty("bearing_after")]
        protected int BearingAfter { get; set; }

        /// <summary>
        /// The clockwise angle from true north to the direction of travel immediately before the maneuver.
        /// </summary>
        ///
        [JsonProperty("bearing_before")]
        protected int BearingBefore { get; set; }

        /// <summary>
        /// An optional integer indicating number of the exit to take. The field exists for the following type field:
        /// </summary>
        [JsonProperty("exit")]
        protected int Exit { get; set; }

        [JsonProperty("location")]
        protected double[] LocationArr { get; set; }

        public Location Location
        {
            get
            {
                if (LocationArr == null)
                    return null;

                return new Location(LocationArr[0], LocationArr[1]);
            }
        }

        /// <summary>
        /// A string indicating the type of maneuver. new identifiers might be introduced without API change Types unknown to the client should be handled like the turn type, the existance of correct modifier values is guranteed.
        /// </summary>
        [JsonProperty("type")]
        protected string Type { get; set; }

        /// <summary>
        /// An optional string indicating the direction change of the maneuver.
        /// </summary>
        [JsonProperty("modifier")]
        protected string Modifier { get; set; }
    }
}
