﻿using Library.Database;
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
            var authorQuery = bookRepository.GetItems()
                .Where(x => x.IsDeleted == false)
                .OrderByExp("Author")
                .Select(x => x.Author);

            var genreQuery = bookRepository.GetItems()
                .Where(x => x.IsDeleted == false)
                .OrderByExp("Genre")
                .Select(x => x.Genre);

            var publisherQuery = bookRepository.GetItems()
                .Where(x => x.IsDeleted == false)
                .OrderByExp("Publisher")
                .Select(x => x.Publisher);

            var books = bookRepository.GetItems().Where(x => x.IsDeleted == false);

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.WhereExp("Name", "Contains", searchString);
            }

            if (!string.IsNullOrEmpty(bookAuthor))
            {
                books = books.WhereExp("Author", "Contains", bookAuthor);
            }

            if (!string.IsNullOrEmpty(bookGenre))
            {
                books = books.WhereExp("Genre", "Contains", bookGenre);
            }

            if (!string.IsNullOrEmpty(bookPublisher))
            {
                books = books.WhereExp("Publisher", "Contains", bookPublisher);
            }

            var bookSearch = new BooksSearchModel
            {
                Authors = new SelectList(await authorQuery.Distinct().ToListAsync()),
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Publishers = new SelectList(await publisherQuery.Distinct().ToListAsync()),
                Books = await books.OrderByExp("Name").ToListAsync()
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
                Img = existingBook.Img,
                ImgPath = existingBook.ImgPath,
                BookStatus = existingBook.BookStatus
            };
            return viewModel;
        }

        public void CreateBook(BookModel model, string path, string uploadedFileName)
        {
            var book = new Book
            {
                Name = model.Name,
                Author = model.Author,
                Genre = model.Genre,
                Publisher = model.Publisher,
                Description = model.Description,
                Img = uploadedFileName,
                ImgPath = path,
            };

            var books = bookRepository.GetItems().ToList();
            var updateOrCreate = true;

            foreach (var oldBook in books)
            {
                if (book.Name == oldBook.Name &
                    book.Author == oldBook.Author &
                    book.Genre == oldBook.Genre &
                    book.Publisher == oldBook.Publisher)
                {
                    updateOrCreate = false;
                    oldBook.IsDeleted = false;
                    bookRepository.Update(oldBook);
                }
            }

            if (updateOrCreate)
            { 
                bookRepository.Create(book);
            }
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
                Publisher = existingBook.Publisher,
                Description = existingBook.Description,
                Img = existingBook.Img,
                ImgPath = existingBook.ImgPath
            };

            return editModel;
        }

        public void EditBookPost(EditBookModel model, string path, string uploadedFileName)
        {
            var existingBook = bookRepository.Get(model.Id);

            if (existingBook != null)
            {
                existingBook.Id = model.Id;
                existingBook.Name = model.Name;
                existingBook.Genre = model.Genre;
                existingBook.Author = model.Author;
                existingBook.Publisher = model.Publisher;
                existingBook.Description = model.Description;
                existingBook.Img = uploadedFileName;
                existingBook.ImgPath = path;
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
