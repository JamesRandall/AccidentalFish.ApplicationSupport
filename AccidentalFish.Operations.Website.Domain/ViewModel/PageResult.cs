using System.Collections.Generic;

namespace AccidentalFish.Operations.Website.Domain.ViewModel
{
    public class PageResult<T>
    {
        public int TotalRows { get; set; }

        public IEnumerable<T> Page { get; set; } 
    }
}
