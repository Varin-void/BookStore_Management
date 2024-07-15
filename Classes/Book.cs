using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Final_Project
{
    public class Book
    {
        public Book()
        {
            Authors = new HashSet<Author>();
            Purchases = new HashSet<Purchase>();
            Discounts = new HashSet<Discount>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public int Pages { get; set; }
        public int Price { get; set; }
        public int PrimePrice { get; set; }
        public DateTime PublishDate { get; set; }
        public bool Sequel { get; set; }
        public int Stock { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual Genre Genres { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual ICollection<Discount> Discounts { get; set; }
    }
}
