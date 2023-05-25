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
    public class CardBanksController : Controller
    {
        private readonly PaymentContext _context;

        public CardBanksController(PaymentContext context)
        {
            _context = context;
        }

        // GET: CardBanks
        public async Task<IActionResult> Index()
        {
            var paymentContext = _context.CardBanks.Include(c => c.Bank).Include(c => c.Customer);
            return View(await paymentContext.ToListAsync());
        }

        // GET: CardBanks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardBank = await _context.CardBanks
                .Include(c => c.Bank)
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.CardBankId == id);
            if (cardBank == null)
            {
                return NotFound();
            }

            return View(cardBank);
        }

        public IActionResult Card_Banks_List()
        {
            ViewData["BankId"] = new SelectList(_context.Banks, "BankId", "BankName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber");
            string member = HttpContext.Session.GetString("customer");
            var paymentContext = _context.CardBanks.Include(c => c.Bank).Include(c => c.Customer);
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
        public IActionResult Card_Banks_Create()
        {
            ViewData["BankId"] = new SelectList(_context.Banks, "BankId", "BankName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber");
            return View();
        }
        [HttpPost]
        public IActionResult Card_Banks_Create(string CardNumber,DateTime ExpireDate,string SecurityCode,int BankId,string CardName)
        {
            var paymentContext = _context.Customers.Include(c => c.Country);

            ViewData["BankId"] = new SelectList(_context.Banks, "BankId", "BankName");
            string member = HttpContext.Session.GetString("customer");
            var list = paymentContext.Where(x => x.AccountNumber == member);

            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber");
            foreach (var item in list)
            {

                return RedirectToAction("Create_Banks_Card",
              new
              {
                  CardNumber = CardNumber,
                  ExpireDate = ExpireDate,
                  SecurityCode = SecurityCode,
                  BankId = BankId,
                  CustomerId = item.CustomerId,
                  CardName = CardName



              }

                );
            }
            return View();
        }
        // GET: CardBanks/Create
        public IActionResult Create()
        {
            ViewData["BankId"] = new SelectList(_context.Banks, "BankId", "BankName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber");
            return View();
        }

        // POST: CardBanks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        public async Task<IActionResult> Create_Banks_Card([Bind("CardBankId,CardNumber,ExpireDate,SecurityCode,BankId,CustomerId,CardName")] CardBank cardBank)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cardBank);
                await _context.SaveChangesAsync();
                return RedirectToAction("Card_Banks_List");
            }
            ViewData["BankId"] = new SelectList(_context.Banks, "BankId", "BankName", cardBank.BankId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber", cardBank.CustomerId);

            return View(cardBank);
        }

        // GET: CardBanks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardBank = await _context.CardBanks.FindAsync(id);
            if (cardBank == null)
            {
                return NotFound();
            }
            ViewData["BankId"] = new SelectList(_context.Banks, "BankId", "BankName", cardBank.BankId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber", cardBank.CustomerId);
            return View(cardBank);
        }

        // POST: CardBanks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CardBankId,CardNumber,ExpireDate,SecurityCode,BankId,CustomerId,CardName")] CardBank cardBank)
        {
            if (id != cardBank.CardBankId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cardBank);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardBankExists(cardBank.CardBankId))
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
            ViewData["BankId"] = new SelectList(_context.Banks, "BankId", "BankName", cardBank.BankId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber", cardBank.CustomerId);
            return View(cardBank);
        }

        // GET: CardBanks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardBank = await _context.CardBanks
                .Include(c => c.Bank)
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.CardBankId == id);
            if (cardBank == null)
            {
                return NotFound();
            }

            return View(cardBank);
        }

        // POST: CardBanks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cardBank = await _context.CardBanks.FindAsync(id);
            _context.CardBanks.Remove(cardBank);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardBankExists(int id)
        {
            return _context.CardBanks.Any(e => e.CardBankId == id);
        }
    }
}
