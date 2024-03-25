using Movies.Api.Movies;
using Movies.Api.Validation;
using Movies.Api;
using FluentValidation;
using System.Collections.Concurrent;
using Wolverine;
using FluentValidation.Results;

namespace Movies.Api.Movies
{
    public class MovieService : IMovieService
    {
        private readonly IValidator<Movie> _movieValidator;
        private readonly ConcurrentDictionary<Guid, Movie> _movies = new();
        private readonly ConcurrentDictionary<string, Guid> _slugToIdJoin = new();

        private readonly IMessageBus _messageBus;

        public MovieService(IValidator<Movie> movieValidator, IMessageBus messageBus)
        {
            _movieValidator = movieValidator;
            _messageBus = messageBus;
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
            await _messageBus.SendAsync(message);

            return movie;
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Movie>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Movie?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie?> GetBySlugAsync(string slug)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Movie?, ValidationFailed>> UpdateAsync(Movie movie)
        {
            throw new NotImplementedException();
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
