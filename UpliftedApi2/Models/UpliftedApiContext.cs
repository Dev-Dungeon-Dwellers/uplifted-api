using Microsoft.EntityFrameworkCore;

namespace UpliftedApi2.Models
{
    public partial class UpliftedApiContext : DbContext
    {
        public UpliftedApiContext(DbContextOptions<UpliftedApiContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<UserGroupMapping> UserGroupMappings { get; set; }
        public virtual DbSet<PrayerRequest> PrayerRequests { get; set; }
        public virtual DbSet<PrayerFulfillment> PrayerFulfillments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<UserGroupMapping>(entity =>
            {
                entity.HasKey(e => e.id); // Define the primary key

                // Define foreign key relationship to User
                entity.HasOne(ugm => ugm.MyUser)
                      .WithMany()
                      .HasForeignKey(ugm => ugm.userId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ugm => ugm.MyRole)
                      .WithMany()
                      .HasForeignKey(ugm => ugm.roleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ugm => ugm.MyGroup)
                      .WithMany()
                      .HasForeignKey(ugm => ugm.groupId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PrayerRequest>(entity =>
            {
                entity.HasKey(e => e.Id); // Define the primary key

                // Foreign key to Group
                entity.HasOne(pr => pr.MyGroup)
                      .WithMany()
                      .HasForeignKey(pr => pr.groupId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Foreign key to User
                entity.HasOne(pr => pr.MyUser)
                      .WithMany()
                      .HasForeignKey(pr => pr.userId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PrayerFulfillment>(entity =>
            {
                entity.HasKey(e => e.Id); // Define the primary key

                // Foreign key to User
                entity.HasOne(pr => pr.myPrayerReqest)
                      .WithMany()
                      .HasForeignKey(pr => pr.prayerRequestId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
