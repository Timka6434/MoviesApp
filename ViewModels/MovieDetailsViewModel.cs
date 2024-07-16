using MoviesApp.Models;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MoviesApp
{
    public class MovieDetailsViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private Movie _movie;

        public Movie Movie
        {
            get => _movie;
            set
            {
                _movie = value;
                OnPropertyChanged();
            }
        }

        public MovieDetailsViewModel()
        {
            _apiService = new ApiService();
        }

        public async Task LoadMovieDetailsAsync(int movieId)
        {
            string url = $"https://kinopoiskapiunofficial.tech/api/v2.2/films/{movieId}";
            string jsonResponse = await _apiService.GetApiDataAsync(url);
            if (jsonResponse != null)
            {
                JObject responseData = JObject.Parse(jsonResponse);
                Movie = new Movie
                {
                    Id = movieId,
                    Title = responseData["nameRu"]?.ToString(),
                    Date = responseData["year"]?.ToString(),
                    Description = responseData["description"]?.ToString(),
                    PosterUrl = responseData["posterUrl"]?.ToString(),
                    Genre = responseData["genres"]?.FirstOrDefault()?["genre"]?.ToString(),
                    Rating = responseData["ratingKinopoisk"]?.ToString(),
                    Runtime = responseData["filmLength"]?.ToString()
                };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
