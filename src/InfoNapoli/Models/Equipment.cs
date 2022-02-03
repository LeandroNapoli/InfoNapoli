using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InfoNapoli.Models
{
    public class Equipment
    {
        public int IdEquipment { get; set; }
        public string Model { get; set; }
        public string Defect { get; set; }
        public int IdCustomer { get; set; }
        public string Commentary { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public int StatusEquip { get; set; }
        public string NameStatus { get; set; }

    }
}