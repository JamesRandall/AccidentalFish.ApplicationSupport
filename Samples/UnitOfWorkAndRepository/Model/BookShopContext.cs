using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace UnitOfWorkAndRepository.Model
{
    public class BookShopContext : DbContext
    {
        protected BookShopContext()
        {
        }

        public BookShopContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }
    }
}
