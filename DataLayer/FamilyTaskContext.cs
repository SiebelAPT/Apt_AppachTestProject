using Core.Abstractions;
using Domain.DataModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataLayer
{
    public class FamilyTaskContext : DbContext
    {

        public FamilyTaskContext(DbContextOptions<FamilyTaskContext> options):base(options)
        {

        }

        public DbSet<MemberDm> Members { get; set; }
        public DbSet<TaskDm> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MemberDm>(entity => {
                entity.HasKey(k => k.Id);
                entity.ToTable("Member");
            });

            modelBuilder.Entity<TaskDm>(entity => {
                entity.HasKey(k => k.Id);
                entity.ToTable("Task");
            });
        }
    }
}