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
    public class BankAccountsController : Controller
    {
        private readonly PaymentContext _context;

        public BankAccountsController(PaymentContext context)
        {
            _context = context;
        }

        // GET: BankAccounts
        public async Task<IActionResult> Index()
        {
            var paymentContext = _context.BankAccounts.Include(b => b.Bank).Include(b => b.Customer);
            return View(await paymentContext.ToListAsync());
        }
        public IActionResult Bank_Account_List()
        {
            ViewData["BankId"] = new SelectList(_context.Banks, "BankId", "BankName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber");
            var paymentContext = _context.Customers.Include(c => c.Country);
            var paymentContext1 = _context.BankAccounts.Include(b => b.Bank).Include(b => b.Customer);
            
            string member = HttpContext.Session.GetString("customer");
            var list = paymentContext.Where(x => x.AccountNumber == member);
            foreach (var item in list)
            {
                ViewData["Message"] = item.CustomerName;
                ViewData["Number"] = item.AccountNumber;
               var banks=  paymentContext1.Where(x => x.CustomerId == item.CustomerId);
                return View(banks);
            }
            return View();
        }

        // GET: BankAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.BankAccounts
                .Include(b => b.Bank)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BankAccountId == id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            return View(bankAccount);
        }
        public IActionResult Bank_Account()
        {
            ViewData["BankId"] = new SelectList(_context.Banks, "BankId", "BankName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber");
            var paymentContext = _context.Customers.Include(c => c.Country);

            string member = HttpContext.Session.GetString("customer");
            var list = paymentContext.Where(x => x.AccountNumber == member);
            foreach (var item in list)
            {
                ViewData["Message"] = item.CustomerName;
                ViewData["Number"] = item.CustomerName;
            }
            return View();
        }
        [HttpPost]
        public IActionResult Bank_Account(int? BankId,string IbanCustomer,string AccountName)
        {
            var paymentContext = _context.Customers.Include(c => c.Country);
            string member = HttpContext.Session.GetString("customer");
            var list = paymentContext.Where(x => x.AccountNumber == member);

            var bank = _context.Banks.Where(x => x.BankId == BankId);
            foreach (var item in bank) {
                foreach (var item1 in list)
                {
                    return RedirectToAction("Create_Bank_Account",
              new
              {
                  IbanCustomer = IbanCustomer,
                  BankName = item.BankName,
                  AccountName = AccountName,
                  CustomerId = item1.CustomerId,
                  BankId = BankId
                 

              }

                );
                }
            }
            return View();
        }
        // GET: BankAccounts/Create
        public IActionResult Create()
        {
            ViewData["BankId"] = new SelectList(_context.Banks, "BankId", "BankName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber");
            return View();
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      
        public async Task<IActionResult> Create_Bank_Account([Bind("BankAccountId,IbanCustomer,BankName,AccountName,CustomerId,BankId")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bankAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction("Bank_Account_List");
            }
            ViewData["BankId"] = new SelectList(_context.Banks, "BankId", "BankName", bankAccount.BankId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber", bankAccount.CustomerId);
            return RedirectToAction("Bank_Account_List");
        }

        // GET: BankAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.BankAccounts.FindAsync(id);
            if (bankAccount == null)
            {
                return NotFound();
            }
            ViewData["BankId"] = new SelectList(_context.Banks, "BankId", "BankName", bankAccount.BankId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber", bankAccount.CustomerId);
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BankAccountId,IbanCustomer,BankName,AccountName,CustomerId,BankId")] BankAccount bankAccount)
        {
            if (id != bankAccount.BankAccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bankAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankAccountExists(bankAccount.BankAccountId))
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
            ViewData["BankId"] = new SelectList(_context.Banks, "BankId", "BankName", bankAccount.BankId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "AccountNumber", bankAccount.CustomerId);
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.BankAccounts
                .Include(b => b.Bank)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BankAccountId == id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(id);
            _context.BankAccounts.Remove(bankAccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BankAccountExists(int id)
        {
            return _context.BankAccounts.Any(e => e.BankAccountId == id);
        }
    }
}
