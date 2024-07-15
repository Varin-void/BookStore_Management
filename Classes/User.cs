using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Final_Project
{
    public class User
    {
        public User()
        {

        }
        public int Id { get; set; }
        public string userName { get; set; }
        public string passWord { get; set; }
        public string role { get; set; }
    }
}
