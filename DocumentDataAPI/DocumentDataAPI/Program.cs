using DocumentDataAPI.Data.Deployment;
using DocumentDataAPI.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Check for "deploy=true" command-line argument
if (app.Configuration.GetValue<bool>("deploy"))
{
    var options = app.Configuration.GetSection(DatabaseOptions.Key).Get<DatabaseOptions>();
    var deployHelper = new DatabaseDeployHelper(options);
    Console.WriteLine($"Deploying to schema: {options.Database}.{options.Schema}");
    deployHelper.Deploy();
    Console.WriteLine("Finished!");
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