using eTicket.Data;
using eTicket.Data.Services;
using eTicket.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eTicket.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService _service;

        public MoviesController(IMoviesService service )
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var allMovies = await _service.GetAllAsync( n => n.Cinema );
            return View(allMovies);
        }

        public async Task<IActionResult> Filter(string searchString)
        {
            var allMovies = await _service.GetAllAsync(n => n.Cinema);

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allMovies.Where(n => n.Name.ToLower().Contains(searchString.ToLower()) || n.Description.ToLower().Contains(searchString.ToLower())).ToList();

                return View("Index", filteredResult);
            }
            return View("Index", allMovies);
        }

        // GET: Movies/Details/id


        public async Task<IActionResult> Details(int id)
        {
            var movieDetail = await _service.GetMovieByIdAsync( id );
            return View(movieDetail);
        }

        //GET: Movies/Create

        public async   Task <IActionResult> Create()
        {
            var movieDropdownData = await _service.GetNewMovieDropdownValues();

            ViewBag.Cinemas = new SelectList(movieDropdownData.Cinemas, "Id", "Name") ;
            ViewBag.Producers = new SelectList(movieDropdownData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownData.Actors, "Id", "FullName");

            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create( NewMovieVM newMovie )
        {
            if (!ModelState.IsValid)
            {
                var movieDropdownData = await _service.GetNewMovieDropdownValues();

                ViewBag.Cinemas = new SelectList(movieDropdownData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownData.Actors, "Id", "FullName");

                return View(newMovie);
            }

            await _service.AddNewMovieAsync( newMovie );
            return RedirectToAction(nameof(Index));
        }

        //GET: Movies/Edit/id

        public async Task<IActionResult> Edit(int id)
        {

            var movieDetails = await _service.GetMovieByIdAsync( id );
            if (movieDetails == null) return View("NotFound");

            var response = new NewMovieVM()
            {
                Id = movieDetails.Id,
                Name = movieDetails.Name,
                Description = movieDetails.Description,
                Price = movieDetails.Price,
                ImageURL = movieDetails.ImageURL,
                MovieCategory = movieDetails.MovieCategory,
                ProducerId = movieDetails.ProducerId,
                CinemaId = movieDetails.CinemaId,
                ActorIds = movieDetails.Actors_Movies.Select(n => n.ActorId).ToList(),
                StartDate = movieDetails.StartDate,
                EndDate = movieDetails.EndDate,
            };

            var movieDropdownData = await _service.GetNewMovieDropdownValues();

            ViewBag.Cinemas = new SelectList(movieDropdownData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownData.Actors, "Id", "FullName");

            return View(response);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id, NewMovieVM newMovie)
        {
            if (id != newMovie.Id) return View("NotFound");


            if (!ModelState.IsValid)
            {
                var movieDropdownData = await _service.GetNewMovieDropdownValues();

                ViewBag.Cinemas = new SelectList(movieDropdownData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownData.Actors, "Id", "FullName");

                return View(newMovie);
            }

            await _service.UpdateMovieAsync(newMovie);
            return RedirectToAction(nameof(Index));
        }

    }
}
