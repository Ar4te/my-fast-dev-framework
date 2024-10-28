using System.Reflection;
using Application.IService;
using Application.IService.RuleEngine;
using Application.Service;
using Application.Service.RuleEngine;
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
builder.Services.AddTransient<IRuleService, RuleService>();
builder.Services.AddTransient<IRuleFlowService, RuleFlowService>();
builder.Services.AddTransient<IRuleFlowNodeService, RuleFlowNodeService>();

builder.Services.AddControllers().AddControllersAsServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(policy => policy.AddDefaultPolicy(t => t.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
var app = builder.Build();
app.UseCors(t => t.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.Services.InitialDb(Assembly.GetAssembly(typeof(BaseEntity))!.GetTypes().Where(t => typeof(BaseEntity).IsAssignableFrom(t)).ToArray());

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();