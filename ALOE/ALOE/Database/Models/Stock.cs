using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALOE.Database
{
    public class Stock
    {
        [Column("StockD"), PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Column("StockTitle")]
        public string Title { get; set; }

        [Column("StockDescription")]
        public string Description { get; set; }

        [Column("StockImagePath")]
        public string ImagePath { get; set; }

        [Column("StockStartDate")]
        public DateTime StartDate { get; set; }

        [Column("StockEndDate")]
        public DateTime EndDate { get; set; }

    }
}
