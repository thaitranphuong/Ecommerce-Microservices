﻿

namespace CartService.Dtos
{
    public class AsyncMessageDto<T>
    {
        public string EventType { get; set; }
        public T Data { get; set; }
    }
}

