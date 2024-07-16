using MoviesApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;

namespace MoviesApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private ObservableCollection<Movie> _movies;
        private ObservableCollection<Movie> _favorites;
        private const int PageSize = 20; // Количество фильмов на странице
        private const int TotalPages = 5; // Число страниц, чтобы получить 100 фильмов (100 / 20 = 5)

        public ObservableCollection<Movie> Movies
        {
            get => _movies;
            set
            {
                _movies = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Movie> Favorites
        {
            get => _favorites;
            set
            {
                _favorites = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            _apiService = new ApiService();
            Movies = new ObservableCollection<Movie>();
            Favorites = new ObservableCollection<Movie>();
            LoadData();
            LoadFavorites();
        }

        public async void LoadData()
        {
            Movies.Clear(); // Очистка текущего списка фильмов перед загрузкой новых данных

            for (int page = 1; page <= TotalPages; page++)
            {
                string url = $"https://kinopoiskapiunofficial.tech/api/v2.2/films/top?type=TOP_100_POPULAR_FILMS&page={page}";
                string jsonResponse = await _apiService.GetApiDataAsync(url);
                if (jsonResponse != null)
                {
                    JObject responseData = JObject.Parse(jsonResponse);
                    var films = responseData["films"]
                        .Select(f => new Movie
                        {
                            Id = (int)f["filmId"],
                            Title = f["nameRu"]?.ToString(),
                            Date = f["year"]?.ToString(),
                            PosterUrl = f["posterUrl"]?.ToString()
                        })
                        .ToList();

                    foreach (var film in films)
                    {
                        Movies.Add(film);
                    }
                }
            }
        }

        public async void AddToFavorites(Movie movie)
        {
            if (movie != null && !Favorites.Any(f => f.Id == movie.Id))
            {
                Favorites.Add(movie);
                await SaveFavorites();
            }
        }

        public async void RemoveFromFavorites(Movie movie)
        {
            if (movie != null)
            {
                Favorites.Remove(movie);
                await SaveFavorites();
            }
        }

        private async Task SaveFavorites()
        {
            string favoritesJson = JsonConvert.SerializeObject(Favorites);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("favorites.json", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, favoritesJson);
        }

        private async void LoadFavorites()
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync("favorites.json");
                string favoritesJson = await FileIO.ReadTextAsync(file);
                var favorites = JsonConvert.DeserializeObject<ObservableCollection<Movie>>(favoritesJson) ?? new ObservableCollection<Movie>();
                Favorites = favorites;
            }
            catch (FileNotFoundException)
            {
                Favorites = new ObservableCollection<Movie>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
