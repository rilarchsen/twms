using TimeWorkedManagementSystem.Middleware;

namespace TimeWorkedManagementSystem
{
    public static class MiddlewareBuilder
    {
        public static void RegisterMiddleware(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<AuthorizationMiddleware>();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action}/{id?}");
        }
    }
}
