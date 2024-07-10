using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Models.Pagination
{
    public class Pagination
    {
        public int Page { get; set; }
        public int TotalPage { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public float Price { get; set; }
    }
}
