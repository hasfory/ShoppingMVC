using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingDomain.Models;
using ShoppingInfrastructure;

namespace ShoppingInfrastructure.Controllers
{
    public class ShoppingCartProductsController : Controller
    {
        private readonly ShoppingDbContext _context;

        public ShoppingCartProductsController(ShoppingDbContext context)
        {
            _context = context;
        }

        // GET: ShoppingCartProducts
        public async Task<IActionResult> Index()
        {
            var shoppingDbContext = _context.ShoppingCartProducts
                .Include(s => s.Product)
                .Include(s => s.User);
            return View(await shoppingDbContext.ToListAsync());
        }

        // GET: ShoppingCartProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCartProduct = await _context.ShoppingCartProducts
                .Include(s => s.Product)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCartProduct == null)
            {
                return NotFound();
            }

            return View(shoppingCartProduct);
        }

        public class ShoppingCartProductsCreateViewModel
        {
            [Display(Name = "Користувач")]
            public int UserId { get; set; }

            [Display(Name = "Товари")]
            public List<int> SelectedProductIds { get; set; } = new List<int>();

            public MultiSelectList Products { get; set; }
        }
        // GET: ShoppingCartProducts/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "PhoneOrEmail");
            return View();
        }

        // POST: ShoppingCartProducts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,ProductId,Id")] ShoppingCartProduct shoppingCartProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingCartProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", shoppingCartProduct.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "PhoneOrEmail", shoppingCartProduct.UserId);
            return View(shoppingCartProduct);
        }


        // GET: ShoppingCartProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCartProduct = await _context.ShoppingCartProducts.FindAsync(id);
            if (shoppingCartProduct == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", shoppingCartProduct.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "PhoneOrEmail", shoppingCartProduct.UserId);
            return View(shoppingCartProduct);
        }

        // POST: ShoppingCartProducts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,ProductId,Id")] ShoppingCartProduct shoppingCartProduct)
        {
            if (id != shoppingCartProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCartProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartProductExists(shoppingCartProduct.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", shoppingCartProduct.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "PhoneOrEmail", shoppingCartProduct.UserId);
            return View(shoppingCartProduct);
        }

        // GET: ShoppingCartProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCartProduct = await _context.ShoppingCartProducts
                .Include(s => s.Product)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCartProduct == null)
            {
                return NotFound();
            }

            return View(shoppingCartProduct);
        }

        // POST: ShoppingCartProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoppingCartProduct = await _context.ShoppingCartProducts.FindAsync(id);
            if (shoppingCartProduct != null)
            {
                _context.ShoppingCartProducts.Remove(shoppingCartProduct);
            }

            try
            {
                _context.ShoppingCartProducts.Remove(shoppingCartProduct);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["DeleteError"] = "Неможливо видалити кошик, оскільки існують пов'язані записи.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingCartProductExists(int id)
        {
            return _context.ShoppingCartProducts.Any(e => e.Id == id);
        }
    }
}
