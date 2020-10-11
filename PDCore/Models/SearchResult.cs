using System.Collections.Generic;

namespace PDCore.Models
{
    public class SearchResult<T>
    {
        public SearchResult(ICollection<T> input)
        {
            Results = input;
        }

        public ICollection<T> Results { get; set; }

        public static implicit operator SearchResult<T>(List<T> input) => new SearchResult<T>(input);
    }
}
