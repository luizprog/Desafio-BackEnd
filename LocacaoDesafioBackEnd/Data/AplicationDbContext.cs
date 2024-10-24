using LocacaoDesafioBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace LocacaoDesafioBackEnd.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Moto> Motos { get; set; }
        public DbSet<Locacao> Locacoes { get; set; }
        public DbSet<Entregador> Entregadores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define nomes das tabelas em minúsculas
            modelBuilder.Entity<Entregador>().ToTable("entregadores");
            modelBuilder.Entity<Moto>().ToTable("motos").HasIndex(m => m.Placa).IsUnique();
            modelBuilder.Entity<Locacao>().ToTable("locacoes");
        }
    }
}

