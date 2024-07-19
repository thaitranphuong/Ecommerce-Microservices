﻿using ProductService.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Dtos
{
    public class AsyncMessageDto<T>
    {
        public string EventType { get; set; }
        public T Data { get; set; }
    }
}

