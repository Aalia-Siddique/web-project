using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Authorization;

//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using WebApplication1.Models;

namespace Web_Project__MVC_.Controllers
{

    public class OrderController : Controller
    {
        private readonly IRepository<Orders> _orderRepository;
        public OrderController(

            IRepository<Orders> orderRepository)

        {

            _orderRepository = orderRepository;
            // _prodRepository = prodRepository;
        }
        public ActionResult ViewOrder()
        {
            //string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";
            //IRepository<Orders> rep = new GenericRepository<Orders>(conn);
            List<Orders> order = _orderRepository.GetAll().ToList();
            return View(order);
            //return View();
        }
        // GET: Order

       // [Authorize(Policy = "BusinessHoursOnly")]
        //public IActionResult AddOrder()
        //{
        //    return View();
        //}
        //public IActionResult Index()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult viewCustomerProducts(string customerName)
        //{
        //    List<Product> products = new List<Product>();
        //    Orders c = new Orders { Name = customerName };
        //    string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True;";

        //    string query = "SELECT p.ProductId, p.Name, p.Price, p.Description, p.Quantity," +
        //        " p.ImagePath " +
        //                   "FROM Product p " +
        //                   "INNER JOIN [Order] o ON p.Name = o.ProductName " +
        //                   "WHERE o.Name = @CustomerName";

        //    using (SqlConnection con = new SqlConnection(connString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            // Use parameterized queries to prevent SQL injection
        //            cmd.Parameters.AddWithValue("@CustomerName", customerName);

        //            con.Open();
        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    Product p = new Product();
        //                    if (!dr.IsDBNull(0))
        //                    {
        //                        p.ProductId = dr.GetInt32(0);
        //                    }
        //                    if (!dr.IsDBNull(1))
        //                    {
        //                        p.Name = dr.GetString(1);
        //                    }
        //                    if (!dr.IsDBNull(2))
        //                    {
        //                        p.Price = dr.GetString(2); // Assuming Price is of type decimal
        //                    }
        //                    if (!dr.IsDBNull(3))
        //                    {
        //                        p.Description = dr.GetString(3);
        //                    }
        //                    if (!dr.IsDBNull(4))
        //                    {
        //                        p.Quantity = dr.GetString(4); // Assuming Quantity is of type int
        //                    }
        //                    if (!dr.IsDBNull(5))
        //                    {
        //                        p.ImagePath = dr.GetString(5);
        //                    }
        //                    products.Add(p);
        //                }
        //            }
        //        }
        //    }

        //    OrderProduct cp = new OrderProduct { Customer = c, Products = products };
        //    return View(cp);
        //}
        //public IActionResult Update()
        //{
        //    return View();
        //}
        //public IActionResult Delete()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Delete(string id)
        //{
        //    Orders p = new Orders();

        //    string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";
        //    IRepository<Orders> rep = new GenericRepository<Orders>(conn);
        //    rep.Delete(id);
        //    return RedirectToAction("ViewOrder", "Order");
        //}
        //[HttpPost]
        //public IActionResult Update(int id, string name, string email, string cnic,
        //    string productName, string address, int quantity)
        //{
        //    Orders p = new Orders();

        //    string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";
        //    IRepository<Orders> rep = new GenericRepository<Order>(conn);
        //    Orders o = new Order();
           
        //    o.Name = name;
        //    o.Email = email;
        //    o.Cnic = cnic;
        //    o.Address = address;
        //    o.Quantity = quantity;
        //    o.ProductName = productName;
        //    rep.Update(o);
        //    return RedirectToAction("ViewOrder", "Order");
        //}

        public ActionResult Submit()
        {
            return View();
        }
        //public ActionResult ViewOrder()
        //{
        //    string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";
        //    IRepository<Order> rep = new GenericRepository<Order>(conn);
        //    List<Order> orders = rep.GetAll().ToList();
        //    return View(orders);

        //    //return View();
        //}
        //[HttpPost]
        //public IActionResult add (string name,string email,string cnic,
        //    string productName, string address, int quantity)
        //{
        //    Order o = new Order();
        //    o.Name = name;
        //    o.Email = email;
        //    o.Cnic = cnic;
        //    o.Address = address;
        //    o.Quantity = quantity;
        //    o.ProductName = productName;
        //    string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";
        //    IRepository<Order> rep = new GenericRepository<Order>(conn);
        //    rep.Add(o);
        //    return RedirectToAction("ViewOrder", "Order");
        //}
    }
}
       /* [HttpPost]
        
        public IActionResult add(string name,string email,string cnic,string address,string quantity, 
            string product)
        {
            
        }
       
       


    }*/