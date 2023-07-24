namespace Cinemath.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public int AgeGroup { get; set; }
        public string Gender { get; set; }

        public Movie() { }
        public Movie(int id, string name, string description, int year, int ageGroup, string gender)
        {
            Id = id;
            Name = name;
            Description = "";
            Year = year;
            AgeGroup = ageGroup;
            Gender = gender;
        }
    }
}
