using Microsoft.EntityFrameworkCore;

namespace UpliftedApi2.Models
{
    public partial class UpliftedApiContext : DbContext
    {
        public UpliftedApiContext(DbContextOptions<UpliftedApiContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
