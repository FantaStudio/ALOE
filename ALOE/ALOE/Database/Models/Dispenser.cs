using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALOE.Database
{
    class Dispenser
    {
        [Column("dispenserID"), PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [Column("dispenserNUMBER")]
        public int Number { get; set; }

        [Column("dispenserObjectID")]
        public int FuelObjectID { get; set; }

        [Column("dispenserWORKSTATUS")]
        public int WorkStatus { get; set; }
    }
}
