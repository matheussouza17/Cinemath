using Cinemath.Data;
using Cinemath.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using System;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CinemathContext _context;
    private readonly HttpClient _httpClient;

    public HomeController(ILogger<HomeController> logger, CinemathContext context)
    {
        _logger = logger;
        _context = context;
        _httpClient = new HttpClient();
    }

    public async Task<IActionResult> Index()
    {
        var movies = await _context.Movie.ToListAsync();

        // Embaralhe a lista de filmes aleatoriamente
        var random = new Random();
        movies = movies.OrderBy(x => random.Next()).ToList();

        var model = new HomepageViewModel
        {
            Movies = movies.Take(4).ToList()
        };

        foreach (var movie in model.Movies)
        {
            if (!string.IsNullOrEmpty(movie.Name))
            {
                string imageUrl = await GetMovieImageUrl(movie.Name);
                model.MovieImageUrls.Add(movie.Id, imageUrl);
            }
        }

        return View(model);
    }


    private async Task<string> GetMovieImageUrl(string movieTitle)
    {
        string apiKey = "a8ec58e252045db51dda98b6f5db8821";
        string apiUrl = $"http://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={movieTitle}";

        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            JObject data = JObject.Parse(json);
            string imageUrl = (string)data["results"][0]["poster_path"];
            return $"https://image.tmdb.org/t/p/original{imageUrl}";
        }

        return null; // Retornar null em caso de erro na busca da imagem
    }
    [HttpGet]
    public async Task<IActionResult> Search(string searchQuery)
    {
        // Realize a pesquisa no banco de dados com base no parâmetro de pesquisa
        var movies = await _context.Movie
            .Where(m => m.Name.Contains(searchQuery))
            .ToListAsync();

        var model = new HomepageViewModel
        {
            Movies = movies
        };

        // Adicione os URLs das imagens aos filmes
        foreach (var movie in movies)
        {
            if (!string.IsNullOrEmpty(movie.Name))
            {
                string imageUrl = await GetMovieImageUrl(movie.Name);
                model.MovieImageUrls.Add(movie.Id, imageUrl);
            }
        }

        return View("Index", model);
    }

}
