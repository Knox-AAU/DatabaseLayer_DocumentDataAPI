using System.Reflection;
using Dapper.FluentMap;
using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Algorithms;
using DocumentDataAPI.Data.Deployment;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Mappers.BiasSchema;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Data.Repositories.BiasSchema;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Data.Services;
using DocumentDataAPI.Options;
using Microsoft.OpenApi.Models;
using Serilog;

const string apiVersion = "v1.3.6";
var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile(Path.Combine(Environment.CurrentDirectory, "appsettings.local.json"), true, true);
}

var databaseOptions = builder.Configuration.GetSection(DatabaseOptions.Key).Get<DatabaseOptions>();

// Add services to the container.
builder.Services
    .AddSingleton<ISqlHelper, DapperSqlHelper>()
    .AddSingleton<IRelevanceFunction, CosineSimilarityCalculator>()
    .AddScoped<IDbConnectionFactory>(_ => new NpgDbConnectionFactory(databaseOptions))
    .AddScoped<DatabaseDeployHelper>()
    .AddScoped<IDocumentContentRepository, NpgDocumentContentRepository>()
    .AddScoped<ISimilarDocumentRepository, NpgSimilarDocumentRepository>()
    .AddScoped<IDocumentRepository, NpgDocumentRepository>()
    .AddScoped<ISourceRepository, NpgSourceRepository>()
    .AddScoped<IWordRatioRepository, NpgWordRatioRepository>()
    .AddScoped<IWordRelevanceRepository, NpgWordRelevanceRepository>()
    .AddScoped<ISearchRepository, NpgSearchRepository>()
    .AddScoped<ICategoryRepository, NpgCategoryRepository>()
    .AddScoped<IBiasDocumentRepository, NpgBiasDocumentRepository>()
    .AddScoped<IBiasPoliticalPartiesRepository, NpgBiasPoliticalPartiesRepository>()
    .AddScoped<IBiasWordCountRepository, NpgBiasWordCountRepository>()
    .AddHttpClient<ILemmatizerService, LemmatizerService>()
    ;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "DocumentDataAPI",
        Version = apiVersion,
        Description =
            "The DocumentDataAPI is the main interface to the document_data database schema, " +
            "and provides endpoints to retrieve, insert, update, and delete its contents."
    });
    string xmlDocFilePath =
        Path.Combine(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name + ".xml");
    config.IncludeXmlComments(xmlDocFilePath);
});
builder.Services.AddCors(options =>
    {
        options.AddPolicy("UnsafeMode",
            policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        options.AddPolicy(name: "KnoxAllowedOrigins",
            policy =>
            {
                policy.WithOrigins("localhost", "http://knox-master01.srv.aau.dk/")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    }
);
builder.Host.UseSerilog((context, config) => { config.WriteTo.Console(); });

var app = builder.Build();

// Set up Dapper mappers
FluentMapper.Initialize(config =>
{
    config.AddMap(new DocumentContentMap());
    config.AddMap(new DocumentMap());
    config.AddMap(new WordRatioMap());
    config.AddMap(new SourceMap());
    config.AddMap(new CategoryMap());
    config.AddMap(new SimilarDocumentMap());
    config.AddMap(new BiasDocumentMap());
    config.AddMap(new BiasPoliticalPartiesMap());
    config.AddMap(new BiasWordCountMap());
});

// Check for "deploy=true" command-line argument
if (app.Configuration.GetValue<bool>("deploy"))
{
    app.Logger.LogInformation("Deploying to schema: {Database}.{Schema}", databaseOptions.Database,
        databaseOptions.DocumentDataSchema);
    app.Logger.LogInformation("Deploying to schema: {Database}.{Schema}", databaseOptions.Database,
        databaseOptions.BiasSchema);
    using IServiceScope scope = app.Services.CreateScope();
    var deployHelper = scope.ServiceProvider.GetRequiredService<DatabaseDeployHelper>();
    try
    {
        deployHelper.ExecuteSqlFromFile("deploy_schema.sql", DatabaseOptions.Schema.DocumentData);
        if (app.Configuration.GetValue<bool>("populate"))
        {
            deployHelper.ExecuteSqlFromFile("populate_tables.sql", DatabaseOptions.Schema.DocumentData);
        }

        deployHelper.ExecuteSqlFromFile("deploy_schema_bias.sql", DatabaseOptions.Schema.Bias);

        app.Logger.LogInformation("Finished!");
    }
    catch (Exception)
    {
        app.Logger.LogError("Deploy was aborted due to errors");
    }

    return;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("KnoxAllowedOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
