using Windows.UI.Xaml.Controls;
using MoviesApp.Models;

namespace MoviesApp
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void AddToFavorites_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var button = sender as Button;
            var movie = button?.DataContext as Movie;
            if (movie != null)
            {
                var viewModel = DataContext as MainViewModel;
                viewModel?.AddToFavorites(movie);
            }
        }

        private void NavigateToFavorites_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var frame = (Frame)Windows.UI.Xaml.Window.Current.Content;
            frame.Navigate(typeof(FavoritesPage));
        }

        private void MoviesGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Movie movie)
            {
                var frame = (Frame)Windows.UI.Xaml.Window.Current.Content;
                frame.Navigate(typeof(MovieDetailsPage), movie);
            }
        }
    }
}
