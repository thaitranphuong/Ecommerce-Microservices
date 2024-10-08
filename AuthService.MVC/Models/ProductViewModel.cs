﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public string Thumbnail { get; set; }

        public float Price { get; set; }

        public bool Enabled { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public ICollection<ProductDetailViewModel> ProductDetails { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }
    }
}
