using Cinemath.Data;
using Cinemath.Helper;
using Cinemath.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Cinemath.Controllers
{
    public class MoviesController : Controller
    {
        private readonly CinemathContext _context;
		private readonly ISessionUser _session;

		public MoviesController(CinemathContext context, ISessionUser session)
		{
            _context = context;
			_session = session;
		}

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return _context.Movie != null ?
                        View(await _context.Movie.ToListAsync()) :
                        Problem("Entity set 'CinemathContext.Movies'  is null.");
        }



        private async Task<string> GetMovieImageUrl(string movieTitle)
        {
            string apiKey = "a8ec58e252045db51dda98b6f5db8821";
            string apiUrl = $"http://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={movieTitle}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(json);
                    string imageUrl = (string)data["results"][0]["poster_path"];
                    return $"https://image.tmdb.org/t/p/original{imageUrl}";
                }
            }

            return null; // Retornar null em caso de erro na busca da imagem
        }



		// GET: Movies/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			User user = _session.GetSessionUser();
			if (id == null || _context.Movie == null)
			{
				return NotFound();
			}

			var movie = await _context.Movie
				.FirstOrDefaultAsync(m => m.Id == id);
			if (movie == null)
			{
				return NotFound();
			}
			if (!string.IsNullOrEmpty(movie.Name))
			{
				string imageUrl = await GetMovieImageUrl(movie.Name);
				ViewBag.MovieImageUrl = imageUrl;
			}

			bool movieInWishList = await _context.WishList.AnyAsync(w => w.MovieId == id && w.UsersId == user.Id);
			var viewModel = new MovieDetailViewModel
			{
				Movie = movie,
				IsInWishList = movieInWishList
			};

			return View(viewModel); // Passa o viewModel para a View em vez de movie
		}


		// GET: Movies/Create
		public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Year,AgeGroup,Gender")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                // Buscar detalhes do filme usando a API IMDb
                if (!string.IsNullOrEmpty(movie.Name))
                {
                    string apiKey = "a8ec58e252045db51dda98b6f5db8821";
                    string movieTitle = movie.Name;
                    string apiUrl = $"http://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={movieTitle}";

                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync(apiUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            string json = await response.Content.ReadAsStringAsync();
                            JObject data = JObject.Parse(json);

                            string description = (string)data["results"][0]["overview"];
                            string dataString = (string)data["results"][0]["release_date"];
                            string name = (string)data["results"][0]["title"];


                            // Preencher as informações obtidas da API
                            if(movie.Description=="" || movie.Description == null)
                                movie.Description = description;
                            if(movie.Year==null)
                                movie.Year = DateTime.ParseExact(dataString, "yyyy-MM-dd", CultureInfo.InvariantCulture).Year;
                           // movie.Name = name;
                        }
                    }
                }

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }


        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Year,AgeGroup,Gender")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'CinemathContext.Movies'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return (_context.Movie?.Any(e => e.Id == id)).GetValueOrDefault();
        }



		public async Task<IActionResult> Homepage()
		{
			var movies = await _context.Movie.Take(5).ToListAsync();

			// Obtenha a lista de desejos do usuário
			User user = _session.GetSessionUser();
			List<WishList> userWishList = await _context.WishList
				.Include(w => w.Movie)
				.Where(w => w.UsersId == user.Id)
				.ToListAsync();

			List<Movie> wishListMovies = userWishList.Select(w => w.Movie).ToList();

			// Preencha o modelo com os filmes e a lista de desejos
			var model = new HomepageViewModel
			{
				Movies = movies,
				//WishLists = wishListMovies
			};

			return View(model);
		}



	}
}
