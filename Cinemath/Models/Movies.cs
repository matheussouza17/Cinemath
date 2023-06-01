using Microsoft.Extensions.Hosting;

namespace Cinemath.Models
{
    public class Movies
    {
        public int Id { get; set; }
        public string Name{get; set; }
        public string Description{get; set; }
        public int Year{get; set; }
        public int AgeGroup{ get; set;}
        //public List<MovieRooms> MovieRooms { get; set; }

        public Movies() { }
        public Movies(int id, string name, string description, int year, int ageGroup)
        {
            Id = id;
            Name = name;
            Description = description;
            Year = year;
            AgeGroup = ageGroup;
           // MovieRoom = movieRoom;
        }
    }
}
