using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace ALOE.Database
{
    class Client
    {
        [Column("clientID"), PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [Column("clientNAME")]
        public string Name { get; set; }
        [Column("clientSURNAME")]
        public string Surname { get; set; }
        [Column("clientPATRONYMIC")]
        public string Middlename { get; set; }
        [Column("clientPHONE")]
        public string Phone { get; set; }
        [Column("clientEMAIL")]
        public string Email { get; set; }
        [Column("clientREGDATE")]
        public DateTime Regdate { get; set; }
        [Column("clientLOGIN")]
        public string Login { get; set; }
        [Column("clientPASSWORD")]
        public string Password { get; set; }
    }
}
