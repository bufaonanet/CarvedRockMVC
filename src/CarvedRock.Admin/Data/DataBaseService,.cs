using Microsoft.EntityFrameworkCore;

namespace CarvedRock.Admin.Data;

public static class DataBaseServicer
{
    public static WebApplication AddMigrations(this WebApplication app)
    {
        using (var scope  = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var ctx = services.GetRequiredService<ProductContext>();
            ctx.Database.Migrate();

            var userCtx = services.GetRequiredService<AdminContext>();
            userCtx.Database.Migrate();

            if (app.Environment.IsDevelopment())
            {
                ctx.SeedInitialData();
            }
        }
        return app;
    }
}