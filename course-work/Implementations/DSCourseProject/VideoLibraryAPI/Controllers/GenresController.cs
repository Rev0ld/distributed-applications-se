using Common.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using VideoLibraryAPI.Infrastructure.Genre;

namespace VideoLibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : BaseCRUDApiController<Genres, GenreIM, GenreFilterIM>
    {
        protected override void PopulateEntity(Genres item, GenreIM model)
        {
            item.Name = model.Name;
            item.Description = model.Description;
        }

        protected override Expression<Func<Genres, bool>> GetFilter(GenreFilterIM filterModel)
        {
            return g =>
                    (string.IsNullOrEmpty(filterModel.Name) || g.Name.Contains(filterModel.Name)) &&
                    (string.IsNullOrEmpty(filterModel.Description) || g.Description.Contains(filterModel.Description));
        }
    }
}
