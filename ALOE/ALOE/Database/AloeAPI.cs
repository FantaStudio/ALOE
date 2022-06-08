using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ALOE.Helpers;

namespace ALOE.Database
{
    class AloeResponse
    {
        public enum ResponseTypes
        {
            Error,
            OK,
        };

        public ResponseTypes ResponseType { get; set; }
        public string ResponseDescription { get; set; }
        public JToken ResponseList { get; set; }
    }

    class AloeAPI
    {
        private static readonly string API_URL = "https://oil.lux.pserver.ru/api.php";

        static HttpClient GetAloeClient() => ApiHelper.CreateClient(API_URL);

        public static async Task<string> CallRequest(string method, List<KeyValuePair<string, string>> urlparams = null)
        {
            if (urlparams == null)
            {
                urlparams = new List<KeyValuePair<string, string>>();
            }
            try
            {
                urlparams.Insert(0, new KeyValuePair<string, string>("method", method));
                FormUrlEncodedContent content = new FormUrlEncodedContent(urlparams);

                HttpClient client = GetAloeClient();
                HttpResponseMessage response = await client.PostAsync(API_URL, content);

                string result = response.Content.ReadAsStringAsync().Result;

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<AloeResponse> CallRequestResponse(string method, List<KeyValuePair<string, string>> urlparams = null)
        {
            try
            {
                string result = await CallRequest(method, urlparams);
                if (result == null)
                {
                    return null;
                }

                JObject aloeJObj = JObject.Parse(result);
                // if it hasn't needed format
                if (!aloeJObj.ContainsKey("responseType") || !aloeJObj.ContainsKey("responseDescription"))
                {
                    return null;
                }

                string responseType = (string)aloeJObj["responseType"].ToObject(typeof(string));
                string responseDesc = (string)aloeJObj["responseDescription"].ToObject(typeof(string));

                if (!Enum.TryParse(responseType, out AloeResponse.ResponseTypes restype))
                {
                    return null;
                }

                return new AloeResponse()
                {
                    ResponseType = restype,
                    ResponseDescription = responseDesc,
                    ResponseList = aloeJObj["list"]
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<T> CallEntity<T>(string method, List<KeyValuePair<string, string>> urlparams = null)
        {
            try
            {
                AloeResponse ap = await CallRequestResponse(method, urlparams);
                if (ap == null){
                    return default(T);
                }
                return JsonConvert.DeserializeObject<T>(ap.ResponseList.ToString());
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static async Task<List<ClientCard>> GetClientCards(string login)
        {
            var urlparams = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("login", login) };

            List<ClientCard> clientCards = await CallEntity<List<ClientCard>>("getUserCardsByLogin", urlparams);
            if (clientCards == null && clientCards.Count < 1)
            {
                return null;
            }

            return clientCards;
        }

        public static async Task<List<FuelType>> GetFuelTypes()
        {
            List<FuelType> fuelTypes = await CallEntity<List<FuelType>>("getFuelTypes");
            if (fuelTypes == null || fuelTypes.Count < 1)
            {
                return null;
            }

            return fuelTypes;
        }

        public static async Task<AloeResponse> TryAuth(string login, string password)
        {
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("login",login),
                new KeyValuePair<string, string>("password",password)
            };

            return await CallRequestResponse("auth", paramList);
        }

        public static async Task<AloeResponse> TryRegister(string login, string password)
        {
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("login",login),
                new KeyValuePair<string, string>("password",password)
            };

            return await CallRequestResponse("register", paramList);
        }

        public static async Task<AloeResponse> SaveUser(string login, List<KeyValuePair<string, string>> properties)
        {
            if (properties == null) return null;
            properties.Add(new KeyValuePair<string, string>("login", login));
            return await CallRequestResponse("setUserByLogin", properties);
        }

        public static async Task<Client> GetClient(string login)
        {
            return await CallEntity<Client>("getUserByLogin", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("login", login) });
        }

        public static async Task<List<FuelObject>> GetFuelStations()
        {
            return await CallEntity<List<FuelObject>>("getFuelStations");
        }

        public static async Task<List<Dispenser>> GetFuelDispensers(int fuelObjectID)
        {
            var dispensers = await CallEntity<List<Dispenser>>("getFuelDispensers", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("stationID", fuelObjectID.ToString()) });
            if (dispensers == null || dispensers.Count < 1) return null;
            return dispensers;
        }

        public static async Task<List<Dispenser>> GetFreeFuelDispensers(int fuelObjectID)
        {
            var dispensers = await CallEntity<List<Dispenser>>("getFreeFuelDispensers", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("stationID", fuelObjectID.ToString()) });
            if (dispensers == null || dispensers.Count < 1) return null;
            return dispensers;
        }

    }
}
