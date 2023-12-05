using Microsoft.EntityFrameworkCore;
using WebProject.Data;

namespace WebProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=WebProjectDB;Trusted_Connection=True;MultipleActiveResultSets=true"));
            builder.Services.AddMvc(option => option.EnableEndpointRouting = false);

            var app = builder.Build();
            //app.UseRouting();
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            //app.UseMvcWithDefaultRoute(point => {
            //    point.MapControllers();
            //});
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Project}/{action=ListOfProject}");
            });

            app.Run();
        }
    }
}