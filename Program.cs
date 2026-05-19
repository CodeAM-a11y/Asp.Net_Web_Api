using Microsoft.EntityFrameworkCore;
using Zählerstände.Data;

namespace Zählerstände;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        var config = builder.Configuration;
        var connStr = config.GetConnectionString("db");
        builder.Services.AddControllers();
        builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDbContext<ZählerständeDbContext>(opts => opts.UseSqlite(connStr));
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");
        app.MapControllers();
        app.Run();
    }
}