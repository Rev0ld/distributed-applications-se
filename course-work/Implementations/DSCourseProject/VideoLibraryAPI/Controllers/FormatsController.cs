using Common.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using VideoLibraryAPI.Infrastructure.Format;

namespace VideoLibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormatsController : BaseCRUDApiController<Formats, FormatIM, FormatFilterIM>
    {
        protected override void PopulateEntity(Formats item, FormatIM model)
        {
            item.Type = model.Type;
            item.Extension = model.Extension;
            item.IsPhysical = model.IsPhysical;
        }
        protected override Expression<Func<Formats, bool>> GetFilter(FormatFilterIM filterModel)
        {
            return f =>
                (string.IsNullOrEmpty(filterModel.Type) || f.Type.Contains(filterModel.Type)) &&
                (string.IsNullOrEmpty(filterModel.Extension) || f.Type.Contains(filterModel.Extension)) &&
                (filterModel.IsPhysical.HasValue || f.IsPhysical.Equals(filterModel.IsPhysical));

        }
    }
}
