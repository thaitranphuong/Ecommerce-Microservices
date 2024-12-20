﻿using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ProductService.Models;
using System;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public class GrpcProductService : GrpcProduct.GrpcProductBase
    {
        private readonly AppDbContext _context;

        public GrpcProductService(AppDbContext context)
        {
            _context = context;
        }

        public async override Task<ProductResponse> GetProduct(ProductRequest request, ServerCallContext context)
        {
            Product product = await _context.Products.Include(p => p.Unit_).FirstOrDefaultAsync(p => p.Id == request.Id);
            if (product == null) return new ProductResponse()
            {
                Id = 0,
                Name = string.Empty,
                Thumbnail = string.Empty,
                Price = 0,
                Unit = string.Empty,
                Quantity = 0,
                Expiry = 0
            };
            var response = new ProductResponse()
            {
                Id = product.Id,
                Name = product.Name,
                Thumbnail = product.Thumbnail,
                Price = product.Price - product.Price * product.DiscountPercent / 100,
                Unit = product.Unit_.Name,
                Quantity = product.Quantity,
                Expiry = product.Expiry
            };

            return response;
        }
    }
}
