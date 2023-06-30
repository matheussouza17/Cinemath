using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinemath.Data;
using Cinemath.Models;
using Cinemath.Helper;

namespace Cinemath.Controllers
{
    public class WishListsController : Controller
    {
        private readonly CinemathContext _context;
        private readonly ISessionUser _sessionUser;
        private readonly CinemathContext _movieContext;

        public WishListsController(CinemathContext context, ISessionUser sessionUser, CinemathContext movieContext)
        {
            _context = context;
            _sessionUser = sessionUser;
            _movieContext = movieContext;
        }

        // GET: WishLists
        public async Task<IActionResult> Index()
        {
            User user = _sessionUser.GetSessionUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Redireciona para a página de login se o usuário não estiver logado
            }

            List<WishList> wishLists = await _context.WishList
       .Include(w => w.Movie) // Inclui os dados relacionados da tabela Movie
       .Where(w => w.UsersId == user.Id)
       .ToListAsync();

            return View(wishLists);
        }

        // GET: WishLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.WishList == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wishList == null)
            {
                return NotFound();
            }

            return View(wishList);
        }

        // GET: WishLists/Create
        public IActionResult Create()
        {
            return View();
        }
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(int movieId)
		{
			User user = _sessionUser.GetSessionUser();
			if (user == null)
			{
				return RedirectToAction("Login", "Account"); // Redireciona para a página de login se o usuário não estiver logado
			}

			// Verifica se o filme já existe na lista de desejos do usuário
			bool movieExists = await _context.WishList.AnyAsync(w => w.MovieId == movieId && w.UsersId == user.Id);
			if (movieExists)
			{
				return View();
			}

			var wishList = new WishList
			{
				MovieId = movieId,
				UsersId = user.Id
			};

			_context.Add(wishList);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index", "WishLists");
		}





		// GET: WishLists/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WishList == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishList.FindAsync(id);
            if (wishList == null)
            {
                return NotFound();
            }
            return View(wishList);
        }

        // POST: WishLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,UsersId")] WishList wishList)
        {
            if (id != wishList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishListExists(wishList.Id))
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
            return View(wishList);
        }

        // GET: WishLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.WishList == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wishList == null)
            {
                return NotFound();
            }

            return View(wishList);
        }

        // POST: WishLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.WishList == null)
            {
                return Problem("Entity set 'CinemathContext.WishList'  is null.");
            }
            var wishList = await _context.WishList.FindAsync(id);
            if (wishList != null)
            {
                _context.WishList.Remove(wishList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WishListExists(int id)
        {
          return (_context.WishList?.Any(e => e.Id == id)).GetValueOrDefault();
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RemoveFromWishList(int movieId)
		{
			User user = _sessionUser.GetSessionUser();
			if (user == null)
			{
				return RedirectToAction("Login", "Account"); // Redireciona para a página de login se o usuário não estiver logado
			}

			// Verifica se o filme existe na lista de desejos do usuário
			var wishList = await _context.WishList.FirstOrDefaultAsync(w => w.MovieId == movieId && w.UsersId == user.Id);
			if (wishList == null)
			{
				return NotFound(); // Retorna um erro caso o filme não esteja na lista de desejos
			}

			_context.Remove(wishList);
			await _context.SaveChangesAsync();

			return RedirectToAction("Index", "WishLists");
		}


	}
}
