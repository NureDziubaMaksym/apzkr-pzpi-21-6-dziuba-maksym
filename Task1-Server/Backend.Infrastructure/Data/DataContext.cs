using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Infrastructure.Models;

namespace Backend.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<FocusGroup> FocusGroups { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<FillingGroup> FillingGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FillingGroup>()
                .HasKey(fg => fg.AddGrId);

            modelBuilder.Entity<FillingGroup>()
                .HasOne(fg => fg.User)
                .WithMany(u => u.FillingGroups)
                .HasForeignKey(fg => fg.UserId);

            modelBuilder.Entity<FillingGroup>()
                .HasOne(fg => fg.FocusGroup)
                .WithMany(fg => fg.FillingGroups)
                .HasForeignKey(fg => fg.FocusId);

            modelBuilder.Entity<FocusGroup>()
                .HasKey(fg => fg.FocGrId);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.FocusGroup)
                .WithMany(fg => fg.Sessions)
                .HasForeignKey(s => s.FocusGroupId);



            base.OnModelCreating(modelBuilder);
        }
    }
}
