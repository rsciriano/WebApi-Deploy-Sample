using Domain.Aggregates.Cinemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregates.Films
{
    public class FilmCinema
    {
        public int CinemaId { get; set; }

        public Cinema Cinema { get; set; }

        public int FilmId { get; set; }

        public Film Film { get; set; }
    }
}
