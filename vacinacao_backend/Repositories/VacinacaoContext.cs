using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using vacinacao_backend.Models;

namespace vacinacao_backend.Repositories {
    public class VacinacaoContext : DbContext {

        public DbSet<Agenda> Agendamentos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Alergia> Alergias { get; set; }
        public DbSet<Vacina> Vacinas { get; set; }

        public VacinacaoContext(DbContextOptions<VacinacaoContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Agenda>()
                .HasOne(a => a.Usuario)
                .WithMany(u => u.Agendamentos)
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Agenda>()
                .HasOne(a => a.Vacina)
                .WithMany(v => v.Agendamentos)
                .HasForeignKey(a => a.VacinaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Alergia>()
                .HasMany(a => a.Usuarios)
                .WithMany(u => u.Alergias);

            modelBuilder.Entity<Alergia>()
                .HasOne(a => a.Vacina)
                .WithOne()
                .HasForeignKey<Alergia>(a => a.VacinaId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
