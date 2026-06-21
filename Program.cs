using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using NexsolCrmBackendVersion2.Settings;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("camelCase", conventionPack, t => true);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    MongoDBSettings? settings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
    return new MongoClient(settings?.ConnectionString);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    MongoDBSettings? settings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
    IMongoClient client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings?.DatabaseName);
});

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();




app.Run();
