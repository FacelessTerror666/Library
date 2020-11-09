using Library.Domain.Interfaces;
using Library.Domain.Models.Books;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService bookService;
        private readonly IWebHostEnvironment appEnvironment;

        public BooksController(IBookService bookService, IWebHostEnvironment appEnvironment)
        {
            this.bookService = bookService;
            this.appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> BooksList(string bookAuthor, string bookGenre,
            string bookPublisher, string searchString)
        {
            var bookSearch = await bookService.BooksSearchAsync(bookAuthor, bookGenre, bookPublisher, searchString);
            return View(bookSearch);
        }

        [HttpGet]
        public ActionResult ViewBook(long id)
        {
            var model = bookService.ViewBook(id);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBook(BookModel model, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                var uploadedFileName = uploadedFile.FileName;
                var path = "/Files/" + uploadedFileName;

                using (var fileStream = new FileStream(appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                bookService.CreateBook(model, path, uploadedFileName);
            }
            return RedirectToAction(nameof(BooksList));
        }

        [HttpGet]
        public ActionResult CreateBook()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> EditBook(EditBookModel model, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                var uploadedFileName = uploadedFile.FileName;
                var path = "/Files/" + uploadedFileName;

                using (var fileStream = new FileStream(appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                bookService.EditBookPost(model, path, uploadedFileName);
            }
            return RedirectToAction(nameof(BooksList));
        }

        [HttpGet]
        public ActionResult EditBook(long id)
        {
            var editModel = bookService.EditBookGet(id);
            return View(editModel);
        }

        [HttpGet]
        public ActionResult DeleteBook(long id)
        {
            bookService.DeleteBook(id);
            return RedirectToAction(nameof(BooksList));
        }

    }
}
