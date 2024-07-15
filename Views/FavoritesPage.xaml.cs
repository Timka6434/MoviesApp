using MoviesApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoviesApp
{
    public sealed partial class FavoritesPage : Page
    {
        private List<Movie> _favorites;

        public FavoritesPage()
        {
            this.InitializeComponent();
            LoadFavorites();
        }

        private async void LoadFavorites()
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync("favorites.json");
                var favoritesJson = await FileIO.ReadTextAsync(file);
                _favorites = JsonSerializer.Deserialize<List<Movie>>(favoritesJson) ?? new List<Movie>();
                FavoritesGridView.ItemsSource = _favorites;
            }
            catch (FileNotFoundException)
            {
                _favorites = new List<Movie>();
            }
        }

        private async void RemoveFromFavorites_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var movie = button?.DataContext as Movie;
            if (movie != null)
            {
                _favorites.RemoveAll(f => f.Id == movie.Id);
                FavoritesGridView.ItemsSource = null;
                FavoritesGridView.ItemsSource = _favorites;
                await SaveFavorites();
            }
        }

        private async Task SaveFavorites()
        {
            var favoritesJson = JsonSerializer.Serialize(_favorites);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("favorites.json", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, favoritesJson);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
