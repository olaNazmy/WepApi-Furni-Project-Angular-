
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using RealApiData.Models;
using RealApiData.Repository;
using RealApiData.Repository.Abstract;
using System.Text;

namespace RealApiData
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();
            //register service for connectionString
            builder.Services.AddDbContext<databaseContext>(op => op.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("con")));
            //configure service for db 

            builder.Services.AddScoped(typeof(GenericRepository<Product>));
            builder.Services.AddScoped(typeof(GenericRepository<User>));
            //
            builder.Services.AddScoped<ProductRepository>();
            builder.Services.AddScoped<IFileService,FileService>();

            //Add cors configuration for allow requests from anyone
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
            //
            builder.Services.AddAuthentication(option =>
            option.DefaultAuthenticateScheme = "myscheme").AddJwtBearer("myscheme",
            op => {

                string key = "Welcome To Our Funi Website, By Ola";
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                op.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = securityKey
                };
            }
            );
           

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
                RequestPath = "/Resources"
            });
            app.UseCors("AllowAllOrigins");

            app.MapControllers();

            app.Run();
        }
    }
}
