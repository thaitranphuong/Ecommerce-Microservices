using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OrderService.AsyncServices;
using OrderService.Models;
using OrderService.Repositories;
using OrderService.Repositories.Implements;
using OrderService.Services;
using OrderService.Services.Implements;
using OrderService.SyncServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderService", Version = "v1" });
            });

            services.AddDbContext<AppDbContext>(options =>
             options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
             new MySqlServerVersion(new Version(8, 0, 0))));

            services.AddAutoMapper(typeof(Startup));

            services.AddGrpc();

            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IOrderService, OrderServices>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IGrpcProductService, GrpcProductService>();
            services.AddScoped<IGrpcUserService, GrpcUserService>();
            services.AddScoped<IMessageProducer, MessageProducer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GrpcOrderDetailService>();
                endpoints.MapControllers();
            });
        }
    }
}
