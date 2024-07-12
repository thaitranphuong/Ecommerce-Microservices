using BlogService.Models;
using BlogService.Repositories;
using BlogService.Repositories.Implements;
using BlogService.Services;
using BlogService.Services.Implements;
using BlogService.SyncServices;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlogService", Version = "v1" });
            });

            services.Configure<MongoDBSettings>(Configuration.GetSection("MongoDB"));

            services.AddSingleton<AppDbContext>();

            services.AddAutoMapper(typeof(Startup));

            services.Configure<CloudinarySetting>(Configuration.GetSection("Cloudinary"));
            services.AddSingleton<Cloudinary>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<CloudinarySetting>>().Value;
                return new Cloudinary(new Account(
                    settings.CloudName,
                    settings.ApiKey,
                    settings.ApiSecret));
            });

            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IBlogService, BlogServices>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IGrpcUserService, GrpcUserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogService v1"));
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
