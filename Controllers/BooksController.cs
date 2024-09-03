using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Labb_4_My_Library.Data;
using Labb_4_My_Library.Models;
using Labb_4_My_Library.Models.DTOs;

namespace Labb_4_My_Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);

            return book == null ? NotFound() : View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] BookDTO bookDto)
        {
            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Title = bookDto.Title,
                    Author = bookDto.Author,
                    IsReturned = bookDto.IsReturned
                };

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookDto);
        }

        // GET: Books/Edit/5
        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book); // Return Book entity to view
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] BookDTO bookDto)
        {
            if (!BookExists(id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var book = await _context.Books.FindAsync(id);
                    if (book == null)
                    {
                        return NotFound();
                    }

                    book.Title = bookDto.Title;
                    book.Author = bookDto.Author;
                    book.IsReturned = bookDto.IsReturned;

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(id))
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

            // Convert DTO back to Book for the view
            var bookForView = new Book
            {
                Id = id,
                Title = bookDto.Title,
                Author = bookDto.Author,
                IsReturned = bookDto.IsReturned
            };

            return View(bookForView);
        }


        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);

            return book == null ? NotFound() : View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
