using System.Threading.Tasks;
using Domain.Aggregates.Cinemas;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class CinemaRepository : ICinemaRepository
    {
        private readonly DatabaseContext _context;

        public CinemaRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Cinema> GetCinemaById(int cinemaId)
        {
            return await _context.Cinemas
                .Include(x => x.Screens)
                    .ThenInclude(s => s.Seats)
                .FirstOrDefaultAsync(x => x.Id == cinemaId);
        }
    }
}
