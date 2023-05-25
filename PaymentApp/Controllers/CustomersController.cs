using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaymentApp.Models;
using Xceed.Wpf.Toolkit;

namespace PaymentApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly PaymentContext _context;

        public CustomersController(PaymentContext context)
        {
            _context = context;
        }

        // GET: Customers
      
        public async Task<IActionResult> Index()
        {
            var paymentContext = _context.Customers.Include(c => c.Country);
           
            string member = HttpContext.Session.GetString("customer");
            var list = paymentContext.Where(x => x.AccountNumber == member);
            foreach (var item in list)
            {
                ViewData["Message"] = item.CustomerName;
            }
            return View();
        }
        public IActionResult Exit()
        {
            HttpContext.Session.Clear();


            return RedirectToAction("Index","Home");
        }
        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        [HttpGet]
        [Route("Login")]
        public IActionResult Account_Details()
        {
            var paymentContext = _context.Customers.Include(c => c.Country);
          string member=  HttpContext.Session.GetString("customer");
         var list =   paymentContext.Where(x => x.AccountNumber == member);
            foreach(var item in list)
            {
                ViewData["Message"] = item.CustomerName;
            }
           
            return View(list.ToList());
        }
        public IActionResult Login()
        {
           


            return View();
        }
        [HttpPost]

        public IActionResult Login(string CustomerName, string CustomerPassword)
        {
            var member = _context.Customers.FirstOrDefault(x => x.CustomerName == CustomerName && x.CustomerPassword == CustomerPassword);
            if(member != null)
            {
                HttpContext.Session.SetString("customer", member.AccountNumber);


             
                    return RedirectToAction("Account_Details");
            }
            else
            {
              return  RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Sign_Up()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName");


            Random rnd = new Random();

            for (int j = 0; j < 4; j++)
            {
                var number =  rnd.Next();
            }


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Sign_Up(int CustomerId,string CustomerName,string CustomerPassword,string CustomerTel,string CustomerMail, int CountryId)
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName");


            Random rnd = new Random();

            for (int j = 0; j < 4; j++)
            {
                var number = rnd.Next();
                return RedirectToAction("Create_customer",
              new
              {
                  CustomerId = CustomerId,
                  CustomerName = CustomerName,
                  CustomerPassword = CustomerPassword,
                  CustomerTel = CustomerTel,
                  CustomerMail = CustomerMail,
                  AccountNumber = number,
                  CountryId = CountryId

              }

                );
            }
            

            return View();
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        public async Task<IActionResult> Create_customer([Bind("CustomerId,CustomerName,CustomerPassword,CustomerTel,CustomerMail,AccountNumber,CountryId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName", customer.CountryId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName", customer.CountryId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustomerName,CustomerPassword,CustomerTel,CustomerMail,AccountNumber,CountryId")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName", customer.CountryId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
