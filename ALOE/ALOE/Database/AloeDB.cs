using ALOE.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ALOE.Database
{
    static class AloeDB
    {
        private const string databaseName = "db.db";
        private static SQLiteAsyncConnection _connection;
        static bool firstUse = false;

        public static SQLiteAsyncConnection Connection
        {
            get
            {
                if(_connection == null)
                {
                    _connection = new SQLiteAsyncConnection(Path.Combine(
                                                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), databaseName));
                }
                return _connection;
            }
        }

        public static async void CreateTables()
        {
            await Connection.CreateTableAsync<FuelObject>();
            await Connection.CreateTableAsync<FuelType>();
            await Connection.CreateTableAsync<Client>();
            await Connection.CreateTableAsync<ClientCard>();
            await Connection.CreateTableAsync<Dispenser>();
            await Connection.CreateTableAsync<Fuel>();
            await Connection.CreateTableAsync<Transaction>();
            await Connection.CreateTableAsync<News>();
            await Connection.CreateTableAsync<Stock>();

            AddTestData();
        }

        private static async void AddTestData()
        {
            if (!firstUse)
            {
                if (await _connection.Table<Client>().CountAsync() < 1)
                {
                    await _connection.InsertAsync(new Client() { Login = "test", Password = "1234" });
                }

                if (await _connection.Table<ClientCard>().CountAsync() < 1)
                {
                    await _connection.InsertAsync(new ClientCard() { ClientID = 1, Bonus = 40 });
                }

                if (await _connection.Table<FuelType>().CountAsync() < 1)
                {
                    await _connection.InsertAsync(new FuelType() { Name = "92", Cost = 47.6F });
                    await _connection.InsertAsync(new FuelType() { Name = "95", Cost = 51.8F });
                    await _connection.InsertAsync(new FuelType() { Name = "100", Cost = 57F });
                    await _connection.InsertAsync(new FuelType() { Name = "ДТ", Cost = 51.7F });
                }

                if (await _connection.Table<FuelObject>().CountAsync() < 1)
                {
                    await _connection.InsertAsync(new FuelObject() { Address = "Боевая ул., 21А", Description = "", IsSmall = 1, Latitude = 46.339535, Longitude = 48.021734 });
                    await _connection.InsertAsync(new FuelObject() { Address = "ул. Ахшарумова, 90Б", Description = "", IsSmall = 1, Latitude = 46.337857, Longitude = 48.041549 });
                    await _connection.InsertAsync(new FuelObject() { Address = "улица Анри Барбюса, 27В", Description = "", IsSmall = 0, Latitude = 46.361490, Longitude = 48.055730 });
                    await _connection.InsertAsync(new FuelObject() { Address = "Ереванская ул., 2", Description = "", IsSmall = 0, Latitude = 46.372458, Longitude = 48.082931 });
                    await _connection.InsertAsync(new FuelObject() { Address = "Новороссийская ул., 4", Description = "", IsSmall = 0, Latitude = 46.383416, Longitude = 48.092875 });
                    await _connection.InsertAsync(new FuelObject() { Address = "ул. Покровская Роща, 18", Description = "", IsSmall = 0, Latitude = 46.350109, Longitude = 48.093067 });
                    await _connection.InsertAsync(new FuelObject() { Address = "ул. Латышева, 5Л", Description = "", IsSmall = 1, Latitude = 46.379616, Longitude = 48.057376 });
                }

                if (await _connection.Table<Dispenser>().CountAsync() < 1)
                {
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 1, Number = 1, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 1, Number = 2, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 1, Number = 3, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 1, Number = 4, WorkStatus = 0 });

                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 2, Number = 1, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 2, Number = 2, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 2, Number = 3, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 2, Number = 4, WorkStatus = 0 });

                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 3, Number = 1, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 3, Number = 2, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 3, Number = 3, WorkStatus = 0 });

                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 4, Number = 1, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 4, Number = 2, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 4, Number = 3, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 4, Number = 4, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 4, Number = 5, WorkStatus = 0 });

                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 5, Number = 1, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 5, Number = 2, WorkStatus = 0 });

                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 6, Number = 1, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 6, Number = 2, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 6, Number = 3, WorkStatus = 0 });

                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 7, Number = 1, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 7, Number = 2, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 7, Number = 3, WorkStatus = 0 });
                    await _connection.InsertAsync(new Dispenser() { FuelObjectID = 7, Number = 4, WorkStatus = 0 });
                }

                if (await _connection.Table<Fuel>().CountAsync() < 1)
                {
                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 1, TypeID = 1, Count = 50000, MaxCount = 100000 });
                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 1, TypeID = 2, Count = 50000, MaxCount = 100000 });
                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 1, TypeID = 3, Count = 50000, MaxCount = 100000 });

                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 2, TypeID = 1, Count = 50000, MaxCount = 100000 });
                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 2, TypeID = 2, Count = 50000, MaxCount = 100000 });
                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 2, TypeID = 3, Count = 50000, MaxCount = 100000 });

                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 3, TypeID = 1, Count = 50000, MaxCount = 100000 });
                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 3, TypeID = 2, Count = 50000, MaxCount = 100000 });
                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 3, TypeID = 3, Count = 50000, MaxCount = 100000 });

                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 4, TypeID = 1, Count = 50000, MaxCount = 100000 });
                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 4, TypeID = 2, Count = 50000, MaxCount = 100000 });
                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 4, TypeID = 3, Count = 50000, MaxCount = 100000 });

                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 5, TypeID = 1, Count = 50000, MaxCount = 100000 });
                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 5, TypeID = 2, Count = 50000, MaxCount = 100000 });
                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 5, TypeID = 3, Count = 50000, MaxCount = 100000 });

                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 6, TypeID = 1, Count = 50000, MaxCount = 100000 });
                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 6, TypeID = 2, Count = 50000, MaxCount = 100000 });
                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 6, TypeID = 3, Count = 50000, MaxCount = 100000 });

                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 7, TypeID = 1, Count = 50000, MaxCount = 100000 });
                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 7, TypeID = 2, Count = 50000, MaxCount = 100000 });
                    await _connection.InsertAsync(new Fuel() { FuelObjectID = 7, TypeID = 3, Count = 50000, MaxCount = 100000 });
                }

                if (await _connection.Table<News>().CountAsync() < 1)
                {
                    await _connection.InsertAsync(new News
                    {
                        Title = "ALOE покупает у Shell 411 АЗС и завод в Торжке",
                        Description = "Shell Overseas Investments B.V. и B.V. Dordtsche Petroleum Maatschappij (дочки Shell plc) подписали соглашение о продаже Шелл нефть, которая владеет розничным и смазочным бизнесом Shell в России, компании ALOE.",
                        PublicationDate = DateTime.ParseExact("12.05.2022", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        ImagePath = "https://neftegaz.ru/upload/resize_cache/webp/iblock/e50/ahue2udrekjhb4g07xrf7lwsttfptbif/podpisanie.webp"
                    });

                    await _connection.InsertAsync(new News
                    {
                        Title = "Сеть АЗС ALOE расширила географию своих станций до 47 регионов России",
                        Description = "В 2022 г. сеть ALOE увеличила количество станций более чем на 4% - до 1432 станций - и расширила географию деятельности в России до 47 регионов.",
                        PublicationDate = DateTime.ParseExact("18.03.2022", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        ImagePath = "https://neftegaz.ru/upload/iblock/6c0/poq0wkh4j5gk642e4yb3w42n13mo3rk5/Set-AZS-_Gazpromneft_.jpg"
                    });

                    await _connection.InsertAsync(new News
                    {
                        Title = "АЗС сети ALOE переходят на реализацию зимнего дизельного топлива",
                        Description = "Когда ночная температура опускается ниже нуля, владельцам дизельных автомобилей следует обратить особое внимание на топливо. Оно должно соответствовать текущим погодным условиям.",
                        PublicationDate = DateTime.ParseExact("05.01.2022", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        ImagePath = "https://neftegaz.ru/upload/resize_cache/webp/upload/iblock/ada/8psfn15580kkro1uqkmabz10848t2r2g/AZS-seti-_Gazpromneft_-perekhodyat-na-realizatsiyu-zimnego-dizelnogo-topliva.webp"
                    });

                    await _connection.InsertAsync(new News
                    {
                        Title = "Газпром не ждет резкого скачка цен на газомоторное топливо на своих АЗС",
                        Description = "Рентабельная цена - 19-20 руб. Задирать эту цену в компании не хотели бы, сообщил предправления.",
                        PublicationDate = DateTime.ParseExact("25.09.2021", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        ImagePath = "https://neftegaz.ru/upload/iblock/ddd/kzv1w4f4weg3ow9ih92jm0ng3ubl8jxa/dengi.png"
                    });
                }

                if (await _connection.Table<Stock>().CountAsync() < 1)
                {

                    await _connection.InsertAsync(new Stock
                    {
                        Title = "ТУРБО АЗС",
                        Description = "Получайте двойные баллы в программе лояльности",
                        StartDate = DateTime.ParseExact("01.05.2022", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact("30.06.2022", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        ImagePath = "https://lukoil.ru/FileSystem/8/10611.png"
                    });

                    await _connection.InsertAsync(new Stock
                    {
                        Title = "ВЫГОДНЫЕ ПРЕДЛОЖЕНИЯ",
                        Description = "К празднику 9 мая мы подготовили для вас сувенирные новинки:\n" +
                        "Автомобильный флаг \"С Днем Победы!\"\n" +
                        "Автомобильный флаг \"Россия\"\n" +
                        "Подвеска на зеркало с георгиевской лентой\n" +
                        "Значок \"Звезда красная с серпом и молотом\"\n" +
                        "Значок \"9 мая\" с гвоздикой\n" +
                        "Солдатская пилотка",
                        StartDate = DateTime.ParseExact("01.05.2022", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact("31.05.2022", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        ImagePath = "https://lukoil.ru/FileSystem/8/10627.png"
                    });

                    await _connection.InsertAsync(new Stock
                    {
                        Title = "ВЫГОДНЫЙ КУРС",
                        Description = "Выгодный курс на топливо: скидка до 2 рублей за каждый литр! Покупайте АИ-92, предъявляйте карту программы лояльности и получите скидку:\n" +
                       "1 руб.с каждого литра за заправку на сумму от 1000 руб.\n" +
                       "2 руб.с каждого литра за заправку на сумму от 1500 руб.\n" +
                       "Если покупаете АИ - 95 или ДТ с картой лояльности – ваша скидка:\n" +
                       "1 руб.с каждого литра за заправку на сумму от 1000 руб.\n" +
                       "2 руб.с каждого литра за заправку на сумму от 2000 руб.",
                        StartDate = DateTime.ParseExact("29.04.2022", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact("31.05.2022", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        ImagePath = "https://auto.lukoil.ru/FileSystem/8/10590.png"
                    });


                    await _connection.InsertAsync(new Stock
                    {
                        Title = "ALOE-ТИНЬКОФФ",
                        Description = "Введите промокод при оформлении карты и получите баллы для оплаты покупок топлива и товаров на АЗС «ЛУКОЙЛ»:\n" +
                        "для кредитной карты – BONUS1000 – 1000 баллов;\n" +
                        "для дебетовой карты – BONUS500 – 500 баллов.",
                        StartDate = DateTime.ParseExact("12.04.2022", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact("31.07.2022", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        ImagePath = "https://obetomnegovoryat.ru/wp-content/uploads/2020/11/7-2.jpg"
                    });
                }

                firstUse = true;
            }
        }

        public static async Task<bool> TryAuth(string login, string password)
        {
            return await Connection.Table<Client>().Where(x => x.Login == login && x.Password == password).FirstOrDefaultAsync() != null;
        }

        public static async Task<bool?> TryRegister(string login, string password)
        {
            if (await Connection.Table<Client>().Where(x => x.Login == login).CountAsync() > 0)
            {
                return null;
            }

            var result = await Connection.InsertAsync(new Client() { Login = login, Password = password });
            var client = await Connection.Table<Client>().Where(x => x.Login == login).FirstOrDefaultAsync();
            await Connection.InsertAsync(new ClientCard { ClientID = client.ID, Bonus = 50  });
            return result > 0;
        }

        public static async Task<List<FuelType>> GetFuelTypes()
        {
            return await Connection.Table<FuelType>().ToListAsync();
        }

        public static async Task<List<ClientCard>> GetClientCards(string login)
        {
            var client = await GetClient(login);
            if (client is null)
            {
                return null;
            }
            return await Connection.Table<ClientCard>()?.Where(x => x.ClientID == client.ID)?.ToListAsync();
        }

        public static async Task<List<Transaction>> GetClientTransactions(string login)
        {
            var client = await GetClient(login);
            if (client is null) return null;
            return await Connection.Table<Transaction>()?.Where(x => x.ClientID == client.ID)?.OrderByDescending(x => x.Date).ToListAsync();
        }

        public static async Task<Client> GetClient(string login)
        {
            return await Connection.Table<Client>().Where(x => x.Login == login).FirstOrDefaultAsync();
        }

        public static async Task<bool> SaveUser(Client client)
        {
            return await Connection.UpdateAsync(client) > 0;
        }

        public static async Task<List<FuelObject>> GetFuelStations()
        {
            return await Connection.Table<FuelObject>()?.ToListAsync();
        }

        public static async Task<List<Dispenser>> GetFreeFuelDispensers(int fuelObjectID)
        {
            return await Connection.Table<Dispenser>().Where(x => x.FuelObjectID == fuelObjectID)?.ToListAsync();
        }

        public static async Task<List<News>> GetNews()
        {
            return await Connection.Table<News>().ToListAsync();
        }

        public static async Task<List<Stock>> GetStocks()
        {
            return await Connection.Table<Stock>().ToListAsync();
        }

        public static async Task<bool> ClientBuyFuel(string login, int fuelObjectID, int dispenserNumber, string fuelType,int fuelCount, float cost, int bonusSub = 0)
        {
            var client = await GetClient(login);
            var cards = await GetClientCards(login);
            try
            {
                var card = cards?.LastOrDefault();
                var bonus = (int)cost / Main.BonusEqual;
                var transaction = new Transaction()
                {
                    ClientID = client.ID,
                    CardID = card?.ID,
                    AddedBonusCount = bonus,
                    SubBonusCount = bonusSub,
                    Cost = cost,
                    DispenserNumber = dispenserNumber,
                    FuelObjectID = fuelObjectID,
                    FuelType = fuelType,
                    LitresCount = fuelCount,
                    Type = "Покупка топлива",
                    Date = DateTime.Now
                };
                var res = await Connection.InsertAsync(transaction);
                if(card != null)
                {
                    if (bonusSub > 0)
                    {
                        card.Bonus -= bonusSub;
                    }
                    card.Bonus += bonus;
                    await Connection.UpdateAsync(card);
                }
                return res > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
