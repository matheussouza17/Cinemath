using Cinemath.Data;
using Cinemath.Models;

//using (SqlConnection connection = new SqlConnection(@"Persist Security Info=False;User ID=developer;Initial Catalog=db-mathcine;Data Source=db-mathcine.database.windows.net"))
//{
//    SqlCommand command = new SqlCommand("SELECT Movies.Name \r\nFROM Movies\r\nWHERE Movies.Id = @valor", connection);
//    command.Parameters.AddWithValue("@valorEntrada", 2);
//    connection.Open();
//    SqlDataReader reader = command.ExecuteReader();
//    var value = reader.Read();
//    reader.Close();
//}

namespace Cinemath.Services
{
    public class MovieService
    {
        private readonly CinemathContext _context;
        public MovieService(CinemathContext context)
        {
            _context = context;
        }
        public List<Movie> FindAll()
        {
            return _context.Movie.OrderBy(x => x.Name).ToList();
        }
    }
}
