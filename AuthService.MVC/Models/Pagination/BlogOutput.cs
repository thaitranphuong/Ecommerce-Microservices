﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Models.Pagination
{
    public class BlogOutput : AbtractOutput<BlogViewModel>
    {
        public string Title { get; set; }
    }
}
