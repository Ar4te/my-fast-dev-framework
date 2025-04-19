using System.Reflection;
using Application.IService;
using Application.Service;
using Common.Extension;
using Domain.Entity.Base;
using Domain.IRepository;
using Infrastructure;
using Infrastructure.UnitOfWork;
using Presentation.Extension;
using SqlSugar;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSqlSugarSetup(builder.Configuration);

builder.Services.AddTransient<IUnitOfWorkManager, UnitOfWorkManager>();
builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddControllers().AddControllersAsServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddJwtHelper(builder.Configuration);
builder.Services.AddSwaggerGenSetup();
builder.Services.AddCors(policy => policy.AddDefaultPolicy(t => t.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
var app = builder.Build();
app.UseCors(t => t.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.Services.InitialDb(Assembly.GetAssembly(typeof(BaseEntity))!.GetTypes().Where(t => typeof(BaseEntity).IsAssignableFrom(t)).ToArray());

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();