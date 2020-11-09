using Library.Domain.Models.Books;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IBookService
    {
        public Task<BooksSearchModel> BooksSearchAsync(string bookAuthor, string bookGenre, 
            string bookPublisher, string searchString);

        public BookViewModel ViewBook(long id);

        public void CreateBook(BookModel model, string path, string uploadedFileName);

        public EditBookModel EditBookGet(long id);

        public void EditBookPost(EditBookModel model, string path, string uploadedFileName);

        public void DeleteBook(long id);
    }
}
