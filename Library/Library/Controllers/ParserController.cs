using Library.Parser;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class ParserController : Controller
    {
        private readonly IBookParserService parserBooks;

        public ParserController(IBookParserService parserBooks)
        {
            this.parserBooks = parserBooks;
        }

        public IActionResult ParserIndex()
        {
            return View();
        }

        public async Task<IActionResult> Parser(int parsCount)
        {
            await parserBooks.Pars(parsCount);
            return RedirectToAction("ParserFinish");
        }

        public IActionResult ParserFinish()
        {
            ViewBag.Message = "Парсинг книг закончен";
            return View();
        }
    }
}
