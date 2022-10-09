using System.Data;
using DocumentDataAPI.Data.Deployment;
using DocumentDataAPI.Options;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var databaseOptions = builder.Configuration.GetSection(DatabaseOptions.Key).Get<DatabaseOptions>();

// Add services to the container.
builder.Services
    .AddSingleton<DatabaseDeployHelper>()
    .AddTransient<IDbConnection>(_ => new NpgsqlConnection(databaseOptions.ConnectionString))
    ;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Check for "deploy=true" command-line argument
if (app.Configuration.GetValue<bool>("deploy"))
{
    app.Logger.LogInformation("Deploying to schema: {Database}.{Schema}", databaseOptions.Database, databaseOptions.Schema);
    
    var deployHelper = app.Services.GetRequiredService<DatabaseDeployHelper>();
    try
    {
        deployHelper.ExecuteSqlFromFile("deploy_schema.sql");
        if (app.Configuration.GetValue<bool>("populate"))
        {
            deployHelper.ExecuteSqlFromFile("populate_schema.sql");
        }

        app.Logger.LogInformation("Finished!");
    }
    catch (Exception)
    {
        app.Logger.LogError("Deploy was aborted due to errors.");
    }
    return;
}

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