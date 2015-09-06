using System;
using System.Data.Entity;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Repository;
using AccidentalFish.ApplicationSupport.Unity;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework;
using Microsoft.Practices.Unity;
using UnitOfWorkAndRepository.Model;

namespace UnitOfWorkAndRepository
{
    /// <summary>
    /// This sample demonstrates basic use of the unit of work and repository patterns.
    /// </summary>
    class Program
    {

        private static IUnitOfWorkFactory _unitOfWorkFactory;

        static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            UnityApplicationFrameworkDependencyResolver resolver = new UnityApplicationFrameworkDependencyResolver(container);

            resolver
                .UseCore()
                .UseEntityFramework();
            
            IUnitOfWorkFactoryProvider provider = container.Resolve<IUnitOfWorkFactoryProvider>();
            _unitOfWorkFactory = provider.Create(
                "UnitOfWorkAndRepository.Model.BookShopContext, UnitOfWorkAndRepository",
                @"Server=(local);Database=BookStoreSample;Connection Timeout=30;;Integrated Security=True");
            Database.SetInitializer(new DropCreateDatabaseAlways<BookShopContext>());
            DemonstrateInsert().Wait();
            DemonstrateSimpleFetch().Wait();

            Console.WriteLine("\nPress a key...");
            Console.ReadKey();
        }

        static async Task DemonstrateInsert()
        {
            using (IUnitOfWorkAsync unitOfWork = _unitOfWorkFactory.CreateAsync())
            {
                IRepositoryAsync<Book> bookRepository = unitOfWork.GetRepository<Book>();
                IRepositoryAsync<Author> authorRepository = unitOfWork.GetRepository<Author>();

                Author douglasAdams = new Author
                {
                    Name = "Douglas Adams"
                };
                Author williamShakespeare = new Author
                {
                    Name = "William Shakespeare"
                };

                authorRepository.Insert(douglasAdams);
                authorRepository.Insert(williamShakespeare);

                bookRepository.Insert(new Book
                {
                    Author = douglasAdams,
                    Title = "The Hitchikers Guide To The Galaxy"
                });
                bookRepository.Insert(new Book
                {
                    Author = douglasAdams,
                    Title = "Life, The Universe and Everything"
                });
                bookRepository.Insert(new Book
                {
                    Author = williamShakespeare,
                    Title = "Romeo and Juliet"
                });
                bookRepository.Insert(new Book
                {
                    Author = williamShakespeare,
                    Title = "Twelfth Night"
                });

                await unitOfWork.SaveAsync();

                Console.WriteLine("Saved books and authors");
            }
        }

        static async Task DemonstrateSimpleFetch()
        {
            using (IUnitOfWorkAsync unitOfWork = _unitOfWorkFactory.CreateAsync())
            {
                IRepositoryAsync<Author> authorRepository = unitOfWork.GetRepository<Author>();
                Author author = await authorRepository
                    .AllIncluding(x => x.Books)
                    .SingleAsync(x => x.Name == "Douglas Adams");
                Console.WriteLine(author.Name);
                foreach (Book book in author.Books)
                {
                    Console.WriteLine("\t{0}", book.Title);
                }
            }
        }
    }
}
