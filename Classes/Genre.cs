using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Final_Project
{
    public class Genre
    {
        public Genre()
        {
            Books =  new HashSet<Book>();
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
