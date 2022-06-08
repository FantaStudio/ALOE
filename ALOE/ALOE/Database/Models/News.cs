using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALOE.Database
{
    public class News
    {

        [Column("newsID"), PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Column("newsTitle")]
        public string Title { get; set; }

        [Column("newsDescription")]
        public string Description { get; set; }

        [Column("newsImagePath")]
        public string ImagePath { get; set; }

        [Column("newsDate")]
        public DateTime PublicationDate { get; set; }
    }
}
