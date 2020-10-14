using Library.Database.Entities;
using Library.Database.Interfaces;
using Library.Domain.Interfaces;
using Library.Domain.Models.Books;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Domain.Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> bookRepository;

        public BookService(IRepository<Book> bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public async Task<BooksSearchModel> BooksSearchAsync(string bookAuthor, string bookGenre, 
            string bookPublisher, string searchString)
        {
            IQueryable<string> authorQuery = bookRepository.GetItems()
                                             .Where(x => x.IsDeleted == false)
                                             .OrderBy(x => x.Author)
                                             .Select(x => x.Author);

            IQueryable<string> genreQuery = bookRepository.GetItems()
                                             .Where(x => x.IsDeleted == false)
                                             .OrderBy(x => x.Genre)
                                             .Select(x => x.Genre);

            IQueryable<string> publisherQuery = bookRepository.GetItems()
                                             .Where(x => x.IsDeleted == false)
                                             .OrderBy(x => x.Publisher)
                                             .Select(x => x.Publisher);

            var books = bookRepository.GetItems().Where(x => x.IsDeleted == false);

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(x => x.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(bookAuthor))
            {
                books = books.Where(x => x.Author == bookAuthor);
            }

            if (!string.IsNullOrEmpty(bookGenre))
            {
                books = books.Where(x => x.Genre == bookGenre);
            }

            if (!string.IsNullOrEmpty(bookPublisher))
            {
                books = books.Where(x => x.Publisher == bookPublisher);
            }

            var bookSearch = new BooksSearchModel
            {
                Authors = new SelectList(await authorQuery.Distinct().ToListAsync()),
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Publishers = new SelectList(await publisherQuery.Distinct().ToListAsync()),
                Books = await books.OrderBy(m => m.Name).ToListAsync()
            };

            return bookSearch;
        }

        public BookViewModel ViewBook(long id)
        {
            var existingBook = bookRepository.Get(id);

            var viewModel = new BookViewModel
            {
                Id = existingBook.Id,
                Name = existingBook.Name,
                Author = existingBook.Author,
                Genre = existingBook.Genre,
                Publisher = existingBook.Publisher,
                Description = existingBook.Description,
                BookStatus = existingBook.BookStatus
            };
            return viewModel;
        }

        public void CreateBook(BookModel model)
        {
            var book = new Book
            {
                Name = model.Name,
                Author = model.Author,
                Genre = model.Genre,
                Publisher = model.Publisher,
                Description = model.Description,
                BookStatus = model.BookStatus
            };

            bookRepository.Create(book);
        }

        public EditBookModel EditBookGet(long id)
        {
            var existingBook = bookRepository.Get(id);

            var editModel = new EditBookModel
            {
                Id = existingBook.Id,
                Name = existingBook.Name,
                Genre = existingBook.Genre,
                Author = existingBook.Author,
                Publisher = existingBook.Publisher
            };

            return editModel;
        }

        public void EditBookPost(EditBookModel model)
        {
            var existingBook = bookRepository.Get(model.Id);

            if (existingBook == null)
            {
                existingBook.Name = model.Name;
                existingBook.Genre = model.Genre;
                existingBook.Author = model.Author;
                existingBook.Publisher = model.Publisher;

                bookRepository.Update(existingBook);
            }
        }

        public void DeleteBook(long id)
        {
            var existingBook = bookRepository.Get(id);
            existingBook.IsDeleted = true;
            bookRepository.Update(existingBook);
        }

    }
}
