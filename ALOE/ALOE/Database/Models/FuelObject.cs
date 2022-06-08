using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALOE.Database
{
    class FuelObject
    {
        [Column("objectID"), PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [Column("objectWORKSTATUS")]
        public int Workstatus { get; set; }
        [Column("objectADDRESS")]
        public string Address { get; set; }
        [Column("objectREQUISITES")]
        public string Requisites { get; set; }
        [Column("objectISSMALL")]
        public int IsSmall { get; set; }
        [Column("objectDESCRIPTION")]
        public string Description { get; set; }
        [Column("objectLATITUDE")]
        public double Latitude { get; set; }
        [Column("objectLONGITUDE")]
        public double Longitude { get; set; }
    }
}
