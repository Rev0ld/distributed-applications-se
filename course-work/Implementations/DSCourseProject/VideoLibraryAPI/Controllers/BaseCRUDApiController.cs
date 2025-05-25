using Common.Entities;
using Common.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.HostFiltering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using System.Reflection;
using VideoLibraryAPI.Infrastructure.Shared;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace VideoLibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public abstract class BaseCRUDApiController<E, EIM, FIM> : ControllerBase
        where E : BaseEntity, new()
        where FIM : FilterIM, new()
    {
        readonly BaseRepository<E> repo = new BaseRepository<E>();

        protected virtual Expression<Func<E, bool>> GetFilter(FIM filterModel)
        {
            return null;
        }
        protected abstract void PopulateEntity(E item, EIM model);

        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            E item = repo.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound(new { success = false, message = $"Entity with ID {id} not found." });
            }

            return Ok(new { success = true, data = item });
        }

        [HttpPost("Get")]
        public IActionResult Search([FromBody] IndexIM<E, FIM> model)
        {
            model.Pager ??= new();
            model.Filter ??= new();

            Expression<Func<E, bool>> filter = GetFilter(model.Filter);

            model.Pager.Page = model.Pager.Page <= 0 ? 1 : model.Pager.Page;
            model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0 ? 10 : model.Pager.ItemsPerPage;
            model.Pager.PagesCount = (int)Math.Ceiling(repo.Count(filter) / (double)(double)model.Pager.ItemsPerPage);
            if (model.Filter.OrderDir == null || model.Filter.OrderDir.IsNullOrEmpty())
            {
                model.Filter.OrderDir = "desc";
            }
            else
            {
                model.Filter.OrderDir = model.Filter.OrderDir.ToLower();
            }

            model.Filter.OrderDir = model.Filter.OrderDir != "desc" ? "asc" : model.Filter.OrderDir;


            if (model.Filter.OrderBy == null)
            {
                model.Filter.OrderBy = "id";
            }
            var orderByMember = typeof(E).GetProperty(model.Filter.OrderBy, BindingFlags.IgnoreCase
                                                    | BindingFlags.Public | BindingFlags.Instance);

            if (orderByMember == null)
            {
                model.Filter.OrderBy = "id";
            }

            List<E> items = repo.GetAll(filter, model.Pager.Page, model.Pager.ItemsPerPage,
                                                model.Filter.OrderBy, model.Filter.OrderDir);
            model.Items = items;

            return Ok
                (
                new
                {
                    success = true,
                    data = model
                }
                );
        }

        [HttpPost]
        public IActionResult Post([FromBody] EIM model)
        {
            E item = new();
            item.IsDeleted = false;
            item.CreatedOn = DateTime.UtcNow;
            item.UpdatedOn = DateTime.UtcNow;
            PopulateEntity(item, model);

            repo.Add(item);
            return Ok(item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] EIM model)
        {
            E item = repo.FirstOrDefault(x => x.Id == id);
            item.UpdatedOn = DateTime.UtcNow;

            if (item == null)
            {
                return NotFound();
            }

            PopulateEntity(item, model);

            repo.Update(item);
            return Ok
                (
                new
                {
                    success = true,
                    updated = item
                }
                );
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            E item = repo.FirstOrDefault(x => x.Id == id);
            item.UpdatedOn = DateTime.UtcNow;

            if (item == null)
            {
                return NotFound();
            }

            item.IsDeleted = true;

            repo.Update(item);
            return Ok
                (
                new
                {
                    success = true,
                    updated = item
                }
                );
        }
    }
}
