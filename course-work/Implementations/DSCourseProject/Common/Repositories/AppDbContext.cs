using Common.Entities.M2MEntities;
using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repositories
{
    class AppDbContext : DbContext
    {
        public DbSet<Authors> Authors { get; set; }

        public DbSet<Copyrights> Copyrights { get; set; }

        public DbSet<Formats> Formats { get; set; }

        public DbSet<Genres> Genres { get; set; }

        public DbSet<Tags> Tags { get; set; }

        public DbSet<Videos> Videos { get; set; }

        public DbSet<AuthorVideo> AuthorVideos { get; set; }

        public DbSet<GenreVideo> GenreVideos { get; set; }

        public DbSet<TagVideo> TagVideos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=IVAN-VIPER\\SQLEXPRESS;Database=DSCourseProjectDb-VideoIndexer;Trusted_Connection=True;TrustServerCertificate=True;")
                            .UseLazyLoadingProxies(); ;
        }
    }
}
