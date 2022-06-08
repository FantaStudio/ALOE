using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALOE.Database
{
    public class Fuel
    {
        [Column("fuelID"), PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [Column("fuelCOUNT")]
        public int Count { get; set; }
        [Column("fuelMAXCOUNT")]
        public float MaxCount { get; set; }

        [Column("fuelFuelTypeID"), ForeignKey(typeof(FuelType))]
        public int TypeID { get; set; }

        [Column("fuelObjectID"), ForeignKey(typeof(FuelObject))]
        public int FuelObjectID { get; set; }
    }
}
