﻿using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
            Product product = await _context.Products.FindAsync(request.Id);
            var response = new ProductResponse()
            {
                Id = product.Id,
                Name = product.Name,
                Thumbnail = product.Thumbnail,
                Price = product.Price
            };

            return response;
        }
    }
}