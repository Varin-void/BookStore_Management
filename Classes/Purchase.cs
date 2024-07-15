using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Final_Project
{
    public class Purchase
    {
        public Purchase()
        {

        }
        public int Id { get; set; }
        public string SaleDate { get; set; }
        public int Quantity { get; set; }
        public int TotalCost { get; set; }
        public virtual Book Books { get; set; } 

    }
}
