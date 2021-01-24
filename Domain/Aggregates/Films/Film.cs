using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Domain.Aggregates.Cinemas;

namespace Domain.Aggregates.Films
{
    public class Film
    {
        class InternalCollection : ICollection<Cinema>
        {
            private readonly Film film;

            public InternalCollection(Film film)
            {
                this.film = film ?? throw new ArgumentNullException(nameof(film));
            }

            public int Count => film.FilmCinemas.Count;

            public bool IsReadOnly => false;

            public void Add(Cinema item)
            {
                film.FilmCinemas.Add(new FilmCinema
                {
                    FilmId = film.Id,
                    Film = film,
                    CinemaId = item.Id,
                    Cinema = item
                }); ;
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(Cinema item)
            {
                return film.FilmCinemas.Any(x => x.Cinema == item);
            }

            public void CopyTo(Cinema[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<Cinema> GetEnumerator()
            {
                return film.FilmCinemas.Select(x => x.Cinema).AsEnumerable().GetEnumerator();
            }

            public bool Remove(Cinema item)
            {
                var relationItem = film.FilmCinemas.FirstOrDefault(x => x.Cinema == item);
                if (relationItem == null)
                {
                    return false;
                }

                return film.FilmCinemas.Remove(relationItem);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return film.FilmCinemas.Select(x => x.Cinema).AsEnumerable().GetEnumerator();
            }
        }

        public const int MinimumFilmDuration = 1;
        public const int MaximumFilmDuration = 200;

        protected Film() { }

        public Film(string title, int durationInMinutes)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (durationInMinutes < MinimumFilmDuration
                || durationInMinutes > MaximumFilmDuration)
            {
                throw new ArgumentOutOfRangeException(nameof(durationInMinutes));
            }

            Title = title;
            DurationInMinutes = durationInMinutes;
            FilmCinemas = new List<FilmCinema>();
            Cinemas = new InternalCollection(this);

        }

        public int Id { get; private set; }

        public string Title { get; private set; }

        public int DurationInMinutes { get; private set; }

        public ICollection<Cinema> Cinemas { get; private set; }

        public ICollection<FilmCinema> FilmCinemas { get; private set; }
    }
}