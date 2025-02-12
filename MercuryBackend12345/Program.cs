var builder = WebApplication.CreateBuilder(args);

// 1. 註冊 Controllers，並加入 NewtonSoft.Json 支援
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(); // ★ 關鍵

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

