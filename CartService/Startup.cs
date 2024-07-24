using CartService.AsyncServices;
using CartService.Models;
using CartService.Repositories;
using CartService.Services;
using CartService.SyncServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace CartService
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CartService", Version = "v1" });
            });

            services.AddDbContext<AppDbContext>(options =>
             options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
             new MySqlServerVersion(new Version(8, 0, 0))));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetSection("Redis:ConnectionString").Value;
            });

            services.AddTransient<IDbConnection>((sp) =>
            {
                return new MySqlConnection(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSingleton<IEventProcessor, EventProcessor>();
            services.AddHostedService<MessageConsumer>();

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<ICartItemService, CartItemService>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IGrpcProductService, GrpcProductService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CartService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
