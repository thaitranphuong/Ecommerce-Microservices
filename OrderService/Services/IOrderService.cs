﻿using OrderService.Constants;
using OrderService.Dtos;
using OrderService.Dtos.Paginations;
using OrderService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Services
{
    public interface IOrderService
    {
        Task<bool> Save(OrderDto dto);
        Task<bool> Update(int id, int status);
        Task<bool> UpdateOrderDetails(OrderDetailDto[] orderDetailDtos);
        Task<OrderDto> FindById(int id);
        Task<OrderOutput> FindAll(OrderStatus status, int page, int limit);
        Task<List<OrderDto>> FindAllByUserId(string userId, OrderStatus status);
        Task<List<OrderDto>> FindAllByYear(int year);
        Task<List<OrderDto>> FindAllByYearToStatistic(int year);

        Task<List<OrderDto>> FindAllByMonthToStatistic(int month, int year);

        Task<List<OrderDto>> FindAllByDateToStatistic(DateTime startDate, DateTime endDate);
    }
}
