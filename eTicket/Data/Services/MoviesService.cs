using eTicket.Data.Base;
using eTicket.Data.ViewModels;
using eTicket.Models;
using Microsoft.EntityFrameworkCore;

namespace eTicket.Data.Services
{
    public class MoviesService : EntityBaseRepository<Movie>, IMoviesService    
    {
        private readonly AppDbContext _context;
        
        public MoviesService( AppDbContext context ) : base(context)
        {
            _context = context;
        }

        public async Task AddNewMovieAsync(NewMovieVM data)
        {
            var newMovie = new Movie()
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                ImageURL = data.ImageURL,
                CinemaId = data.CinemaId,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                MovieCategory = data.MovieCategory,
                ProducerId = data.ProducerId,
            };

            await _context.Movies.AddAsync( newMovie );
            await _context.SaveChangesAsync();

            //Add Movie Actors

            foreach (var actorId in data.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = newMovie.Id,
                    ActorId = actorId
                };

                await _context.Actors_Movie.AddAsync( newActorMovie );

            }

            await _context.SaveChangesAsync();

        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movieDetails = _context.Movies.Include(c => c.Cinema)
                .Include(p => p.Producer)
                .Include(am => am.Actors_Movies)
                .ThenInclude(a => a.Actor)
                .FirstOrDefaultAsync( n => n.Id == id);

            return await movieDetails;
        }

        public async Task<NewMovieDropdownsVM> GetNewMovieDropdownValues()
        {
            var response = new NewMovieDropdownsVM();

            response.Actors = await _context.Actors.OrderBy(a => a.FullName).ToListAsync();
            response.Cinemas = await _context.Cinemas.OrderBy(c => c.Name).ToListAsync();
            response.Producers = await _context.Producers.OrderBy(a => a.FullName).ToListAsync();

            return response;
        }

        public async Task UpdateMovieAsync(NewMovieVM data)
        {
            var dbMovie =  _context.Movies.Where(n => n.Id == data.Id).ToList();
            var newDbMovie = await _context.Movies.FirstOrDefaultAsync(n=> n.Id == data.Id);

            if (newDbMovie != null)
            {
                newDbMovie.Name = data.Name;
                newDbMovie.Description = data.Description;
                newDbMovie.Price = data.Price;
                newDbMovie.ImageURL = data.ImageURL;
                newDbMovie.CinemaId = data.CinemaId;
                newDbMovie.StartDate = data.StartDate;
                newDbMovie.EndDate = data.EndDate;
                newDbMovie.MovieCategory = data.MovieCategory;
                newDbMovie.ProducerId = data.ProducerId;
                await _context.SaveChangesAsync();
            }

            _context.Movies.RemoveRange(dbMovie);

            _context.Movies.AddRange(newDbMovie);

            

            // Remove existing actors

            var existingActorsDB = _context.Actors_Movie.Where(n=>n.MovieId == data.Id).ToList();
            _context.Actors_Movie.RemoveRange(existingActorsDB);
            await _context.SaveChangesAsync();
   
           
       
            //Add Movie Actors

            foreach (var actorId in data.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = data.Id,
                    ActorId = actorId
                };

                await _context.Actors_Movie.AddAsync(newActorMovie);

            }

            await _context.SaveChangesAsync();

        }
    }
}
