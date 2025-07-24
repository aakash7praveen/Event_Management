using EventManagementAPI;
using EventManagementAPI.Helpers;
using EventManagementAPI.Mapper;
using EventManagementAPI.Models;
using EventManagementAPI.Repositories;
using EventManagementAPI.Repositories.Interfaces;
using EventManagementAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>

{

    options.AddPolicy("AllowAll", policy =>

    {

        policy.AllowAnyOrigin()

              .AllowAnyHeader()

              .AllowAnyMethod();

    });

});


builder.Services.AddControllers();

builder.Services.AddSingleton<DbHelper>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserMapper>();

builder.Services.AddSingleton<DbHelper>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<EventMapper>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingProfile)); 
// OR:
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 

var app = builder.Build();
app.UseCors("AllowAll");
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
