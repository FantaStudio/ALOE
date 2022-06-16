using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ALOE.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using ALOE.Database;
using System.Reflection;
using Xamarin.Forms.GoogleMaps;
using ALOE.OSRM;
using System.Timers;

namespace ALOE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class MapPage : ContentPage
    {
        FuelObject SelectedFuelObject
        {
            get
            {
                Pin fuelStationPin = MainMap.SelectedPin;
                if (fuelStationPin == null) return null;
                if (fuelStationPin.Tag is FuelObject)
                    return fuelStationPin.Tag as FuelObject;
                else
                    return null;
            }
        }
        Polyline RoutePath { get; set; }

        public MapPage()
        {
            InitializeComponent();
            ApplyMapTheme();

            AddFuelStationsOnMap();

            MainMap.PinClicked += MainMap_PinClicked;
            MainMap.SelectedPinChanged += MainMap_SelectedPinChanged;

            MoveToMyPosition();
        }

        private void ApplyMapTheme()
        {
            var assembly = typeof(MapPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"ALOE.UI.mapTheme.json");
            string themeFile;
            using (var reader = new System.IO.StreamReader(stream))
            {
                themeFile = reader.ReadToEnd();
                MainMap.MapStyle = MapStyle.FromJson(themeFile);
            }
        }

        private void StartLoader()
        {
            Loader.IsVisible = true;
            Loader.IsRunning = true;
        }

        private void StopLoader()
        {
            Loader.IsVisible = false;
            Loader.IsRunning = false;
        }

        private async void MoveToMyPosition()
        {
            MainMap.MoveToLocation(new Location(46.347609, 48.030168), Distance.FromKilometers(7));

            Location myLocation = await LocationHelper.GetLocation();
            if (myLocation == null)
            {
                return;
            }

            MainMap.MoveToLocation(myLocation);
        }

        private async void AddFuelStationsOnMap()
        {
            StartLoader();
            try
            {
                List<FuelObject> fuelStations = await AloeDB.GetFuelStations();
                if (fuelStations != null && fuelStations.Count > 0)
                {
                    foreach (var station in fuelStations)
                    {
                        Pin fuelPin = new Pin()
                        {
                            Label = "АЗС \"ALOE\"",
                            Address = station.Address,
                            Position = new Position(station.Latitude, station.Longitude),
                            Icon = BitmapDescriptorFactory.FromBundle("fuel_station.png"),
                            Tag = station
                        };
                        MainMap.Pins.Add(fuelPin);
                    }
                }
            }
            catch (Exception ex)
            {
                AloeSnackbar.Show(ex.Message);
            }

            StopLoader();
        }

        private void MainMap_SelectedPinChanged(object sender, SelectedPinChangedEventArgs e)
        {
            if (e.SelectedPin == null)
                MainMap.FuelInfoIsShowed = false;
        }

        private void MainMap_PinClicked(object sender, PinClickedEventArgs e)
        {
            if (e.Pin == null) return;
            if (e.Pin.Tag is FuelObject)
            {
                FuelObject fobj = e.Pin.Tag as FuelObject;
                FuelObjectAddressLabel.Text = fobj.Address;
                FuelObjectTypeLabel.Text = "Тип АЗС: " + (fobj.IsSmall == 1 ? "Маленькая" : "Большая");
                MainMap.FuelInfoIsShowed = true;
            }
        }

        private void ClearPath() 
        {
            if (RoutePath != null)
                MainMap.Polylines.Remove(RoutePath);
        }

        private void CreatePath(Location[] points)
        {
            if (points == null || points.Length < 2) return;

            //Clear
            ClearPath();

            //Fill
            Polyline poly = new Polyline();
            poly.StrokeColor = (Color)Application.Current.Resources["VioletLight"];

            foreach (var pos in points)
            {
                poly.Positions.Add(Main.LocationToPosition(pos));
            }

            MainMap.Polylines.Add(poly);
            RoutePath = poly;

            // Move to start path point
            MainMap.MoveToPosition(poly.Positions.FirstOrDefault()); 

            //int positionIndex = 1;
            //Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            //{
            //    if (points.Length > positionIndex)
            //    {
            //        UpdatePosition(points[positionIndex]);
            //        positionIndex++;
            //        return true;
            //    }
            //    else
            //        return false;
            //});
        }

        private void UpdatePosition(Location pos)
        {
            MainMap.MoveToPosition(Main.LocationToPosition(pos), Distance.FromMeters(200));
            var prevPos = MainMap.Polylines?.FirstOrDefault()?.Positions?.FirstOrDefault();
            MainMap.Polylines?.FirstOrDefault()?.Positions?.Remove(prevPos.Value);
        }

        private async void CreateRouteToFuelStation_Clicked(object sender, EventArgs e)
        {
            if (AntispamHelper.IsBlocked("create_route")) return;
            AntispamHelper.AddAntispam("create_route");

            try
            {
                var fobj = SelectedFuelObject;
                if (fobj == null)
                {
                    MainMap.FuelInfoIsShowed = false;
                    return;
                }

                StartLoader();
                Location myLocation = await LocationHelper.GetLocation();
                if (myLocation == null)
                {
                    throw new Exception("Не удалось найти ваше местоположение, проверьте включена ли у вас геолокация");
                }

                Route rout = await Route.GetRoute(myLocation, new Location(SelectedFuelObject.Latitude, SelectedFuelObject.Longitude)).TimeoutAfter(TimeSpan.FromSeconds(25));
                if (rout == null)
                {
                    throw new Exception("Не удалось найти подходящий маршрут");
                }
                StopLoader();
                CreatePath(rout.Geometry);
            }
            catch (Exception ex)
            {
                StopLoader();
                AloeSnackbar.Show(ex.Message);
            }
        }

        [Obsolete]
        private async void RefillStart_Clicked(object sender, EventArgs e)
        {
            if (SelectedFuelObject == null)
            {
                MainMap.FuelInfoIsShowed = false;
                return;
            }

            if (AntispamHelper.IsBlocked("refill_start")) return;
            AntispamHelper.AddAntispam("refill_start");

            try
            {
                // Dispensers
                StartLoader();
                var dispensers = await AloeDB.GetFreeFuelDispensers(SelectedFuelObject.ID);
                if (dispensers == null || dispensers.Count < 1) throw new Exception("Все колонки заняты или не работают");
                StopLoader();

                string[] dispensersMassive = new string[dispensers.Count];
                Dictionary<string, int> dispenserNumberToIndex = new Dictionary<string, int>();
                for (int i = 0; i < dispensersMassive.Length; i++)
                {
                    var number = dispensers[i].Number.ToString();
                    dispensersMassive[i] = number.ToString();
                    if (!dispenserNumberToIndex.ContainsKey(number))
                    {
                        dispenserNumberToIndex.Add(number, i);
                    }
                    else
                    {
                        dispenserNumberToIndex[number] = i;
                    }
                }

                string dispenserTextNumber = await DisplayActionSheet("Выберите колонку", "Отмена", null, dispensersMassive);
                if (dispenserTextNumber == "Отмена" || dispenserTextNumber == null) return;

                //Fuel type
                StartLoader();
                var fuelTypes = await AloeDB.GetFuelTypes();
                if (fuelTypes == null || fuelTypes.Count < 1) throw new Exception("Невозможно подключиться к серверу");
                StopLoader();

                string[] fueltypesMassive = new string[fuelTypes.Count];
                Dictionary<string, int> fueltTypeToIndex = new Dictionary<string, int>();
                for (int i = 0; i < fueltypesMassive.Length; i++)
                {
                    fueltypesMassive[i] = fuelTypes[i].Name;
                    if (!fueltTypeToIndex.ContainsKey(fuelTypes[i].Name))
                    {
                        fueltTypeToIndex.Add(fuelTypes[i].Name, i);
                    }
                    else
                    {
                        fueltTypeToIndex[fuelTypes[i].Name] = i;
                    }
                }

                string fuelTypeText = await DisplayActionSheet("Выберите тип бензина", "Отмена", null, fueltypesMassive);
                if (fuelTypeText == "Отмена" || fuelTypeText == null) return;

                //Fuel count
                string fuelLitres = null;
                int litres;
                while (!int.TryParse(fuelLitres, out litres) && fuelLitres != "Отмена")
                {
                    fuelLitres = await DisplayPromptAsync("Сколько заправить?", "Введите количество литров", "ОК", "Отмена", "литры", -1, Keyboard.Numeric, "1");
                }

                Dispenser currentDispenser = dispensers[dispenserNumberToIndex[dispenserTextNumber]];
                FuelType currentFuelType = fuelTypes[fueltTypeToIndex[fuelTypeText]];

                var fullPrice = litres * currentFuelType.Cost;
                string payAmount = $"{fullPrice} {Main.Currency}";
                string billText = $"Номер заправки: {SelectedFuelObject.ID}\n" +
                                  $"Адрес заправки: {SelectedFuelObject.Address}\n" +
                                  $"Номер колонки: {currentDispenser.Number}\n" +
                                  $"Тип бензина: {currentFuelType.Name}\n" +
                                  $"Кол-во литров: {litres}\n" +
                                  $"Стоимость: {payAmount}";

                bool payAccept = await DisplayAlert("Счёт", billText, "Оплатить", "Отмена");
                if (!payAccept) return;

                bool buyResult = false;
                var cards = await AloeDB.GetClientCards(Main.CURRENT_USER_LOGIN);
                if (cards != null && cards.Count > 0)
                {
                    var userBonusCard = cards.Last();
                    var bonusCanUse = userBonusCard.Bonus;
                    var maxBonusAmountForCost = (int)fullPrice * Main.MaxUseBonusPercent / 100;
                    if(bonusCanUse > maxBonusAmountForCost)
                    {
                        bonusCanUse = maxBonusAmountForCost;
                    }

                    bool useBonus = await DisplayAlert("Использовать баллы?",
                        $"У вас {userBonusCard.Bonus} баллов\n" +
                        $"Спишется: {bonusCanUse}\n" +
                        $"Итоговая цена: {fullPrice - bonusCanUse}\n\n" +
                        $"Помните, что баллами можно оплатить только {Main.MaxUseBonusPercent}% покупки",
                        "Да",
                        "Нет");

                    if (!useBonus)
                    {
                        bonusCanUse = 0;
                    }

                    fullPrice -= bonusCanUse;

                    buyResult = await AloeDB.ClientBuyFuel(Main.CURRENT_USER_LOGIN, SelectedFuelObject.ID, currentDispenser.Number, currentFuelType.Name, litres, fullPrice, bonusCanUse);
                }
                else
                {
                    buyResult = await AloeDB.ClientBuyFuel(Main.CURRENT_USER_LOGIN, SelectedFuelObject.ID, currentDispenser.Number, currentFuelType.Name, litres, fullPrice);
                }

                if (buyResult)
                {
                    StartLoader();
                    await Task.Delay(2000);
                    StopLoader();
                    await DisplayAlert("Круто!", "Вы успешно оплатили заправку, подъезжайте к колонке, чтобы заправиться", "Хорошо");
                }
                else
                {
                    throw new Exception("Все колонки заняты или не работают");
                }
            }
            catch (Exception exception)
            {
                await DisplayAlert("Ошибка", exception.Message, "OK");
                StopLoader();
            }
        }

    }
}