using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetoCore2.Models;

namespace ProjetoCore2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Inscricao> Inscricao { get; set; }
        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<Modelo> Modelo { get; set; }
        public DbSet<Nivel> Nivel { get; set; }
        public DbSet<Tipo> Tipo { get; set; }
        public DbSet<BlogEntry> Postagem { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<SubCategoria> SubCategoria { get; set; }
        public DbSet<Escala> Escala { get; set; }
    }
}