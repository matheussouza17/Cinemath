using Cinemath.Data;

namespace Cinemath.Services
{
    public class MovieRoomService
    {
        private readonly CinemathContext _context;
        public MovieRoomService(CinemathContext context)
        {
            _context = context;
        }


    }

}
