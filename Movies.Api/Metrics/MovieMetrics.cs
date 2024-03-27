using System.Diagnostics.Metrics;

namespace Movies.Api.Metrics
{
    public class MovieMetrics
    {
        public const string MeterName = "Movies.Api";

        private readonly Counter<long> _movieRequestCounter;
        private readonly Histogram<double> _movieRequestDuration;
        public MovieMetrics(IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create(MeterName);
            _movieRequestCounter = meter.CreateCounter<long>(
                "movie.api.movie_requests.count");

            _movieRequestDuration = meter.CreateHistogram<double>(
                "movie.api.movie_requests.duration", "ms");
        }

        public void IncreaseMovieRequestCount() {
            _movieRequestCounter.Add(1);
        }
    }
}
