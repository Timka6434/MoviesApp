using MoviesApp.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MoviesApp
{
    public sealed partial class MovieDetailsPage : Page
    {
        public MovieDetailsPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Movie movie)
            {
                var viewModel = DataContext as MovieDetailsViewModel;
                if (viewModel != null)
                {
                    await viewModel.LoadMovieDetailsAsync(movie.Id);
                }
            }
        }

        private void BackButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
