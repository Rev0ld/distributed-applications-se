using Common.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace VideoLibraryAPI.Infrastructure.Shared
{
    public class IndexIM<E, FIM>
        where E : BaseEntity
        where FIM : FilterIM
    {
        public List<E>? Items { get; set; }
        public PagerIM? Pager { get; set; }
        public FIM? Filter { get; set; }
    }
}
