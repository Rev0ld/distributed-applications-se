using VideoLibraryAPI.Infrastructure.Author;
using Common.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Microsoft.AspNetCore.Authorization;

namespace VideoLibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : BaseCRUDApiController<Authors, AuthorIM, AuthorFilterIM>
    {
        protected override void PopulateEntity(Authors item, AuthorIM model)
        {
            item.FirstName = model.FirstName;
            item.MiddleName = model.MiddleName;
            item.LastName = model.LastName;
            item.Biography = model.Biography;
        }
        protected override Expression<Func<Authors, bool>> GetFilter(AuthorFilterIM filterModel)
        {
            return a =>
            (string.IsNullOrEmpty(filterModel.FirstName) || a.FirstName.Contains(filterModel.FirstName)) &&
            (string.IsNullOrEmpty(filterModel.MiddleName) || a.MiddleName.Contains(filterModel.MiddleName)) &&
            (string.IsNullOrEmpty(filterModel.LastName) || a.LastName.Contains(filterModel.LastName)) &&
            (string.IsNullOrEmpty(filterModel.Biography) || a.Biography.Contains(filterModel.Biography));

        }

    }
}
