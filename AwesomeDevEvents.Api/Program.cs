using AwesomeDevEvents.Api.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        //builder.Services.AddDbContext<DevEventDbContext>( o => o.UseInMemoryDatabase("DevEventDb")); // uso da memoria do sistema

        var conectionString = builder.Configuration.GetConnectionString("DevEventsCs"); // acesso ao DB
        builder.Services.AddDbContext<DevEventDbContext>(o => o.UseSqlServer(conectionString));

        //builder.Services.AddDbContext<DevEventDbContext>(options => {
        //    options.UseSqlServer(builder.Configuration.GetConnectionString("DevEventsCs"));
        //});


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(w =>
        {
            w.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "AwesomeDevEvents.API",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Name = "Wesclei",
                    Email = "wescleiaraujo@gmail.com",
                    Url = new Uri("https://www.linkedin.com/in/wesclei-araujo-304773127/")
                }
            });

            var xmlFile = "AwesomeDevEvents.API.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            w.IncludeXmlComments(xmlPath);

        });

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