using Movies.Api.Movies;
using Movies.Api.Validation;
using Movies.Api;
using FluentValidation;
using System.Collections.Concurrent;
using Wolverine;
using FluentValidation.Results;
using MassTransit;

namespace Movies.Api.Movies
{
    public class MovieService : IMovieService
    {
        private readonly IValidator<Movie> _movieValidator;
        private readonly ConcurrentDictionary<Guid, Movie> _movies = new();
        private readonly ConcurrentDictionary<string, Guid> _slugToIdJoin = new();

        //private readonly IMessageBus _messageBus;// WOLVERINE
        private readonly IBus _bus;

        public MovieService(IValidator<Movie> movieValidator, IBus bus)
        {
            _movieValidator = movieValidator;
            //_messageBus = messageBus;
            _bus = bus;
        }

        public async Task<Result<Movie, ValidationFailed>> CreateAsync(Movie movie)
        {
            var validationResult = await _movieValidator.ValidateAsync(movie);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            if (_slugToIdJoin.ContainsKey(movie.Slug))
            {
                var error = new ValidationFailure("Slug", "This movie already exists in the system");
                return new ValidationFailed(error);
            }

            _slugToIdJoin[movie.Slug] = movie.Id;
            _movies[movie.Id] = movie;

            var message = movie.MapToCreated();

            await _bus.Publish(message);
            //await _messageBus.SendAsync(message); this is not for production
            //be careful here this can be send to the database but not publishing
            //await _messageBus.PublishAsync(message);// this is for WOLVERINE

            return movie;
        }

        public Task<Movie?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_movies.GetValueOrDefault(id));
        }

        public Task<Movie?> GetBySlugAsync(string slug)
        {
            var id = _slugToIdJoin.GetValueOrDefault(slug);
            if (id == Guid.Empty)
            {
                return Task.FromResult<Movie?>(null);
            }

            var movie = _movies.GetValueOrDefault(id);
            return Task.FromResult(movie);
        }

        public Task<IEnumerable<Movie>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Movie>>(_movies.Values);
        }

        public async Task<Result<Movie?, ValidationFailed>> UpdateAsync(Movie movie)
        {
            var validationResult = _movieValidator.Validate(movie);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            var movieExists = _movies.ContainsKey(movie.Id);
            if (!movieExists)
            {
                return default(Movie?);
            }

            var oldSlug = _movies[movie.Id].Slug;
            _slugToIdJoin.Remove(oldSlug, out _);

            _slugToIdJoin[movie.Slug] = movie.Id;
            _movies[movie.Id] = movie;

            var movieMessage = movie.MapToUpdated();
            await _bus.Publish(movieMessage);// this for MassTransit

            return movie;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            _movies.Remove(id, out var movie);
            if (movie is null)
            {
                return false;
            }

            _slugToIdJoin.Remove(movie.Slug, out _);

            var movieMessage = movie.MapToDeleted();
            await _bus.Publish(movieMessage); // this is Using MassTransit

            return true;
        }
    }
}


public interface IMovieService
{
    Task<Result<Movie, ValidationFailed>> CreateAsync(Movie movie);

    Task<Movie?> GetByIdAsync(Guid id);

    Task<Movie?> GetBySlugAsync(string slug);

    Task<IEnumerable<Movie>> GetAllAsync();

    Task<Result<Movie?, ValidationFailed>> UpdateAsync(Movie movie);

    Task<bool> DeleteByIdAsync(Guid id);
}
