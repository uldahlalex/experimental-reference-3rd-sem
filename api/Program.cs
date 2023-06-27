using api.ActionFilters;
using api.Helpers;
using api.Middleware;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.Net.Http.Headers;
using Npgsql;
using Service;

var builder = WebApplication.CreateBuilder(args);

//SETUP LOGGER
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddNpgsqlDataSource(InfrastructureUtilityService.ProperlyFormattedConnectionString,
        dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
}

if (builder.Environment.IsProduction())
{
    builder.Services.AddNpgsqlDataSource(InfrastructureUtilityService.ProperlyFormattedConnectionString);
}

//SETUP REPOSITORIES
builder.Services.AddSingleton<ReadingListRepository>();
builder.Services.AddSingleton<BookRepository>();
builder.Services.AddSingleton<AuthorRepository>();
builder.Services.AddSingleton<UserRepository>();

//NON-API SERVICES
builder.Services.AddSingleton<BookService>();
builder.Services.AddSingleton<ReadingListService>();
builder.Services.AddSingleton<AuthorService>();
builder.Services.AddSingleton<BookService>();
builder.Services.AddSingleton<AuthenticationService>();
builder.Services.AddSingleton<ResponseHelper>();


//MIDDLEWARE
builder.Services.AddCors();

//SETUP OTHER SERVICES
builder.Services.AddControllers();
builder.Services.AddSpaStaticFiles(configuration => { configuration.RootPath = "./../frontend/www/"; });
builder.Services.AddScoped<AuthenticationFilter>();

if (builder.Environment.IsDevelopment())
{
    //FOR SWAGGER / OPENAPI IN DEV MODE
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});


app.UseSpaStaticFiles(new StaticFileOptions()
{
    OnPrepareResponse = ctx =>
    {
        const int durationInSeconds = 60 * 60 * 24;
        ctx.Context.Response.Headers[HeaderNames.CacheControl] =
            "public,max-age=" + durationInSeconds;
    }
});

app.Map("/frontend",
    (IApplicationBuilder frontendApp) => { frontendApp.UseSpa(spa => { spa.Options.SourcePath = "./app/www/"; }); });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RouteCheck>();



//Testing if run command has correctly passed arguments
InfrastructureUtilityService.TestDataSource(app.Services.GetService<NpgsqlDataSource>()!);
app.Services.GetService<AuthenticationService>()!.PrintWarningIfSecretIsNotValid();

app.Run();