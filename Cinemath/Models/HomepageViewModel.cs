namespace Cinemath.Models
{
    public class HomepageViewModel
    {
        public List<Movie> Movies
        {
            get; set;
        }
        public Dictionary<int, string> MovieImageUrls
        {
            get; set;
        }

        public HomepageViewModel()
        {
            MovieImageUrls = new Dictionary<int, string>();
        }


    }


}
