using Library.Database.Entities;
using Library.Database.Interfaces;
using Library.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ThreadingTask = System.Threading.Tasks;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly IRepository<Book> bookRepository;

        public BooksController(IRepository<Book> bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        [HttpGet]
        public async ThreadingTask.Task<IActionResult> BooksList()
        {
            var books = await bookRepository.GetItems()
                .Where(x => x.IsDeleted == false)
                .Select(x => new BooksListModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Author = x.Author,
                    Genre = x.Genre,
                    Publisher = x.Publisher,
                    BookStatus = x.BookStatus
                })
                .OrderBy(x => x.Id)
                .ToListAsync();

            return View(books);
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
                    BookStatus = Database.Enums.BookStatus.Free
                };

                bookRepository.Create(book);

                return RedirectToAction(nameof(BooksList));
        }

        [HttpGet]
        public ActionResult CreateBook()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DeleteBook(long id)
        {
            var existingBook = bookRepository.Get(id);
            existingBook.IsDeleted = true;
            bookRepository.Update(existingBook);

            return RedirectToAction(nameof(BooksList));
        }

    }
}
