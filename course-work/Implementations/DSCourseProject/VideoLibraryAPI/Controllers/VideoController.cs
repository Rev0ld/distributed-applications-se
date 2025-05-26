using Common.Entities;
using Common.Entities.M2MEntities;
using Common.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.SqlServer.Server;
using System.Linq.Expressions;
using System.Reflection;
using VideoLibraryAPI.Infrastructure.Author;
using VideoLibraryAPI.Infrastructure.Genre;
using VideoLibraryAPI.Infrastructure.Shared;
using VideoLibraryAPI.Infrastructure.Tag;
using VideoLibraryAPI.Infrastructure.Video;
using Microsoft.IdentityModel.Tokens;
using Azure;
using Microsoft.AspNetCore.Authorization;

namespace VideoLibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VideoController : BaseCRUDApiController<Videos, VideoIM, VideoFilterIM>
    {
        protected override void PopulateEntity(Videos item, VideoIM model)
        {
            BaseRepository<Formats> formatRepo = new();
            item.Title = model.Title;
            item.FileId = model.FileId;
            item.Size = model.Size;
            item.FormatId = model.FormatId;
            item.Description = model.Description;
            item.YearOfPublishing = model.YearOfPublishing;
            item.CopyrightId = model.CopyrightId;
        }
        protected override Expression<Func<Videos, bool>> GetFilter(VideoFilterIM filterModel)
        {
            return v =>
        (string.IsNullOrEmpty(filterModel.Title) || v.Title.Contains(filterModel.Title)) &&
        (string.IsNullOrEmpty(filterModel.FileId) || v.FileId.Contains(filterModel.FileId)) &&
        (!filterModel.Size.HasValue || v.Size == filterModel.Size) &&
        (!filterModel.YearOfPublishing.HasValue || v.YearOfPublishing == filterModel.YearOfPublishing) &&
        (string.IsNullOrEmpty(filterModel.Description) || v.Description.Contains(filterModel.Description)) &&

        (string.IsNullOrEmpty(filterModel.FormatExtension) || v.Format.Extension.Contains(filterModel.FormatExtension)) &&
        (!filterModel.FormatIsPhysical.HasValue || v.Format.IsPhysical == filterModel.FormatIsPhysical) &&

        (string.IsNullOrEmpty(filterModel.CopyrightShortName) || v.Copyright.Name.Contains(filterModel.CopyrightShortName)) &&

        (string.IsNullOrEmpty(filterModel.AuthorFirstName) || v.Authors.Any(av => av.Author.FirstName.Contains(filterModel.AuthorFirstName))) &&
        (string.IsNullOrEmpty(filterModel.AuthorMiddleName) || v.Authors.Any(av => av.Author.MiddleName.Contains(filterModel.AuthorMiddleName))) &&
        (string.IsNullOrEmpty(filterModel.AuthorLastName) || v.Authors.Any(av => av.Author.LastName.Contains(filterModel.AuthorLastName))) &&

        (string.IsNullOrEmpty(filterModel.GenreName) || v.Genres.Any(gv => gv.Genre.Name.Contains(filterModel.GenreName)));
        }

        protected Expression<Func<AuthorVideo, bool>> GetFilterAuthorVideo(AuthorFilterIM filterModel, int id)
        {
            return av =>
            (string.IsNullOrEmpty(filterModel.FirstName) || av.Author.FirstName.Contains(filterModel.FirstName)) &&
            (string.IsNullOrEmpty(filterModel.MiddleName) || av.Author.MiddleName.Contains(filterModel.MiddleName)) &&
            (string.IsNullOrEmpty(filterModel.LastName) || av.Author.LastName.Contains(filterModel.LastName)) &&
            (string.IsNullOrEmpty(filterModel.Biography) || av.Author.Biography.Contains(filterModel.Biography)) &&
            (av.VideoId == id);
        }
        protected Expression<Func<GenreVideo, bool>> GetFilterGenreVideo(GenreFilterIM filterModel, int id)
        {
            return gv =>
            (string.IsNullOrEmpty(filterModel.Name) || gv.Genre.Name.Contains(filterModel.Name)) &&
            (string.IsNullOrEmpty(filterModel.Description) || gv.Genre.Description.Contains(filterModel.Description)) &&
            (gv.VideoId == id);
        }
        protected Expression<Func<TagVideo, bool>> GetFilterTagVideo(TagFilterIM filterModel, int id)
        {
            return tv =>
            (string.IsNullOrEmpty(filterModel.Name) || tv.Tag.Name.Contains(filterModel.Name)) &&
            (string.IsNullOrEmpty(filterModel.Description) || tv.Tag.Description.Contains(filterModel.Description)) &&
            (tv.VideoId == id);
        }


        

        [HttpPost("get/author/{videoId}")]
        public IActionResult GetAuthors(int videoId, [FromBody] IndexIM<AuthorVideo, AuthorFilterIM> model)
        {
            BaseRepository<AuthorVideo> repo = new();

            Expression<Func<AuthorVideo, bool>> filter = GetFilterAuthorVideo(model.Filter, videoId);

            model.Pager ??= new();
            model.Filter ??= new();

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
            var orderByMember = typeof(AuthorVideo).GetProperty(model.Filter.OrderBy, BindingFlags.IgnoreCase
                                                    | BindingFlags.Public | BindingFlags.Instance);


            if (orderByMember == null)
            {
                model.Filter.OrderBy = "id";
            }


            List<Authors> items = repo
                                    .GetAll(filter, model.Pager.Page, model.Pager.ItemsPerPage,
                                                model.Filter.OrderBy, model.Filter.OrderDir)
                                    .Select(x => x.Author)
                                    .ToList();

            return Ok
                (
                new
                {
                    success = true,
                    data = new
                    {
                        items,
                        model.Pager,
                        model.Filter
                    }
                }
                );
        }

        [HttpGet("author/{videoId}")]
        public IActionResult GetAllAuthorsIds(int videoId)
        {
            BaseRepository<AuthorVideo> repo = new();

            var authorIds = repo.GetAll(x => x.VideoId == videoId).Select(x => x.AuthorId).ToList();

            return Ok(new
            {
                success = true,
                data = authorIds
            });
        }

        [HttpPost("author/{videoId}/{authorId}")]
        public IActionResult AddAuthor(int videoId, int authorId)
        {
            BaseRepository<AuthorVideo> authorVideoRepo = new();
            BaseRepository<Authors> authorRepo = new();
            BaseRepository<Videos> videoRepo = new();

            if (authorRepo.GetAll(x => x.Id == authorId).Any() && videoRepo.GetAll(x => x.Id == videoId).Any())
            {
                AuthorVideo authorVideo = new();
                authorVideo.AuthorId = authorId;
                authorVideo.VideoId = videoId;

                authorVideoRepo.Add(authorVideo);

                return Ok
                    (
                    new
                    {
                        success = true,
                        data = authorVideo
                    }
                    );
            }
            return BadRequest(
                new
                {

                    success = false,
                    errorMessage = "Error in imput params!"

                }

                );

        }

        [HttpPost("author/update/{videoId}/")]
        public IActionResult UpdateAuthors(int videoId, [FromBody] List<int> authorIds)
        {
            BaseRepository<AuthorVideo> authorVideoRepo = new();
            BaseRepository<Videos> videoRepo = new();
            BaseRepository<Authors> authorRepo = new();

            if (!videoRepo.GetAll(x => x.Id == videoId).Any())
            {
                return BadRequest(new { success = false, errorMessage = "Invalid video ID." });
            }

            var existingAuthorIds = authorVideoRepo
                .GetAll(x => x.VideoId == videoId)
                .Select(x => x.AuthorId)
                .ToList();

            var newAuthorIds = authorIds.Distinct().ToList();

            var toAdd = newAuthorIds.Except(existingAuthorIds).ToList();
            var toRemove = existingAuthorIds.Except(newAuthorIds).ToList();

            foreach (var authorId in toAdd)
            {
                if (authorRepo.GetAll(x => x.Id == authorId).Any())
                {
                    authorVideoRepo.Add(new AuthorVideo
                    {
                        VideoId = videoId,
                        AuthorId = authorId,
                        IsDeleted = false,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow
                    });

                }
            }

            foreach (var authorId in toRemove)
            {
                var av = authorVideoRepo.FirstOrDefault(x => x.VideoId == videoId && x.AuthorId == authorId);
                if (av != null)
                {
                    authorVideoRepo.Delete(av);
                }
            }

            return Ok(new
            {
                success = true,
                data = new
                {
                    added = toAdd,
                    removed = toRemove
                }
            });
        }

        [HttpDelete("author/{videoId}/{authorId}")]
        public IActionResult RemoveAuthor(int videoId, int authorId)
        {
            BaseRepository<AuthorVideo> authorVideoRepo = new();
            BaseRepository<Authors> authorRepo = new();
            BaseRepository<Videos> videoRepo = new();

            if (authorRepo.GetAll(x => x.Id == authorId).Any() && authorVideoRepo.GetAll(x => x.AuthorId == authorId && x.VideoId == videoId).Any() && videoRepo.GetAll(x => x.Id == videoId).Any())
            {
                AuthorVideo authorVideo = authorVideoRepo.FirstOrDefault(x => x.VideoId == videoId && x.AuthorId == authorId);
                authorVideoRepo.Delete(authorVideo);
                return Ok
                (
                new
                {
                    success = true,
                    data = authorVideo
                }
                );
            }

            return BadRequest(
                new
                {

                    success = false,
                    errorMessage = "Error in imput params!"

                }

                );

        }



        [HttpPost("get/genre/{videoId}")]
        public IActionResult GetGenres(int videoId, [FromBody] IndexIM<TagVideo, GenreFilterIM> model)
        {
            BaseRepository<GenreVideo> repo = new();

            Expression<Func<GenreVideo, bool>> filter = GetFilterGenreVideo(model.Filter, videoId);

            model.Pager ??= new();
            model.Filter ??= new();

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
            var orderByMember = typeof(AuthorVideo).GetProperty(model.Filter.OrderBy, BindingFlags.IgnoreCase
                                                    | BindingFlags.Public | BindingFlags.Instance);


            if (orderByMember == null)
            {
                model.Filter.OrderBy = "id";
            }

            List<Genres> items = repo
                                    .GetAll(filter, model.Pager.Page, model.Pager.ItemsPerPage,
                                                model.Filter.OrderBy, model.Filter.OrderDir)
                                    .Select(x => x.Genre)
                                    .ToList();

            return Ok
                (
                new
                {
                    success = true,
                    data = new
                    {
                        items,
                        model.Pager,
                        model.Filter
                    }

                }
                );
        }

        [HttpGet("genre/{videoId}")]
        public IActionResult GetAllGenresIds(int videoId) 
        {
            BaseRepository<GenreVideo> repo = new();

            var genreIds = repo.GetAll(x => x.VideoId == videoId).Select(x => x.GenreId).ToList();

            return Ok(new 
            {
                success = true,
                data = genreIds
            });
        }

        [HttpPost("genre/{videoId}/{genreId}")]
        public IActionResult AddGenre(int videoId, int genreId)
        {
            BaseRepository<GenreVideo> genreVideoRepo = new();
            BaseRepository<Genres> genreRepo = new();
            BaseRepository<Videos> videoRepo = new();

            if (genreRepo.GetAll(x => x.Id == genreId).Any() && videoRepo.GetAll(x => x.Id == videoId).Any())
            {
                GenreVideo genreVideo = new();
                genreVideo.GenreId = genreId;
                genreVideo.VideoId = videoId;

                genreVideoRepo.Add(genreVideo);

                return Ok
                    (
                    new
                    {
                        success = true,
                        data = genreVideo

                    }
                );
            }
            return BadRequest(
                new
                {

                    success = false,
                    errorMessage = "Error in imput params!"

                }

                );

        }

        [HttpPost("genre/update/{videoId}/")]
        public IActionResult UpdateGenres(int videoId, [FromBody] List<int> genreIds) 
        {
            BaseRepository<GenreVideo> genreVideoRepo = new();
            BaseRepository<Videos> videoRepo = new();
            BaseRepository<Genres> genreRepo = new();

            if (!videoRepo.GetAll(x=> x.Id == videoId).Any()) 
            {
                return BadRequest(new { success = false, errorMessage = "Invalid video ID." });
            }

            var exisitingGenreIds = genreVideoRepo
                .GetAll(x => x.VideoId == videoId)
                .Select(x => x.GenreId)
                .ToList();

            var newGenreIds = genreIds.Distinct().ToList();

            var toAdd = newGenreIds.Except(exisitingGenreIds).ToList();
            var toRemove = exisitingGenreIds.Except(newGenreIds).ToList();

            foreach (var genreId in toAdd) 
            {
                if (genreRepo.GetAll(x => x.Id == genreId).Any()) 
                {
                    genreVideoRepo.Add( new GenreVideo 
                    {
                        VideoId = videoId,
                        GenreId = genreId,
                        IsDeleted = false,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow

                    });
                }
            }

            foreach (var genreId in toRemove)
            {
                var gv = genreVideoRepo.FirstOrDefault(x => x.VideoId == videoId && x.GenreId == genreId);
                if (gv != null) 
                {
                    genreVideoRepo.Delete(gv);
                }
            }
            return Ok(new
            {
                success = true,
                data = new
                {
                    added = toAdd,
                    removed = toRemove
                }
            });

        }

        [HttpDelete("genre/{videoId}")]
        public IActionResult RemoveGenre(int videoId, [FromBody] GenreVideoIM model)
        {
            BaseRepository<GenreVideo> genreVideoRepo = new();
            BaseRepository<Genres> genreRepo = new();
            BaseRepository<Videos> videoRepo = new();

            if (genreRepo.GetAll(x => x.Id == model.GenreId).Any() && genreVideoRepo.GetAll(x => x.GenreId == model.GenreId && x.VideoId == videoId).Any() && videoRepo.GetAll(x => x.Id == videoId).Any())
            {
                GenreVideo genreVideo = genreVideoRepo.FirstOrDefault(x => x.VideoId == videoId && x.GenreId == model.GenreId);
                genreVideoRepo.Delete(genreVideo);

                return Ok
                    (
                    new
                    {
                        success = true,
                        data = genreVideo
                    }
                    );
            }
            return BadRequest(
                new
                {

                    success = false,
                    errorMessage = "Error in imput params!"

                }

                );

        }



        [HttpPost("get/tag/{videoId}")]
        public IActionResult GetTag(int videoId, [FromBody] IndexIM<TagVideo, TagFilterIM> model)
        {
            BaseRepository<TagVideo> repo = new();
            Expression<Func<TagVideo, bool>> filter = GetFilterTagVideo(model.Filter, videoId);

            model.Pager ??= new();
            model.Filter ??= new();

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
            var orderByMember = typeof(AuthorVideo).GetProperty(model.Filter.OrderBy, BindingFlags.IgnoreCase
                                                    | BindingFlags.Public | BindingFlags.Instance);


            if (orderByMember == null)
            {
                model.Filter.OrderBy = "id";
            }

            List<Tags> items = repo
                                .GetAll(filter, model.Pager.Page, model.Pager.ItemsPerPage,
                                                model.Filter.OrderBy, model.Filter.OrderDir)
                                .Select(x => x.Tag)
                                .ToList();

            return Ok
                (
                new
                {
                    success = true,
                    data = new
                    {
                        items,
                        model.Pager,
                        model.Filter
                    }
                }
                );
        }

        [HttpGet("tag/{videoId}")]
        public IActionResult GetAllTagsIds(int videoId)
        {
            BaseRepository<TagVideo> repo = new();

            var tagIds = repo.GetAll(x => x.VideoId == videoId).Select(x => x.TagId).ToList();

            return Ok(new
            {
                success = true,
                data = tagIds
            });
        }

        [HttpPost("tag/{videoId}/{tagId}")]
        public IActionResult AddTag(int videoId, int tagId)
        {
            BaseRepository<TagVideo> tagVideoRepo = new();
            BaseRepository<Tags> tagRepo = new();
            BaseRepository<Videos> videoRepo = new();

            if (tagRepo.GetAll(x => x.Id == tagId).Any() && videoRepo.GetAll(x => x.Id == videoId).Any())
            {
                TagVideo tagVideo = new();

                tagVideo.TagId = tagId;
                tagVideo.VideoId = videoId;

                tagVideoRepo.Add(tagVideo);

                return Ok
                    (
                    new
                    {
                        success = true,
                        data = tagVideo
                    }
                );
            }
            return BadRequest(
                new
                {

                    success = false,
                    errorMessage = "Error in imput params!"

                }

                );

        }

        [HttpPost("tag/update/{videoId}/")]
        public IActionResult UpdateTags(int videoId, [FromBody] List<int> tagIds)
        {
            BaseRepository<TagVideo> tagVideoRepo = new();
            BaseRepository<Videos> videoRepo = new();
            BaseRepository<Tags> tagRepo = new();

            if (!videoRepo.GetAll(x => x.Id == videoId).Any())
            {
                return BadRequest(new { success = false, errorMessage = "Invalid video ID." });
            }

            var exisitingTagIds = tagVideoRepo
                .GetAll(x => x.VideoId == videoId)
                .Select(x => x.TagId)
                .ToList();

            var newTagIds = tagIds.Distinct().ToList();

            var toAdd = newTagIds.Except(exisitingTagIds).ToList();
            var toRemove = exisitingTagIds.Except(newTagIds).ToList();

            foreach (var tagId in toAdd)
            {
                if (tagRepo.GetAll(x => x.Id == tagId).Any())
                {
                    tagVideoRepo.Add(new TagVideo
                    {
                        VideoId = videoId,
                        TagId = tagId,
                        IsDeleted = false,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow

                    });
                }
            }

            foreach (var tagId in toRemove)
            {
                var tv = tagVideoRepo.FirstOrDefault(x => x.VideoId == videoId && x.TagId == tagId);
                if (tv != null)
                {
                    tagVideoRepo.Delete(tv);
                }
            }
            return Ok(new
            {
                success = true,
                data = new
                {
                    added = toAdd,
                    removed = toRemove
                }
            });

        }

        [HttpDelete("tag/{videoId}")]
        public IActionResult RemoveTag(int videoId, [FromBody] TagVideoIM model)
        {
            BaseRepository<TagVideo> tagVideoRepo = new();
            BaseRepository<Tags> tagRepo = new();
            BaseRepository<Videos> videoRepo = new();

            if (tagRepo.GetAll(x => x.Id == model.TagId).Any() && tagVideoRepo.GetAll(x => x.TagId == model.TagId && x.VideoId == videoId).Any() && videoRepo.GetAll(x => x.Id == videoId).Any())
            {
                TagVideo tagVideo = tagVideoRepo.FirstOrDefault(x => x.VideoId == videoId && x.TagId == model.TagId);
                tagVideoRepo.Delete(tagVideo);

                return Ok
                    (
                    new
                    {
                        success = true,
                        data = tagVideo
                    }
                    );

            }
            return BadRequest(
                new
                {

                    success = false,
                    errorMessage = "Error in imput params!"

                }

                );


        }
    }
}
