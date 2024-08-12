using Microsoft.EntityFrameworkCore;
using Task8.Data;
using Task8.Repository;
using Task8.Repository.IRepository;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SoftechWorldWideContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<IEmployeeRepository , EmployeeRepository>();
builder.Services.AddScoped<IKafkaService, KafkaService>();
builder.Services.AddScoped<IKafkaRepository , KafkaRepository>();
builder.Services.AddScoped<IRabbitMQService, RabbitMqService>();
builder.Services.AddScoped<IRabbitMQRepository , RabbitMQRepository>();
builder.Services.AddScoped<IElasticSearchService, ElasticSearchService>();
builder.Services.AddScoped<IAzureBusService , AzureBusService>();


builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

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
