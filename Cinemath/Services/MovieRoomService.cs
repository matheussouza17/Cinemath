using Cinemath.Data;
using Cinemath.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

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
