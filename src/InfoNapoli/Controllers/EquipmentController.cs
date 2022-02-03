using InfoNapoli.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using System.Web.Mvc;

namespace InfoNapoli.Controllers
{
    public class EquipmentController : Controller
    {
        static string strConexao = ConfigurationManager.ConnectionStrings["conexaoInfoNapoli"].ConnectionString;

        // GET: Equipment
        public ActionResult Index()
        {
            return View();
        }

        // GET: Equipment/DetailsEquipment/5
        public ActionResult DetailsEquipment(int idEquipment)
        {
            try
            {
                using (var conexaoBD = new SqlConnection(strConexao))
                {
                    var detailsEquipment = conexaoBD.Query<Equipment>("Select * From Equipment where IdEquipment = @idEquipment", new { idEquipment }).FirstOrDefault();

                    return View(detailsEquipment);
                }
            }
            catch
            {
                return View();
            }


        }

        // GET: Equipment/Create
        public ActionResult CreateEquipment(int idCustomer)
        {
            using (var conexaoBd = new SqlConnection(strConexao))
            {
                Equipment equipment = new Equipment();
                equipment.IdCustomer = idCustomer;
                equipment.EntryDate = DateTime.Now;

                var nameCustomer = conexaoBd.Query<Customer>("Select Name From Customers Where IdCustomer = @idCustomer", new { idCustomer }).FirstOrDefault();

                ViewData["NameCustomer"] = nameCustomer.Name;

                return View(equipment);
            }



        }

        // POST: Equipment/Create
        [HttpPost]
        public ActionResult CreateEquipment(Equipment equipment)
        {
            try
            {
                using (var conexaoBD = new SqlConnection(strConexao))
                {
                    equipment.EntryDate = DateTime.Now;
                    conexaoBD.Execute("insert into Equipment(Model, Defect, IdCustomer, Commentary, EntryDate, StatusEquip) values (@Model, @Defect, @IdCustomer, @Commentary, @EntryDate, 2)", equipment);
                }

                return RedirectToAction("IndexCustomer", "Customer");
            }
            catch
            {
                return View();
            }
        }

        // GET: Equipment/Edit/5
        public ActionResult EditEquipment(int idEquipment)
        {
            using (var conexaoBD = new SqlConnection(strConexao))
            {
                var equipment = conexaoBD.Query<Equipment>("Select * From Equipment Where IdEquipment = @idEquipment", new { idEquipment }).FirstOrDefault();
                var customer = conexaoBD.Query<Customer>("Select Name From Customers Where IdCustomer = @idCustomer", new { equipment.IdCustomer }).FirstOrDefault();

                ViewData["NameCustomer"] = customer.Name; //Traz o nome do Customer na tela
                TempData["NameCustomer2"] = customer.Name; //Usado para manter o dado até a próxima requisição Linha 83 para manter .Keep()
                TempData.Keep("NameCustomer2");

                return View("EditEquipment", equipment);
            }
        }

        // POST: Equipment/Edit/5
        [HttpPost]
        public ActionResult EditEquipment(Equipment equipment)
        {
            try
            {
                if (equipment.EntryDate == null)
                {
                    equipment.EntryDate = DateTime.Now;
                }
                using (var conexaoBD = new SqlConnection(strConexao))
                {
                    conexaoBD.Execute("Update Equipment Set Model = @Model, Defect = @Defect, Commentary = @Commentary Where IdEquipment = @idEquipment", new { equipment.Model, equipment.Defect, equipment.Commentary, equipment.IdEquipment });
                }
                var teste = TempData["NameCustomer2"]; // Exemplo de dado mantido

                return RedirectToAction("DetailsCustomer", "Customer", new { idCustomer = equipment.IdCustomer });
            }
            catch
            {
                return View();
            }

        }


        // GET: Equipment/Cancel/5
        public ActionResult CancelEquipment(int idEquipment)
        {
            using (var conexaoBD = new SqlConnection(strConexao))
            {
                var equipment = conexaoBD.Query<Equipment>("Select * from Equipment where IdEquipment = @idEquipment", new { idEquipment }).First();
                return View("CancelEquipment", equipment);
            }

        }



        // POST: Equipment/Delete/5
        [HttpPost]
        public ActionResult CancelEquipment(Equipment equipment)
        {
            try
            {
                using (var conexaoBD = new SqlConnection(strConexao))
                {
                    var consult = conexaoBD.Query<Equipment>("Select * from Equipment where IdEquipment = @idEquipment", new { equipment.IdEquipment }).FirstOrDefault();
                    conexaoBD.Execute("Update Equipment Set StatusEquip = 1 where IdEquipment = @IdEquipment", new { equipment.IdEquipment });
                    return RedirectToAction("DetailsCustomer", "Customer", new { idCustomer = consult.IdCustomer });
                }

            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeliveryEquipment(int idEquipment)
        {
            try
            {
                using (var conexaoBD = new SqlConnection(strConexao))
                {
                    var departureDate = DateTime.Now;
                    var statusEquip = 3;
                    var equipment = conexaoBD.Query<Equipment>("Select * from Equipment where IdEquipment = @idEquipment", new { idEquipment }).FirstOrDefault();

                    conexaoBD.Execute("Update Equipment Set StatusEquip = @statusEquip, DepartureDate = @departureDate where IdEquipment = @idEquipment", new { statusEquip, departureDate, idEquipment });

                    return RedirectToAction("DetailsCustomer", "Customer", new { idCustomer = equipment.IdCustomer });
                }

            }
            catch
            {
                return View();
            }


        }
    }
}
