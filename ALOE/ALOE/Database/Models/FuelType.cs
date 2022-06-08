using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALOE.Database
{
    class FuelType
    {
        [Column("fuelTypeID"), PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [Column("fuelNAME")]
        public string Name { get; set; }
        [Column("fuelCOST")]
        public float Cost { get; set; }
        [Column("fuelDESCRIPTION")]
        public string Description { get; set; }
    }
}
