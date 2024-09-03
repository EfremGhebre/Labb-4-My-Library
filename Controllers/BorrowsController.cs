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
    public class BorrowsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BorrowsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Borrows
        public async Task<IActionResult> Index()
        {
            return View(await _context.Borrows
                .Include(b => b.Book)
                .Include(b => b.Customer)
                .ToListAsync());
        }

        // GET: Borrows/Details
        public async Task<IActionResult> Details(int? bookId, int? customerId)
        {
            if (bookId == null || customerId == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrows
                .Include(b => b.Book)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BookId == bookId && m.CustomerId == customerId);

            return borrow == null ? NotFound() : View(borrow);
        }

        // GET: Borrows/Create
        [HttpGet]
        public IActionResult Create()
        {
            // Load customers and books to populate dropdown lists
            ViewBag.Customers = new SelectList(_context.Customers, "Id", "Name");
            ViewBag.Books = new SelectList(_context.Books, "Id", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] BorrowDTO borrowDto)
        {
            if (ModelState.IsValid)
            {
                // Check if the customer has already borrowed the same book
                var existingBorrow = await _context.Borrows
                    .AnyAsync(b => b.CustomerId == borrowDto.CustomerId && b.BookId == borrowDto.BookId && (b.ReturnDate == null || b.ReturnDate > DateTime.Now));

                if (existingBorrow)
                {
                    // If an active borrow exists, add a model error
                    ModelState.AddModelError(string.Empty, "This customer has already borrowed this book.");
                }
                else
                {
                    var borrow = new Borrow
                    {
                        BookId = borrowDto.BookId,
                        CustomerId = borrowDto.CustomerId,
                        BorrowDate = borrowDto.BorrowDate,
                        ReturnDate = borrowDto.ReturnDate
                    };

                    _context.Add(borrow);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // Reload the customers and books in case of validation failure
            ViewBag.Customers = new SelectList(_context.Customers, "Id", "Name");
            ViewBag.Books = new SelectList(_context.Books, "Id", "Title");

            return View(borrowDto);
        }


        // GET: Borrows/Edit
        public async Task<IActionResult> Edit(int? bookId, int? customerId)
        {
            if (bookId == null || customerId == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrows
                .Include(b => b.Book)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.CustomerId == customerId && b.BookId == bookId);

            if (borrow == null)
            {
                return NotFound();
            }

            // Directly pass the Borrow entity to the view
            return View(borrow);
        }

        // POST: Borrows/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int customerId, int bookId, [FromForm] BorrowDTO borrowDto)
        {
            if (borrowDto.BookId != bookId || borrowDto.CustomerId != customerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var borrow = await _context.Borrows.FindAsync(customerId, bookId);
                    if (borrow == null)
                    {
                        return NotFound();
                    }

                    borrow.BorrowDate = borrowDto.BorrowDate;
                    borrow.ReturnDate = borrowDto.ReturnDate;

                    _context.Update(borrow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowExists(customerId, bookId))
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

            // If the ModelState is invalid, reload the Borrow entity and pass it back to the view
            var borrowForView = new Borrow
            {
                CustomerId = borrowDto.CustomerId,
                BookId = borrowDto.BookId,
                BorrowDate = borrowDto.BorrowDate,
                ReturnDate = borrowDto.ReturnDate
            };

            // Reload necessary navigation properties
            borrowForView.Book = await _context.Books.FindAsync(borrowDto.BookId);
            borrowForView.Customer = await _context.Customers.FindAsync(borrowDto.CustomerId);

            return View(borrowForView);
        }


        // GET: Borrows/Delete
        public async Task<IActionResult> Delete(int? bookId, int? customerId)
        {
            if (bookId == null || customerId == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrows
                .Include(b => b.Book)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BookId == bookId && m.CustomerId == customerId);

            return borrow == null ? NotFound() : View(borrow);
        }

        // POST: Borrows/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int customerId, int bookId)
        {
            var borrow = await _context.Borrows.FindAsync(customerId, bookId);
            if (borrow == null)
            {
                return NotFound();
            }

            _context.Borrows.Remove(borrow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowExists(int customerId, int bookId)
        {
            return _context.Borrows.Any(e => e.CustomerId == customerId && e.BookId == bookId);
        }
    }
}
