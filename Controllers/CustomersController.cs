using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Labb_4_My_Library.Data;
using Labb_4_My_Library.Models;
using Labb_4_My_Library.Models.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Labb_4_My_Library.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);

            return customer == null ? NotFound() : View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CustomerDTO customerDto)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    Name = customerDto.Name,
                    Email = customerDto.Email,
                    PhoneNumber = customerDto.PhoneNumber
                };

                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerDto);
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

            var customerDto = new CustomerDTO
            {
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            };

            // Return the customer entity to the view
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] CustomerDTO customerDto)
        {
            if (!CustomerExists(id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var customer = await _context.Customers.FindAsync(id);
                    if (customer == null)
                    {
                        return NotFound();
                    }

                    customer.Name = customerDto.Name;
                    customer.Email = customerDto.Email;
                    customer.PhoneNumber = customerDto.PhoneNumber;

                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Or log the error for further analysis
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Convert DTO back to Customer for the view
            var customerForView = new Customer
            {
                Id = id,
                Name = customerDto.Name,
                Email = customerDto.Email,
                PhoneNumber = customerDto.PhoneNumber
            };

            return View(customerForView);
        }


        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);

            return customer == null ? NotFound() : View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
        public async Task<IActionResult> LoanedBooksByCustomerId(int? id)
        {
            ViewBag.Customers = new SelectList(_context.Customers, "Id", "Name");

            if (id == null)
            {
                return View();
            }

            var customer = await _context.Customers
                .Include(c => c.Borrows)
                .ThenInclude(b => b.Book)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
    }
}
