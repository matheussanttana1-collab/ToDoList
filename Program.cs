using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.EndPoints;
using ToDoList.Models;
using ToDoList.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("TarefasConnection");
builder.Services.AddDbContext<TarefasContext>(opts => 
	opts.UseLazyLoadingProxies().UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddIdentityApiEndpoints<Usuario>().AddEntityFrameworkStores<TarefasContext>();
	
builder.Services.AddScoped<TarefasService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.MapGroup("Auth").MapIdentityApi<Usuario>().WithTags("Authorization");

app.MapPost("Auth/logout", async (SignInManager<Usuario> signInManager) =>
{
	await signInManager.SignOutAsync();
	return Results.Ok();
}).RequireAuthorization().WithTags("Authorization");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapTarefasEndpoints();

app.Run();
