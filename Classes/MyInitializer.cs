using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Final_Project
{
    public class MyInitializer : CreateDatabaseIfNotExists<DbBookStore>
    {
        protected override void Seed(DbBookStore context)
        {
            IList<Author> authors = new List<Author>();
            authors.Add(new Author
            {
                FirstName = "Isaac",
                LastName = "Asimov"
            });
            authors.Add(new Author
            {
                FirstName = "Clifford",
                LastName = "Simak"
            });
            authors.Add(new Author
            {
                FirstName = "Ray",
                LastName = "Bradbury"
            });
            authors.Add(new Author
            {
                FirstName = "Harry",
                LastName = "Harrison"
            });

            IList<Publisher> publishers = new List<Publisher>();
            publishers.Add(new Publisher
            {
                PublisherName = "HarperCollins"
            });
            publishers.Add(new Publisher
            {
                PublisherName = "Oxford University Press"
            });
            publishers.Add(new Publisher
            {
                PublisherName = "Cambridge University Press"
            });

            IList<Genre> genres = new List<Genre>();
            genres.Add(new Genre
            {
                Description = "Comedy"
            });

            genres.Add(new Genre
            {
                Description = "Novel"
            });

            genres.Add(new Genre
            {
                Description = "Fiction"
            });

            genres.Add(new Genre
            {
                Description = "Thriller"
            });

            genres.Add(new Genre
            {
                Description = "Mystery"
            });

            IList<User> users = new List<User>();
            users.Add(new User
            {
                userName = "admin",
                passWord = "admin",
                role = "Admin"
            });
            users.Add(new User
            {
                userName = "seller",
                passWord = "seller",
                role = "Seller"
            });

            IList<Book> books = new List<Book>();
            books.Add(new Book
            {
                Title = "To Kill a Mockingbird",
                Pages = 100,
                PublishDate = new DateTime(2000, 11, 26),
                PrimePrice = 10,
                Price = 12,
                Sequel = false,
                Stock = 100,
                Publisher = publishers[0],
                Genres = genres[0]
            });

            books.Add(new Book
            {
                Title = "The Lord of the Rings",
                Pages = 400,
                PublishDate = new DateTime(1980, 11, 26),
                PrimePrice = 10,
                Price = 25,
                Sequel = true,
                Stock = 100,
                Publisher = publishers[1],
                Genres = genres[1]
            });

            books[0].Authors.Add(authors[0]);
            books[1].Authors.Add(authors[1]);

            context.Books.Add(books[0]);
            context.Books.Add(books[1]);
            context.Authors.AddRange(authors);
            context.Publishers.AddRange(publishers);
            context.Genres.AddRange(genres);
            context.Users.AddRange(users);

            base.Seed(context);
        }
    }
}
