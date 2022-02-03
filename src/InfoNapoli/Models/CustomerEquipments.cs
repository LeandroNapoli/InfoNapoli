using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfoNapoli.Models
{
    public class CustomerEquipments
    {
        public string Name { get; set; }
        public List<Equipment> Equipments { get; set; }
    }
}