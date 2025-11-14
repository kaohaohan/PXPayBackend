using Microsoft.EntityFrameworkCore;
using PXPayBackend.Data;
using PXPayBackend.Services;
using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddMemoryCache();
// 註冊radis 
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "ECommerceCache_"; 
});

// Rate Limiting
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Limit = 10,
            Period = "1m"
        }
    };
});
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();


// 注册 Service 層（業務邏輯）
// Scoped：每個 HTTP 請求會建立一個新的實例
builder.Services.AddScoped<IProductService, ProductService>();

// 注册 CORS（允許前端跨域請求）
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

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

// app.UseHttpsRedirection(); // 暫時關閉 HTTPS 重定向，方便測試

// 啟用 CORS
app.UseCors("AllowAll");

// 啟用 Rate Limiting 中間件
// app.UseIpRateLimiting();

app.UseAuthorization();

app.MapControllers();

// 啟動時自動建立測試資料（已註解，資料已存在）
// using (var scope = app.Services.CreateScope())
// {
//     var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
//     await productService.CreateTestProductsAsync();
// }

// 使用 port 5000
app.Run("http://localhost:5000");
