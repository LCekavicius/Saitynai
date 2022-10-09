using WebApi.Data;
using WebApi.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<LaucekERPDbContext>();
builder.Services.AddTransient<ICompaniesRepository, CompaniesRepository>();
builder.Services.AddTransient<IProductionOrdersRepository, ProductionOrdersRepository>();
builder.Services.AddTransient<IWorksRepository, WorksRepository>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();

app.Run();
