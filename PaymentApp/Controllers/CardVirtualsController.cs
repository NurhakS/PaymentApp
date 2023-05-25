using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DEdge;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaymentApp.Models;
using Xceed.Wpf.Toolkit;

namespace PaymentApp.Controllers
{
    public class CardVirtualsController : Controller
    {
        private readonly PaymentContext _context;

        public CardVirtualsController(PaymentContext context)
        {
            _context = context;
        }

        // GET: CardVirtuals
        public async Task<IActionResult> Index()
        {
            var paymentContext = _context.CardVirtuals.Include(c => c.Customer);
            return View(await paymentContext.ToListAsync());
        }

        // GET: CardVirtuals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardVirtual = await _context.CardVirtuals
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.CardIdVirtual == id);
            if (cardVirtual == null)
            {
                return NotFound();
            }

            return View(cardVirtual);
        }
        public IActionResult Card_Virtual_Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber");
            return View();
        }
        [HttpPost]
        public IActionResult Card_Virtual_Create(string NameCard)
        {
            
            var paymentContext = _context.Customers.Include(c => c.Country);
            string member = HttpContext.Session.GetString("customer");
            var list = paymentContext.Where(x => x.AccountNumber == member);
            Random rnd = new Random();
            Random gen = new Random();
            int range = 5 * 365; //5 years  
            DateTime randomDate = DateTime.Today.AddYears(6);
            randomDate.AddMonths(5);
            foreach (var item in list) { 
            for (int j = 0; j <  6; j++)
            {
                var number = rnd.Next();
                    var cvv1 = gen.Next().ToString().Substring(7);
                    var cvv2 = gen.Next().ToString().Take(2);
                    var cvv3 = gen.Next().ToString().Take(4);
                    string cvv = cvv1.ToString() + cvv2.ToString() + cvv3.ToString();



                    return RedirectToAction("Create_Virtual_Card",
              new
              {
                  CardNumber = number,
                  ExpireDate = randomDate,
                  SecurityCode = cvv1,
                  NameCard = NameCard,
                  CustomerId = item.CustomerId
                  


              }

                );
            }
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber");
            return View();
        }
        public IActionResult Card_Virtual_List()
        {
            string member = HttpContext.Session.GetString("customer");
            var paymentContext = _context.CardVirtuals.Include(c => c.Customer);
            var paymentContext1 = _context.Customers.Include(c => c.Country);

            var list = paymentContext1.Where(x => x.AccountNumber == member);

            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber");
            foreach (var item in list)
            {
                ViewData["Message"] = item.CustomerName;
                ViewData["Number"] = item.AccountNumber;
                var cards = paymentContext.Where(x => x.CustomerId == item.CustomerId);
                return View(cards);
            }
            return View();
        }
        // GET: CardVirtuals/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber");
            return View();
        }

        // POST: CardVirtuals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        public async Task<IActionResult> Create_Virtual_Card([Bind("CardIdVirtual,CardNumber,ExpireDate,SecurityCode,NameCard,CustomerId")] CardVirtual cardVirtual)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cardVirtual);
                await _context.SaveChangesAsync();
                return RedirectToAction("Card_Virtual_List");
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber", cardVirtual.CustomerId);
            return RedirectToAction("Card_Virtual_List");
        }

        // GET: CardVirtuals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardVirtual = await _context.CardVirtuals.FindAsync(id);
            if (cardVirtual == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber", cardVirtual.CustomerId);
            return View(cardVirtual);
        }

        // POST: CardVirtuals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CardIdVirtual,CardNumber,ExpireDate,SecurityCode,NameCard,CustomerId")] CardVirtual cardVirtual)
        {
            if (id != cardVirtual.CardIdVirtual)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cardVirtual);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardVirtualExists(cardVirtual.CardIdVirtual))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber", cardVirtual.CustomerId);
            return View(cardVirtual);
        }

        // GET: CardVirtuals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardVirtual = await _context.CardVirtuals
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.CardIdVirtual == id);
            if (cardVirtual == null)
            {
                return NotFound();
            }

            return View(cardVirtual);
        }

        // POST: CardVirtuals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cardVirtual = await _context.CardVirtuals.FindAsync(id);
            _context.CardVirtuals.Remove(cardVirtual);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardVirtualExists(int id)
        {
            return _context.CardVirtuals.Any(e => e.CardIdVirtual == id);
        }
    }
}
