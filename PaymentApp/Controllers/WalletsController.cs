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
    public class WalletsController : Controller
    {
        private readonly PaymentContext _context;

        public WalletsController(PaymentContext context)
        {
            _context = context;
        }

        // GET: Wallets
        public async Task<IActionResult> Index()
        {
            var paymentContext = _context.Wallets.Include(w => w.CurrencyNavigation).Include(w => w.Customer);
            return View(await paymentContext.ToListAsync());
        }
        public async Task<IActionResult> Wallet_Lists()
        {
            string member = HttpContext.Session.GetString("customer");

            var paymentContext = _context.Wallets.Include(w => w.CurrencyNavigation).Include(w => w.Customer);
          
           
            var paymentContext1 = _context.Customers.Include(c => c.Country);
            var list = paymentContext1.Where(x => x.AccountNumber == member);

            foreach (var item in list)
            {
                ViewData["Message"] = item.CustomerName;
                ViewData["Number"] = item.AccountNumber;
                var wallets = paymentContext.Where(x => x.CustomerId == item.CustomerId);
                return View(wallets);
            }
            return View(await paymentContext.ToListAsync());
        }

        // GET: Wallets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets
                .Include(w => w.CurrencyNavigation)
                .Include(w => w.Customer)
                .FirstOrDefaultAsync(m => m.WalletId == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        public IActionResult Wallet_Create()
        {
            ViewData["Currency"] = new SelectList(_context.Currencies, "CurrencyId", "CurrencyName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber");
            return View();
        }
        [HttpPost]
        public IActionResult Wallet_Create(int Currency)
        {
            var paymentContext2 = _context.Wallets.Include(w => w.CurrencyNavigation).Include(w => w.Customer);

            ViewData["Currency"] = new SelectList(_context.Currencies, "CurrencyId", "CurrencyName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber");
            var paymentContext = _context.Customers.Include(c => c.Country);
            string member = HttpContext.Session.GetString("customer");
            var list = paymentContext.Where(x => x.AccountNumber == member);
            foreach (var item in list)
            {
                var exist = paymentContext2.Where(x => x.AccountNumber == member && x.Currency == Currency);
                if (exist.Count()>= 1)   
                {
                    return RedirectToAction("Wallet_Lists");


                }
                return RedirectToAction("Create_Wallet",
              new
              {
                  AccountNumber =member,
                  Balance = 0,
                  CustomerId = item.CustomerId,
                  Currency = Currency
               



              }

                );
            }
            return View();
        }
        // GET: Wallets/Create
        public IActionResult Create()
        {
            ViewData["Currency"] = new SelectList(_context.Currencies, "CurrencyId", "CurrencyName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber");
            return View();
        }

        // POST: Wallets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    
        public async Task<IActionResult> Create_Wallet([Bind("WalletId,AccountNumber,Balance,CustomerId,Currency")] Wallet wallet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wallet);
                await _context.SaveChangesAsync();
                return RedirectToAction("Wallet_Lists");
            }
            ViewData["Currency"] = new SelectList(_context.Currencies, "CurrencyId", "CurrencyName", wallet.Currency);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber", wallet.CustomerId);
            return View(wallet);
        }

        // GET: Wallets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet == null)
            {
                return NotFound();
            }
            ViewData["Currency"] = new SelectList(_context.Currencies, "CurrencyId", "CurrencyName", wallet.Currency);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber", wallet.CustomerId);
            return View(wallet);
        }

        // POST: Wallets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        public async Task<IActionResult> Wallet_Update([Bind("WalletId,AccountNumber,Balance,CustomerId,Currency")] Wallet wallet)
        {
        

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wallet);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Transfer_Confirm", "Transfers");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WalletExists(wallet.WalletId))
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
            ViewData["Currency"] = new SelectList(_context.Currencies, "CurrencyId", "CurrencyName", wallet.Currency);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber", wallet.CustomerId);
            return View(wallet);
        }

        // GET: Wallets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets
                .Include(w => w.CurrencyNavigation)
                .Include(w => w.Customer)
                .FirstOrDefaultAsync(m => m.WalletId == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        // POST: Wallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wallet = await _context.Wallets.FindAsync(id);
            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WalletExists(int id)
        {
            return _context.Wallets.Any(e => e.WalletId == id);
        }
    }
}
