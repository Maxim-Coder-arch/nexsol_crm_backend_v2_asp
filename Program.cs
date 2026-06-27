using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using NexsolCrmBackendVersion2.Services.Auth;
using NexsolCrmBackendVersion2.Settings;
using Scalar.AspNetCore;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("camelCase", conventionPack, t => true);

builder.Services.AddOpenApi();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = new CustomPropertyNamingPolicy();
    });

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

builder.Services.AddScoped<AuthService>();



var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "default-key-not-for-production";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();

public class CustomPropertyNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (name == "_Id")
            return "_id";
        return JsonNamingPolicy.CamelCase.ConvertName(name);
    }
}