using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using MoviesApp.Models;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace MoviesApp
{
    public sealed partial class MainPage : Page
    {
        private readonly ApiService _apiService;

        public MainPage()
        {
            this.InitializeComponent();
            _apiService = new ApiService();
            LoadData();
        }

        private async void LoadData()
        {
            string url = $"https://kinopoiskapiunofficial.tech/api/v2.2/films/top?type=TOP_100_POPULAR_FILMS";

            string jsonResponse = await _apiService.GetApiDataAsync(url);
            if (jsonResponse != null)
            {
                // Парсинг JSON ответа в JObject
                JObject responseData = JObject.Parse(jsonResponse);

                // Используем LINQ для выборки данных из JObject
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

                // Устанавливаем полученный список фильмов в качестве источника данных для элемента FilmsListBox
                FilmsGridView.ItemsSource = films;
            }
        }
    }
}
