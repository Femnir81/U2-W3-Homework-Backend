using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace U2_W3_Homework_Backend.Models
{
    public partial class ModelDBContext : DbContext
    {
        public ModelDBContext()
            : base("name=ModelDBContext")
        {
        }

        public virtual DbSet<Ordini> Ordini { get; set; }
        public virtual DbSet<Pizze> Pizze { get; set; }
        public virtual DbSet<Utenti> Utenti { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pizze>()
                .Property(e => e.Prezzo)
                .HasPrecision(15, 2);

            modelBuilder.Entity<Pizze>()
                .HasMany(e => e.Ordini)
                .WithRequired(e => e.Pizze)
                .HasForeignKey(e => e.IDPizze)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Utenti>()
                .HasMany(e => e.Ordini)
                .WithRequired(e => e.Utenti)
                .HasForeignKey(e => e.IDUtenti)
                .WillCascadeOnDelete(false);
        }
    }
}
