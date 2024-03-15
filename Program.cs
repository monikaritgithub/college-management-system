namespace OracleSampleProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "course",
                pattern: "courses/{action=Index}/{id?}",
                defaults: new { controller = "Course" });

            app.MapControllerRoute(
                name: "lesson",
                pattern: "lessons/{action=Index}/{id?}",
                defaults: new { controller = "Lesson" });

            app.MapControllerRoute(
            name: "instructor",
            pattern: "instructors/{action=Index}/{id?}",
            defaults: new { controller = "Instructor" });


            app.Run();
        }
    }
}