using Common.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using VideoLibraryAPI.Infrastructure.Copyright;

namespace VideoLibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CopyrightsController : BaseCRUDApiController<Copyrights, CopyrightIM, CopyrightFilterIM>
    {
        protected override void PopulateEntity(Copyrights item, CopyrightIM model)
        {
            item.Name = model.Name;
            item.ShortName = model.ShortName;
            item.Description = model.Description;
        }
        protected override Expression<Func<Copyrights, bool>> GetFilter(CopyrightFilterIM filterModel)
        {
            return c =>
            (string.IsNullOrEmpty(filterModel.Name) || c.Name.Contains(filterModel.Name)) &&
            (string.IsNullOrEmpty(filterModel.ShortName) || c.ShortName.Contains(filterModel.ShortName)) &&
            (string.IsNullOrEmpty(filterModel.Description) || c.Description.Contains(filterModel.Description));
        }
    }
}
