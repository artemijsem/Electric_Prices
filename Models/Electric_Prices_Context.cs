using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Electric_Prices.Models
{
    public partial class Electric_Prices_Context : DbContext
    {
    
        public Electric_Prices_Context()
        {
        }

        public Electric_Prices_Context(DbContextOptions<Electric_Prices_Context> options)
            : base(options)
        {
        }

        public virtual DbSet<EpApiTable> EpApiTables { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=Electric_Prices_Db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EpApiTable>(entity =>
            {
                

                entity.ToTable("EP_API_TABLE");

                entity.Property(e => e.Data).HasColumnName("data");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date")
                    .HasDefaultValueSql("(getdate())");

                

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Success).HasColumnName("success");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            });
            
            

            OnModelCreatingPartial(modelBuilder);

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
