using Microsoft.EntityFrameworkCore;
using SubredditApp.Data;
using SubredditApp.Service;
using SubredditApp.Services;

namespace SubredditApp
{
    /// <summary>
    /// Class Program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Register the IHttpClientFactory
            builder.Services.AddHttpClient();

            // In Memory Database 
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("YourDatabaseName"));

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IRedditService, RedditService>();
            builder.Services.AddScoped<IDataService, DataService>();
                    
            builder.Services.AddHostedService<SubRedditPoolingService>();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

            app.MapControllers();

            app.Run();
        }
    }
}
