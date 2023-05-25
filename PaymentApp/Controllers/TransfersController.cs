using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DEdge;
using X.PagedList.Web.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaymentApp.Models;
using Xceed.Wpf.Toolkit;
namespace PaymentApp.Controllers
{
    public class TransfersController : Controller
    {
        private readonly PaymentContext _context;

        public TransfersController(PaymentContext context)
        {
            _context = context;
        }

        // GET: Transfers
        public async Task<IActionResult> Index()
        {
            var paymentContext  = _context.Transfers.Include(t => t.CurrencyNavigation);
            return View(await paymentContext.ToListAsync());
        }
        public async Task<IActionResult> Wallet_Details_Given()
        {
            var paymentContext = _context.Transfers.Include(t => t.CurrencyNavigation);
            var paymentContext1 = _context.Customers.Include(c => c.Country);
            string member = HttpContext.Session.GetString("customer");
            var list = paymentContext1.Where(x => x.AccountNumber == member);
            foreach (var item in list)
            {
                ViewData["Message"] = item.CustomerName;
                ViewData["Number"] = item.AccountNumber;
                var transfers = paymentContext.Where(x => x.AccountNumberGiver == item.AccountNumber);
                return View(transfers.ToList());
            }
            return View(await paymentContext.ToListAsync());

        }
        public async Task<IActionResult> Wallet_Details()
        {
            var paymentContext = _context.Transfers.Include(t => t.CurrencyNavigation);
            var paymentContext1 = _context.Customers.Include(c => c.Country);
            string member = HttpContext.Session.GetString("customer");
            var list = paymentContext1.Where(x => x.AccountNumber == member);
            foreach(var item in list)
            {
                ViewData["Message"] = item.CustomerName;
                ViewData["Number"] = item.AccountNumber;
                var transfers=  paymentContext.Where(x => x.AccountNumberTaker == item.AccountNumber);
                return View(transfers.ToList());
            }
            return View(await paymentContext.ToListAsync());

        }

        // GET: Transfers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfers
                .Include(t => t.CurrencyNavigation)
                .FirstOrDefaultAsync(m => m.TransId == id);
            if (transfer == null)
            {
                return NotFound();
            }

            return View(transfer);
        }
        public async Task<IActionResult> Transfer_Error()
        {
            string member = HttpContext.Session.GetString("customer");

            var paymentContext1 = _context.Customers.Include(c => c.Country);
            var list = paymentContext1.Where(x => x.AccountNumber == member);
            ViewData["Currency"] = new SelectList(_context.Currencies, "CurrencyId", "CurrencyName");

            foreach (var item in list)
            {
                ViewData["Message"] = item.CustomerName;
                ViewData["Number"] = item.AccountNumber;
                return View();
            }
            return View();
        }
        public IActionResult Transfer_Confirm(string AccountNumberTaker, int Currency, float Value)
        {
            string member = HttpContext.Session.GetString("customer");

            var paymentContext1 = _context.Customers.Include(c => c.Country);
            var list = paymentContext1.Where(x => x.AccountNumber == member);
            ViewData["Currency"] = new SelectList(_context.Currencies, "CurrencyId", "CurrencyName");

            foreach (var item in list)
            {
                ViewData["Message"] = item.CustomerName;
                ViewData["Number"] = item.AccountNumber;
                return View();
            }
            return View();
        }
        public IActionResult Transfer_Send()
        {
            ViewData["Currency"] = new SelectList(_context.Currencies, "CurrencyId", "CurrencyName");
            string member = HttpContext.Session.GetString("customer");

            var paymentContext1 = _context.Customers.Include(c => c.Country);
            var list = paymentContext1.Where(x => x.AccountNumber == member);
            ViewData["Currency"] = new SelectList(_context.Currencies, "CurrencyId", "CurrencyName");

            foreach (var item in list)
            {
                ViewData["Message"] = item.CustomerName;
                ViewData["Number"] = item.AccountNumber;
                return View();
            }
            return View();
        }
        [HttpPost]
        public IActionResult Transfer_Send(string AccountNumberTaker, int Currency ,float Value)
        {
            ViewData["Currency"] = new SelectList(_context.Currencies, "CurrencyId", "CurrencyName");
            var paymentContext1 = _context.Wallets.Include(w => w.CurrencyNavigation).Include(w => w.Customer)  ;

            string member = HttpContext.Session.GetString("customer");
            var list = paymentContext1.Where(x => x.AccountNumber == AccountNumberTaker);
            if(list.Count() >= 1)
            { 
               
                  var detect=  list.Where(x => x.Currency  == Currency );

                if (detect.Count() >=1 ) { 
                    foreach(var item in detect) { 
            return RedirectToAction("Create_Transfer",
              new
              {
                  AccountNumberGiver= member,

                  AccountNumberTaker = AccountNumberTaker,
                  Value = Value,
                  Currency = Currency
                 
              




              }

                );
                    }
                }
                else
                {
                    return RedirectToAction("Transfer_Error");
                }

            }
            else
            {
                return RedirectToAction("Transfer_Error");
            }
            return View();
        }
        // GET: Transfers/Create
        public IActionResult Create()
        {
            ViewData["Currency"] = new SelectList(_context.Currencies, "CurrencyId", "CurrencyName");
            return View();
        }

        // POST: Transfers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    
        public async Task<IActionResult> Create_Transfer([Bind("TransId,AccountNumberGiver,AccountNumberTaker,Value,Currency")] Transfer transfer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transfer);
                await _context.SaveChangesAsync();
                var paymentContext1 = _context.Wallets.Include(w => w.CurrencyNavigation).Include(w => w.Customer);
                var paymentContext2 = _context.Transfers.Include(t => t.CurrencyNavigation);
             var my_list =  paymentContext2.Where(x => x.AccountNumberTaker == transfer.AccountNumberTaker);
                var list = paymentContext1.Where(x => x.AccountNumber == transfer.AccountNumberTaker);
                var detect = list.Where(x => x.Currency == transfer.Currency);
                foreach(var item in detect) { 
                return RedirectToAction("Wallet_Update","Wallets", new {
                    
                    WalletId= item.WalletId,
                    AccountNumber= item.AccountNumber,
                    Balance =my_list.Sum(x=> x.Value),
                    CustomerId = item.CustomerId,
                    Currency = item.Currency
                });
                }
            }
            ViewData["Currency"] = new SelectList(_context.Currencies, "CurrencyId", "CurrencyName", transfer.Currency);
            return View(transfer);
        }

        // GET: Transfers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfers.FindAsync(id);
            if (transfer == null)
            {
                return NotFound();
            }
            ViewData["Currency"] = new SelectList(_context.Currencies, "CurrencyId", "CurrencyName", transfer.Currency);
            return View(transfer);
        }

        // POST: Transfers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransId,AccountNumberGiver,AccountNumberTaker,Value,Currency")] Transfer transfer)
        {
            if (id != transfer.TransId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transfer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransferExists(transfer.TransId))
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
            ViewData["Currency"] = new SelectList(_context.Currencies, "CurrencyId", "CurrencyName", transfer.Currency);
            return View(transfer);
        }

        // GET: Transfers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfers
                .Include(t => t.CurrencyNavigation)
                .FirstOrDefaultAsync(m => m.TransId == id);
            if (transfer == null)
            {
                return NotFound();
            }

            return View(transfer);
        }

        // POST: Transfers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transfer = await _context.Transfers.FindAsync(id);
            _context.Transfers.Remove(transfer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransferExists(int id)
        {
            return _context.Transfers.Any(e => e.TransId == id);
        }
    }
}
