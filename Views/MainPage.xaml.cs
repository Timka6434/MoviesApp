using MoviesApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace MoviesApp
{
    public sealed partial class MainPage : Page
    {
        private readonly ApiService _apiService;
        private List<Movie> _favorites;

        public MainPage()
        {
            this.InitializeComponent();
            _apiService = new ApiService();
            _favorites = new List<Movie>();
            LoadData();
            LoadFavorites();
        }

        private async void LoadData()
        {
            string url = $"https://kinopoiskapiunofficial.tech/api/v2.2/films/top?type=TOP_100_POPULAR_FILMS";

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
                        Description = f["description"]?.ToString(),
                        PosterUrl = f["posterUrl"]?.ToString()
                    })
                    .ToList();

                FilmsGridView.ItemsSource = films;
            }
        }

        private async void AddToFavorites_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var button = sender as Button;
            var movie = button?.DataContext as Movie;
            if (movie != null && !_favorites.Any(f => f.Id == movie.Id))
            {
                _favorites.Add(movie);
                await SaveFavorites();
            }
        }

        private async void RemoveFromFavorites_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var button = sender as Button;
            var movie = button?.DataContext as Movie;
            if (movie != null)
            {
                _favorites.RemoveAll(f => f.Id == movie.Id);
                await SaveFavorites();
            }
        }

        private async Task SaveFavorites()
        {
            string favoritesJson = JsonConvert.SerializeObject(_favorites);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("favorites.json", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, favoritesJson);
        }

        private async void LoadFavorites()
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync("favorites.json");
                string favoritesJson = await FileIO.ReadTextAsync(file);
                _favorites = JsonConvert.DeserializeObject<List<Movie>>(favoritesJson) ?? new List<Movie>();
            }
            catch (FileNotFoundException)
            {
                _favorites = new List<Movie>();
            }
        }
        private void NavigateToFavorites_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FavoritesPage));
        }
    }
}
