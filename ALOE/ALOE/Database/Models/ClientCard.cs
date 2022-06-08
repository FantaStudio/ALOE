using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace ALOE.Database
{
    class ClientCard
    {
        [Column("cardID"), PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Column("cardClientID"), ForeignKey(typeof(Client))]
        public int ClientID { get; set; }

        [Column("cardBONUSCOUNT")]
        public int Bonus { get; set; }
    }

}
