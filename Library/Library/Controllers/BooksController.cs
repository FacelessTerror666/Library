using Library.Database.Entities;
using Library.Database.Interfaces;
using Library.Domain.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository<Book> bookRepository;

        public BooksController(IBookRepository<Book> bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public async Task<IActionResult> BooksList(string bookAuthor, string bookGenre, string bookPublisher, string searchString)
        {
            IQueryable<string> authorQuery = from b in bookRepository.GetBooks()
                                             where b.IsDeleted == false
                                             orderby b.Author
                                             select b.Author;

            IQueryable<string> genreQuery = from b in bookRepository.GetBooks()
                                            where b.IsDeleted == false
                                            orderby b.Genre
                                            select b.Genre;

            IQueryable<string> publisherQuery = from b in bookRepository.GetBooks()
                                                where b.IsDeleted == false
                                                orderby b.Publisher
                                                select b.Publisher;

            var books = from m in bookRepository.GetBooks()
                        where m.IsDeleted == false
                        select new BooksListModel
                        {
                            Id = m.Id,
                            Name = m.Name,
                            Author = m.Author,
                            Genre = m.Genre,
                            Publisher = m.Publisher,
                            BookStatus = m.BookStatus
                        };

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

            var bookFilter = new BooksListModel
            {
                Authors = new SelectList(await authorQuery.Distinct().ToListAsync()),
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Publishers = new SelectList(await publisherQuery.Distinct().ToListAsync()),
                Books = await books.OrderBy(m => m.Name).ToListAsync()
            };

            return View(bookFilter);
        }

        [HttpGet]
        public ActionResult ViewBook(long id)
        {
            var existingBook = bookRepository.GetBooks()
                .FirstOrDefault(x => x.Id == id);

            var model = new BookViewModel
            {
                Id = existingBook.Id,
                Name = existingBook.Name,
                Author = existingBook.Author,
                Genre = existingBook.Genre,
                Publisher = existingBook.Publisher,
                Description = existingBook.Description,
                BookStatus = existingBook.BookStatus
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateBook(BookModel model)
        {
            var book = new Book
            {
                Name = model.Name,
                Author = model.Author,
                Genre = model.Genre,
                Publisher = model.Publisher,
                Description = model.Description,
                BookStatus = Database.Enums.BookStatus.Free
            };

            bookRepository.CreateBook(book);

            return RedirectToAction(nameof(BooksList));
        }

        [HttpGet]
        public ActionResult CreateBook()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditBook(long id)
        {
            var existingBook = bookRepository.GetBooks()
                .FirstOrDefault(x => x.Id == id);
            if (existingBook == null)
            {
                return RedirectToAction(nameof(BooksList));
            }
            var editModel = new EditBookModel
            {
                Id = existingBook.Id,
                Name = existingBook.Name,
                Genre = existingBook.Genre,
                Author = existingBook.Author,
                Publisher = existingBook.Publisher
            };
            return View(editModel);
        }

        [HttpPost]
        public ActionResult EditBook(EditBookModel model)
        {
            var existingBook = bookRepository.GetBooks()
            .FirstOrDefault(x => x.Id == model.Id);

            if (existingBook == null)
            {
                return RedirectToAction(nameof(BooksList));
            }

            existingBook.Name = model.Name;
            existingBook.Genre = model.Genre;
            existingBook.Author = model.Author;
            existingBook.Publisher = model.Publisher;

            bookRepository.UpdateBook(existingBook);

            return RedirectToAction(nameof(BooksList));

        }

        [HttpGet]
        public ActionResult DeleteBook(long id)
        {
            var existingBook = bookRepository.GetBook(id);
            existingBook.IsDeleted = true;
            bookRepository.UpdateBook(existingBook);

            return RedirectToAction(nameof(BooksList));
        }

    }
}
