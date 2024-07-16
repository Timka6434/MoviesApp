namespace MoviesApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string PosterUrl { get; set; }
        public bool IsFavorite { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
        public string Rating { get; set; }
        public string Runtime { get; set; }
    }
}
