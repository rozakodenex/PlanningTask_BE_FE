
using PlanningTasksEF.Data;
using PlanningTasksEF.Hubs;

namespace PlanningTasksEF
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            builder.Services.AddCors(options =>
                options.AddPolicy(name: "kodok", policy =>
                policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((x) => true)
                .AllowCredentials()

                )
            );

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DataContext>();

            builder.Services.AddSignalR();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Tasks}/{action}/{id?}");

            app.MapHub<EventHub>("/EventHub");

            app.MapHub<ChatHub>("/chatHub");

            app.MapControllers();

            app.UseCors("kodok");

            app.Run();
        }
    }
}