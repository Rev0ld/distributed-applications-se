using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repositories
{
    public class BaseRepository<T>
        where T : class
    {
        private DbContext Context { get; set; }

        private DbSet<T> Items { get; set; }

        public BaseRepository()
        {
            Context = new AppDbContext();
            Items = Context.Set<T>();
        }
        public int Count(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = Items;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter)
        {
            return Items.FirstOrDefault(filter);
        }

        public List<T> GetAll(Expression<Func<T, bool>>? filter = null, int page = 1, int itemsPerPage = int.MaxValue,
                                                string? orderBy = null, string? orderDir = null)
        {

            if (orderBy.IsNullOrEmpty())
            {
                orderBy = "id";
            }

            IQueryable<T> query = Items;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var param = Expression.Parameter(typeof(T), "x");

            var orderByMember = typeof(T).GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public
                                                             | BindingFlags.Instance);

            if (orderByMember == null)
            {
                orderBy = "id";
                orderByMember = typeof(T).GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public
                                                             | BindingFlags.Instance);
            }

            var property = Expression.Property(param, orderBy);


            var order = Expression.Lambda<Func<T, object>>(
                                Expression.Convert(property, typeof(object)),
                                param);

            if (orderDir == "desc")
                query = query.OrderByDescending(order);
            else
                query = query.OrderBy(order);

            return query
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();
        }
        public void Add(T item)
        {
            Items.Add(item);
            Context.SaveChanges();
        }

        public void Update(T item)
        {
            Items.Update(item);
            Context.SaveChanges();
        }

        public void Delete(T item)
        {
            Items.Remove(item);
            Context.SaveChanges();
        }


    }
}
