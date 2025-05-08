using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PasteIt.Data;
using PasteIt.Models;

namespace PasteIt.Controllers
{
    public class SnippetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SnippetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Snippets
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Snippets.ToListAsync());
        //}

        // GET: Snippets/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var snippet = await _context.Snippets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (snippet == null)
            {
                return RedirectToAction("Create", "Snippets");
            }

            if (snippet.ExpiresAt <= DateTime.Now)
            {
                await _context.Snippets.Where(s => s.Id == snippet.Id).ExecuteDeleteAsync();
                return RedirectToAction("Create", "Snippets");
            }

            string cookieKey = $"snippet_viewed_{id}";
            if (!Request.Cookies.ContainsKey(cookieKey))
            {
                snippet.ViewCount++;
                _context.Snippets.Update(snippet);
                await _context.SaveChangesAsync();

                Response.Cookies.Append(cookieKey, "true", new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddHours(1)
                });
            }

            return View(snippet);
        }

        // GET: Snippets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Snippets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Text")] Snippet snippet, int? expireOption)
        {
            
                string generatedId;
                do
                {
                    generatedId = GenerateId();
                } while (SnippetExists(generatedId));

                snippet.Id = generatedId;
                snippet.ViewCount = 0;
                snippet.CreatedAt = DateTime.Now;

                if (expireOption.HasValue && expireOption.Value > 0 && expireOption.Value < 43200)
                {
                    snippet.ExpiresAt = DateTime.Now.AddMinutes(expireOption.Value);
                }
                else
                {
                    snippet.ExpiresAt = DateTime.Now;
                }

                _context.Add(snippet);
                await _context.SaveChangesAsync();

                return RedirectToRoute("details", new { id = snippet.Id });


        }

        // GET: Snippets/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var snippet = await _context.Snippets.FindAsync(id);
        //    if (snippet == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(snippet);
        //}

        // POST: Snippets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Id,Title,ViewCount,Text,CreatedAt,ExpiresAt")] Snippet snippet)
        //{
        //    if (id != snippet.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(snippet);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!SnippetExists(snippet.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(snippet);
        //}

        // GET: Snippets/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var snippet = await _context.Snippets
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (snippet == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(snippet);
        //}

        // POST: Snippets/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var snippet = await _context.Snippets.FindAsync(id);
        //    if (snippet != null)
        //    {
        //        _context.Snippets.Remove(snippet);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool SnippetExists(string id)
        {
            return _context.Snippets.Any(e => e.Id == id);
        }

        private string GenerateId()
        {
            string id = "";
            Random random = new Random();

            for(int i = 0; i < 8; i++)
            {
                char randomChar = (char)random.Next('A', 'Z' + 1);

                id += randomChar;
            }

            return id;
        }
    }
}
