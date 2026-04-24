using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Orders.Api.Extensions;
using DotNetEnv;
using Microsoft.Extensions.Configuration;
using DotNetEnv.Configuration;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Orders.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureEnvironmentVariables();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureVersioning();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();

builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication();
builder.Services.ConfigureSwagger();

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
    config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
    config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
}).AddXmlDataContractSerializerFormatters()
.AddCustomCSVFormatter()
.AddApplicationPart(typeof(Orders.Api.Presentation.AssemblyReference).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// builder.Services.AddScoped<ValidationFilterAttribute>();
// builder.Services.AddScoped<ValidateMediaTypeAttribute>();

// builder.Services.AddControllers().AddApplicationPart(typeof(Orders.Api.Presentation.AssemblyReference).Assembly);


var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

// app pipeline
if (app.Environment.IsProduction())
    app.UseHsts();

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseIpRateLimiting();
app.UseCors("CorsPolicy");
app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Ed API v1");
    s.SwaggerEndpoint("/swagger/v2/swagger.json", "Ed API v2");
});

app.MapControllers();

// Configure the HTTP request pipeline.

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }

NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
    new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
    .Services.BuildServiceProvider()
    .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
    .OfType<NewtonsoftJsonPatchInputFormatter>().First();

