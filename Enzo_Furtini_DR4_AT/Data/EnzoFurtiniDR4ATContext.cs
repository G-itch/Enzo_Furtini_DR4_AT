using Microsoft.EntityFrameworkCore;
using Enzo_Furtini_DR4_AT.Models;

namespace Enzo_Furtini_DR4_AT.Data
{
    public class EnzoFurtiniDR4ATContext : DbContext
    {
        public EnzoFurtiniDR4ATContext(DbContextOptions<EnzoFurtiniDR4ATContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<PaisDestino> PaisesDestino { get; set; }
        public DbSet<CidadeDestino> CidadesDestino { get; set; }
        public DbSet<PacoteTuristico> PacotesTuristicos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração do Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Telefone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CPF).IsRequired().HasMaxLength(14);
                entity.Property(e => e.DataCadastro).HasDefaultValueSql("GETDATE()");
                
                // Índice único para CPF
                entity.HasIndex(e => e.CPF).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configuração do PaisDestino
            modelBuilder.Entity<PaisDestino>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Codigo).HasMaxLength(3);
            });

            // Configuração do CidadeDestino
            modelBuilder.Entity<CidadeDestino>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descricao).HasMaxLength(500);
                
                // Relacionamento com PaisDestino
                entity.HasOne(e => e.PaisDestino)
                      .WithMany(p => p.Cidades)
                      .HasForeignKey(e => e.PaisDestinoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração do PacoteTuristico
            modelBuilder.Entity<PacoteTuristico>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Descricao).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Preco).HasColumnType("decimal(18,2)");
                
                // Relacionamento muitos-para-muitos com CidadeDestino
                entity.HasMany(e => e.Destinos)
                      .WithMany(c => c.PacotesTuristicos)
                      .UsingEntity(j => j.ToTable("PacoteTuristicoDestinos"));
            });

            // Configuração do Reserva
            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ValorTotal).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Observacoes).HasMaxLength(500);
                
                // Relacionamento com Cliente
                entity.HasOne(e => e.Cliente)
                      .WithMany(c => c.Reservas)
                      .HasForeignKey(e => e.ClienteId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                // Relacionamento com PacoteTuristico
                entity.HasOne(e => e.PacoteTuristico)
                      .WithMany(p => p.Reservas)
                      .HasForeignKey(e => e.PacoteTuristicoId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                // Índice composto para evitar reservas duplicadas
                entity.HasIndex(e => new { e.ClienteId, e.PacoteTuristicoId, e.DataReserva })
                      .IsUnique();
            });

            // Configuração de exclusão lógica global
            modelBuilder.Entity<Cliente>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<PaisDestino>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<CidadeDestino>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<PacoteTuristico>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Reserva>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
} 