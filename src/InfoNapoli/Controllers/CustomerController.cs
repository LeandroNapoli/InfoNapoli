using InfoNapoli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Collections;


namespace InfoNapoli.Controllers
{
    public class CustomerController : Controller
    {
        static string strConexao = ConfigurationManager.ConnectionStrings["conexaoInfoNapoli"].ConnectionString;

        // GET: Customer
        public ActionResult IndexCustomer()
        {
            var customers = GetCustomer();

            return View("IndexCustomer", customers);
        }

        // GET: Customer/Details/5
        public ActionResult DetailsCustomer(int idCustomer)
        {

            using (var conexaoBD = new SqlConnection(strConexao))
            {
                conexaoBD.Open();
                var detailsCustomer = conexaoBD.Query<Customer>("Select * from Customers Where IdCustomer = @idCustomer", new { idCustomer }).SingleOrDefault();
                CustomerEquipments customerEquipments = new CustomerEquipments();
                customerEquipments.Name = detailsCustomer.Name;


                var detailsEquipments = conexaoBD.Query<Equipment>("Select * from Equipment Where IdCustomer = @idCustomer", new { idCustomer });
                customerEquipments.Equipments = new List<Equipment>();
                customerEquipments.Equipments = detailsEquipments.ToList();

                if (customerEquipments.Equipments == default)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View("DetailsCustomer", customerEquipments);
                }

            }

        }

        // GET: Customer/Create
        public ActionResult CreateCustomer()
        {
            return View("CreateCustomer", new Customer());
        }
        // POST: Customer/Create
        [HttpPost]
        public ActionResult CreateCustomer(Customer customer)
        {
            try
            {
                using (var conexaoBD = new SqlConnection(strConexao))
                {
                    conexaoBD.Execute(@"insert into Customers(Name, StatusCustomer, CellPhone) values (@Name, 1, @CellPhone)", customer);
                }

                return RedirectToAction("IndexCustomer");
            }
            catch
            {
                return View("CreateCustomer");
            }
        }

        // GET: Customer/Edit/5
        public ActionResult EditCustomer(int idCustomer)
        {
            using (var conexaoBD = new SqlConnection(strConexao))
            {
                var customer = conexaoBD.Query<Customer>("Select * From Customers Where IdCustomer = @idCustomer", new { idCustomer }).FirstOrDefault();

                return View("EditCustomer", customer);
            }

        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult EditCustomer(int idCustomer, string name)
        {
            try
            {
                using (var conexaoBD = new SqlConnection(strConexao))
                {
                    conexaoBD.Execute("Update Customers Set Name = @Name Where IdCustomer = @idCustomer", new { idCustomer, name });
                }

                return RedirectToAction("IndexCustomer");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Delete/5
        public ActionResult DeleteCustomer(int idCustomer)
        {
            using (var conexaoBD = new SqlConnection(strConexao))
            {
                var customer = conexaoBD.Query<Customer>("Select * from Customers Where IdCustomer = @idCustomer", new { idCustomer }).SingleOrDefault();
                return View("DeleteCustomer", customer);
            }

        }

        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult DeleteCustomer(int idCustomer, Customer customer)
        {
            try
            {
                using (var conexaoBD = new SqlConnection(strConexao))
                {

                    //conexaoBD.Execute("Update Equipment Set StatusEquip = 0 where IdCustomer = @IdCustomer", new { idCustomer });
                    conexaoBD.Execute("Update Customers Set StatusCustomer = 0 where IdCustomer = @IdCustomer", new { idCustomer });
                    

                }

                return RedirectToAction("IndexCustomer");
            }
            catch
            {
                return View("DeleteCustomer");
            }
        }

        private static List<Customer> GetCustomer()
        {
            using (var conexaoBD = new SqlConnection(strConexao))
            {
                conexaoBD.Open();
                var customers = conexaoBD.Query<Customer>(@"Select * from Customers where StatusCustomer = 1");
                return (List<Customer>)customers;
            }
        }

    }
}

