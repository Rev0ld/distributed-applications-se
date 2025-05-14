using Azure;
using Common.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using VideoLibraryAPI.Infrastructure.Tag;

namespace VideoLibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : BaseCRUDApiController<Tags, TagIM, TagFilterIM>
    {
        protected override void PopulateEntity(Tags item, TagIM model)
        {
            item.Name = model.Name;
            item.Description = model.Description;
        }
        protected override Expression<Func<Tags, bool>> GetFilter(TagFilterIM filterModel)
        {
            return t =>
                    (string.IsNullOrEmpty(filterModel.Name) || t.Name.Contains(filterModel.Name)) &&
                    (string.IsNullOrEmpty(filterModel.Description) || t.Description.Contains(filterModel.Description));
        }
    }
}
