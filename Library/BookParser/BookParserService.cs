﻿using HtmlAgilityPack;
using Library.Database.Entities;
using Library.Database.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Library.Parser
{
    public class BookParserService : IBookParserService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<BookParserService> logger;
        private readonly IHttpClientFactory clientFactory;

        public BookParserService(IServiceProvider serviceProvider, ILogger<BookParserService> logger, IHttpClientFactory clientFactory)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            this.clientFactory = clientFactory;
        }

        public async Task Pars(int parsCount)
        {
            using var scope = serviceProvider.CreateScope();
            var bookRepository = scope.ServiceProvider.GetService<IRepository<Book>>();
            var lastIdRepository = scope.ServiceProvider.GetService<IRepository<LastParserId>>();
            var books = bookRepository.GetItems();
            var httpСlient = clientFactory.CreateClient();
            var startId = lastIdRepository.Get();

            if (startId == null)
            {
                startId = new LastParserId { LastId = 242295 };
                lastIdRepository.Create(startId);
            }

            for (int i = 0; i < parsCount; i++)
            {
                //получение страницы сайта "Читай город"                   
                var response = await httpСlient.GetAsync("https://www.chitai-gorod.ru/catalog/book/" + startId.LastId);
                var pageContents = await response.Content.ReadAsStringAsync();
                var pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContents);

                //проверка на существование страницы 
                if ((pageDocument.DocumentNode.SelectSingleNode("//title") == null) 
                    ^ (pageDocument.DocumentNode.SelectSingleNode("//title").InnerText == "Читай-город 404"))
                {
                    startId.LastId++;
                    parsCount++;
                    continue;
                }

                //параметры книги 
                var bookName = "Название не найдено";
                var bookAuthor = "Автор не найден";
                var bookGenre = "Жанр не найден";
                var bookPublisher = "Издатель не найден";
                var bookDesc = "Это книга.";
                var bookImg = pageDocument.DocumentNode.SelectSingleNode("//div[@class='product__image']//img[@class='lazyload']")
                    .Attributes["data-src"].Value;

                //проверка на существование параметров и присвоение их
                if (pageDocument.DocumentNode.SelectSingleNode("//meta[@property='og:title']") != null)
                {
                    bookName = pageDocument.DocumentNode
                        .SelectSingleNode("//meta[@property='og:title']").Attributes["content"].Value;
                }

                if (pageDocument.DocumentNode.SelectSingleNode("//meta[@property='book:author']") != null)
                {
                    bookAuthor = pageDocument.DocumentNode
                        .SelectSingleNode("//meta[@property='book:author']").Attributes["content"].Value;
                }

                if (pageDocument.DocumentNode.SelectSingleNode("(//div[@class='container']//span[@itemprop='name'])[4]") != null)
                {
                    bookGenre = pageDocument.DocumentNode
                        .SelectSingleNode("(//div[@class='container']//span[@itemprop='name'])[4]").InnerText;
                }

                if (pageDocument.DocumentNode.SelectSingleNode("(//div[@class='product-prop__value']//a[@class='link'])[1]") != null)
                {
                    bookPublisher = pageDocument.DocumentNode
                        .SelectSingleNode("(//div[@class='product-prop__value']//a[@class='link'])[1]").InnerText;
                }

                if (pageDocument.DocumentNode.SelectSingleNode("//div[@itemprop='description']") != null)
                {
                    bookDesc = pageDocument.DocumentNode.SelectSingleNode("//div[@itemprop='description']").InnerText;
                }

                //скачивание обложки
                var path = "/Files/" + bookName + ".jpg";
                var localFilename = "wwwroot" + path;
                var webClient = new WebClient();
                {
                    webClient.DownloadFile(bookImg, localFilename);
                }

                var book = new Book
                {
                    Name = bookName,
                    Author = bookAuthor,
                    Genre = bookGenre,
                    Publisher = bookPublisher,
                    Description = bookDesc,
                    Img = bookName,
                    ImgPath = path,
                };

                await bookRepository.CreateAsync(book);
                startId.LastId++;
                lastIdRepository.Update(startId);
                logger.LogInformation("Добавлена книга: " + bookName);
            }
        }
    }
}
