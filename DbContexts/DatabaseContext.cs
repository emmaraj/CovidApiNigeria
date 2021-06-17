using CovidApiNigeria.Models;
using Microsoft.EntityFrameworkCore;

namespace CovidApiNigeria.DbContexts {
    public class DatabaseContext : DbContext {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {
        }

        public DbSet<DataModel> CovidNigeriaData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //Set Primary Key for CovidNigeriaData Table
            modelBuilder.Entity<DataModel>()
                .HasKey(o => new { o.StateName, o.Date });
        }
    }
}
