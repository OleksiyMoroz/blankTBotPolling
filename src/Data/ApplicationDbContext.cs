using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace src.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {}
        public DbSet<FavoriteVideo> FavoriteVideos { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
    }
}