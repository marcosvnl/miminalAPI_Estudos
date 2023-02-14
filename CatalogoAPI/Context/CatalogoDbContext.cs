using CatalogoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI.Context
{
    public class CatalogoDbContext : DbContext
    {
        public CatalogoDbContext(DbContextOptions<CatalogoDbContext> options) : base(options) { }
        
        DbSet<Produto>? Produtos { get; set; }   
        DbSet<Categoria>? Categorias { get; set; }

        //FluentAPI EF
        protected override void OnModelCreating(ModelBuilder model)
        {
            //Categoria
            model.Entity<Categoria>().HasKey(c => c.CategoriaId);
            model.Entity<Categoria>().Property(c => c.Nome).HasMaxLength(100).IsRequired();
            model.Entity<Categoria>().Property(c => c.Descricao).HasMaxLength(150).IsRequired();

            //Produto
            model.Entity<Produto>().HasKey(p => p.ProdutoId);
            model.Entity<Produto>().Property(p => p.Nome).HasMaxLength(100).IsRequired();
            model.Entity<Produto>().Property(p => p.Descricao).HasMaxLength(150);
            model.Entity<Produto>().Property(p => p.Imagem).HasMaxLength(100);
            model.Entity<Produto>().Property(p => p.Preco).HasPrecision(14, 2);

            //Relacionamento
            model.Entity<Produto>()
                .HasOne<Categoria>(c => c.Categoria)
                .WithMany(p => p.Produtos)
                .HasForeignKey(c => c.CategoriaId); 
        }
    }
}
