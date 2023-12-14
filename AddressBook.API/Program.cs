using AddressBook.Domain.Abstractions.Helpers;
using AddressBook.Domain.Abstractions.Services;
using AddressBook.Domain.Abstractions.Wrappers;
using AddressBook.Domain.Helpers;
using AddressBook.Domain.Wrappers;
using AddressBook.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJsonSerializerHelper, JsonSerializerHelper>();
builder.Services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});

var apiSettings = builder.Configuration.GetSection("ApiSettings");
var baseUrl = apiSettings.GetValue<string>("BaseUrl");
builder.Services.AddHttpClient("Client", c => c.BaseAddress = new Uri(baseUrl));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");

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
