using LocacaoDesafioBackEnd.Models;
using LocacaoDesafioBackEnd.Models.Notifications;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LocacaoDesafioBackEnd.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Moto> Motos { get; set; }
        public DbSet<Locacao> Locacoes { get; set; }
        public DbSet<Entregador> Entregadores { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }
        public DbSet<MotoNotificacao> MotoNotificacoes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define nomes das tabelas em minúsculas
            modelBuilder.Entity<Entregador>().ToTable("entregadores");
            modelBuilder.Entity<Moto>().ToTable("motos").HasIndex(m => m.Placa).IsUnique();
            modelBuilder.Entity<Locacao>().ToTable("locacoes");
            modelBuilder.Entity<MotoNotificacao>()
            .HasOne(mn => mn.Notificacao)
            .WithMany()
            .HasForeignKey(mn => mn.NotificacaoId);
        }
    }
}

