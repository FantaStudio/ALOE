using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace ALOE.Database
{
    public class Transaction
    {
        [Column("transactionID"), PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Column("transactionCardID"), ForeignKey(typeof(ClientCard))]
        public int? CardID { get; set; }

        [Column("transactionClientID"), ForeignKey(typeof(Client))]
        public int ClientID { get; set; }

        [Column("transactionFuelObjectID"), ForeignKey(typeof(FuelObject))]
        public int FuelObjectID { get; set; }

        [Column("transactionFuelType")]
        public string FuelType { get; set; }

        [Column("transactionDispenserNumber")]
        public int DispenserNumber { get; set; }

        [Column("transactionLitresCount")]
        public int LitresCount { get; set; }

        [Column("transactionCost")]
        public float Cost { get; set; }

        [Column("transactionDATE")]
        public DateTime Date { get; set; }
        [Column("transactionTYPE")]
        public string Type { get; set; }

        [Column("transactionCOUNTBONUSAdded")]
        public int AddedBonusCount { get; set; }

        [Column("transactionCOUNTBONUSSub")]
        public int SubBonusCount { get; set; }

    }
}
