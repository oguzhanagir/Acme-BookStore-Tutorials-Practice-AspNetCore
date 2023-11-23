using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore;

public class BookStoreDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Book, Guid> _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly AuthorManager _authorManager;
    public BookStoreDataSeederContributor(IRepository<Book, Guid> bookRepository, AuthorManager authorManager, IAuthorRepository authorRepository)
    {
        _bookRepository = bookRepository;
        _authorManager = authorManager;
        _authorRepository = authorRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _bookRepository.GetCountAsync() > 0)
        {
            return;
        }

        var orwell = await _authorRepository.InsertAsync(
            await _authorManager.CreateAsync(
                "George Orwell",
                new DateTime(1903, 06, 25),
                "Orwell produced literary criticism and poetry, fiction and polemical journalism; and is best known for the allegorical novella Animal Farm (1945) and the dystopian novel Nineteen Eighty-Four (1949)."
            )
        );

        var douglas = await _authorRepository.InsertAsync(
            await _authorManager.CreateAsync(
                "Douglas Adams",
                new DateTime(1952, 03, 11),
                "Douglas Adams was an English author, screenwriter, essayist, humorist, satirist and dramatist. Adams was an advocate for environmentalism and conservation, a lover of fast cars, technological innovation and the Apple Macintosh, and a self-proclaimed 'radical atheist'."
            )
        );


        var keith = await _authorRepository.InsertAsync(
                 await _authorManager.CreateAsync(
                     "Keith Ferrazzi",
                     new DateTime(1966, 07, 14),
                     "Keith Ferrazzi is an American entrepreneur and recognized global thought leader in the relational and collaborative sciences. As Chairman of Ferrazzi Greenlight and its Research Institute, he works to identify behaviors that block global organizations from reaching their goals and to transform them by coaching new behaviors that increase growth and shareholder value."
                 )
             );

        await _bookRepository.InsertAsync(
            new Book
            {
                AuthorId = orwell.Id, 
                Name = "1984",
                Type = BookType.Dystopia,
                PublishDate = new DateTime(1949, 6, 8),
                Price = 19.84f
            },
            autoSave: true
        );

        await _bookRepository.InsertAsync(
            new Book
            {
                AuthorId = douglas.Id, 
                Name = "The Hitchhiker's Guide to the Galaxy",
                Type = BookType.ScienceFiction,
                PublishDate = new DateTime(1995, 9, 27),
                Price = 42.0f
            },
            autoSave: true
        );

        await _bookRepository.InsertAsync(
            new Book
            {
                AuthorId = douglas.Id,
                Name = "Never Eat Alone",
                Type = BookType.ScienceFiction,
                PublishDate = new DateTime(2014, 12, 15),
                Price = 150.0f
            },
            autoSave: true
        );
    }
}